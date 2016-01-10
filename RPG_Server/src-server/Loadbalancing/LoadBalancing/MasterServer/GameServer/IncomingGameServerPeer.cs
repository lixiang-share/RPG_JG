// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IncomingGameServerPeer.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the IncomingGameServerPeer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.MasterServer.GameServer
{
    #region using directives

    using System;
    using System.Collections;
    using System.Net;

    using ExitGames.Logging;

    using Photon.LoadBalancing.Common;
    using Photon.LoadBalancing.LoadShedding;
    using Photon.LoadBalancing.Operations;
    using Photon.LoadBalancing.ServerToServer.Events;
    using Photon.LoadBalancing.ServerToServer.Operations;
    using Photon.SocketServer;
    using Photon.SocketServer.ServerToServer;

    using OperationCode = Photon.LoadBalancing.ServerToServer.Operations.OperationCode;

    #endregion

    public class IncomingGameServerPeer : ServerPeerBase
    {
        #region Constants and Fields

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly MasterApplication application;

        #endregion

        #region Constructors and Destructors

        public IncomingGameServerPeer(InitRequest initRequest, MasterApplication application)
            : base(initRequest.Protocol, initRequest.PhotonPeer)
        {
            this.application = application;
            log.InfoFormat("game server connection from {0}:{1} established (id={2})", this.RemoteIP, this.RemotePort, this.ConnectionId);
        }

        #endregion

        #region Properties

        public string Key { get; protected set; }
        
        public Guid? ServerId { get; protected set; }

        public string TcpAddress { get; protected set; }

        public string UdpAddress { get; protected set; }

        public string WebSocketAddress { get; protected set; }

        public FeedbackLevel LoadLevel { get; private set; }

        public ServerState State { get; private set; }

        public int PeerCount { get; private set; }

        #endregion

        #region Public Methods

        public void RemoveGameServerPeerOnMaster()
        {
            if (this.ServerId.HasValue)
            {
                this.application.GameServers.OnDisconnect(this);
                this.application.LoadBalancer.TryRemoveServer(this);
                this.application.RemoveGameServerFromLobby(this); 
            }
        }

        public override string ToString()
        {
            if (this.ServerId.HasValue)
            {
                return string.Format("GameServer({2}) at {0}/{1}", this.TcpAddress, this.UdpAddress, this.ServerId);
            }

            return base.ToString();
        }

        #endregion

        #region Methods

        protected virtual Hashtable GetAuthlist()
        {
            return null;
        }

        protected virtual byte[] SharedKey
        {
            get { return null; }
        }

        protected virtual OperationResponse HandleRegisterGameServerRequest(OperationRequest request)
        {
            var registerRequest = new RegisterGameServer(this.Protocol, request);

            if (registerRequest.IsValid == false)
            {
                string msg = registerRequest.GetErrorMessage();
                log.ErrorFormat("RegisterGameServer contract error: {0}", msg);

                return new OperationResponse(request.OperationCode) { DebugMessage = msg, ReturnCode = (short)ErrorCode.OperationInvalid };
            }

            IPAddress masterAddress = this.application.GetInternalMasterNodeIpAddress();
            var contract = new RegisterGameServerResponse { InternalAddress = masterAddress.GetAddressBytes() };

            // is master
            if (this.application.IsMaster)
            {
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat(
                        "Received register request: Address={0}, UdpPort={1}, TcpPort={2}, WebSocketPort={3}, State={4}",
                        registerRequest.GameServerAddress,
                        registerRequest.UdpPort,
                        registerRequest.TcpPort,
                        registerRequest.WebSocketPort,
                        (ServerState)registerRequest.ServerState);
                }

                if (registerRequest.UdpPort.HasValue)
                {
                    this.UdpAddress = registerRequest.GameServerAddress + ":" + registerRequest.UdpPort;
                }

                if (registerRequest.TcpPort.HasValue)
                {
                    this.TcpAddress = registerRequest.GameServerAddress + ":" + registerRequest.TcpPort;
                }

                if (registerRequest.WebSocketPort.HasValue && registerRequest.WebSocketPort != 0)
                {
                    this.WebSocketAddress = registerRequest.GameServerAddress + ":" + registerRequest.WebSocketPort;
                }

                this.ServerId = new Guid(registerRequest.ServerId);
                this.State = (ServerState)registerRequest.ServerState;

                this.Key = string.Format("{0}-{1}-{2}", registerRequest.GameServerAddress, registerRequest.UdpPort, registerRequest.TcpPort);

                this.application.GameServers.OnConnect(this);

                if (this.State == ServerState.Normal)
                {
                    this.application.LoadBalancer.TryAddServer(this, 0);
                }

                contract.AuthList = this.GetAuthlist();
                contract.SharedKey = this.SharedKey;

                return new OperationResponse(request.OperationCode, contract);
            }

            return new OperationResponse(request.OperationCode, contract) { ReturnCode = (short)ErrorCode.RedirectRepeat, DebugMessage = "RedirectRepeat" };
        }

        protected virtual void HandleRemoveGameState(IEventData eventData)
        {
            var removeEvent = new RemoveGameEvent(this.Protocol, eventData);
            if (removeEvent.IsValid == false)
            {
                string msg = removeEvent.GetErrorMessage();
                log.ErrorFormat("RemoveGame contract error: {0}", msg);
                return;
            }

            this.application.DefaultApplication.OnGameRemovedOnGameServer(removeEvent.GameId);
        }

        protected virtual void HandleUpdateGameServerEvent(IEventData eventData)
        {
            var updateGameServer = new UpdateServerEvent(this.Protocol, eventData);
            if (updateGameServer.IsValid == false)
            {
                string msg = updateGameServer.GetErrorMessage();
                log.ErrorFormat("UpdateServer contract error: {0}", msg);
                return;
            }

            var previuosLoadLevel = this.LoadLevel;

            this.LoadLevel = (FeedbackLevel)updateGameServer.LoadIndex;
            this.PeerCount = updateGameServer.PeerCount;

            if ((ServerState)updateGameServer.State != this.State)
            {
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("GameServer state changed for {0}: old={1}, new={2} ", this.TcpAddress, this.State, (ServerState)updateGameServer.State);
                }

                this.State = (ServerState)updateGameServer.State;
                if (this.State == ServerState.Normal)
                {
                    if (this.application.LoadBalancer.TryAddServer(this, this.LoadLevel) == false)
                    {
                        log.WarnFormat("Failed to add game server to load balancer: serverId={0}", this.ServerId);
                    }
                }
                else if (this.State == ServerState.Offline)
                {
                    ////this.RemoveGameServerPeerOnMaster();
                }
                else
                {
                    this.application.LoadBalancer.TryRemoveServer(this);
                }
            }
            else if (previuosLoadLevel != this.LoadLevel)
            {
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("UpdateGameServer - from LoadLevel {0} to {1}, PeerCount {2}", previuosLoadLevel, this.LoadLevel, this.PeerCount);
                }
                
                if (!this.application.LoadBalancer.TryUpdateServer(this, this.LoadLevel))
                {
                    log.WarnFormat("Failed to update game server state for {0}", this.TcpAddress);
                }
            } 
        }

        protected virtual void HandleUpdateGameState(IEventData eventData)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("HandleUpdateGameState");
            }

            var updateEvent = new UpdateGameEvent(this.Protocol, eventData);
            if (updateEvent.IsValid == false)
            {
                string msg = updateEvent.GetErrorMessage();
                log.ErrorFormat("UpdateGame contract error: {0}", msg);
                return;
            }

            this.application.DefaultApplication.OnGameUpdateOnGameServer(updateEvent, this);
        }

        private void HandleUpdateAppStatsEvent(IEventData eventData)
        {
            if (MasterApplication.AppStats != null)
            {
                var updateAppStatsEvent = new UpdateAppStatsEvent(this.Protocol, eventData);
                MasterApplication.AppStats.UpdateGameServerStats(this, updateAppStatsEvent.PlayerCount, updateAppStatsEvent.GameCount);
            }
        }

        protected override void OnDisconnect(PhotonHostRuntimeInterfaces.DisconnectReason reasonCode, string reasonDetail)
        {
            if (log.IsInfoEnabled)
            {
                string serverId = this.ServerId.HasValue ? this.ServerId.ToString() : "{null}";
                log.InfoFormat("OnDisconnect: game server connection closed (connectionId={0}, serverId={1}, reason={2})", this.ConnectionId, serverId, reasonCode);
            }

            this.RemoveGameServerPeerOnMaster();
        }

        protected override void OnEvent(IEventData eventData, SendParameters sendParameters)
        {
            try
            {
                if (!this.ServerId.HasValue)
                {
                    log.Warn("received game server event but server is not registered");
                    return;
                }

                switch ((ServerEventCode)eventData.Code)
                {
                    default:
                        if (log.IsDebugEnabled)
                        {
                            log.DebugFormat("Received unknown event code {0}", eventData.Code);
                        }

                        break;

                    case ServerEventCode.UpdateServer:
                        this.HandleUpdateGameServerEvent(eventData);
                        break;

                    case ServerEventCode.UpdateGameState:
                        this.HandleUpdateGameState(eventData);
                        break;

                    case ServerEventCode.RemoveGameState:
                        this.HandleRemoveGameState(eventData);
                        break;

                    case ServerEventCode.UpdateAppStats:
                        this.HandleUpdateAppStatsEvent(eventData);
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        protected override void OnOperationRequest(OperationRequest request, SendParameters sendParameters)
        {
            try
            {
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("OnOperationRequest: pid={0}, op={1}", this.ConnectionId, request.OperationCode);
                }

                OperationResponse response;

                switch ((OperationCode)request.OperationCode)
                {
                    default:
                        response = new OperationResponse(request.OperationCode) { ReturnCode = -1, DebugMessage = "Unknown operation code" };
                        break;

                    case OperationCode.RegisterGameServer:
                        {
                            response = this.ServerId.HasValue
                                           ? new OperationResponse(request.OperationCode) { ReturnCode = -1, DebugMessage = "already registered" }
                                           : this.HandleRegisterGameServerRequest(request);
                            break;
                        }
                }

                this.SendOperationResponse(response, sendParameters);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        protected override void OnOperationResponse(OperationResponse operationResponse, SendParameters sendParameters)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}