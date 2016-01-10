// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MasterClientPeer.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the MasterClientPeer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.MasterServer
{
    #region using directives

    using System.Threading;

    using ExitGames.Logging;

    using Photon.LoadBalancing.MasterServer.Lobby;
    using Photon.LoadBalancing.Operations;
    using Photon.SocketServer;

    using AppLobby = Photon.LoadBalancing.MasterServer.Lobby.AppLobby;

    #endregion

    public class MasterClientPeer : PeerBase, ILobbyPeer
    {
        #region Constants and Fields

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private IGameListSubscibtion gameChannelSubscription;

        private GameApplication application;

        #endregion

        #region Constructors and Destructors

        public MasterClientPeer(InitRequest initRequest)
            : base(initRequest.Protocol, initRequest.PhotonPeer)
        {
            if (MasterApplication.AppStats != null)
            {
                MasterApplication.AppStats.IncrementMasterPeerCount();
                MasterApplication.AppStats.AddSubscriber(this);
            }
        }

        #endregion

        #region Properties

        public string UserId { get; set; }

        public virtual GameApplication Application
        {
            get
            {
                return this.application;
            }

            protected set
            {
                var oldApp = Interlocked.Exchange(ref this.application, value);
                if (oldApp != null)
                {
                    oldApp.OnClientDisconnected(this);
                }

                if (value != null)
                {
                    value.OnClientConnected(this);
                }
            }
        }

        protected AppLobby AppLobby { get; set; }
        
        public IGameListSubscibtion GameChannelSubscription
        {
            get
            {
                return this.gameChannelSubscription;
            }

            set
            {
                var oldsubscription = Interlocked.Exchange(ref this.gameChannelSubscription, value);
                if (oldsubscription != null)
                {
                    oldsubscription.Dispose();
                }
            }
        }

        #endregion

        #region Methods

        protected override void OnDisconnect(PhotonHostRuntimeInterfaces.DisconnectReason reasonCode, string reasonDetail)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Disconnect: pid={0}: reason={1}, detail={2}", this.ConnectionId, reasonCode, reasonDetail);
            }

            // remove peer from the lobby if he has joined one
            if (this.AppLobby != null)
            {
                this.AppLobby.RemovePeer(this);
                this.AppLobby = null;
            }

            // remove the peer from the application
            this.Application = null;

            // update application statistics
            if (MasterApplication.AppStats != null)
            {
                MasterApplication.AppStats.DecrementMasterPeerCount();
                MasterApplication.AppStats.RemoveSubscriber(this);
            }
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("OnOperationRequest: pid={0}, op={1}", this.ConnectionId, operationRequest.OperationCode);
            }

            OperationResponse response;
            switch ((OperationCode)operationRequest.OperationCode)
            {
                default:
                    response = new OperationResponse(operationRequest.OperationCode) { ReturnCode = (short)ErrorCode.OperationInvalid, DebugMessage = "Unknown operation code" };
                    break;

                case OperationCode.Authenticate:
                    response = this.HandleAuthenticate(operationRequest);
                    break;

                case OperationCode.JoinLobby:
                    response = this.HandleJoinLobby(operationRequest, sendParameters);
                    break;

                case OperationCode.LeaveLobby:
                    response = this.HandleLeaveLobby(operationRequest);
                    break;

                case OperationCode.CreateGame:
                    response = this.HandleCreateGame(operationRequest, sendParameters);
                    break;

                case OperationCode.JoinGame:
                    response = this.HandleJoinGame(operationRequest, sendParameters);
                    break;

                case OperationCode.JoinRandomGame:
                    response = this.HandleJoinRandomGame(operationRequest, sendParameters);
                    break;

                case OperationCode.FiendFriends:
                    response = this.HandleFiendFriends(operationRequest, sendParameters);
                    break;
            }

            if (response != null)
            {
                this.SendOperationResponse(response, sendParameters);
            }
        }

        private OperationResponse HandleAuthenticate(OperationRequest operationRequest)
        {
            OperationResponse response;

            var request = new AuthenticateRequest(this.Protocol, operationRequest);
            if (!OperationHelper.ValidateOperation(request, log, out response))
            {
                return response;
            }

            this.UserId = request.UserId;

            this.Application = ((MasterApplication) ApplicationBase.Instance).DefaultApplication;

            // publish operation response
            var responseObject = new AuthenticateResponse { QueuePosition = 0 };
            return new OperationResponse(operationRequest.OperationCode, responseObject);
        }

        public OperationResponse HandleJoinLobby(OperationRequest operationRequest, SendParameters sendParameters)
        {
            var joinLobbyRequest = new JoinLobbyRequest(this.Protocol, operationRequest);
            
            OperationResponse response;
            if (OperationHelper.ValidateOperation(joinLobbyRequest, log, out response) == false)
            {
                return response;
            }

            // remove peer from the currently joined lobby
            if (this.AppLobby != null)
            {
                this.AppLobby.RemovePeer(this);
                this.AppLobby = null;
            }

            AppLobby lobby;
            if (!this.Application.LobbyFactory.GetOrCreateAppLobby(joinLobbyRequest.LobbyName, (AppLobbyType)joinLobbyRequest.LobbyType , out lobby))
            {
                return new OperationResponse
                    {
                        OperationCode = operationRequest.OperationCode,
                        ReturnCode = (short)ErrorCode.OperationDenied,
                        DebugMessage = "Cannot create lobby"
                    };
            }

            this.AppLobby = lobby;
            this.AppLobby.JoinLobby(this, joinLobbyRequest, sendParameters);

            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Joined lobby: {0}, {1}", joinLobbyRequest.LobbyName, joinLobbyRequest.LobbyType);
            }

            return null;
        }

        public OperationResponse HandleLeaveLobby(OperationRequest operationRequest)
        {
            this.GameChannelSubscription = null;

            if (this.AppLobby == null)
            {
                return new OperationResponse { OperationCode = operationRequest.OperationCode, ReturnCode = 0, DebugMessage = "lobby not joined" };
            }

            this.AppLobby.RemovePeer(this);
            this.AppLobby = null;

            return new OperationResponse(operationRequest.OperationCode);
        }

        public OperationResponse HandleCreateGame(OperationRequest operationRequest, SendParameters sendParameters)
        {
            var createGameRequest = new CreateGameRequest(this.Protocol, operationRequest);

            OperationResponse response;
            if (OperationHelper.ValidateOperation(createGameRequest, log, out response) == false)
            {
                return response;
            }


            if (string.IsNullOrEmpty(createGameRequest.LobbyName) && this.AppLobby != null)
            {
                this.AppLobby.EnqueueOperation(this, operationRequest, sendParameters);
                return null;
            }

            AppLobby lobby;
            if (!this.Application.LobbyFactory.GetOrCreateAppLobby(createGameRequest.LobbyName, (AppLobbyType)createGameRequest.LobbyType , out lobby))
            {
                return new OperationResponse
                    {
                        OperationCode = operationRequest.OperationCode,
                        ReturnCode = (short)ErrorCode.OperationDenied,
                        DebugMessage = "Lobby does not exists"
                    };
            }

            lobby.EnqueueOperation(this, operationRequest, sendParameters);
            return null;
        }

        public OperationResponse HandleFiendFriends(OperationRequest operationRequest, SendParameters sendParameters)
        {
            // validate the operation request
            OperationResponse response;
            var operation = new FindFriendsRequest(this.Protocol, operationRequest);
            if (OperationHelper.ValidateOperation(operation, log, out response) == false)
            {
                return response;
            }

            // check if player online cache is available for the application
            var playerCache = this.Application.PlayerOnlineCache;
            if (playerCache == null)
            {
                return new OperationResponse((byte)OperationCode.FiendFriends)
                {
                    ReturnCode = (short)ErrorCode.OperationDenied,
                    DebugMessage = "Friend list not available"
                };
            }

            playerCache.FiendFriends(this, operation, sendParameters);
            return null;
        }

        public OperationResponse HandleJoinGame(OperationRequest operationRequest, SendParameters sendParameters)
        {
            var joinGameRequest = new JoinGameRequest(this.Protocol, operationRequest);

            OperationResponse response;
            if (OperationHelper.ValidateOperation(joinGameRequest, log, out response) == false)
            {
                return response;
            }

            GameState gameState;
            if (this.Application.TryGetGame(joinGameRequest.GameId, out gameState))
            {
                gameState.Lobby.EnqueueOperation(this, operationRequest, sendParameters);
                return null;
            }

            if (joinGameRequest.CreateIfNotExists == false)
            {
                return new OperationResponse { OperationCode = operationRequest.OperationCode, ReturnCode = (int)ErrorCode.GameIdNotExists, DebugMessage = "Game does not exist" };
            }

            AppLobby lobby;
            if (!this.Application.LobbyFactory.GetOrCreateAppLobby(joinGameRequest.LobbyName, (AppLobbyType)joinGameRequest.LobbyType, out lobby))
            {
                return new OperationResponse { OperationCode = operationRequest.OperationCode, ReturnCode = (int)ErrorCode.OperationDenied, DebugMessage = "Lobby does not exist" };
            }

            lobby.EnqueueOperation(this, operationRequest, sendParameters);
            return null;
        }

        public OperationResponse HandleJoinRandomGame(OperationRequest operationRequest, SendParameters sendParameters)
        {
            var joinRandomGameRequest = new JoinRandomGameRequest(this.Protocol, operationRequest);

            OperationResponse response;
            if (OperationHelper.ValidateOperation(joinRandomGameRequest, log, out response) == false)
            {
                return response;
            }

            if (string.IsNullOrEmpty(joinRandomGameRequest.LobbyName) && this.AppLobby != null)
            {
                this.AppLobby.EnqueueOperation(this, operationRequest, sendParameters);
                return null;
            }

            AppLobby lobby;
            if (!this.Application.LobbyFactory.GetOrCreateAppLobby(joinRandomGameRequest.LobbyName, (AppLobbyType)joinRandomGameRequest.LobbyType, out lobby))
            {
                return new OperationResponse { OperationCode = operationRequest.OperationCode, ReturnCode = (int)ErrorCode.OperationDenied, DebugMessage = "Lobby does not exist" };
            }

            lobby.EnqueueOperation(this, operationRequest, sendParameters);
            return null;
        }

        #endregion
    }
}