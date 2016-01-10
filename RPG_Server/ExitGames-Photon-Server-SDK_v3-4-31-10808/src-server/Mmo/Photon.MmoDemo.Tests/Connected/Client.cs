// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Client.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The client.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Tests.Connected
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    using ExitGames.Client.Photon;
    using ExitGames.Concurrency.Fibers;

    using ExitGames.Logging;

    using Photon.MmoDemo.Common;

    /// <summary>
    /// The client.
    /// </summary>
    public class Client : IDisposable
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The connected.
        /// </summary>
        private readonly AutoResetEvent connectResetEvent;

        /// <summary>
        /// The fiber.
        /// </summary>
        private readonly IFiber fiber;

        /// <summary>
        /// The network peer.
        /// </summary>
        private readonly PhotonPeer peer;

        /// <summary>
        /// The peer listener.
        /// </summary>
        private readonly PeerListener peerListener;

        /// <summary>
        /// The reset event.
        /// </summary>
        private readonly AutoResetEvent resetEvent = new AutoResetEvent(false);

        /// <summary>
        /// The stop watch.
        /// </summary>
        private readonly Stopwatch stopWatch = new Stopwatch();

        /// <summary>
        /// The username.
        /// </summary>
        private readonly string username;

        /// <summary>
        /// The eventsReceived time.
        /// </summary>
        private static long eventReceiveTimeFast;

        /// <summary>
        /// The eventsReceived time max.
        /// </summary>
        private static long eventReceiveTimeMax;

        /// <summary>
        /// The eventsReceived time middle.
        /// </summary>
        private static long eventReceiveTimeMiddle;

        /// <summary>
        /// The eventsReceived time slow.
        /// </summary>
        private static long eventReceiveTimeSlow;

        /// <summary>
        /// The eventsReceived.
        /// </summary>
        private static long eventsReceivedFast;

        /// <summary>
        /// The eventsReceived middle.
        /// </summary>
        private static long eventsReceivedMiddle;

        /// <summary>
        /// The eventsReceived sent.
        /// </summary>
        private static long eventsReceivedSent;

        /// <summary>
        /// The eventsReceived slow.
        /// </summary>
        private static long eventsReceivedSlow;

        /// <summary>
        /// The exceptions.
        /// </summary>
        private static long exceptions;

        /// <summary>
        /// The operations sent.
        /// </summary>
        private static long operationsSent;

        /// <summary>
        /// The connected.
        /// </summary>
        private bool connected;

        /// <summary>
        /// The received event.
        /// </summary>
        private EventData receivedEvent;

        /// <summary>
        /// The received response.
        /// </summary>
        private OperationResponse receivedResponse;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        public Client(string username)
        {
            this.username = username;
            this.fiber = new PoolFiber();
            this.fiber.Start();
            this.peerListener = new PeerListener(username, this.OnConnectCallback);
            this.peer = new PhotonPeer(this.peerListener, Settings.UseTcp) { ChannelCount = 3 };

            this.connectResetEvent = new AutoResetEvent(false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="name">
        /// The client's name.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        public Client(string name, float[] position)
            : this(name)
        {
            this.Position = position;
        }

        /// <summary>
        /// Gets EventsReceived.
        /// </summary>
        public static long EventsReceivedFast
        {
            get
            {
                return Interlocked.Read(ref eventsReceivedFast);
            }
        }

        /// <summary>
        /// Gets EventsReceivedMiddle.
        /// </summary>
        public static long EventsReceivedMiddle
        {
            get
            {
                return Interlocked.Read(ref eventsReceivedMiddle);
            }
        }

        /// <summary>
        /// Gets EventsReceivedSlow.
        /// </summary>
        public static long EventsReceivedSlow
        {
            get
            {
                return Interlocked.Read(ref eventsReceivedSlow);
            }
        }

        /// <summary>
        /// Gets EventsReceivedTimeMax.
        /// </summary>
        public static long EventsReceivedTimeMax
        {
            get
            {
                return Interlocked.Read(ref eventReceiveTimeMax);
            }
        }

        /// <summary>
        /// Gets EventsReceivedTime.
        /// </summary>
        public static long EventsReceivedTimeTotalFast
        {
            get
            {
                return Interlocked.Read(ref eventReceiveTimeFast);
            }
        }

        /// <summary>
        /// Gets EventsReceivedTimeTotalMiddle.
        /// </summary>
        public static long EventsReceivedTimeTotalMiddle
        {
            get
            {
                return Interlocked.Read(ref eventReceiveTimeMiddle);
            }
        }

        /// <summary>
        /// Gets EventsReceivedTimeTotalSlow.
        /// </summary>
        public static long EventsReceivedTimeTotalSlow
        {
            get
            {
                return Interlocked.Read(ref eventReceiveTimeSlow);
            }
        }

        /// <summary>
        /// Gets Exceptions.
        /// </summary>
        public static long Exceptions
        {
            get
            {
                return Interlocked.Read(ref exceptions);
            }
        }

        /// <summary>
        /// Gets OperationsSent.
        /// </summary>
        public static long OperationsSent
        {
            get
            {
                return Interlocked.Read(ref operationsSent);
            }
        }

        /// <summary>
        /// Gets OperationFiber.
        /// </summary>
        public IFiber OperationFiber
        {
            get
            {
                return this.fiber;
            }
        }

        /// <summary>
        /// Gets or sets Position.
        /// </summary>
        public float[] Position { get; set; }

        /// <summary>
        /// Gets Username.
        /// </summary>
        public string Username
        {
            get
            {
                return this.username;
            }
        }

        /// <summary>
        /// The reset stats.
        /// </summary>
        public static void ResetStats()
        {
            Interlocked.Exchange(ref exceptions, 0);
            Interlocked.Exchange(ref eventsReceivedSent, 0);
            Interlocked.Exchange(ref eventReceiveTimeFast, 0);
            Interlocked.Exchange(ref eventReceiveTimeMiddle, 0);
            Interlocked.Exchange(ref eventReceiveTimeSlow, 0);
            Interlocked.Exchange(ref eventReceiveTimeMax, 0);
            Interlocked.Exchange(ref eventsReceivedFast, 0);
            Interlocked.Exchange(ref eventsReceivedMiddle, 0);
            Interlocked.Exchange(ref eventsReceivedSlow, 0);
            Interlocked.Exchange(ref operationsSent, 0);
            PeerListener.ResetStats();
        }

        /// <summary>
        /// The connect.
        /// </summary>
        public void BeginConnect()
        {
            this.peer.Connect(Settings.ServerAddress, Settings.ApplicationId);
            this.fiber.Enqueue(this.WaitForConnect);
        }

        /// <summary>
        /// The disconnect.
        /// </summary>
        public void BeginDisconnect()
        {
            this.fiber.Enqueue(this.DoDisconnect);
        }

        /// <summary>
        /// The begin receive event.
        /// </summary>
        /// <param name="eventCode">
        /// The event code.
        /// </param>
        /// <param name="checkAction">
        /// The check action.
        /// </param>
        /// <param name="delay">
        /// The delay.
        /// </param>
        public void BeginReceiveEvent(EventCode eventCode, Func<EventData, bool> checkAction, int delay)
        {
            this.stopWatch.Reset();
            this.fiber.Schedule(
                () =>
                    {
                        this.stopWatch.Start();
                        this.ReceiveEvent(eventCode, checkAction);
                    }, 
                delay);
        }

        /// <summary>
        /// The begin receive response.
        /// </summary>
        /// <param name="delay">
        /// The delay.
        /// </param>
        public void BeginReceiveResponse(int delay)
        {
            this.stopWatch.Reset();
            this.fiber.Schedule(
                () =>
                    {
                        this.stopWatch.Start();
                        this.ReceiveResponse();
                    }, 
                delay);
        }

        /// <summary>
        /// The connect.
        /// </summary>
        /// <returns>
        /// true if connected.
        /// </returns>
        public bool Connect()
        {
            this.BeginConnect();
            return this.EndConnect();
        }

        /// <summary>
        /// The disconnect.
        /// </summary>
        /// <returns>
        /// true if disconnected.
        /// </returns>
        public bool Disconnect()
        {
            this.BeginDisconnect();
            return this.EndDisconnect();
        }

        /// <summary>
        /// The end connect.
        /// </summary>
        /// <returns>
        /// true if connected.
        /// </returns>
        public bool EndConnect()
        {
            if (this.connectResetEvent.WaitOne(Settings.ConnectTimeoutMilliseconds))
            {
                return this.connected;
            }

            return false;
        }

        /// <summary>
        /// The end disconnect.
        /// </summary>
        /// <returns>
        /// true if disconnected.
        /// </returns>
        public bool EndDisconnect()
        {
            return this.connectResetEvent.WaitOne(Settings.ConnectTimeoutMilliseconds);
        }

        /// <summary>
        /// The end task.
        /// </summary>
        /// <param name="timeoutMilliseconds">
        /// The timeout milliseconds.
        /// </param>
        /// <param name="data">
        /// The received data.
        /// </param>
        /// <returns>
        /// true if task has ended.
        /// </returns>
        public bool EndReceiveEvent(int timeoutMilliseconds, out EventData data)
        {
            if (this.resetEvent.WaitOne(timeoutMilliseconds))
            {
                data = this.receivedEvent;
                return true;
            }

            data = null;
            return false;
        }

        /// <summary>
        /// The end receive response.
        /// </summary>
        /// <param name="timeoutMilliseconds">
        /// The timeout milliseconds.
        /// </param>
        /// <param name="data">
        /// The response data.
        /// </param>
        /// <returns>
        /// true if response was received.
        /// </returns>
        public bool EndReceiveResponse(int timeoutMilliseconds, out OperationResponse data)
        {
            if (this.resetEvent.WaitOne(timeoutMilliseconds))
            {
                data = this.receivedResponse;
                return true;
            }

            data = null;
            return false;
        }

        /// <summary>
        /// The send operation.
        /// </summary>
        /// <param name="operationCode">
        /// The operation EventCode.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="reliable">
        /// The reliable.
        /// </param>
        public void SendOperation(byte operationCode, Dictionary<byte, object> parameter, bool reliable)
        {
            this.fiber.Enqueue(() => this.DoSendOperation(operationCode, parameter, reliable));
        }

        #region Implemented Interfaces

        #region IDisposable

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.Disconnect();
        }

        #endregion

        #endregion

        /// <summary>
        /// The handle exception.
        /// </summary>
        /// <param name="e">
        /// The exception.
        /// </param>
        internal void HandleException(Exception e)
        {
            Interlocked.Increment(ref exceptions);
            log.Error(e);
        }

        /// <summary>
        /// The do disconnect.
        /// </summary>
        private void DoDisconnect()
        {
            try
            {
                if (this.connected)
                {
                    this.peer.Disconnect();
                    this.connected = false;
                }

                this.connectResetEvent.Set();
            }
            catch (Exception e)
            {
                log.Error(e);
                Interlocked.Increment(ref exceptions);
            }
        }

        /// <summary>
        /// The do send operation.
        /// </summary>
        /// <param name="operationCode">
        /// The operation code.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="reliable">
        /// The reliable.
        /// </param>
        private void DoSendOperation(byte operationCode, Dictionary<byte, object> parameter, bool reliable)
        {
            try
            {
                Interlocked.Increment(ref operationsSent);
                this.peer.OpCustom(operationCode, parameter, reliable);
                this.peer.Service();
            }
            catch (Exception e)
            {
                log.Error(e);
                Interlocked.Increment(ref exceptions);
            }
        }

        /// <summary>
        /// The on connect.
        /// </summary>
        /// <param name="obj">
        /// The connected peer listener.
        /// </param>
        /// <param name="success">
        /// true if connected.
        /// </param>
        private void OnConnectCallback(PeerListener obj, bool success)
        {
            this.connected = success;
            this.connectResetEvent.Set();
        }

        /// <summary>
        /// The on event received.
        /// </summary>
        private void OnEventReceived()
        {
            this.stopWatch.Stop();
            Stopwatch t = this.stopWatch;
            if (t.ElapsedMilliseconds < 50)
            {
                Interlocked.Increment(ref eventsReceivedFast);
                Interlocked.Add(ref eventReceiveTimeFast, t.ElapsedMilliseconds);
            }
            else if (t.ElapsedMilliseconds < 200)
            {
                Interlocked.Increment(ref eventsReceivedMiddle);
                Interlocked.Add(ref eventReceiveTimeMiddle, t.ElapsedMilliseconds);
            }
            else
            {
                Interlocked.Increment(ref eventsReceivedSlow);
                Interlocked.Add(ref eventReceiveTimeSlow, t.ElapsedMilliseconds);
            }

            if (t.ElapsedMilliseconds > EventsReceivedTimeMax)
            {
                Interlocked.Exchange(ref eventReceiveTimeMax, t.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// The on response received.
        /// </summary>
        private void OnResponseReceived()
        {
            this.stopWatch.Stop();
            Stopwatch t = this.stopWatch;
            if (t.ElapsedMilliseconds < 50)
            {
                Interlocked.Increment(ref eventsReceivedFast);
                Interlocked.Add(ref eventReceiveTimeFast, t.ElapsedMilliseconds);
            }
            else if (t.ElapsedMilliseconds < 200)
            {
                Interlocked.Increment(ref eventsReceivedMiddle);
                Interlocked.Add(ref eventReceiveTimeMiddle, t.ElapsedMilliseconds);
            }
            else
            {
                Interlocked.Increment(ref eventsReceivedSlow);
                Interlocked.Add(ref eventReceiveTimeSlow, t.ElapsedMilliseconds);
            }

            if (t.ElapsedMilliseconds > EventsReceivedTimeMax)
            {
                Interlocked.Exchange(ref eventReceiveTimeMax, t.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// The receive chat event.
        /// </summary>
        /// <param name="eventCode">
        /// The event EventCode.
        /// </param>
        /// <param name="checkAction">
        /// The check Action.
        /// </param>
        private void ReceiveEvent(EventCode eventCode, Func<EventData, bool> checkAction)
        {
            try
            {
                this.peer.Service();

                while (this.peerListener.EventList.Count > 0)
                {
                    var ev = this.peerListener.EventList[0];
                    this.peerListener.EventList.RemoveAt(0);
                    if ((byte)ev.Code == (byte)eventCode && checkAction(ev))
                    {
                        this.receivedEvent = ev;
                        this.OnEventReceived();
                        this.resetEvent.Set();
                        return;
                    }
                }

                if (this.stopWatch.ElapsedMilliseconds > Settings.WaitTime)
                {
                    Interlocked.Increment(ref exceptions);
                    log.ErrorFormat("client {0} did not receive event {2} in time. {1}ms waited", this.Username, this.stopWatch.ElapsedMilliseconds, eventCode);
                }
                else
                {
                    this.fiber.Schedule(() => this.ReceiveEvent(eventCode, checkAction), 10);
                }
            }
            catch (Exception e)
            {
                this.HandleException(e);
            }
        }

        /// <summary>
        /// The receive response.
        /// </summary>
        private void ReceiveResponse()
        {
            try
            {
                this.peer.Service();

                if (this.peerListener.ResponseList.Count > 0)
                {
                    var ev = this.peerListener.ResponseList[0];
                    this.peerListener.ResponseList.RemoveAt(0);

                    this.receivedResponse = ev;
                    this.OnResponseReceived();
                    this.resetEvent.Set();

                    if (this.peerListener.ResponseList.Count > 0)
                    {
                        log.WarnFormat("more responses in queue: {0}", this.peerListener.ResponseList.Count);
                    }

                    return;
                }

                if (this.stopWatch.ElapsedMilliseconds > Settings.WaitTime)
                {
                    Interlocked.Increment(ref exceptions);
                    log.ErrorFormat("client {0} did not receive response in time. {1}ms waited", this.Username, this.stopWatch.ElapsedMilliseconds);
                }
                else
                {
                    this.fiber.Schedule(this.ReceiveResponse, 10);
                }
            }
            catch (Exception e)
            {
                this.HandleException(e);
            }
        }

        /// <summary>
        /// The wait for connect.
        /// </summary>
        private void WaitForConnect()
        {
            try
            {
                if (this.connected == false)
                {
                    this.peer.Service();
                    this.fiber.Enqueue(this.WaitForConnect);
                }
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }
    }
}