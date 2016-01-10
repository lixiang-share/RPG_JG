
namespace Photon.LoadBalancing.Common.Net
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading;

    public class AsyncHttpRequest
    {
        public readonly HttpWebRequest WebRequest;

        private IAsyncResult asyncResult;

        private RegisteredWaitHandle timeoutWaitHandle;

        private int status;

        private byte[] readBuffer;

        private byte[] postContent;

        private Stream webResponseStream;

        private readonly Action<AsyncHttpRequest> callBackAction;

        public AsyncHttpRequest(HttpWebRequest webRequest, Action<AsyncHttpRequest> callBack)
            : this(webRequest, callBack, null)
        {
        }

        public AsyncHttpRequest(HttpWebRequest webRequest, Action<AsyncHttpRequest> callBack, object state)
        {
            this.WebRequest = webRequest;
            this.callBackAction = callBack;
            this.State = state;

            this.ReadBufferSize = 1024;
        }

        public int ReadBufferSize { get; set; }

        public HttpWebResponse WebResponse { get; private set; }

        public MemoryStream ResponseStream { get; private set; }

        public object State { get; set; }

        public AsyncHttpRequestStatus Status
        {
            get
            {
                return (AsyncHttpRequestStatus)this.status;
            }
        }

        public WebExceptionStatus WebStatus { get; private set; }

        /// <summary>
        /// Gets the Exception that caused the request to end prematurely. 
        /// If the request completed successfully, this will return null.
        /// </summary>
        public Exception Exception { get; private set; }

        public void GetAsync()
        {
            this.asyncResult = this.WebRequest.BeginGetResponse(this.GetResponseCallBack, null);
            this.timeoutWaitHandle = ThreadPool.UnsafeRegisterWaitForSingleObject(this.asyncResult.AsyncWaitHandle, this.TimeoutCallback, null, this.WebRequest.Timeout, true);
        }

        public void PostAsync(byte[] content)
        {
            this.postContent = content;
            this.asyncResult = this.WebRequest.BeginGetRequestStream(this.GetRequestStreamCallback, null);
            this.timeoutWaitHandle = ThreadPool.UnsafeRegisterWaitForSingleObject(this.asyncResult.AsyncWaitHandle, this.TimeoutCallback, null, this.WebRequest.Timeout, true);
        }

        public bool Cancel()
        {
            if (!this.TrySetStatus(AsyncHttpRequestStatus.Running, AsyncHttpRequestStatus.Canceled))
            {
                return false;
            }

            this.WebRequest.Abort();
            this.UnregisterTimeoutWaitHandle();
            this.WebStatus = WebExceptionStatus.RequestCanceled;
            return true;
        }

        private void GetRequestStreamCallback(IAsyncResult ar)
        {
            try
            {
                if (!this.TrySetStatus(AsyncHttpRequestStatus.Running, AsyncHttpRequestStatus.Running))
                {
                    return;
                }

                this.UnregisterTimeoutWaitHandle();

                var requestStream = this.WebRequest.EndGetRequestStream(ar);
                requestStream.Write(this.postContent, 0, this.postContent.Length);
                requestStream.Flush();
                requestStream.Close();
                requestStream.Dispose();

                this.asyncResult = this.WebRequest.BeginGetResponse(this.GetResponseCallBack, null);
                this.timeoutWaitHandle = ThreadPool.UnsafeRegisterWaitForSingleObject(this.asyncResult.AsyncWaitHandle, this.TimeoutCallback, null, this.WebRequest.Timeout, true);
            }
            catch (Exception ex)
            {
                this.SetException(ex);
            }
        }

        private void GetResponseCallBack(IAsyncResult ar)
        {
            try
            {            
                if (!this.TrySetStatus(AsyncHttpRequestStatus.Running, AsyncHttpRequestStatus.Completed))
                {
                    return;
                }

                this.UnregisterTimeoutWaitHandle();

                this.WebResponse = (HttpWebResponse)this.WebRequest.EndGetResponse(ar);
                this.WebStatus = WebExceptionStatus.Success;

                this.webResponseStream = this.WebResponse.GetResponseStream();

                int len = this.WebResponse.ContentLength == -1 ? ReadBufferSize : (int)this.WebResponse.ContentLength;
                this.ResponseStream = new MemoryStream(len);
                this.readBuffer = new byte[this.ReadBufferSize];
                this.webResponseStream.BeginRead(this.readBuffer, 0, this.readBuffer.Length, this.ReadCallback, null);

            }
            catch (Exception ex)
            {
                this.SetException(ex);
            }
        }

        private void UnregisterTimeoutWaitHandle()
        {
            if (this.timeoutWaitHandle != null)
            {
                this.timeoutWaitHandle.Unregister(this.asyncResult.AsyncWaitHandle);
                this.timeoutWaitHandle = null;
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                int bytesRead = this.webResponseStream.EndRead(ar);
                if (bytesRead <= 0)
                {
                    this.ResponseStream.Position = 0;
                    this.EndRequest();
                    return;
                }

                this.ResponseStream.Write(this.readBuffer, 0, bytesRead);
                this.webResponseStream.BeginRead(this.readBuffer, 0, this.readBuffer.Length, this.ReadCallback, null);
            }
            catch (Exception ex)
            {
                this.SetException(ex);
            }
        }

        private void TimeoutCallback(object state, bool timedOut)
        {
            try
            {
                if (timedOut == false)
                {
                    return;
                }

                if (!this.TrySetStatus(AsyncHttpRequestStatus.Running, AsyncHttpRequestStatus.Faulted))
                {
                    return;
                }

                this.WebRequest.Abort();
                this.SetException(new WebException("The operation has timed out", WebExceptionStatus.Timeout));
            }
            catch (Exception ex)
            {
                this.SetException(ex);
            }
        }

        private void SetStatus(AsyncHttpRequestStatus newStatus)
        {
            Interlocked.Exchange(ref this.status, (int)newStatus);
        }

        private bool TrySetStatus(AsyncHttpRequestStatus oldStatus, AsyncHttpRequestStatus newStatus)
        {
            return Interlocked.CompareExchange(ref this.status, (int)newStatus, (int)oldStatus) == (int)oldStatus;
        }

        private void SetException(Exception ex)
        {
            this.SetStatus(AsyncHttpRequestStatus.Faulted);
            this.Exception = ex;
            this.WebResponse = null;
            this.ResponseStream = null;

            var webException = ex as WebException;
            if (webException != null)
            {
                this.WebStatus = webException.Status;
            }
            else
            {
                this.WebStatus = WebExceptionStatus.UnknownError;
            }
            
            this.EndRequest();
        }

        private void EndRequest()
        {
            this.Cleanup();
            this.callBackAction(this);
        }

        private void Cleanup()
        {
            if (this.webResponseStream != null)
            {
                this.webResponseStream.Dispose();
                this.webResponseStream = null;
            }

            if (this.WebResponse != null)
            {
                this.WebResponse.Close();
            }
        }
    }
}
