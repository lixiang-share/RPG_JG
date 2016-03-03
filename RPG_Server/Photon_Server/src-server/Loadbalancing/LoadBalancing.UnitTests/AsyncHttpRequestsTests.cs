
namespace Photon.LoadBalancing.UnitTests
{
    using System;
    using System.Net;
    using System.Threading;

    using NUnit.Framework;

    using Photon.LoadBalancing.Common.Net;

    [TestFixture]
    [Explicit("Can only run with administrative privileges")]
    public class AsyncHttpRequestsTests
    {
        private const int TimeOutMilliseconds = 1000;

        private HttpListener httpListener;

        private readonly AutoResetEvent resetEvent = new AutoResetEvent(false);

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
        public void GetAsync()
        {
            var request = this.CreateAsyncWebRequest(string.Empty);
            request.GetAsync();
            
            Assert.IsTrue(this.resetEvent.WaitOne(TimeOutMilliseconds * 10000));
            Assert.AreEqual(AsyncHttpRequestStatus.Completed, request.Status);
            Assert.IsNotNull(request.WebResponse);
            Assert.IsNotNull(request.ResponseStream);
            Assert.IsNull(request.Exception);

            var buffer = request.ResponseStream.ToArray();
            var message = System.Text.Encoding.UTF8.GetString(buffer);
            Assert.AreEqual("Hello", message);
        }

        [Test]
        public void PostAsync()
        {
            var data = new byte[1024];
            data[0] = 12;
            data[1023] = 42;

            var request = this.CreateAsyncWebRequest(string.Empty);
            request.WebRequest.Method = "POST";
            request.PostAsync(data);

            Assert.IsTrue(this.resetEvent.WaitOne(TimeOutMilliseconds));
            Assert.AreEqual(AsyncHttpRequestStatus.Completed, request.Status);
            Assert.IsNotNull(request.WebResponse);
            Assert.IsNotNull(request.ResponseStream);
            Assert.IsNull(request.Exception);

            var buffer = request.ResponseStream.ToArray();
            Assert.AreEqual(data, buffer);
        }

        [Test]
        public void TimeOut()
        {
            var request = this.CreateAsyncWebRequest("Timeout");
            request.GetAsync();

            Assert.IsTrue(this.resetEvent.WaitOne(TimeOutMilliseconds * 2));
            Assert.AreEqual(AsyncHttpRequestStatus.Faulted, request.Status);
            Assert.IsNull(request.WebResponse);
            Assert.IsNull(request.ResponseStream);
            Assert.IsNotNull(request.Exception);

            Assert.IsInstanceOf<WebException>(request.Exception);
            var exception = (WebException)request.Exception;
            Assert.AreEqual(WebExceptionStatus.Timeout, exception.Status);
        }

        [Test]
        public void WebException()
        {
            var request = this.CreateAsyncWebRequest("Forbidden");
            request.GetAsync();

            Assert.IsTrue(this.resetEvent.WaitOne(TimeOutMilliseconds));
            Assert.AreEqual(AsyncHttpRequestStatus.Faulted, request.Status);
            Assert.IsNull(request.WebResponse);
            Assert.IsNull(request.ResponseStream);
            Assert.IsNotNull(request.Exception);

            Assert.IsInstanceOf<WebException>(request.Exception);
            var exception = (WebException)request.Exception;
            Assert.AreEqual(WebExceptionStatus.ProtocolError, exception.Status);
            var response = (HttpWebResponse)exception.Response;
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Test]
        public void CancelRequest()
        {
            var request = this.CreateAsyncWebRequest(string.Empty);
            request.GetAsync();
            var canceled = request.Cancel();

            Assert.IsTrue(canceled);
            Assert.AreEqual(AsyncHttpRequestStatus.Canceled, request.Status);
            Assert.IsNull(request.WebResponse);
            Assert.IsNull(request.ResponseStream);
            Assert.IsNull(request.Exception);
        }

        [Test]
        public void CancelRequestAfterCompleted()
        {
            var request = this.CreateAsyncWebRequest(string.Empty);
            request.GetAsync();

            Assert.IsTrue(this.resetEvent.WaitOne(TimeOutMilliseconds));
            Assert.AreEqual(AsyncHttpRequestStatus.Completed, request.Status);
            Assert.IsNotNull(request.WebResponse);
            Assert.IsNotNull(request.ResponseStream);
            Assert.IsNull(request.Exception);

            var canceled = request.Cancel();
            Assert.IsFalse(canceled);
            Assert.AreEqual(AsyncHttpRequestStatus.Completed, request.Status);
        }

        private AsyncHttpRequest CreateAsyncWebRequest(string function)
        {
            var webRequest = this.CreateWebRequest(function);
            return new AsyncHttpRequest(webRequest, ResponseCallBack);
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
                buffer = System.Text.Encoding.UTF8.GetBytes("Hello");
            }
            
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Flush();
            context.Response.OutputStream.Close();

        }

        private void ResponseCallBack(AsyncHttpRequest request)
        {
            this.resetEvent.Set();
        }
    }
}

