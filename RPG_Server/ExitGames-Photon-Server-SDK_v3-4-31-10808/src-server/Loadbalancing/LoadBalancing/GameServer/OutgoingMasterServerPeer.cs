// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OutgoingMasterServerPeer.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the OutgoingMasterServerPeer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.GameServer
{
    #region using directives

    using System;
    using System.Collections.Generic;
    using System.Net;

    using ExitGames.Logging;

    using Lite;
    using Lite.Messages;

    using Photon.LoadBalancing.Common;
    using Photon.LoadBalancing.LoadShedding;
    using Photon.LoadBalancing.Operations;
    using Photon.LoadBalancing.ServerToServer.Events;
    using Photon.LoadBalancing.ServerToServer.Operations;
    using Photon.SocketServer;
    using Photon.SocketServer.ServerToServer;

    using PhotonHostRuntimeInterfaces;

    using OperationCode = Photon.LoadBalancing.ServerToServer.Operations.OperationCode;

    #endregion

    public class OutgoingMasterServerPeer : ServerPeerBase
    {
        #region Constants and Fields

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly GameApplication application;

        private bool redirected;

        private IDisposable updateLoop;

        #endregion

        #region Constructors and Destructors

        public OutgoingMasterServerPeer(IRpcProtocol protocol, IPhotonPeer nativePeer, GameApplication application)
            : base(protocol, nativePeer)
        {
            this.application = application;
            log.InfoFormat("connection to master at {0}:{1} established (id={2}), serverId={3}", this.RemoteIP, this.RemotePort, this.ConnectionId, GameApplication.ServerId);
            this.RequestFiber.Enqueue(this.Register);
        }

        #endregion

        #region Properties

        protected bool IsRegistered { get; set; }

        #endregion

        #region Public Methods

        public void RemoveGameState(string gameId)
        {
            if (!this.IsRegistered)
            {
                return;
            }

            var parameter = new Dictionary<byte, object> { { (byte)ParameterCode.GameId, gameId }, };
            var eventData = new EventData { Code = (byte)ServerEventCode.RemoveGameState, Parameters = parameter };
            this.SendEvent(eventData, new SendParameters());
        }

        public virtual void UpdateAllGameStates()
        {
            if (!this.IsRegistered)
            {
                return; 
            }

            foreach (var gameId in GameCache.Instance.GetRoomNames())
            {
                Room room; 
                if (GameCache.Instance.TryGetRoomWithoutReference(gameId, out room))
                {
                    room.EnqueueMessage(new RoomMessage((byte)GameMessageCodes.ReinitializeGameStateOnMaster));
                }                
            }
        }

        public void UpdateServerState(FeedbackLevel workload, int peerCount, ServerState state)
        {
            if (!this.IsRegistered)
            {
                return;
            }

            var contract = new UpdateServerEvent { LoadIndex = (byte)workload, PeerCount = peerCount, State = (int)state };
            var eventData = new EventData((byte)ServerEventCode.UpdateServer, contract);
            this.SendEvent(eventData, new SendParameters());
        }

        public void UpdateServerState()
        {
            if (this.Connected == false)
            {
                return;
            }

            this.UpdateServerState(
                GameApplication.Instance.WorkloadController.FeedbackLevel,
                GameApplication.Instance.PeerCount,
                GameApplication.Instance.WorkloadController.ServerState);
        }

        #endregion

        #region Methods

        protected virtual void HandleRegisterGameServerResponse(OperationResponse operationResponse)
        {
            var contract = new RegisterGameServerResponse(this.Protocol, operationResponse);
            if (!contract.IsValid)
            {
                if (operationResponse.ReturnCode != (short)ErrorCode.Ok)
                {
                    log.ErrorFormat("RegisterGameServer returned with err {0}: {1}", operationResponse.ReturnCode, operationResponse.DebugMessage);
                }

                log.Error("RegisterGameServerResponse contract invalid: " + contract.GetErrorMessage());
                this.Disconnect();
                return;
            }

            switch (operationResponse.ReturnCode)
            {
                case (short)ErrorCode.Ok:
                    {
                        log.InfoFormat("Successfully registered at master server: serverId={0}", GameApplication.ServerId);
                        this.IsRegistered = true;
                        this.UpdateAllGameStates();
                        this.StartUpdateLoop();
                        break;
                    }

                case (short)ErrorCode.RedirectRepeat:
                    {
                        // TODO: decide whether to connect to internal or external address (config)
                        // use a new peer since we otherwise might get confused with callbacks like disconnect
                        var address = new IPAddress(contract.InternalAddress);
                        log.InfoFormat("Connected master server is not the leader; Reconnecting to master at IP {0}...", address);
                        this.Reconnect(address); // don't use proxy for direct connections

                        // enable for external address connections
                        //// var address = new IPAddress(contract.ExternalAddress);
                        //// log.InfoFormat("Connected master server is not the leader; Reconnecting to node {0} at IP {1}...", contract.MasterNode, address);
                        //// this.Reconnect(address, contract.MasterNode);
                        break;
                    }

                default:
                    {
                        log.WarnFormat("Failed to register at master: err={0}, msg={1}, serverid={2}", operationResponse.ReturnCode, operationResponse.DebugMessage, GameApplication.ServerId);
                        this.Disconnect();
                        break;
                    }
            }
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            this.IsRegistered = false;
            this.StopUpdateLoop();

            // if RegisterGameServerResponse tells us to connect somewhere else we don't need to reconnect here
            if (this.redirected)
            {
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("{0} disconnected from master server: reason={1}, detail={2}, serverId={3}", this.ConnectionId, reasonCode, reasonDetail, GameApplication.ServerId);
                }
            }
            else
            {
                log.InfoFormat("connection to master closed (id={0}, reason={1}, detail={2}), serverId={3}", this.ConnectionId, reasonCode, reasonDetail, GameApplication.ServerId);
                this.application.ReconnectToMaster();
            }
        }

        protected override void OnEvent(IEventData eventData, SendParameters sendParameters)
        {
        }

        protected override void OnOperationRequest(OperationRequest request, SendParameters sendParameters)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Received unknown operation code {0}", request.OperationCode);
            }

            var response = new OperationResponse { OperationCode = request.OperationCode, ReturnCode = -1, DebugMessage = "Unknown operation code" };
            this.SendOperationResponse(response, sendParameters);
        }
        
        protected override void OnOperationResponse(OperationResponse operationResponse, SendParameters sendParameters)
        {
            switch ((OperationCode)operationResponse.OperationCode)
            {
                default:
                    {
                        if (log.IsDebugEnabled)
                        {
                            log.DebugFormat("Received unknown operation code {0}", operationResponse.OperationCode);
                        }

                        break;
                    }

                case OperationCode.RegisterGameServer:
                    {
                        this.HandleRegisterGameServerResponse(operationResponse);
                        break;
                    }
            }
        }

        protected void Reconnect(IPAddress address)
        {
            this.redirected = true;

            log.InfoFormat("Reconnecting to master: serverId={0}", GameApplication.ServerId);

            GameApplication.Instance.ConnectToMaster(new IPEndPoint(address, this.RemotePort));
            this.Disconnect();
            this.Dispose();
        }

        protected virtual void Register()
        {
            var contract = new RegisterGameServer
                {
                    GameServerAddress = GameApplication.Instance.PublicIpAddress.ToString(),
                    
                    UdpPort = GameServerSettings.Default.RelayPortUdp == 0 ? this.application.GamingUdpPort : GameServerSettings.Default.RelayPortUdp + this.application.GetCurrentNodeId() - 1,
                    TcpPort = GameServerSettings.Default.RelayPortTcp == 0 ? this.application.GamingTcpPort : GameServerSettings.Default.RelayPortTcp + this.application.GetCurrentNodeId() - 1,
                    WebSocketPort = GameServerSettings.Default.RelayPortWebSocket == 0 ? this.application.GamingWebSocketPort : GameServerSettings.Default.RelayPortWebSocket + this.application.GetCurrentNodeId() - 1,                    
                    ServerId = GameApplication.ServerId.ToString(),
                    ServerState = (int)this.application.WorkloadController.ServerState
                };

            if (log.IsInfoEnabled)
            {
                log.InfoFormat(
                    "Registering game server with address {0}, TCP {1}, UDP {2}, WebSocket {3}, ServerID {4}",
                    contract.GameServerAddress,
                    contract.TcpPort,
                    contract.UdpPort,
                    contract.WebSocketPort,
                    contract.ServerId);
            }

            var request = new OperationRequest((byte)OperationCode.RegisterGameServer, contract);
            this.SendOperationRequest(request, new SendParameters());
        }

        protected void StartUpdateLoop()
        {
            if (this.updateLoop != null)
            {
                log.Error("Update Loop already started! Duplicate RegisterGameServer response?");
                this.updateLoop.Dispose();
            }

            this.updateLoop = this.RequestFiber.ScheduleOnInterval(this.UpdateServerState, 1000, 1000);
            GameApplication.Instance.WorkloadController.FeedbacklevelChanged += this.WorkloadController_OnFeedbacklevelChanged;
        }

        protected void StopUpdateLoop()
        {
            if (this.updateLoop != null)
            {
                this.updateLoop.Dispose();
                this.updateLoop = null;

                GameApplication.Instance.WorkloadController.FeedbacklevelChanged -= this.WorkloadController_OnFeedbacklevelChanged;
            }
        }

        private void WorkloadController_OnFeedbacklevelChanged(object sender, EventArgs e)
        {
            this.UpdateServerState();
        }

        #endregion
    }
}