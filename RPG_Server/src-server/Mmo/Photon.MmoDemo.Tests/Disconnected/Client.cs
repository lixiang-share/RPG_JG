// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Client.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The client.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Tests.Disconnected
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    using ExitGames.Logging;

    using Photon.MmoDemo.Common;
    using Photon.MmoDemo.Server;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    using Settings = Photon.MmoDemo.Tests.Settings;

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
        /// The network peer.
        /// </summary>
        private readonly Peer peer;

        /// <summary>
        /// the dummy peer
        /// </summary>
        private readonly DummyPeer dummyPeer;

        /// <summary>
        /// The protocol.
        /// </summary>
        private readonly IRpcProtocol protocol;

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
            this.protocol = Protocol.GpBinaryV162;
            this.dummyPeer = new DummyPeer(this.protocol, username);
            this.peer = new MmoPeer(this.protocol, this.dummyPeer);
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
        /// Gets EventsReceivedTotal.
        /// </summary>
        public static long EventsReceivedTotal
        {
            get
            {
                return DummyPeer.EventsReceived;
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
                return Interlocked.Read(ref eventsReceivedSent);
            }
        }

        /// <summary>
        /// Gets Peer.
        /// </summary>
        public Peer Peer
        {
            get
            {
                return this.peer;
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
            DummyPeer.ResetStats();
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
        public void BeginReceiveEvent(EventCode eventCode, Func<EventData, bool> checkAction)
        {
            this.stopWatch.Reset();

            this.peer.RequestFiber.Schedule(
                () =>
                    {
                        this.stopWatch.Start();
                        this.ReceiveEvent(eventCode, checkAction);
                    }, 
                10);
        }

        /// <summary>
        /// The begin receive response.
        /// </summary>
        public void BeginReceiveResponse()
        {
            this.stopWatch.Reset();

            this.peer.RequestFiber.Schedule(
                () =>
                    {
                        this.stopWatch.Start();
                        this.ReceiveResponse();
                    }, 
                10);
        }

        /// <summary>
        /// The disconnect.
        /// </summary>
        public void Disconnect()
        {
            PeerHelper.SimulateDisconnect(this.peer);
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
        /// The response.
        /// </param>
        /// <returns>
        /// true if received response.
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
        /// <param name="request">
        /// The request.
        /// </param>
        public void SendOperation(OperationRequest request)
        {
            Interlocked.Increment(ref eventsReceivedSent);
            PeerHelper.InvokeOnOperationRequest(this.peer, request, new SendParameters());
        }

        #region Implemented Interfaces

        #region IDisposable

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.peer.Dispose();
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
                while (this.dummyPeer.EventList.Count > 0)
                {
                    EventData ev = this.dummyPeer.EventList[0];
                    this.dummyPeer.EventList.RemoveAt(0);
                    if (ev.Code == (short)eventCode && checkAction(ev))
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
                    this.peer.RequestFiber.Schedule(() => this.ReceiveEvent(eventCode, checkAction), 10);
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
                if (this.dummyPeer.ResponseList.Count > 0)
                {
                    OperationResponse ev = this.dummyPeer.ResponseList[0];
                    this.dummyPeer.ResponseList.RemoveAt(0);

                    this.receivedResponse = ev;
                    this.OnEventReceived();
                    this.resetEvent.Set();

                    if (this.dummyPeer.ResponseList.Count > 0)
                    {
                        log.WarnFormat("more responses in queue: {0}", this.dummyPeer.ResponseList.Count);
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
                    this.peer.RequestFiber.Schedule(this.ReceiveResponse, 10);
                }
            }
            catch (Exception e)
            {
                this.HandleException(e);
            }
        }
    }
}