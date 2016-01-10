
namespace Photon.LoadBalancing.Common.Net
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using ExitGames.Concurrency.Fibers;
    using ExitGames.Logging;

    public delegate void HttpRequestQueueCallback(HttpRequestQueueResultCode result, AsyncHttpRequest request, object userState);

    public enum HttpRequestQueueResultCode
    {
        Success,
        RequestTimeout,
        QueueTimeout,
        Offline,
        QueueFull,
        Error,
    }

    public enum HttpRequestQueueState
    {
        Running,
        Connecting,
        Reconnecting,
        Offline,
    }

    public class HttpRequestQueue : IDisposable
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly IFiber fiber;

        private readonly LinkedList<QueuedRequest> queue = new LinkedList<QueuedRequest>();

        public readonly TimeSpan QueueTimeout = TimeSpan.FromSeconds(5);

        private DateTime nextReconnectTime;

        public HttpRequestQueue()
        {
            this.QueueState = HttpRequestQueueState.Connecting;
            this.MaxConcurrentRequests = 1;
            this.MaxQueuedRequests = 100;

            this.fiber = new PoolFiber();
            this.fiber.Start();
        }

        public HttpRequestQueue(IFiber fiber)
        {
            this.QueueState = HttpRequestQueueState.Connecting;
            this.MaxConcurrentRequests = 1;
            this.MaxQueuedRequests = 100;
            this.ReconnectInterval = TimeSpan.FromMinutes(1);

            this.fiber = fiber;
            this.fiber.Start();
        }

        ~HttpRequestQueue()
        {
            this.Dispose(false);
        }

        public int QueuedRequestCount
        {
            get
            {
                return this.queue.Count;
            }
        }

        public HttpRequestQueueState QueueState { get; private set; }

        public int RunningRequestsCount { get; private set; }

        public int MaxConcurrentRequests { get; set; }

        public int MaxQueuedRequests { get; set; }

        public int MaxTimedOutRequests { get; set; }

        public int TimedOutRequests { get; private set; }

        public TimeSpan ReconnectInterval { get; set; }

        public void Enqueue(string requestUri, HttpRequestQueueCallback callback, object state)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(requestUri);
            this.fiber.Enqueue(() => this.ExecuteRequest(webRequest, callback, state));
        }

        public void Enqueue(HttpWebRequest webRequest, HttpRequestQueueCallback callback, object state)
        {
            this.fiber.Enqueue(() => this.ExecuteRequest(webRequest, callback, state));
        }

        private void ExecuteRequest(HttpWebRequest webRequest, HttpRequestQueueCallback callback, object state)
        {
            if (this.queue.Count > this.MaxQueuedRequests)
            {
                callback(HttpRequestQueueResultCode.QueueFull, null, state);
                return;
            }

            var request = new QueuedRequest { Request = webRequest, Callback = callback, PostData = null, State = state, };

            switch (this.QueueState)
            {
                case HttpRequestQueueState.Connecting:
                    this.ExecuteRequestConnecting(request);
                    break;

                case HttpRequestQueueState.Reconnecting:
                    this.ExecuteRequestReconnecting(request);
                    break;

                case HttpRequestQueueState.Running:
                    this.ExecuteRequestOnline(request);
                    break;

                case HttpRequestQueueState.Offline:
                    this.ExecuteRequestOffline(request);
                    break;
            }
        }

        private void ExecuteHttpRequest(QueuedRequest request)
        {
            try
            {
                this.RunningRequestsCount++;
                var asyncHttpRequest = new AsyncHttpRequest(request.Request, this.WebRequestCallback, request);
                if (request.PostData == null)
                {
                    asyncHttpRequest.GetAsync();
                }
                else
                {
                    asyncHttpRequest.PostAsync(request.PostData);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void ExecuteRequestOnline(QueuedRequest request)
        {
            if (this.RunningRequestsCount >= this.MaxConcurrentRequests)
            {
                this.queue.AddLast(request);
                return;
            }

            this.ExecuteHttpRequest(request);
        }

        private void ExecuteRequestConnecting(QueuedRequest request)
        {
            if (this.RunningRequestsCount < this.MaxConcurrentRequests)
            {
                this.ExecuteHttpRequest(request);
                return;
            }

            this.queue.AddLast(request);
        }

        private void ExecuteRequestReconnecting(QueuedRequest request)
        {
            if (this.RunningRequestsCount < 1)
            {
                this.ExecuteHttpRequest(request);
                return;
            }

            request.Callback(HttpRequestQueueResultCode.Offline, null, request.State);
        }

        private void ExecuteRequestOffline(QueuedRequest request)
        {
            if (DateTime.UtcNow >= this.nextReconnectTime)
            {
                this.QueueState = HttpRequestQueueState.Reconnecting;
                this.ExecuteHttpRequest(request);
                return;
            }

            request.Callback(HttpRequestQueueResultCode.Offline, null, request.State);
        }

        private void WebRequestCallback(AsyncHttpRequest request)
        {
            this.fiber.Enqueue(() => this.ProcessWebResponse(request));
        }

        private void ProcessWebResponse(AsyncHttpRequest request)
        {
            this.RunningRequestsCount--;

            switch (this.QueueState)
            {
                case HttpRequestQueueState.Connecting:
                    this.ProcessResponseConnecting(request);
                    break;

                case HttpRequestQueueState.Running:
                    this.ProcessResponseRunning(request);
                    break;

                case HttpRequestQueueState.Reconnecting:
                    this.ProcessResponseReConnecting(request);
                    break;

                case HttpRequestQueueState.Offline:
                    this.ProcessResponseOffline(request);
                    break;
            }

            this.ProcessQueuedItems();           
        }

        private void ProcessResponseConnecting(AsyncHttpRequest request)
        {
            var queuedRequest = (QueuedRequest)request.State;

            switch (request.WebStatus)
            {
                case WebExceptionStatus.Success:
                    this.QueueState = HttpRequestQueueState.Running;
                    DecrementTimedOutCount();
                    queuedRequest.Callback(HttpRequestQueueResultCode.Success, request, queuedRequest.State);
                    break;

                case WebExceptionStatus.Timeout:
                    IncrementTimedOutCount();
                    queuedRequest.Callback(HttpRequestQueueResultCode.RequestTimeout, request, queuedRequest.State);
                    break;

                default:
                    this.SetOffline();
                    queuedRequest.Callback(HttpRequestQueueResultCode.Error, request, queuedRequest.State);
                    break;
            }
        }

        private void ProcessResponseReConnecting(AsyncHttpRequest request)
        {
            var queuedRequest = (QueuedRequest)request.State;

            switch (request.WebStatus)
            {
                case WebExceptionStatus.Success:
                    this.QueueState = HttpRequestQueueState.Running;
                    queuedRequest.Callback(HttpRequestQueueResultCode.Success, request, queuedRequest.State);
                    break;

                case WebExceptionStatus.Timeout:
                    this.SetOffline();
                    queuedRequest.Callback(HttpRequestQueueResultCode.RequestTimeout, request, queuedRequest.State);
                    break;

                default:
                    this.SetOffline();
                    queuedRequest.Callback(HttpRequestQueueResultCode.Error, request, queuedRequest.State);
                    break;
            }
        }

        private void ProcessResponseRunning(AsyncHttpRequest request)
        {
            var queuedRequest = (QueuedRequest)request.State;

            switch (request.WebStatus)
            {
                case WebExceptionStatus.Success:
                    DecrementTimedOutCount();
                    queuedRequest.Callback(HttpRequestQueueResultCode.Success, request, queuedRequest.State);
                    break;

                case WebExceptionStatus.Timeout:
                    IncrementTimedOutCount();
                    queuedRequest.Callback(HttpRequestQueueResultCode.RequestTimeout, request, queuedRequest.State);
                    break;

                default:
                    this.SetOffline();
                    queuedRequest.Callback(HttpRequestQueueResultCode.Error, request, queuedRequest.State);
                    break;
            }
        }

        private void ProcessResponseOffline(AsyncHttpRequest request)
        {
            var queuedRequest = (QueuedRequest)request.State;
            queuedRequest.Callback(HttpRequestQueueResultCode.Offline, null, queuedRequest.State);
        }

        private void ProcessQueuedItems()
        {
            if (this.queue.Count == 0)
            {
                return;
            }

            // remove all request that timed out already
            DateTime now = DateTime.UtcNow;
            DateTime maxRequestDate = now.Subtract(this.QueueTimeout);
            while (this.queue.Count > 0)
            {
                var nextRequest = this.queue.First.Value;
                if (nextRequest.CreateDate > maxRequestDate)
                {
                    break;
                }

                nextRequest.Callback(HttpRequestQueueResultCode.QueueTimeout, null, nextRequest.State);
                this.queue.RemoveFirst();
            }

            // execute requests until max concurrent requests are reached
            while (this.queue.Count > 0 && this.RunningRequestsCount < this.MaxConcurrentRequests)
            {
                var nextRequest = this.queue.First.Value;
                this.queue.RemoveFirst();
                this.ExecuteHttpRequest(nextRequest);
            }
        }

        private void SetOffline()
        {
            if (this.QueueState == HttpRequestQueueState.Offline)
            {
                return;
            }

            this.QueueState = HttpRequestQueueState.Offline;
            this.nextReconnectTime = DateTime.UtcNow.Add(this.ReconnectInterval);

            if (log.IsInfoEnabled)
            {
                log.InfoFormat("Request queue has been set offline");
            }

            foreach (var item in this.queue)
            {
                item.Callback(HttpRequestQueueResultCode.Offline, null, item.State);
            }

            this.queue.Clear();
        }

        private void DecrementTimedOutCount()
        {
            if (this.MaxTimedOutRequests > 0 && this.TimedOutRequests > 0)
            {
                this.TimedOutRequests--;
            }
        }

        private void IncrementTimedOutCount()
        {
            if (this.MaxTimedOutRequests <= 0)
            {
                return;
            }

            this.TimedOutRequests++;
            if (this.TimedOutRequests >= this.MaxTimedOutRequests)
            {
                this.SetOffline();
            }
        }

        /// <summary>
        /// Releases the unmanaged resources and disposes of the managed resources used by the <see cref="HttpRequestQueue"/>.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="HttpRequestQueue"/> and optionally disposes of the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources; false to releases only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing == false)
            {
                return;
            }

            this.fiber.Dispose();
        }

        private class QueuedRequest
        {
            public readonly DateTime CreateDate = DateTime.UtcNow;

            public HttpWebRequest Request { get; set; }

            public HttpRequestQueueCallback Callback { get; set; }

            public byte[] PostData { get; set; }

            public object State { get; set; }
        }
    }
}
