// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestClient.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The test client.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.UnitTests.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    using ExitGames.Client.Photon;
    using ExitGames.Client.Photon.LoadBalancing;
    using ExitGames.Concurrency.Fibers;
    using ExitGames.Logging;
    using ExitGames.Threading;

    /// <summary>
    ///   The test client.
    /// </summary>
    public class TestClient : IDisposable, IPhotonPeerListener
    {
        #region Constants and Fields

        /// <summary>
        ///   The log.
        /// </summary>
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///   The auto reset event init.
        /// </summary>
        private readonly AutoResetEvent autoResetEventInit;

        /// <summary>
        ///   The event queue.
        /// </summary>
        public Queue<EventData> EventQueue { get; private set; }


        /// <summary>
        ///   The fiber.
        /// </summary>
        private readonly PoolFiber fiber = new PoolFiber(new FailSafeBatchExecutor());

        /// <summary>
        ///   The operation response queue.
        /// </summary>
        public Queue<OperationResponse> OperationResponseQueue { get; private set; }

     
        /// <summary>
        ///   The service.
        /// </summary>
        private long service;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "TestClient" /> class.
        /// </summary>
        public TestClient(ConnectionProtocol connectionProtocol)
        {
            this.autoResetEventInit = new AutoResetEvent(false);
            this.OperationResponseQueue = new Queue<OperationResponse>(1000); 
            this.EventQueue = new Queue<EventData>(1000);
           
            this.PhotonClient = new LoadBalancingPeer(this, connectionProtocol) { DebugOut = DebugLevel.INFO };
            this.fiber.Start();
        }

        #endregion

        #region Events

        /// <summary>
        ///   The connected.
        /// </summary>
        public static event Action<TestClient> Connected;

        /// <summary>
        ///   The disconnected.
        /// </summary>
        public static event Action<TestClient> Disonnected;

        /// <summary>
        ///   The event received.
        /// </summary>
        public static event Action<TestClient, EventData> EventReceived;

        /// <summary>
        ///   The response received.
        /// </summary>
        public static event Action<TestClient, OperationResponse> ResponseReceived;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the underling <see cref = "PhotonClient" />.
        /// </summary>
        public LoadBalancingPeer PhotonClient { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        ///   The close.
        /// </summary>
        public void Close()
        {
            this.fiber.Enqueue(
                () =>
                {
                    this.PhotonClient.Disconnect();
                    this.fiber.Dispose();
                });
        }

        /// <summary>
        ///   The connect.
        /// </summary>
        /// <param name = "hostName">
        ///   The host name.
        /// </param>
        /// <param name = "port">
        ///   The port.
        /// </param>
        /// <param name = "applicationId">
        ///   The application Id.
        /// </param>
        public void Connect(string hostName, int port, string applicationId)
        {
            this.fiber.Enqueue(
                () =>
                {
                    this.PhotonClient.Connect(string.Format("{0}:{1}", hostName, port), applicationId);
                    this.StartService();
                });
        }

        /// <summary>
        ///   The send operation request.
        /// </summary>
        /// <param name = "operationRequest">
        ///   The operation request.
        /// </param>
        public void SendOperationRequest(OperationRequest operationRequest)
        {
            this.fiber.Enqueue(
                () =>
                this.PhotonClient.OpCustom(
                    operationRequest.OperationCode,
                    operationRequest.Parameters,
                    true,
                    0));
        }

        /// <summary>
        ///   Sends a couple of operation requests.
        /// </summary>
        /// <param name = "operationRequests">
        ///   The operation requests.
        /// </param>
        public void SendOperationRequests(OperationRequest[] operationRequests)
        {
            this.fiber.Enqueue(
                () =>
                {
                    foreach (OperationRequest operationRequest in operationRequests)
                    {
                        this.PhotonClient.OpCustom(
                            operationRequest.OperationCode,
                            operationRequest.Parameters,
                            true,
                            0);
                    }
                });
        }

        /// <summary>
        ///   The wait for connect.
        /// </summary>
        /// <param name = "millisecondsWaitTime">
        ///   The milliseconds wait time.
        /// </param>
        /// <returns>
        ///   true if connected.
        /// </returns>
        public bool WaitForConnect(int millisecondsWaitTime)
        {
            bool result = this.autoResetEventInit.WaitOne(millisecondsWaitTime);
            return result;
        }

        /// <summary>
        ///   The wait for event.
        /// </summary>
        /// <param name = "millisecodsWaitTime">
        ///   The millisecods wait time.
        /// </param>
        /// <returns>
        ///   the event
        /// </returns>
        public EventData WaitForEvent(int millisecodsWaitTime)
        {
            var timeout = Environment.TickCount + millisecodsWaitTime;

            while (Environment.TickCount < timeout)
            {
                if (this.EventQueue.Count > 0)
                {
                    return this.EventQueue.Dequeue();
                }
            }

            throw new TimeoutException();
        }

        /// <summary>
        ///   Wait for an event with a specific event code. Discard all other events that arrive in the meantime.
        /// </summary>
        /// <param name = "millisecodsWaitTime">
        ///   The millisecods wait time.
        /// </param>
        /// <param name="eventCode">Wait for an event with this event code.</param>
        /// <returns>
        ///   the event
        /// </returns>
        public EventData WaitForEvent(byte eventCode, int millisecodsWaitTime)
        {
            var timeout = Environment.TickCount + millisecodsWaitTime;

            while (Environment.TickCount < timeout)
            {
                if (this.EventQueue.Count > 0)
                {
                    var ev = this.EventQueue.Dequeue();
                    if (ev.Code == eventCode)
                    {
                        return ev;
                    } 
                }
            }

            throw new TimeoutException();
        }

        /// <summary>
        ///   The wait for operation response.
        /// </summary>
        /// <param name = "millisecodsWaitTime">
        ///   The millisecods wait time.
        /// </param>
        /// <returns>
        ///   the response
        /// </returns>
        public OperationResponse WaitForOperationResponse(int millisecodsWaitTime)
        {
            var timeout = Environment.TickCount + millisecodsWaitTime;

            while (Environment.TickCount < timeout)
            {
                if (this.OperationResponseQueue.Count > 0)
                {
                    return this.OperationResponseQueue.Dequeue();
                }
            }

            throw new TimeoutException();
        }

        #endregion

        #region Implemented Interfaces

        #region IDisposable

        /// <summary>
        ///   The dispose.
        /// </summary>
        public void Dispose()
        {
            this.fiber.Enqueue(
                () =>
                {
                    this.PhotonClient.Disconnect();
                    this.StopService();
                    this.fiber.Dispose();
                });
        }

        #endregion

        #region IPhotonPeerListener

        /// <summary>
        ///   The debug return.
        /// </summary>
        /// <param name = "debugLevel">
        ///   The debug Level.
        /// </param>
        /// <param name = "debug">
        ///   The debug message.
        /// </param>
        public void DebugReturn(DebugLevel debugLevel, string debug)
        {
            switch (debugLevel)
            {
                case DebugLevel.ALL:
                    if (log.IsDebugEnabled)
                    {
                        log.Debug("DebugReturn: " + debug);
                    }

                    break;

                case DebugLevel.INFO:
                    log.Info("DebugReturn(INFO): " + debug);
                    break;

                case DebugLevel.WARNING:
                    log.Warn("DebugReturn(WARN): " + debug);
                    break;

                case DebugLevel.ERROR:
                    log.Error("DebugReturn(ERROR): " + debug);
                    break;
            }
        }

        public void OnEvent(EventData @event)
        {
            var eventData = new EventData { Code = @event.Code, Parameters = @event.Parameters };
            if (log.IsDebugEnabled)
            {
                log.Debug("EventReceived");
                //LogHelper.WriteDictionaryContent(eventData.Parameters, 0);
            }

            this.EventQueue.Enqueue(eventData);

            OnEventReceived(this, eventData);
        }

        public void OnOperationResponse(OperationResponse operationResponse)
        {
            var response = new OperationResponse
            {
                OperationCode = operationResponse.OperationCode,
                Parameters = operationResponse.Parameters,
                ReturnCode = operationResponse.ReturnCode,
                DebugMessage = operationResponse.DebugMessage,
            };

            if (log.IsDebugEnabled)
            {
                LogOperationResponse(response);
            }

            this.OperationResponseQueue.Enqueue(response);

            OnResponseReceived(this, response);
        }

        public void OnStatusChanged(StatusCode returnCode)
        {
            switch (returnCode)
            {
                case StatusCode.Connect:
                    {
                        this.autoResetEventInit.Set();
                        OnConnected(this);
                        break;
                    }

                case StatusCode.Disconnect:
                    {
                        OnDisonnected(this);
                        break;
                    }

                case StatusCode.DisconnectByServerLogic:
                case StatusCode.DisconnectByServer:
                case StatusCode.DisconnectByServerUserLimit:
                    {
                        log.Warn(returnCode);
                        OnDisonnected(this);
                        break;
                    }

                case StatusCode.QueueOutgoingReliableWarning:
                case StatusCode.QueueIncomingReliableWarning:
                    {
                        break;
                    }

                default:
                    {
                        log.Warn(returnCode);
                        break;
                    }
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        ///   The log operation response.
        /// </summary>
        /// <param name = "response">
        ///   The response.
        /// </param>
        private static void LogOperationResponse(OperationResponse response)
        {
            foreach (KeyValuePair<byte, object> item in response.Parameters)
            {
                log.DebugFormat(string.Format("{0}({1}): {2}", item.Key, item.Key, item.Value));
            }
        }

        /// <summary>
        ///   The on connected.
        /// </summary>
        /// <param name = "client">
        ///   The client.
        /// </param>
        private static void OnConnected(TestClient client)
        {
            Action<TestClient> connected = Connected;
            if (connected != null)
            {
                connected(client);
            }
        }

        /// <summary>
        ///   invokes <see cref = "Disconnected" />.
        /// </summary>
        /// <param name = "client">
        ///   The client.
        /// </param>
        private static void OnDisonnected(TestClient client)
        {
            Action<TestClient> handler = Disonnected;
            if (handler != null)
            {
                handler(client);
            }
        }

        /// <summary>
        ///   The on event received.
        /// </summary>
        /// <param name = "client">
        ///   The client.
        /// </param>
        /// <param name = "data">
        ///   The data.
        /// </param>
        private static void OnEventReceived(TestClient client, EventData data)
        {
            Action<TestClient, EventData> received = EventReceived;
            if (received != null)
            {
                received(client, data);
            }
        }

        /// <summary>
        ///   The on response received.
        /// </summary>
        /// <param name = "client">
        ///   The client.
        /// </param>
        /// <param name = "response">
        ///   The response.
        /// </param>
        private static void OnResponseReceived(TestClient client, OperationResponse response)
        {
            Action<TestClient, OperationResponse> received = ResponseReceived;
            if (received != null)
            {
                received(client, response);
            }
        }

        /// <summary>
        ///   The service.
        /// </summary>
        private void Service()
        {
            if (Interlocked.Read(ref this.service) == 1)
            {
                this.PhotonClient.Service();
                this.fiber.Schedule(this.Service, 50);
            }
        }

        /// <summary>
        ///   The start service.
        /// </summary>
        private void StartService()
        {
            Interlocked.Exchange(ref this.service, 1);
            this.fiber.Enqueue(this.Service);
        }

        /// <summary>
        ///   The stop service.
        /// </summary>
        private void StopService()
        {
            Interlocked.Exchange(ref this.service, 0);
        }

        #endregion
    }
}