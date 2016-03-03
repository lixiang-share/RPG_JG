// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyPeer.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The network peer dummy.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Tests.Disconnected
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using ExitGames.Logging;

    using Photon.MmoDemo.Common;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc.Protocols;

    using PhotonHostRuntimeInterfaces;

    /// <summary>
    ///   The network peer dummy.
    /// </summary>
    public class DummyPeer : IPhotonPeer
    {
        #region Constants and Fields

        /// <summary>
        ///   The logger.
        /// </summary>
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///   The connection id.
        /// </summary>
        private readonly int connectionId;

        /// <summary>
        ///   The event queue.
        /// </summary>
        private readonly List<EventData> eventList = new List<EventData>();

        /// <summary>
        ///   The protocol.
        /// </summary>
        private readonly IRpcProtocol protocol;

        /// <summary>
        ///   The response list.
        /// </summary>
        private readonly List<OperationResponse> responseList = new List<OperationResponse>();

        /// <summary>
        ///   The username.
        /// </summary>
        private readonly string username;

        /// <summary>
        ///   The connection ids.
        /// </summary>
        private static int connectionIds;

        /// <summary>
        ///   The events received.
        /// </summary>
        private static long eventsReceived;

        /// <summary>
        ///   The response received.
        /// </summary>
        private static long responseReceived;

        /// <summary>
        /// The user data object.
        /// </summary>
        private object userData;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DummyPeer" /> class.
        /// </summary>
        /// <param name = "protocol">
        ///   The protocol.
        /// </param>
        /// <param name = "username">
        ///   The username.
        /// </param>
        public DummyPeer(IRpcProtocol protocol, string username)
        {
            this.protocol = protocol;
            this.username = username;
            this.connectionId = Interlocked.Increment(ref connectionIds);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DummyPeer" /> class.
        /// </summary>
        /// <param name = "username">
        ///   The username.
        /// </param>
        public DummyPeer(string username)
            : this(SocketServer.Protocol.GpBinaryV162, username)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets EventsReceived.
        /// </summary>
        public static long EventsReceived
        {
            get
            {
                return Interlocked.Read(ref eventsReceived);
            }
        }

        /// <summary>
        ///   Gets EventList.
        /// </summary>
        public List<EventData> EventList
        {
            get
            {
                return this.eventList;
            }
        }

        /// <summary>
        ///   Gets Protocol.
        /// </summary>
        public IRpcProtocol Protocol
        {
            get
            {
                return this.protocol;
            }
        }

        /// <summary>
        ///   Gets ResponseList.
        /// </summary>
        public List<OperationResponse> ResponseList
        {
            get
            {
                return this.responseList;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///   The reset stats.
        /// </summary>
        public static void ResetStats()
        {
            Interlocked.Exchange(ref eventsReceived, 0);
        }

        /// <summary>
        ///   The on disconnect.
        /// </summary>
        public void OnDisconnect()
        {
        }

        #endregion

        #region Implemented Interfaces

        #region IPhotonPeer

        IntPtr IPhotonPeer._InternalGetPeerInfo(int why)
        {
            return IntPtr.Zero;
        }

        public void SetDebugString(string debugString)
        {
        }

        public void GetStats(out int rtt, out int rttVariance, out int numFailures)
        {
            rtt = 0;
            rttVariance = 0;
            numFailures = 0;
        }

        SendResults IPhotonPeer._InternalBroadcastSend(byte[] data, MessageReliablity reliability, byte channelId, MessageContentType messageContentType)
        {
            return new SendResults();
        }

        public int GetLastTouched()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   The disconnect client.
        /// </summary>
        public void DisconnectClient()
        {
        }

        public void AbortClient()
        {
        }

        /// <summary>
        ///   Called when outgoing messages are flushed.
        /// </summary>
        public void Flush()
        {
        }

        /// <summary>
        ///   The get connection id.
        /// </summary>
        /// <returns>
        ///   The connection id.
        /// </returns>
        public int GetConnectionID()
        {
            return this.connectionId;
        }

        /// <summary>
        ///   The get local port.
        /// </summary>
        /// <returns>
        ///   Always zero.
        /// </returns>
        public ushort GetLocalPort()
        {
            return 0;
        }

        /// <summary>
        ///   The get remote ip.
        /// </summary>
        /// <returns>
        ///   The localhost IP.
        /// </returns>
        public string GetRemoteIP()
        {
            return "127.0.0.1";
        }

        /// <summary>
        ///   The local remote ip.
        /// </summary>
        /// <returns>
        ///   The localhost IP.
        /// </returns>
        public string GetLocalIP()
        {
            return "127.0.0.1";
        }

        /// <summary>
        ///   Gets the remote port.
        /// </summary>
        /// <returns>
        ///   Always zero.
        /// </returns>
        public ushort GetRemotePort()
        {
            return 0;
        }

        /// <summary>
        /// Gets the user data object.
        /// </summary>
        /// <returns>
        /// The user data object.
        /// </returns>
        public object GetUserData()
        {
            return this.userData;
        }

        /// <summary>
        /// Gets the <see cref="ListenerType"/>.
        /// </summary>
        /// <returns>
        /// Value <see cref="ListenerType.TCPListener"/>.
        /// </returns>
        public ListenerType GetListenerType()
        {
            return ListenerType.TCPListener;
        }

        /// <summary>
        /// Gets the <see cref="PeerType"/>.
        /// </summary>
        /// <returns>
        /// Value <see cref="PeerType.TCPPeer"/>.
        /// </returns>
        public PeerType GetPeerType()
        {
            return PeerType.TCPPeer;
        }

        /// <summary>
        ///   Send data to the peer.
        /// </summary>
        /// <param name = "data">
        ///   The serialized data.
        /// </param>
        /// <param name = "reliability">
        ///   The reliability.
        /// </param>
        /// <param name = "channelId">
        ///   The channel id.
        /// </param>
        /// <returns>
        ///   Always <see cref = "SendResults.SentOk" />.
        /// </returns>
        public SendResults Send(byte[] data, MessageReliablity reliability, byte channelId, MessageContentType messageContentType)
        {
            RtsMessageHeader messageType;
            if (this.Protocol.TryParseMessageHeader(data, out messageType) == false)
            {
                throw new InvalidOperationException();
            }

            switch (messageType.MessageType)
            {
                case RtsMessageType.Event:
                    {
                        EventData eventData;
                        if (this.Protocol.TryParseEventData(data, out eventData))
                        {
                            Interlocked.Increment(ref eventsReceived);
                            this.eventList.Add(eventData);

                            if (log.IsDebugEnabled)
                            {
                                if (eventData.Parameters.ContainsKey((byte)ParameterCode.ItemId))
                                {
                                    log.DebugFormat(
                                        "{0} receives event, {1} total - code {2}, source {3}", 
                                        this.username, 
                                        this.eventList.Count, 
                                        (EventCode)eventData.Code,
                                        eventData.Parameters[(byte)ParameterCode.ItemId]);
                                }
                                else
                                {
                                    log.DebugFormat(
                                        "{0} receives event, {1} total - code {2}", 
                                        this.username, 
                                        this.eventList.Count,
                                        (EventCode)eventData.Code);
                                }
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }

                        break;
                    }

                case RtsMessageType.OperationResponse:
                    {
                        OperationResponse response;
                        if (this.Protocol.TryParseOperationResponse(data, out response))
                        {
                            Interlocked.Increment(ref responseReceived);
                            this.responseList.Add(response);

                            if (response.ReturnCode != (int)ReturnCode.Ok)
                            {
                                log.ErrorFormat(
                                    "ERR {0}, OP {1}, DBG {2}", (ReturnCode)response.ReturnCode, (OperationCode)response.OperationCode, response.DebugMessage);
                            }
                            else if (log.IsDebugEnabled)
                            {
                                if (response.Parameters.ContainsKey((byte)ParameterCode.ItemId))
                                {
                                    log.DebugFormat(
                                        "{0} receives response, {1} total - code {2}, source {3}", 
                                        this.username, 
                                        this.responseList.Count, 
                                        (OperationCode)response.OperationCode,
                                        response.Parameters[(byte)ParameterCode.ItemId]);
                                }
                                else
                                {
                                    log.DebugFormat(
                                        "{0} receives response, {1} total - code {2}", 
                                        this.username, 
                                        this.responseList.Count, 
                                        (OperationCode)response.OperationCode);
                                }
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }

                        break;
                    }
            }

            return SendResults.SentOk;
        }

        /// <summary>
        /// Sets the user data object.
        /// </summary>
        /// <param name="userDataObject">
        /// The user data object.
        /// </param>
        public void SetUserData(object userDataObject)
        {
            Interlocked.Exchange(ref this.userData, userDataObject);
        }

        #endregion

        #endregion
    }
}