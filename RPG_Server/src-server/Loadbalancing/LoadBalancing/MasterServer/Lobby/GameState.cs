// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameState.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the GameState type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.MasterServer.Lobby
{
    #region using directives

    using System;
    using System.Collections;
    using System.Collections.Generic;

    using ExitGames.Logging;

    using Photon.LoadBalancing.Common;
    using Photon.LoadBalancing.MasterServer.GameServer;
    using Photon.LoadBalancing.Operations;
    using Photon.LoadBalancing.ServerToServer.Events;
    using Photon.SocketServer;

    #endregion

    public class GameState
    {
        #region Constants and Fields

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public readonly AppLobby Lobby;

        private readonly DateTime createDateUtc = DateTime.UtcNow;

        private readonly IncomingGameServerPeer gameServer;

        /// <summary>
        ///   Used track peers which currently are joining the game.
        /// </summary>
        private readonly LinkedList<PeerState> joiningPeers = new LinkedList<PeerState>();

        private readonly LinkedList<string> userIdList = new LinkedList<string>();

        private readonly Dictionary<object, object> properties = new Dictionary<object, object>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "GameState" /> class.
        /// </summary>
        /// <param name="lobby">
        /// The lobby to which the game belongs.
        /// </param>
        /// <param name = "id">
        ///   The game id.
        /// </param>
        /// <param name = "maxPlayer">
        ///   The maximum number of player who can join the game.
        /// </param>
        /// <param name = "gameServerPeer">
        ///   The game server peer.
        /// </param>
        public GameState(AppLobby lobby, string id, byte maxPlayer, IncomingGameServerPeer gameServerPeer)
        {
            this.Lobby = lobby;
            this.Id = id;
            this.MaxPlayer = maxPlayer;
            this.IsOpen = true;
            this.IsVisible = true;
            this.IsCreatedOnGameServer = false;
            this.GameServerPlayerCount = 0;
            this.gameServer = gameServerPeer;
        }

        #endregion

        #region Properties

        public DateTime CreateDateUtc
        {
            get
            {
                return this.createDateUtc;
            }
        }

        /// <summary>
        ///   Gets the address of the game server on which the game is or should be created.
        /// </summary>
        public IncomingGameServerPeer GameServer
        {
            get
            {
                return this.gameServer;
            }
        }

        /// <summary>
        ///   Gets the number of players who joined the game on the game server.
        /// </summary>
        public int GameServerPlayerCount { get; private set; }

        /// <summary>
        ///   Gets the game id.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the game is created on a game server instance.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is created on game server; otherwise, <c>false</c>.
        /// </value>
        public bool IsCreatedOnGameServer { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the game is open for players to join the game.
        /// </summary>
        /// <value><c>true</c> if the game is open; otherwise, <c>false</c>.</value>
        public bool IsOpen { get; set; }

        /// <summary>
        ///   Gets a value indicating whether this instance is visble in the lobby.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is visble in lobby; otherwise, <c>false</c>.
        /// </value>
        public bool IsVisbleInLobby
        {
            get
            {
                return this.IsVisible && this.IsCreatedOnGameServer;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the game should be visible to other players.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the game is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsVisible { get; set; }

        /// <summary>
        ///   Gets the number of players currently joining the game.
        /// </summary>
        public int JoiningPlayerCount
        {
            get
            {
                return this.joiningPeers.Count;
            }
        }

        /// <summary>
        ///   Gets or sets the maximum number of player for the game.
        /// </summary>
        public int MaxPlayer { get; set; }
        
        /// <summary>
        ///   Gets the number of players joined the game.
        /// </summary>
        public int PlayerCount
        {
            get
            {
                return this.GameServerPlayerCount + this.JoiningPlayerCount;
            }
        }

        public Dictionary<object, object> Properties
        {
            get
            {
                return this.properties;
            }
        }

        #endregion

        #region Public Methods

        public void AddPeer(ILobbyPeer peer)
        {
            var peerState = new PeerState(peer);

            this.joiningPeers.AddLast(peerState);
            
            if (string.IsNullOrEmpty(peerState.UserId) == false)
            {
                this.userIdList.AddLast(peer.UserId);
            }

            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Added peer: gameId={0}, userId={1}, joiningPeers={2}", this.Id, peer.UserId, this.joiningPeers.Count);
            }

            // check if max player is reached and inform the game list
            if (this.MaxPlayer > 0 && this.PlayerCount >= this.MaxPlayer)
            {
                this.Lobby.GameList.OnMaxPlayerReached(this);
            }

            // update player state in the online players cache
            if (this.Lobby.Application.PlayerOnlineCache != null && string.IsNullOrEmpty(peer.UserId) == false)
            {
                this.Lobby.Application.PlayerOnlineCache.OnJoinedGamed(peer.UserId, this);
            }
        }

        public void CheckJoinTimeOuts(DateTime minDateTime)
        {
            var oldPlayerCount = this.PlayerCount;

            LinkedListNode<PeerState> node = this.joiningPeers.First;
            while (node != null)
            {
                PeerState peerState = node.Value;
                LinkedListNode<PeerState> nextNode = node.Next;

                if (peerState.UtcCreated < minDateTime)
                {
                    this.joiningPeers.Remove(node);

                    if (string.IsNullOrEmpty(peerState.UserId) == false)
                    {
                        this.userIdList.Remove(peerState.UserId);
                        if (this.Lobby.Application.PlayerOnlineCache != null)
                        {
                            this.Lobby.Application.PlayerOnlineCache.OnDisconnectFromGameServer(peerState.UserId);
                        }
                    }
                }

                node = nextNode;
            }

            if (this.MaxPlayer > 0 && oldPlayerCount >= this.MaxPlayer)
            {
                if (this.PlayerCount < this.MaxPlayer)
                {
                    this.Lobby.GameList.OnPlayerCountFallBelowMaxPlayer(this);
                }
            }
        }

        public string GetServerAddress(ILobbyPeer peer)
        {
            switch (peer.NetworkProtocol)
            {
                case NetworkProtocolType.Udp:
                    return this.GameServer.UdpAddress;

                case NetworkProtocolType.Tcp:
                    return this.GameServer.TcpAddress;

                case NetworkProtocolType.WebSocket:
                    return this.GameServer.WebSocketAddress;

                default:
                    return null;
            }
        }

        public bool MatchGameProperties(Hashtable matchProperties)
        {
            if (matchProperties == null || matchProperties.Count == 0)
            {
                return true;
            }

            foreach (object key in matchProperties.Keys)
            {
                object gameProperty;
                if (!this.properties.TryGetValue(key, out gameProperty))
                {
                    return false;
                }

                if (gameProperty.Equals(matchProperties[key]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public Hashtable ToHashTable()
        {
            var h = new Hashtable();
            foreach (KeyValuePair<object, object> keyValue in this.properties)
            {
                h.Add(keyValue.Key, keyValue.Value);
            }

            h[(byte)GameParameter.PlayerCount] = (byte)this.GameServerPlayerCount;
            h[(byte)GameParameter.MaxPlayer] = (byte)this.MaxPlayer;
            h[(byte)GameParameter.IsOpen] = this.IsOpen;
            h.Remove((byte)GameParameter.IsVisible);

            return h;
        }

        public bool TrySetProperties(Hashtable gameProperties, out bool changed, out string debugMessage)
        {
            changed = false;

            byte? maxPlayer;
            bool? isOpen;
            bool? isVisible;
            object[] propertyFilter;
            
            if (!GameParameterReader.TryReadDefaultParameter(gameProperties, out maxPlayer, out isOpen, out isVisible, out propertyFilter, out debugMessage))
            {
                return false;
            }

            if (maxPlayer.HasValue && maxPlayer.Value != this.MaxPlayer)
            {
                this.MaxPlayer = maxPlayer.Value;
                this.properties[(byte)GameParameter.MaxPlayer] = (byte)this.MaxPlayer;
                changed = true;
            }

            if (isOpen.HasValue && isOpen.Value != this.IsOpen)
            {
                this.IsOpen = isOpen.Value;
                this.properties[(byte)GameParameter.IsOpen] = this.MaxPlayer;
                changed = true;
            }

            if (isVisible.HasValue && isVisible.Value != this.IsVisible)
            {
                this.IsVisible = isVisible.Value;
                changed = true;
            }

            this.properties.Clear();
            foreach (DictionaryEntry entry in gameProperties)
            {
                if (entry.Value != null)
                {
                    this.properties[entry.Key] = entry.Value;
                }
            }

            debugMessage = string.Empty;
            return true;
        }

        public bool Update(UpdateGameEvent updateOperation)
        {
            bool changed = false;

            if (this.IsCreatedOnGameServer == false)
            {
                this.IsCreatedOnGameServer = true;
                changed = true;
            }

            if (this.GameServerPlayerCount != updateOperation.ActorCount)
            {
                this.GameServerPlayerCount = updateOperation.ActorCount;
                changed = true;
            }

            if (updateOperation.NewUsers != null)
            {
                foreach (string userId in updateOperation.NewUsers)
                {
                    this.OnPeerJoinedGameServer(userId);
                }
            }

            if (updateOperation.RemovedUsers != null)
            {
                foreach (string userId in updateOperation.RemovedUsers)
                {
                    this.OnPeerLeftGameServer(userId);
                }
            }

            if (updateOperation.MaxPlayers.HasValue && updateOperation.MaxPlayers.Value != this.MaxPlayer)
            {
                this.MaxPlayer = updateOperation.MaxPlayers.Value;
                this.properties[(byte)GameParameter.MaxPlayer] = this.MaxPlayer;
                changed = true;
            }

            if (updateOperation.IsOpen.HasValue && updateOperation.IsOpen.Value != this.IsOpen)
            {
                this.IsOpen = updateOperation.IsOpen.Value;
                this.properties[(byte)GameParameter.IsOpen] = this.MaxPlayer;
                changed = true;
            }

            if (updateOperation.IsVisible.HasValue && updateOperation.IsVisible.Value != this.IsVisible)
            {
                this.IsVisible = updateOperation.IsVisible.Value;
                changed = true;
            }

            if (updateOperation.PropertyFilter != null)
            {
                var lobbyProperties = new HashSet<object>(updateOperation.PropertyFilter);

                var keys = new object[this.properties.Keys.Count];
                this.properties.Keys.CopyTo(keys, 0);

                foreach (var key in keys)
                {
                    if (lobbyProperties.Contains(key) == false)
                    {
                        this.properties.Remove(key);
                        changed = true;
                    }
                }

                // add max players even if it's not in the property filter
                // MaxPlayer is always reported to the client and available 
                // for JoinRandom matchmaking
                this.properties[(byte)GameParameter.MaxPlayer] = (byte)this.MaxPlayer;
            }

            if (updateOperation.GameProperties != null)
            {
                changed |= this.UpdateProperties(updateOperation.GameProperties);
            }

            return changed;
        }

        public void OnRemoved()
        {
            if (this.Lobby.Application.PlayerOnlineCache != null && userIdList.Count > 0)
            {
                foreach (string playerId in this.userIdList)
                {
                    this.Lobby.Application.PlayerOnlineCache.OnDisconnectFromGameServer(playerId);
                }
            }

        }
       
        public override string ToString()
        {
            return
                string.Format(
                    "GameState {0}: Lobby: {9}, PlayerCount: {1}, Created on GS: {2} at {3}, GSPlayerCount: {4}, IsOpen: {5}, IsVisibleInLobby: {6}, IsVisible: {7}, UtcCreated: {8}",
                    this.Id,
                    this.PlayerCount,
                    this.IsCreatedOnGameServer,
                    this.gameServer != null ? this.gameServer.ToString() : string.Empty,
                    this.GameServerPlayerCount,
                    this.IsOpen,
                    this.IsVisbleInLobby,
                    this.IsVisible,
                    this.CreateDateUtc,
                    this.Lobby.LobbyName);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Invoked for peers which has joined the game on the game server instance.
        /// </summary>
        /// <param name = "userId">The user id of the peer joined.</param>
        private void OnPeerJoinedGameServer(string userId)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("User joined on game server: gameId={0}, userId={1}", this.Id, userId);
            }

            // remove the peer from the joining list
            bool removed = this.RemoveFromJoiningList(userId);
            if (removed == false && log.IsDebugEnabled)
            {
                log.DebugFormat("User not found in joining list: gameId={0}, userId={1}", this.Id, userId);
            }

            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            // update player state in the online players cache
            if (this.Lobby.Application.PlayerOnlineCache != null)
            {
                this.Lobby.Application.PlayerOnlineCache.OnJoinedGamed(userId, this);
            }
        }

        /// <summary>
        ///   Invoked for peers which has left the game on the game server instance.
        /// </summary>
        /// <param name = "userId">The user id of the peer left.</param>
        private void OnPeerLeftGameServer(string userId)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("User left on game server: gameId={0}, userId={1}", this.Id, userId);
            }

            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            this.userIdList.Remove(userId);

            // update player state in the online players cache
            if (this.Lobby.Application.PlayerOnlineCache != null)
            {
                this.Lobby.Application.PlayerOnlineCache.OnDisconnectFromGameServer(userId);
            }
        }

        /// <summary>
        ///   Removes a peer with the specified user id from the list of joining peers.
        /// </summary>
        /// <param name = "userId">The user id of the peer to remove</param>
        /// <returns>True if the peer has been removed; otherwise false.</returns>
        private bool RemoveFromJoiningList(string userId)
        {
            if (userId == null)
            {
                userId = string.Empty;
            }

            LinkedListNode<PeerState> node = this.joiningPeers.First;

            while (node != null)
            {
                if (node.Value.UserId == userId)
                {
                    this.joiningPeers.Remove(node);
                    return true;
                }

                node = node.Next;
            }

            return false;
        }

        private bool UpdateProperties(Hashtable props)
        {
            bool changed = false;

            foreach (DictionaryEntry entry in props)
            {
                object oldValue;

                if (this.properties.TryGetValue(entry.Key, out oldValue))
                {
                    if (entry.Value == null)
                    {
                        changed = true;
                        this.properties.Remove(entry.Key);
                    }
                    else
                    {
                        if (oldValue == null || !oldValue.Equals(entry.Value))
                        {
                            changed = true;
                            this.properties[entry.Key] = entry.Value;
                        }
                    }
                }
                else
                {
                    if (entry.Value != null)
                    {
                        changed = true;
                        this.properties[entry.Key] = entry.Value;
                    }
                }
            }

            return changed;
        }

        #endregion
    }
}