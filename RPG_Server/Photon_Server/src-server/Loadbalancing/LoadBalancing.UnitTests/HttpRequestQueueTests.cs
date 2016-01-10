namespace Photon.LoadBalancing.UnitTests
{
    using System;
    using System.Text;
    using System.Net;
    using System.Threading;

    using ExitGames.Concurrency.Fibers;

    using NUnit.Framework;

    using Photon.LoadBalancing.Common.Net;

    [TestFixture]
    [Explicit("Can only run with administrative privileges")]
    public class HttpRequestQueueTests
    {
        private const int TimeOutMilliseconds = 1000;

        private HttpListener httpListener;

        private readonly AutoResetEvent resetEvent = new AutoResetEvent(false);

        private int responseCount;

        [TestFixtureSetUp]
        public void Setup()
        {
            this.httpListener = new HttpListener();
            this.httpListener.Prefixes.Add("http://localhost:8080/");
            this.httpListener.Start();
            Console.WriteLine("Listener is running");

            this.httpListener.BeginGetContext(this.HttpListerCallback, null);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            this.httpListener.Stop();
            this.httpListener.Close();
            this.httpListener = null;
        }

        [Test]
        public void SingleRequest()
        {
            var queue = new HttpRequestQueue();
            queue.Enqueue("http://localhost:8080/", this.ResponseCallBack, null);
            Assert.IsTrue(this.resetEvent.WaitOne(TimeOutMilliseconds));
        }

        [Test]
        public void MaxConcurrentRequests()
        {
            const int requestCount = 10;
            this.responseCount = 0;

            var fiber = new StubFiber();
            fiber.ExecutePendingImmediately = false;

            var queue = new HttpRequestQueue(fiber);
            queue.MaxConcurrentRequests = 2;

            for (int i = 0; i < requestCount; i++)
            {
                queue.Enqueue("http://localhost:8080/", this.ResponseCallBack, null);
            }
            
            fiber.ExecuteAllPending();
            Assert.AreEqual(1, queue.RunningRequestsCount);
            Assert.AreEqual(requestCount - 1, queue.QueuedRequestCount);

            var startTime = DateTime.UtcNow;
            var time = TimeSpan.FromMilliseconds(TimeOutMilliseconds * requestCount);

            while (this.responseCount < 10)
            {
                Assert.Less(DateTime.UtcNow.Subtract(startTime), time, "Received not all responses in the expected time");

                if (this.responseCount > 0)
                {
                    Assert.Less(queue.MaxConcurrentRequests, queue.MaxConcurrentRequests + 1);
                }

                fiber.ExecuteAllPending();
                Thread.Sleep(50);
            }
        }

        [Test]
        public void MaxTimedOutRequests()
        {
            var queue = new HttpRequestQueue();

            // get queue into running state
            var webRequest = this.CreateWebRequest(string.Empty);
            var requestState = new RequestState();
            queue.Enqueue(webRequest, this.ResponseCallBack, requestState);
            Assert.IsTrue(requestState.WaitOne());
            Assert.AreEqual(HttpRequestQueueResultCode.Success, requestState.Result);
            Assert.AreEqual(HttpRequestQueueState.Running, queue.QueueState);

            // timeout a request
            webRequest = this.CreateWebRequest("Timeout");
            requestState = new RequestState();
            queue.Enqueue(webRequest, this.ResponseCallBack, requestState);
            Assert.IsTrue(requestState.WaitOne(TimeOutMilliseconds * 2));
            Assert.AreEqual(HttpRequestQueueResultCode.RequestTimeout, requestState.Result);

            // queue should be still in running state because no value for 
            // MaxTimedOutRequests are specified
            Assert.AreEqual(HttpRequestQueueState.Running, queue.QueueState);
            Assert.AreEqual(0, queue.TimedOutRequests);

            queue.MaxTimedOutRequests = 2;
            webRequest = this.CreateWebRequest("Timeout");
            requestState = new RequestState();
            queue.Enqueue(webRequest, this.ResponseCallBack, requestState);
            Assert.IsTrue(requestState.WaitOne(TimeOutMilliseconds * 2));
            Assert.AreEqual(HttpRequestQueueResultCode.RequestTimeout, requestState.Result);

            // queue should be still in running state because the specified MaxTimedOutRequests 
            // value is not reached
            Assert.AreEqual(HttpRequestQueueState.Running, queue.QueueState);
            Assert.AreEqual(1, queue.TimedOutRequests);

            webRequest = this.CreateWebRequest(string.Empty);
            requestState = new RequestState();
            queue.Enqueue(webRequest, this.ResponseCallBack, requestState);
            Assert.IsTrue(requestState.WaitOne(TimeOutMilliseconds));
            Assert.AreEqual(HttpRequestQueueResultCode.Success, requestState.Result);

            webRequest = this.CreateWebRequest("Timeout");
            requestState = new RequestState();
            queue.Enqueue(webRequest, this.ResponseCallBack, requestState);
            Assert.IsTrue(requestState.WaitOne(TimeOutMilliseconds * 2));
            Assert.AreEqual(HttpRequestQueueResultCode.RequestTimeout, requestState.Result);

            // queue should be still in running state because the counter for 
            // timed out requests is reset by the successfull request
            Assert.AreEqual(HttpRequestQueueState.Running, queue.QueueState);
            Assert.AreEqual(1, queue.TimedOutRequests);

            webRequest = this.CreateWebRequest("Timeout");
            requestState = new RequestState();
            queue.Enqueue(webRequest, this.ResponseCallBack, requestState);
            Assert.IsTrue(requestState.WaitOne(TimeOutMilliseconds * 2));
            Assert.AreEqual(HttpRequestQueueResultCode.RequestTimeout, requestState.Result);

            // queue should be in offline state because the counter for 
            // timed out requests reached the MaxTimedOutRequests value
            Assert.AreEqual(2, queue.TimedOutRequests);
            Assert.AreEqual(HttpRequestQueueState.Offline, queue.QueueState);
        }

        [Test]
        public void Offline()
        {
            var fiber = new StubFiber();
            fiber.ExecutePendingImmediately = false;

            var queue = new HttpRequestQueue();
            queue.MaxConcurrentRequests = 2;
            queue.ReconnectInterval = TimeSpan.FromMilliseconds(500);

            var state = new RequestState();
            var state2 = new RequestState();
            var state3 = new RequestState();
            
            queue.Enqueue("http://localhost:8080?func=Forbidden", this.ResponseCallBack, state);
            queue.Enqueue("http://localhost:8080?func=Forbidden", this.ResponseCallBack, state2);
            queue.Enqueue("http://localhost:8080?func=Forbidden", this.ResponseCallBack, state3);

            Assert.IsTrue(state.ResetEvent.WaitOne(TimeOutMilliseconds));
            Assert.AreEqual(HttpRequestQueueResultCode.Error, state.Result);

            Assert.IsTrue(state2.ResetEvent.WaitOne(TimeOutMilliseconds));
            Assert.AreEqual(HttpRequestQueueResultCode.Offline, state2.Result);

            Assert.IsTrue(state3.ResetEvent.WaitOne(TimeOutMilliseconds));
            Assert.AreEqual(HttpRequestQueueResultCode.Offline, state3.Result);

            Assert.AreEqual(HttpRequestQueueState.Offline, queue.QueueState);

            state = new RequestState();
            queue.Enqueue("http://localhost:8080", this.ResponseCallBack, state);
            Assert.IsTrue(state.ResetEvent.WaitOne(TimeOutMilliseconds));
            Assert.AreEqual(HttpRequestQueueResultCode.Offline, state.Result);
            Assert.AreEqual(HttpRequestQueueState.Offline, queue.QueueState);

            Thread.Sleep(750);
            state = new RequestState();
            state2 = new RequestState();
            queue.Enqueue("http://localhost:8080", this.ResponseCallBack, state);
            queue.Enqueue("http://localhost:8080", this.ResponseCallBack, state2);
            Assert.IsTrue(state.ResetEvent.WaitOne(TimeOutMilliseconds));
            Assert.AreEqual(HttpRequestQueueResultCode.Success, state.Result);

            Assert.IsTrue(state2.ResetEvent.WaitOne(TimeOutMilliseconds));
            Assert.AreEqual(HttpRequestQueueResultCode.Offline, state2.Result);
            
            Assert.AreEqual(HttpRequestQueueState.Running, queue.QueueState);
        }

        private HttpWebRequest CreateWebRequest(string function)
        {
            string url = string.Format("http://localhost:8080?func={0}", function);
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = TimeOutMilliseconds;
            return request;
        }

        private void HttpListerCallback(IAsyncResult asyncResult)
        {
            if (this.httpListener == null || this.httpListener.IsListening == false)
            {
                return;
            }

            HttpListenerContext context = this.httpListener.EndGetContext(asyncResult);
            this.httpListener.BeginGetContext(this.HttpListerCallback, null);

            HttpListenerRequest request = context.Request;
            var response = context.Response;
            response.StatusCode = (int)HttpStatusCode.OK;

            var methode = request.QueryString["func"];
            switch (methode)
            {
                case "Forbidden":
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;

                case "Timeout":
                    return;
            }

            byte[] buffer;

            if (request.HttpMethod == "POST")
            {
                buffer = new byte[request.ContentLength64];
                request.InputStream.Read(buffer, 0, buffer.Length);
            }
            else
            {
                buffer = Encoding.UTF8.GetBytes("Hello");
            }

            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Flush();
            context.Response.OutputStream.Close();

        }

        private void ResponseCallBack(HttpRequestQueueResultCode result, AsyncHttpRequest response, object state)
        {
            Interlocked.Increment(ref this.responseCount);

            var requestState = state as RequestState;
            if (requestState != null)
            {
                requestState.Result = result;
                requestState.Response = response;
                requestState.State = state;
                requestState.ResetEvent.Set();
            }

            this.resetEvent.Set();
        }

        private class RequestState
        {
            public readonly AutoResetEvent ResetEvent = new AutoResetEvent(false);

            public HttpRequestQueueResultCode Result { get; set; }

            public AsyncHttpRequest Response { get; set; }
            
            public object State { get; set; }

            public bool WaitOne()
            {
                return this.ResetEvent.WaitOne(TimeOutMilliseconds);
            }

            public bool WaitOne(int timeout)
            {
                return this.ResetEvent.WaitOne(timeout);
            }
        }
    }
}
