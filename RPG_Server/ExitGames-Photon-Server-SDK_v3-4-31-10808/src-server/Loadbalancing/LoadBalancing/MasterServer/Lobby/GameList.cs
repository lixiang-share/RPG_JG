// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameList.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the GameList type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.MasterServer.Lobby
{
    #region using directives

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using ExitGames.Logging;

    using Photon.LoadBalancing.Events;
    using Photon.LoadBalancing.MasterServer.GameServer;
    using Photon.LoadBalancing.Operations;
    using Photon.LoadBalancing.ServerToServer.Events;
    using Photon.SocketServer;

    #endregion

    public class GameList : IGameList
    {
        #region Constants and Fields

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public readonly AppLobby Lobby;

        private readonly Dictionary<string, GameState> changedGames;

        private readonly LinkedListDictionary<string, GameState> gameDict;

        private readonly HashSet<string> removedGames;

        private readonly HashSet<PeerBase> peers = new HashSet<PeerBase>();

        private readonly Random rnd = new Random();

        private LinkedListNode<GameState> nextJoinRandomStartNode;

        #endregion

        #region Constructors and Destructors

        public GameList(AppLobby lobby)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Creating new GameList");
            }

            this.Lobby = lobby;
            this.gameDict = new LinkedListDictionary<string, GameState>();
            this.changedGames = new Dictionary<string, GameState>();
            this.removedGames = new HashSet<string>();
        }

        #endregion

        #region Properties

        public int ChangedGamesCount
        {
            get
            {
                return this.changedGames.Count + this.removedGames.Count;
            }
        }

        public int Count
        {
            get
            {
                return this.gameDict.Count;
            }
        }

        #endregion

        #region Public Methods

        public void AddGameState(GameState gameState)
        {
            this.gameDict.Add(gameState.Id, gameState);
        }

        public void CheckJoinTimeOuts(int timeOutSeconds)
        {
            this.CheckJoinTimeOuts(TimeSpan.FromSeconds(timeOutSeconds));
        }

        public int CheckJoinTimeOuts(TimeSpan timeOut)
        {
            DateTime minDate = DateTime.UtcNow.Subtract(timeOut);
            return this.CheckJoinTimeOuts(minDate);
        }

        public int CheckJoinTimeOuts(DateTime minDateTime)
        {
            int oldJoiningCount = 0;
            int joiningPlayerCount = 0;

            var toRemove = new List<GameState>();

            foreach (GameState gameState in this.gameDict)
            {
                if (gameState.JoiningPlayerCount > 0)
                {
                    oldJoiningCount += gameState.JoiningPlayerCount;
                    gameState.CheckJoinTimeOuts(minDateTime);

                    // check if there are still players left for the game
                    if (gameState.PlayerCount == 0)
                    {
                        toRemove.Add(gameState);
                    }

                    joiningPlayerCount += gameState.JoiningPlayerCount;
                }
            }

            // remove all games where no players left
            foreach (GameState gameState in toRemove)
            {
                this.RemoveGameState(gameState.Id);
            }

            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Checked join timeouts: before={0}, after={1}", oldJoiningCount, joiningPlayerCount);
            }

            return joiningPlayerCount;
        }

        public bool ContainsGameId(string gameId)
        {
            return this.gameDict.ContainsKey(gameId);
        }

        public Hashtable GetAllGames(int maxCount)
        {
            if (maxCount <= 0)
            {
                maxCount = this.gameDict.Count;
            }

            var hashTable = new Hashtable(maxCount);

            int i = 0;
            foreach (GameState game in this.gameDict)
            {
                if (game.IsVisbleInLobby)
                {
                    Hashtable gameProperties = game.ToHashTable();
                    hashTable.Add(game.Id, gameProperties);
                    i++;
                }

                if (i == maxCount)
                {
                    break;
                }
            }

            return hashTable;
        }

        public Hashtable GetChangedGames()
        {
            if (this.changedGames.Count == 0 && this.removedGames.Count == 0)
            {
                return null;
            }

            var hashTable = new Hashtable(this.changedGames.Count + this.removedGames.Count);

            foreach (GameState gameInfo in this.changedGames.Values)
            {
                if (gameInfo.IsVisbleInLobby)
                {
                    Hashtable gameProperties = gameInfo.ToHashTable();
                    hashTable.Add(gameInfo.Id, gameProperties);
                }
            }

            foreach (string gameId in this.removedGames)
            {
                hashTable.Add(gameId, new Hashtable { { (byte)GameParameter.Removed, true } });
            }

            this.changedGames.Clear();
            this.removedGames.Clear();

            return hashTable;
        }

        public void RemoveGameServer(IncomingGameServerPeer gameServer)
        {
            // find games belonging to the game server instance
            List<GameState> instanceStates = this.gameDict.Where(gameState => gameState.GameServer == gameServer).ToList();

            // remove game server instance games
            foreach (GameState gameState in instanceStates)
            {
                this.RemoveGameState(gameState.Id);
            }
        }

        public bool RemoveGameState(string gameId)
        {
            this.Lobby.Application.RemoveGame(gameId);

            GameState gameState;
            if (!this.gameDict.TryGet(gameId, out gameState))
            {
                return false;
            }

            if (log.IsDebugEnabled)
            {
                LogGameState("RemoveGameState:", gameState);
            }

            if (this.nextJoinRandomStartNode != null && this.nextJoinRandomStartNode.Value == gameState)
            {
                this.AdvanceNextJoinRandomStartNode();
            }

            gameState.OnRemoved();

            this.gameDict.Remove(gameId);
            this.changedGames.Remove(gameId);
            this.removedGames.Add(gameId);

            return true;
        }

        public bool TryGetGame(string gameId, out GameState gameState)
        {
            return this.gameDict.TryGet(gameId, out gameState);
        }

        public ErrorCode TryGetRandomGame(JoinRandomType joinType, ILobbyPeer peer, Hashtable gameProperties, string query, out GameState gameState, out string message)
        {
            message = null;

            if (this.gameDict.Count == 0)
            {
                gameState = null;
                return ErrorCode.NoMatchFound;
            }

            LinkedListNode<GameState> startNode;
            switch (joinType)
            {
                default:
                    startNode = this.gameDict.First;
                    break;

                case JoinRandomType.FromLastMatch:
                    startNode = this.nextJoinRandomStartNode ?? this.gameDict.First;
                    break;

                case JoinRandomType.Random:
                    var startNodeIndex = this.rnd.Next(this.gameDict.Count);
                    startNode = this.gameDict.GetAtIntext(startNodeIndex);
                    break;
            }

            if (!TryGetRandomGame(startNode, gameProperties, out gameState))
            {
                return ErrorCode.NoMatchFound;
            }

            return ErrorCode.Ok;
        }

        public bool UpdateGameState(UpdateGameEvent updateOperation, IncomingGameServerPeer incomingGameServerPeer, out GameState gameState)
        {
            // try to get the game state 
            if (this.gameDict.TryGet(updateOperation.GameId, out gameState) == false)
            {
                if (updateOperation.Reinitialize)
                {
                    if (log.IsDebugEnabled)
                    {
                        log.DebugFormat("Reinitialize: Add Game State {0}", updateOperation.GameId);
                    }

                    if (!this.Lobby.Application.TryGetGame(updateOperation.GameId, out gameState))
                    {
                        if (log.IsDebugEnabled)
                        {
                            log.DebugFormat("Could not find game to reinitialize: {0}", updateOperation.GameId);
                        }

                        return false;
                    }

                    this.gameDict.Add(updateOperation.GameId, gameState);

                }
                else
                {
                    if (log.IsDebugEnabled)
                    {
                        log.DebugFormat("Game not found: {0}", updateOperation.GameId);
                    }
                    return false;
                }
            }

            bool oldVisible = gameState.IsVisbleInLobby;
            bool changed = gameState.Update(updateOperation);

            if (changed)
            {
                if (gameState.IsVisbleInLobby)
                {
                    this.changedGames[gameState.Id] = gameState;

                    if (oldVisible == false)
                    {
                        this.removedGames.Remove(gameState.Id);
                    }
                }
                else
                {
                    if (oldVisible)
                    {
                        this.changedGames.Remove(gameState.Id);
                        this.removedGames.Add(gameState.Id);
                    }
                }

                if (log.IsDebugEnabled)
                {
                    LogGameState("UpdateGameState: ", gameState);
                }
            }

            return true;
        }

        public IGameListSubscibtion AddSubscription(PeerBase peer, Hashtable gamePropertyFilter, int maxGameCount)
        {
            var subscribtion = new Subscribtion(this, peer, maxGameCount);
            this.peers.Add(peer);
            return subscribtion;
        }

        public void PublishGameChanges()
        {
            if (this.ChangedGamesCount > 0)
            {
                Hashtable gameList = this.GetChangedGames();

                var e = new GameListUpdateEvent { Data = gameList };
                var eventData = new EventData((byte)EventCode.GameListUpdate, e);
                ApplicationBase.Instance.BroadCastEvent(eventData, this.peers, new SendParameters());
            }
        }

        public void OnMaxPlayerReached(GameState gameState)
        {
        }

        public void OnPlayerCountFallBelowMaxPlayer(GameState gameState)
        {
        }

        #endregion

        #region Methods

        private static void LogGameState(string prefix, GameState gameState)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat(
                    "{0}id={1}, peers={2}, max={3}, open={4}, visible={5}, peersJoining={6}", 
                    prefix, 
                    gameState.Id, 
                    gameState.GameServerPlayerCount, 
                    gameState.MaxPlayer, 
                    gameState.IsOpen, 
                    gameState.IsVisible, 
                    gameState.JoiningPlayerCount);
            }
        }

        private void AdvanceNextJoinRandomStartNode()
        {
            if (this.nextJoinRandomStartNode == null)
            {
                return;
            }

            if (log.IsDebugEnabled)
            {
                log.DebugFormat(
                    "Changed last join random match: oldGameId={0}, newGameId={1}",
                    this.nextJoinRandomStartNode.Value.Id,
                    this.nextJoinRandomStartNode.Next == null ? "{null}" : this.nextJoinRandomStartNode.Value.Id);
            }

            this.nextJoinRandomStartNode = this.nextJoinRandomStartNode.Next;
        }

        private bool TryGetRandomGame(LinkedListNode<GameState> startNode, Hashtable gameProperties, out GameState gameState)
        {
            var node = startNode;

            do
            {
                var game = node.Value;
                node = node.Next ?? this.gameDict.First;

                if (!game.IsOpen || !game.IsVisible || !game.IsCreatedOnGameServer || (game.MaxPlayer > 0 && game.PlayerCount >= game.MaxPlayer))
                {
                    continue;
                }

                if (gameProperties != null && game.MatchGameProperties(gameProperties) == false)
                {
                    continue;
                }

                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("Found match. Next start node gameid={0}", node.Value.Id);
                }

                this.nextJoinRandomStartNode = node;
                gameState = game;
                return true;
            }
            while (node != startNode);

            gameState = null;
            return false;
        }

        #endregion

        private class Subscribtion : IGameListSubscibtion
        {
            private readonly PeerBase peer;

            private readonly int maxGameCount;

            private GameList gameList;

            public Subscribtion(GameList gameList, PeerBase peer, int maxGameCount)
            {
                this.gameList = gameList;
                this.peer = peer;
                this.maxGameCount = maxGameCount;
            }

            public void Dispose()
            {
                var gl = Interlocked.Exchange(ref this.gameList, null);
                if (gl != null)
                {
                    gl.peers.Remove(this.peer);
                }
            }

            public Hashtable GetGameList()
            {
                var gl = this.gameList;
                if (gl == null)
                {
                    // subscription has been disposed (client has diconnect) during the request handling
                    return new Hashtable();
                }

                return gl.GetAllGames(this.maxGameCount);
            }
        }

    }
}