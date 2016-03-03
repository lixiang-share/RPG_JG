// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppLobby.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the AppLobby type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.MasterServer.Lobby
{
    #region using directives

    using System;
    using System.Collections;
    using System.Collections.Generic;

    using ExitGames.Concurrency.Fibers;
    using ExitGames.Logging;

    using Photon.LoadBalancing.Events;
    using Photon.LoadBalancing.MasterServer.ChannelLobby;
    using Photon.LoadBalancing.MasterServer.GameServer;
    using Photon.LoadBalancing.Operations;
    using Photon.LoadBalancing.ServerToServer.Events;
    using Photon.LoadBalancing.Common;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;
    using Photon.SocketServer.Rpc.Protocols;

    #endregion

    public class AppLobby
    {
        #region Constants and Fields

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public readonly GameApplication Application;

        public readonly string LobbyName;

        public readonly TimeSpan JoinTimeOut = TimeSpan.FromSeconds(5);

        public readonly int MaxPlayersDefault; 

        internal readonly IGameList GameList;

        private readonly HashSet<PeerBase> peers = new HashSet<PeerBase>();

        private IDisposable schedule;

        private IDisposable checkJoinTimeoutSchedule;

        #endregion

        #region Constructors and Destructors

        public AppLobby(GameApplication application, string lobbyName, AppLobbyType lobbyType)
            : this(application, lobbyName, lobbyType, 0, TimeSpan.FromSeconds(15))
        {
        }

        public AppLobby(GameApplication application, string lobbyName, AppLobbyType lobbyType, int maxPlayersDefault, TimeSpan joinTimeOut)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Creating lobby: name={0}, type={1}", lobbyName, lobbyType);
            }

            this.Application = application;
            this.LobbyName = lobbyName;
            this.MaxPlayersDefault = maxPlayersDefault;
            this.JoinTimeOut = joinTimeOut;

            switch (lobbyType)
            {
                default:
                    this.GameList = new GameList(this);
                    break;

                case AppLobbyType.ChannelLobby:
                    this.GameList = new GameChannelList(this);
                    break;

                case AppLobbyType.SqlLobby:
                    this.GameList = new SqlGameList(this);
                    break;
            }

            this.ExecutionFiber = new PoolFiber();
            this.ExecutionFiber.Start();
        }

        #endregion

        #region Properties

        public PoolFiber ExecutionFiber { get; private set; }

        #endregion

        #region Public Methods

        public void EnqueueOperation(MasterClientPeer peer, OperationRequest operationRequest, SendParameters sendParameters)
        {
            this.ExecutionFiber.Enqueue(() => this.ExecuteOperation(peer, operationRequest, sendParameters));
        }



        public void RemoveGame(string gameId)
        {
            this.ExecutionFiber.Enqueue(() => this.HandleRemoveGameState(gameId));
        }

        public void RemoveGameServer(IncomingGameServerPeer gameServer)
        {
            this.ExecutionFiber.Enqueue(() => this.HandleRemoveGameServer(gameServer));
        }

        public void RemovePeer(MasterClientPeer serverPeer)
        {
            this.ExecutionFiber.Enqueue(() => this.HandleRemovePeer(serverPeer));
        }

        public void UpdateGameState(UpdateGameEvent operation, IncomingGameServerPeer incomingGameServerPeer)
        {
            this.ExecutionFiber.Enqueue(() => this.HandleUpdateGameState(operation, incomingGameServerPeer));
        }

        public void JoinLobby(MasterClientPeer peer, JoinLobbyRequest joinLobbyrequest, SendParameters sendParameters)
        {
            this.ExecutionFiber.Enqueue(() => this.HandleJoinLobby(peer, joinLobbyrequest, sendParameters));
        }

        #endregion

        #region Methods

        protected virtual void ExecuteOperation(MasterClientPeer peer, OperationRequest operationRequest, SendParameters sendParameters)
        {
            try
            {
                OperationResponse response;

                switch ((OperationCode)operationRequest.OperationCode)
                {
                    default:
                        response = new OperationResponse(operationRequest.OperationCode) { ReturnCode = -1, DebugMessage = "Unknown operation code" };
                        break;

                    case OperationCode.JoinLobby:
                        var joinLobbyRequest = new JoinLobbyRequest(peer.Protocol, operationRequest);
                        if (OperationHelper.ValidateOperation(joinLobbyRequest, log, out response))
                        {
                            response = this.HandleJoinLobby(peer, joinLobbyRequest, sendParameters);
                        }
                        break;

                    case OperationCode.LeaveLobby:
                        response = this.HandleLeaveLobby(peer, operationRequest);
                        break;

                    case OperationCode.CreateGame:
                        response = this.HandleCreateGame(peer, operationRequest);
                        break;

                    case OperationCode.JoinGame:
                        response = this.HandleJoinGame(peer, operationRequest);
                        break;

                    case OperationCode.JoinRandomGame:
                        response = this.HandleJoinRandomGame(peer, operationRequest);
                        break;

                    case OperationCode.DebugGame:
                        response = this.HandleDebugGame(peer, operationRequest);
                        break;
                }

                if (response != null)
                {
                    peer.SendOperationResponse(response, sendParameters);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        protected virtual object GetCreateGameResponse(MasterClientPeer peer, GameState gameState)
        {
            return new CreateGameResponse { GameId = gameState.Id, Address = gameState.GetServerAddress(peer) };
        }

        protected virtual object GetJoinGameResponse(MasterClientPeer peer, GameState gameState)
        {
            return new JoinGameResponse { Address = gameState.GetServerAddress(peer) };
        }

        protected virtual object GetJoinRandomGameResponse(MasterClientPeer peer, GameState gameState)
        {
            return new JoinRandomGameResponse { GameId = gameState.Id, Address = gameState.GetServerAddress(peer) };
        }

        protected virtual DebugGameResponse GetDebugGameResponse(MasterClientPeer peer, GameState gameState)
        {
            return new DebugGameResponse
                {
                    Address = gameState.GetServerAddress(peer), 
                    Info = gameState.ToString()
                };
        }

        protected virtual OperationResponse HandleCreateGame(MasterClientPeer peer, OperationRequest operationRequest)
        {
            // validate the operation request
            OperationResponse response;
            var operation = new CreateGameRequest(peer.Protocol, operationRequest);
            if (OperationHelper.ValidateOperation(operation, log, out response) == false)
            {
                return response;
            }

            // special handling for game properties send by AS3/Flash (Amf3 protocol) or JSON clients
            var protocol = peer.Protocol.ProtocolType;
            if (protocol == ProtocolType.Amf3V152 || protocol == ProtocolType.Json)
            {
                Utilities.ConvertAs3WellKnownPropertyKeys(operation.GameProperties, null);   // special treatment for game properties sent by AS3/Flash
            }

            // if no gameId is specified by the client generate a unique id 
            if (string.IsNullOrEmpty(operation.GameId))
            {
                operation.GameId = Guid.NewGuid().ToString();
            }
           
            // try to create game
            GameState gameState;
            bool gameCreated;
            if (!this.TryCreateGame(operation, operation.GameId, false, operation.GameProperties, out gameCreated, out gameState, out response))
            {
                return response;
            }

            // add peer to game
            gameState.AddPeer(peer);

            this.ScheduleCheckJoinTimeOuts();

            // publish operation response
            object createGameResponse = this.GetCreateGameResponse(peer, gameState);
            return new OperationResponse(operationRequest.OperationCode, createGameResponse);
        }

        protected virtual OperationResponse HandleJoinGame(MasterClientPeer peer, OperationRequest operationRequest)
        {
            // validate operation
            var operation = new JoinGameRequest(peer.Protocol, operationRequest);
            OperationResponse response;
            if (OperationHelper.ValidateOperation(operation, log, out response) == false)
            {
                return response;
            }

            // special handling for game properties send by AS3/Flash (Amf3 protocol) or JSON clients
            var protocol = peer.Protocol.ProtocolType;
            if (protocol == ProtocolType.Amf3V152 || protocol == ProtocolType.Json)
            {
                Utilities.ConvertAs3WellKnownPropertyKeys(operation.GameProperties, null);   
            }


            // try to find game by id
            GameState gameState;
            bool gameCreated = false;
            if (operation.CreateIfNotExists == false)
            {
                // The client does not want to create the game if it does not exists.
                // In this case the game must have been created on the game server before it can be joined.
                if (this.GameList.TryGetGame(operation.GameId, out gameState) == false || gameState.IsCreatedOnGameServer == false)
                {
                    return new OperationResponse { OperationCode = operationRequest.OperationCode, ReturnCode = (int)ErrorCode.GameIdNotExists, DebugMessage = "Game does not exist" };
                }
            }
            else
            {
                // The client will create the game if it does not exists already.
                if (!this.GameList.TryGetGame(operation.GameId, out gameState))
                {
                    if (!this.TryCreateGame(operation, operation.GameId, true, operation.GameProperties, out gameCreated, out gameState, out response))
                    {
                        return response;
                    }
                }
            }

            // game properties have only be to checked if the game was not created by the client
            if (gameCreated == false)
            {
                // check if max players of the game is already reached
                if (gameState.MaxPlayer > 0 && gameState.PlayerCount >= gameState.MaxPlayer)
                {
                    return new OperationResponse { OperationCode = operationRequest.OperationCode, ReturnCode = (int)ErrorCode.GameFull, DebugMessage = "Game full" };
                }

                // check if the game is open
                if (gameState.IsOpen == false)
                {
                    return new OperationResponse { OperationCode = operationRequest.OperationCode, ReturnCode = (int)ErrorCode.GameClosed, DebugMessage = "Game closed" };
                }
            }

            // add peer to game
            gameState.AddPeer(peer);

            this.ScheduleCheckJoinTimeOuts();

            // publish operation response
            object joinResponse = this.GetJoinGameResponse(peer, gameState);
            return new OperationResponse(operationRequest.OperationCode, joinResponse);
        }

        protected virtual OperationResponse HandleJoinLobby(MasterClientPeer peer, JoinLobbyRequest operation, SendParameters sendParameters)
        {
            try
            {
                // special handling for game properties send by AS3/Flash (Amf3 protocol) clients
                if (peer.Protocol.ProtocolType == ProtocolType.Amf3V152 || peer.Protocol.ProtocolType == ProtocolType.Json)
                {
                    Utilities.ConvertAs3WellKnownPropertyKeys(operation.GameProperties, null);
                }

                peer.GameChannelSubscription = null;

                var subscription = this.GameList.AddSubscription(peer, operation.GameProperties, operation.GameListCount);
                peer.GameChannelSubscription = subscription;
                peer.SendOperationResponse(new OperationResponse(operation.OperationRequest.OperationCode), sendParameters);


                // publish game list to peer after the response has been sent
                var gameList = subscription.GetGameList();
                var e = new GameListEvent { Data = gameList };
                var eventData = new EventData((byte)EventCode.GameList, e);
                peer.SendEvent(eventData, new SendParameters());

                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        protected virtual OperationResponse HandleJoinRandomGame(MasterClientPeer peer, OperationRequest operationRequest)
        {
            // validate the operation request
            var operation = new JoinRandomGameRequest(peer.Protocol, operationRequest);
            OperationResponse response;
            if (OperationHelper.ValidateOperation(operation, log, out response) == false)
            {
                return response;
            }

            // special handling for game properties send by AS3/Flash (Amf3 protocol) clients
            if (peer.Protocol.ProtocolType == ProtocolType.Amf3V152 || peer.Protocol.ProtocolType == ProtocolType.Json)
            {
                Utilities.ConvertAs3WellKnownPropertyKeys(operation.GameProperties, null);   
            }

            // try to find a match
            GameState game;
            string errorMessage;
            var result = this.GameList.TryGetRandomGame((JoinRandomType)operation.JoinRandomType, peer, operation.GameProperties, operation.QueryData, out game, out errorMessage);
            if (result != ErrorCode.Ok)
            {
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = "No match found";
                }

                response = new OperationResponse { OperationCode = operationRequest.OperationCode, ReturnCode = (short)result, DebugMessage = errorMessage};
                return response;
            }

            // match found, add peer to game and notify the peer
            game.AddPeer(peer);

            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Found match: connectionId={0}, userId={1}, gameId={2}", peer.ConnectionId, peer.UserId, game.Id);
            }

            this.ScheduleCheckJoinTimeOuts();

            object joinResponse = this.GetJoinRandomGameResponse(peer, game);
            return new OperationResponse(operationRequest.OperationCode, joinResponse);
        }

        protected virtual OperationResponse HandleLeaveLobby(MasterClientPeer peer, OperationRequest operationRequest)
        {
            peer.GameChannelSubscription = null;

            if (this.peers.Remove(peer))
            {
                return new OperationResponse { OperationCode = operationRequest.OperationCode };
            }

            return new OperationResponse { OperationCode = operationRequest.OperationCode, ReturnCode = 0, DebugMessage = "lobby not joined" };
        }

        protected virtual OperationResponse HandleDebugGame(MasterClientPeer peer, OperationRequest operationRequest)
        {
            var operation = new DebugGameRequest(peer.Protocol, operationRequest);
            OperationResponse response; 
            if (OperationHelper.ValidateOperation(operation, log, out response) == false)
            {
                return response; 
            }

            GameState gameState;
            if (this.GameList.TryGetGame(operation.GameId, out gameState) == false)
            {
                return new OperationResponse
                    {
                        OperationCode = operationRequest.OperationCode, ReturnCode = (int)ErrorCode.GameIdNotExists, DebugMessage = "Game does not exist"
                    };
            }

            var debugGameResponse = this.GetDebugGameResponse(peer, gameState); 

            log.InfoFormat("DebugGame: {0}", debugGameResponse.Info);

            return new OperationResponse(operationRequest.OperationCode, debugGameResponse);
        }
        
        protected virtual void OnGameStateChanged(GameState gameState)
        {
        }

        protected virtual void OnRemovePeer(MasterClientPeer peer)
        {
        }

        private void ScheduleCheckJoinTimeOuts()
        {
            if (this.checkJoinTimeoutSchedule == null)
            {
                this.checkJoinTimeoutSchedule = this.ExecutionFiber.Schedule(this.CheckJoinTimeOuts, (long)this.JoinTimeOut.TotalMilliseconds / 2);
            }
        }

        private void CheckJoinTimeOuts()
        {
            try
            {
                this.checkJoinTimeoutSchedule.Dispose();
                var joiningPlayersLeft = this.GameList.CheckJoinTimeOuts(this.JoinTimeOut);
                if (joiningPlayersLeft > 0)
                {
                    this.ExecutionFiber.Schedule(this.CheckJoinTimeOuts, (long)this.JoinTimeOut.TotalMilliseconds / 2);
                }
                else
                {
                    this.checkJoinTimeoutSchedule = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void HandleRemoveGameServer(IncomingGameServerPeer gameServer)
        {
            try
            {
                this.GameList.RemoveGameServer(gameServer);
                this.SchedulePublishGameChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void HandleRemoveGameState(string gameId)
        {
            try
            {
                GameState gameState;
                if (this.GameList.TryGetGame(gameId, out gameState) == false)
                {
                    if (log.IsDebugEnabled)
                    {
                        log.DebugFormat("HandleRemoveGameState: Game not found - gameId={0}", gameId);
                    }

                    return;
                }

                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("HandleRemoveGameState: gameId={0}, joiningPlayers={1}", gameId, gameState.JoiningPlayerCount);
                }

                this.GameList.RemoveGameState(gameId);
                this.SchedulePublishGameChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void HandleRemovePeer(MasterClientPeer peer)
        {
            try
            {
                peer.GameChannelSubscription = null;
                this.peers.Remove(peer);
                this.OnRemovePeer(peer);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void HandleUpdateGameState(UpdateGameEvent operation, IncomingGameServerPeer incomingGameServerPeer)
        {
            try
            {
                GameState gameState;

                if (this.GameList.UpdateGameState(operation, incomingGameServerPeer, out gameState) == false)
                {
                    return;
                }

                this.SchedulePublishGameChanges();

                this.OnGameStateChanged(gameState);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void SchedulePublishGameChanges()
        {
            if (this.schedule == null)
            {
                this.schedule = this.ExecutionFiber.Schedule(this.PublishGameChanges, 1000);
            }
        }

        private void PublishGameChanges()
        {
            try
            {
                this.schedule = null;
                this.GameList.PublishGameChanges();                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private bool TryCreateGame(Operation operation, string gameId, bool createIfNotExists, Hashtable properties, out bool gameCreated, out GameState gameState, out OperationResponse errorResponse)
        {
            gameState = null;
            gameCreated = false;

            // try to get a game server instance from the load balancer            
            IncomingGameServerPeer gameServer;
            if (!this.Application.LoadBalancer.TryGetServer(out gameServer))
            {
                errorResponse = new OperationResponse(operation.OperationRequest.OperationCode)
                    {
                        ReturnCode = (int)ErrorCode.ServerFull,
                        DebugMessage = "Failed to get server instance."
                    };

                return false;
            }

            // try to create or get game state
            if (createIfNotExists)
            {
                gameCreated = this.Application.GetOrCreateGame(gameId, this, this.GameList, (byte)this.MaxPlayersDefault, gameServer, out gameState);
            }
            else
            {
                if (!this.Application.TryCreateGame(gameId, this, this.GameList, (byte)this.MaxPlayersDefault, gameServer, out gameState))
                {
                    errorResponse = new OperationResponse(operation.OperationRequest.OperationCode)
                    {
                        ReturnCode = (int)ErrorCode.GameIdAlreadyExists,
                        DebugMessage = "A game with the specified id already exist."
                    };

                    return false;
                }

                gameCreated = true;
            }
          

            if (properties != null)
            {
                bool changed;
                string debugMessage;

                if (!gameState.TrySetProperties(properties, out changed, out debugMessage))
                {
                    errorResponse = new OperationResponse(operation.OperationRequest.OperationCode)
                        {
                            ReturnCode = (int)ErrorCode.OperationInvalid,
                            DebugMessage = debugMessage
                        };
                    return false;
                }
            }

            this.GameList.AddGameState(gameState);
            this.SchedulePublishGameChanges();

            errorResponse = null;
            return true;
        }

        #endregion
    }
}