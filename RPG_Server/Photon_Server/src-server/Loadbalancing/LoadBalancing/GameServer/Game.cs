// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Game.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the Game type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.GameServer
{
    #region using directives

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using ExitGames.Logging;
    using Lite;
    using Lite.Diagnostics.OperationLogging;
    using Lite.Operations;
    using Photon.LoadBalancing.Common;
    using Photon.LoadBalancing.Common.Net;
    using Photon.LoadBalancing.MasterServer.Lobby;
    using Photon.LoadBalancing.Operations;
    using Photon.LoadBalancing.ServerToServer.Events;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;
    using Photon.SocketServer.Rpc.Protocols;

    using OperationCode = Photon.LoadBalancing.Operations.OperationCode;

    #endregion

    public class Game : LiteGame
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private byte maxPlayers;

        private bool isVisible = true;

        private bool isOpen = true;

        private string lobbyId;

        private AppLobbyType lobbyType;

        /// <summary>
        /// Contains the keys of the game properties hashtable which should be listet in the lobby.
        /// </summary>
        private HashSet<object> lobbyProperties;
        

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        /// <param name="gameId">The game id.</param>
        /// <param name="roomCache">The room cache the game blongs to.</param>
        /// <param name="emptyRoomLiveTime">
        ///   A value indicating how long the room instance will be keeped alive 
        ///   in the room cache after all peers have left the room.
        /// </param>
        public Game(string gameId, GameCache roomCache = null, int emptyRoomLiveTime = 0)
            : base(gameId, roomCache, emptyRoomLiveTime)
        {
            if (GameApplication.Instance.AppStatsPublisher != null)
            {
                GameApplication.Instance.AppStatsPublisher.IncrementGameCount();
            }
        }

        public string LobbyId
        {
            get
            {
                return this.lobbyId;
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; 
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                this.RemoveGameStateFromMaster();

                if (GameApplication.Instance.AppStatsPublisher != null)
                {
                    GameApplication.Instance.AppStatsPublisher.DecrementGameCount();
                }
            }
        }

        protected virtual Actor HandleJoinGameOperation(LitePeer peer, JoinRequest joinRequest, SendParameters sendParameters)
        {
            if (!this.ValidateGame(peer, joinRequest.OperationRequest, sendParameters)) 
            {
                return null; 
            }

            // special treatment for game and actor properties sent by AS3/Flash
            var protocol = peer.Protocol.ProtocolType;
            if (protocol == ProtocolType.Amf3V152 || protocol == ProtocolType.Json)
            {
                Utilities.ConvertAs3WellKnownPropertyKeys(joinRequest.GameProperties, joinRequest.ActorProperties);  
            }

            var gamePeer = (GameClientPeer)peer;
            Actor actor = this.HandleJoinOperation(peer, joinRequest, sendParameters);

            if (actor == null)
            {
                return null;
            }

            // update game state at master server            
            var peerId = gamePeer.PeerId ?? string.Empty;
            this.UpdateGameStateOnMaster(null, null, null, null, joinRequest.GameProperties, peerId);

            return actor;            
        }

        
        protected virtual Actor HandleCreateGameOperation(LitePeer peer, JoinGameRequest createRequest, SendParameters sendParameters)
        {
            if (!this.ValidateGame(peer, createRequest.OperationRequest, sendParameters)) 
            {
                return null; 
            }

            HandleCreateGameOperationBody(peer, createRequest, sendParameters, false);
            return null;
        }

        protected virtual void HandleCreateGameOperationBody(LitePeer peer, JoinGameRequest 
            createRequest, SendParameters sendParameters, bool restored)
        {
            var gamePeer = (GameClientPeer)peer;

            byte? newMaxPlayer;
            bool? newIsOpen;
            bool? newIsVisible;
            object[] newLobbyProperties;
            Hashtable properties = createRequest.GameProperties;
            if (restored)
            {
                properties = Properties.GetProperties();

                /// we set it to null in order to prevent property update in HandleJoinOperation
                createRequest.GameProperties = null;
            }

            SetupGameProperties(peer, createRequest, properties,
                ref sendParameters, out newMaxPlayer, out newIsOpen, out newIsVisible, out newLobbyProperties);

            Actor actor = this.HandleJoinOperation(peer, createRequest, sendParameters);
            if (actor == null)
            {
                return ;
            }

            this.lobbyId = createRequest.LobbyName;
            this.lobbyType = (AppLobbyType)createRequest.LobbyType;

            if (log.IsDebugEnabled)
            {
                log.DebugFormat("CreateGame: name={0}, lobyName={1}, lobbyType={2}", this.Name, this.lobbyId, this.lobbyType);
            }

            // set default properties
            if (newMaxPlayer.HasValue && newMaxPlayer.Value != this.maxPlayers)
            {
                this.maxPlayers = newMaxPlayer.Value;
            }

            if (newIsOpen.HasValue && newIsOpen.Value != this.isOpen)
            {
                this.isOpen = newIsOpen.Value;
            }

            if (newIsVisible.HasValue && newIsVisible.Value != this.isVisible)
            {
                this.isVisible = newIsVisible.Value;
            }

            if (newLobbyProperties != null)
            {
                this.lobbyProperties = new HashSet<object>(newLobbyProperties);
            }

            // update game state at master server            
            var peerId = gamePeer.PeerId ?? string.Empty;
            Hashtable gameProperties = this.GetLobbyGameProperties(properties);
            this.UpdateGameStateOnMaster(newMaxPlayer, newIsOpen, newIsVisible, newLobbyProperties, gameProperties, peerId);

            return;
        }

        private void SetupGameProperties(LitePeer peer, JoinGameRequest createRequest, 
            Hashtable gameProperties, ref SendParameters sendParameters, out byte? newMaxPlayer, out bool? newIsOpen, out bool? newIsVisible, out object[] newLobbyProperties)
        {
            newMaxPlayer = null;
            newIsOpen = null;
            newIsVisible = null;
            newLobbyProperties = null;

            // special treatment for game and actor properties sent by AS3/Flash or JSON clients
            var protocol = peer.Protocol.ProtocolType;
            if (protocol == ProtocolType.Amf3V152 || protocol == ProtocolType.Json)
            {
                Utilities.ConvertAs3WellKnownPropertyKeys(createRequest.GameProperties, createRequest.ActorProperties);
            }

            // try to parse build in properties for the first actor (creator of the game)
            if (this.Actors.Count == 0)
            {
                if (gameProperties != null && gameProperties.Count > 0)
                {
                    if (!TryParseDefaultProperties(peer, createRequest, gameProperties, 
                        sendParameters, out newMaxPlayer, out newIsOpen, out newIsVisible, out newLobbyProperties))
                    {
                        return;
                    }
                }
            }
            return;
        }

        protected override int RemovePeerFromGame(LitePeer peer, LeaveRequest leaveRequest)
        {
            int result = base.RemovePeerFromGame(peer, leaveRequest);

            if (this.IsDisposed)
            {
                return result;
            }

            // If there are still peers left an UpdateGameStateOperation with the new 
            // actor count will be send to the master server.
            // If there are no actors left the RoomCache will dispose this instance and a 
            // RemoveGameStateOperation will be sent to the master.
            if (this.Actors.Count > 0)
            {
                var gamePeer = (GameClientPeer)peer;
                var peerId = gamePeer.PeerId ?? string.Empty;
                this.UpdateGameStateOnMaster(null, null, null, null, null, null, peerId);
            }

            return result;
        }

        protected override void HandleGetPropertiesOperation(LitePeer peer, GetPropertiesRequest getPropertiesRequest, SendParameters sendParameters)
        {
            // special handling for game properties send by AS3/Flash (Amf3 protocol) clients or JSON clients
            if (peer.Protocol.ProtocolType == ProtocolType.Amf3V152 || peer.Protocol.ProtocolType == ProtocolType.Json)
            {
                Utilities.ConvertAs3WellKnownPropertyKeys(getPropertiesRequest.GamePropertyKeys, getPropertiesRequest.ActorPropertyKeys);
            }

            base.HandleGetPropertiesOperation(peer, getPropertiesRequest, sendParameters);
        }

        protected override void HandleSetPropertiesOperation(LitePeer peer, SetPropertiesRequest request, SendParameters sendParameters)
        {
            byte? newMaxPlayer = null;
            bool? newIsOpen = null;
            bool? newIsVisible = null;
            object[] newLobbyProperties = null;

            // try to parse build in propeties if game properties should be set (ActorNumber == 0)
            bool updateGameProperties = request.ActorNumber == 0 && request.Properties != null && request.Properties.Count > 0;

            // special handling for game and actor properties send by AS3/Flash (Amf3 protocol) or JSON clients
            if (peer.Protocol.ProtocolType == ProtocolType.Amf3V152 || peer.Protocol.ProtocolType == ProtocolType.Json)
            {
                if (updateGameProperties)
                {
                    Utilities.ConvertAs3WellKnownPropertyKeys(request.Properties, null);   
                }
                else
                {
                    Utilities.ConvertAs3WellKnownPropertyKeys(null, request.Properties);   
                }
            }

            if (updateGameProperties)
            {
                if (!TryParseDefaultProperties(peer, request, request.Properties, sendParameters, out newMaxPlayer, out newIsOpen, out newIsVisible, out newLobbyProperties))
                {
                    return;
                }
            }

            base.HandleSetPropertiesOperation(peer, request, sendParameters);

            // report to master only if game properties are updated
            if (!updateGameProperties)
            {
                return;
            }

            // set default properties
            if (newMaxPlayer.HasValue && newMaxPlayer.Value != this.maxPlayers)
            {
                this.maxPlayers = newMaxPlayer.Value;
            }

            if (newIsOpen.HasValue && newIsOpen.Value != this.isOpen)
            {
                this.isOpen = newIsOpen.Value;
            }

            if (newIsVisible.HasValue && newIsVisible.Value != this.isVisible)
            {
                this.isVisible = newIsVisible.Value;
            }

            if (newLobbyProperties != null)
            {
                this.lobbyProperties = new HashSet<object>(newLobbyProperties);
            }

            Hashtable gameProperties;
            if (newLobbyProperties != null)
            {
                // if the property filter for the app lobby properties has been changed
                // all game properties are resend to the master server because the application 
                // lobby might not contain all properties specified.
                gameProperties = this.GetLobbyGameProperties(this.Properties.GetProperties());
            }
            else
            {
                // property filter hasn't chjanged; only the changed properties will
                // be updatet in the application lobby
                gameProperties = this.GetLobbyGameProperties(request.Properties);
            }

            this.UpdateGameStateOnMaster(newMaxPlayer, newIsOpen, newIsVisible, newLobbyProperties, gameProperties);
        }

        protected virtual void HandleDebugGameOperation(LitePeer peer, DebugGameRequest request, SendParameters sendParameters)
        {
            // Room: Properties; # of cached events
            // Actors:  Count, Last Activity, Actor #, Peer State, Connection ID
            // Room Reference
                    
            // get info from request (was gathered in Peer class before operation was forwarded to the game): 
            var peerInfo = request.Info;
            var debugInfo = peerInfo + this; 

            if (Log.IsInfoEnabled)
            {
                Log.Info("DebugGame: " + debugInfo);
            }

            this.LogQueue.WriteLog();

            var debugGameResponse = new DebugGameResponse { Info = debugInfo };

            peer.SendOperationResponse(new OperationResponse(request.OperationRequest.OperationCode, debugGameResponse), sendParameters); 
        }

        protected override void ExecuteOperation(LitePeer peer, OperationRequest operationRequest, SendParameters sendParameters)
        {
            try
            {
                if (Log.IsDebugEnabled)
                {
                    Log.DebugFormat("Executing operation {0}", operationRequest.OperationCode);
                }

                switch (operationRequest.OperationCode)
                {
                    case (byte)OperationCode.CreateGame:
                        var createGameRequest = new JoinGameRequest(peer.Protocol, operationRequest);
                        if (peer.ValidateOperation(createGameRequest, sendParameters) == false)
                        {
                            return;
                        }

                        if (this.LogQueue.Log.IsDebugEnabled)
                        {

                            this.LogQueue.Add(
                                new LogEntry(
                                    "ExecuteOperation: " + (OperationCode)operationRequest.OperationCode,
                                    "Peer=" + peer.ConnectionId));
                        }
                        this.HandleCreateGameOperation(peer, createGameRequest, sendParameters);
                        break;

                    case (byte)OperationCode.JoinGame:
                        var joinGameRequest = new JoinRequest(peer.Protocol, operationRequest);
                        if (peer.ValidateOperation(joinGameRequest, sendParameters) == false)
                        {
                            return;
                        }

                        if (this.LogQueue.Log.IsDebugEnabled)
                        {

                            this.LogQueue.Add(
                                new LogEntry(
                                    "ExecuteOperation: " + (OperationCode)operationRequest.OperationCode,
                                    "Peer=" + peer.ConnectionId));
                        }
                        this.HandleJoinGameOperation(peer, joinGameRequest, sendParameters);
                        break;

                    // Lite operation code for join is not allowed in load balanced games.
                    case (byte)Lite.Operations.OperationCode.Join:
                        var response = new OperationResponse
                            {
                                OperationCode = operationRequest.OperationCode,
                                ReturnCode = (short)ErrorCode.OperationDenied,
                                DebugMessage = "Invalid operation code"
                            };
                        peer.SendOperationResponse(response, sendParameters);
                        break;

                    case (byte)OperationCode.DebugGame:
                        var debugGameRequest = new DebugGameRequest(peer.Protocol, operationRequest);
                        if (peer.ValidateOperation(debugGameRequest, sendParameters) == false)
                        {
                            return;
                        }

                        if (this.LogQueue.Log.IsDebugEnabled)
                        {

                            this.LogQueue.Add(
                                new LogEntry(
                                    "ExecuteOperation: " + (OperationCode)operationRequest.OperationCode,
                                    "Peer=" + peer.ConnectionId));
                        }

                        this.HandleDebugGameOperation(peer, debugGameRequest, sendParameters);
                        break;

                    // all other operation codes will be handled by the Lite game implementation
                    default:
                        base.ExecuteOperation(peer, operationRequest, sendParameters);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        protected override void ProcessMessage(Lite.Messages.IMessage message)
        {
            try
            {
                switch (message.Action)
                {
                    case (byte)GameMessageCodes.ReinitializeGameStateOnMaster:
                        if (Actors.Count == 0)
                        {
                            Log.WarnFormat("Reinitialize tried to update GameState with ActorCount = 0. " + this);
                        }
                        else
                        {
                            var gameProperties = this.GetLobbyGameProperties(this.Properties.GetProperties());
                            object[] lobbyPropertyFilter = null;
                            if (this.lobbyProperties != null)
                            {
                                lobbyPropertyFilter = new object[this.lobbyProperties.Count];
                                this.lobbyProperties.CopyTo(lobbyPropertyFilter);
                            }

                            this.UpdateGameStateOnMaster(this.maxPlayers, this.isOpen, this.isVisible, lobbyPropertyFilter, gameProperties, null, null, true);
                        }

                        break;

                    case (byte)GameMessageCodes.CheckGame:
                        this.CheckGame();
                        break;

                    default:
                        base.ProcessMessage(message);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        /// <summary>
        /// Check routine to validate that the game is valid (ie., it is removed from the game cache if it has no longer any actors etc.). 
        /// CheckGame() is called by the Application at regular intervals. 
        /// </summary>
        protected virtual void CheckGame()
        {
            if (this.Actors.Count == 0)
            {
                // double check if the game is still in cache: 
                Room room; 
                if (GameCache.Instance.TryGetRoomWithoutReference(this.Name, out room))
                {
                    Log.WarnFormat("Game with 0 Actors is still in cache: {0}", GameCache.Instance.GetDebugString(room.Name));
                }
            }
        }

     

        protected virtual void UpdateGameStateOnMaster(
            byte? newMaxPlayer = null, 
            bool? newIsOpen = null,
            bool? newIsVisble = null,
            object[] lobbyPropertyFilter = null,
            Hashtable gameProperties = null, 
            string newPeerId = null, 
            string removedPeerId = null, 
            bool reinitialize = false)
        {
            if (reinitialize && Actors.Count == 0)
            {
                Log.WarnFormat("Reinitialize tried to update GameState with ActorCount = 0. " + this);
                return;
            }

            var updateGameEvent = new UpdateGameEvent
                {
                    GameId = this.Name,
                    ActorCount = (byte)this.Actors.Count,
                    Reinitialize = reinitialize,
                    MaxPlayers = newMaxPlayer,
                    IsOpen = newIsOpen,
                    IsVisible = newIsVisble,
                    PropertyFilter = lobbyPropertyFilter
                };

            if (reinitialize)
            {
                updateGameEvent.LobbyId = this.lobbyId;
                updateGameEvent.LobbyType = (byte)this.lobbyType;
            }

            if (gameProperties != null && gameProperties.Count > 0)
            {
                updateGameEvent.GameProperties = gameProperties;
            }

            if (newPeerId != null)
            {
                updateGameEvent.NewUsers = new[] { newPeerId };
            }

            if (removedPeerId != null)
            {
                updateGameEvent.RemovedUsers = new[] { removedPeerId };
            }

            this.UpdateGameStateOnMaster(updateGameEvent);
        }

        protected virtual void UpdateGameStateOnMaster(UpdateGameEvent updateEvent)
        {
            var eventData = new EventData((byte)ServerEventCode.UpdateGameState, updateEvent);
            GameApplication.Instance.MasterPeer.SendEvent(eventData, new SendParameters());
        }

        protected virtual void RemoveGameStateFromMaster()
        {
            GameApplication.Instance.MasterPeer.RemoveGameState(this.Name);
        }

        private static bool TryParseDefaultProperties(
            LitePeer peer, Operation operation, Hashtable propertyTable, SendParameters sendParameters, out byte? maxPlayer, out bool? isOpen, out bool? isVisible, out object[] properties)
        {
            string debugMessage;

            if (!GameParameterReader.TryReadDefaultParameter(propertyTable, out maxPlayer, out isOpen, out isVisible, out properties, out debugMessage))
            {
                var response = new OperationResponse { OperationCode = operation.OperationRequest.OperationCode, ReturnCode = (short)ErrorCode.OperationInvalid, DebugMessage = debugMessage };
                peer.SendOperationResponse(response, sendParameters);
                return false;
            }

            return true;
        }

        protected bool ValidateGame(LitePeer peer, OperationRequest operationRequest, SendParameters sendParameters)
        {
            var gamePeer = (GameClientPeer)peer;

            // check if the game is open
            if (this.isOpen == false)
            {
                var errorResponse = new OperationResponse { OperationCode = operationRequest.OperationCode, ReturnCode = (int)ErrorCode.GameClosed, DebugMessage = "Game closed" };
                peer.SendOperationResponse(errorResponse, sendParameters);
                gamePeer.OnJoinFailed(ErrorCode.GameClosed);
                return false;
            }

            // check if the maximum number of players has already been reached
            if (this.maxPlayers > 0 && this.Actors.Count >= this.maxPlayers)
            {
                var errorResponse = new OperationResponse { OperationCode = operationRequest.OperationCode, ReturnCode = (int)ErrorCode.GameFull, DebugMessage = "Game full" };
                peer.SendOperationResponse(errorResponse, sendParameters);
                gamePeer.OnJoinFailed(ErrorCode.GameFull);
                return false;
            }

            return true;
        }

        private Hashtable GetLobbyGameProperties(Hashtable source)
        {
            if (source == null || source.Count == 0)
            {
                return null;
            }

            Hashtable gameProperties;

            if (this.lobbyProperties != null)
            {
                // filter for game properties is set, only properties in the specified list 
                // will be reported to the lobby 
                gameProperties = new Hashtable(this.lobbyProperties.Count);

                foreach (var entry in this.lobbyProperties)
                {
                    if (source.ContainsKey(entry))
                    {
                        gameProperties.Add(entry, source[entry]);
                    }
                }
            }
            else
            {
                // if no filter is set for properties which should be listet in the lobby
                // all properties are send
                gameProperties = source;
                gameProperties.Remove((byte)GameParameter.MaxPlayer);
                gameProperties.Remove((byte)GameParameter.IsOpen);
                gameProperties.Remove((byte)GameParameter.IsVisible);
                gameProperties.Remove((byte)GameParameter.Properties);
            }

            return gameProperties;
        }

    }
}