// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameList.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the GameList type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.MasterServer.ChannelLobby
{
    #region using directives

    using System;
    using System.Collections;
    using System.Collections.Generic;

    using ExitGames.Logging;

    using Photon.LoadBalancing.MasterServer.GameServer;
    using Photon.LoadBalancing.MasterServer.Lobby;
    using Photon.LoadBalancing.Operations;
    using Photon.LoadBalancing.ServerToServer.Events;
    using Photon.SocketServer;

    #endregion

    public class GameChannelList : IGameList
    {
        #region Constants and Fields

        internal readonly Dictionary<string, GameState> GameDict;

        internal readonly Dictionary<GameChannelKey, GameChannel> GameChannels = new Dictionary<GameChannelKey, GameChannel>();

        public readonly AppLobby Lobby;

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructors and Destructors

        public GameChannelList(AppLobby lobby)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Creating new GameChannelList");
            }

            this.Lobby = lobby;
            this.GameDict = new Dictionary<string, GameState>();
        }

        #endregion

        #region Properties

        public int Count
        {
            get
            {
                return this.GameDict.Count;
            }
        }

        #endregion

        #region Public Methods

        public void AddGameState(GameState gameState)
        {
            this.GameDict.Add(gameState.Id, gameState);
        }

        public int CheckJoinTimeOuts(int timeOutSeconds)
        {
            return this.CheckJoinTimeOuts(TimeSpan.FromSeconds(timeOutSeconds));
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

            foreach (GameState gameState in this.GameDict.Values)
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
            return this.GameDict.ContainsKey(gameId);
        }

        public IGameListSubscibtion AddSubscription(PeerBase peer, Hashtable gamePropertyFilter, int maxGameCount)
        {
            if (gamePropertyFilter == null)
            {
                gamePropertyFilter = new Hashtable(0);
            }

            GameChannel gameChannel;
            var key = new GameChannelKey(gamePropertyFilter);

            if (!this.GameChannels.TryGetValue(key, out gameChannel))
            {
                gameChannel = new GameChannel(this, key);
                this.GameChannels.Add(key, gameChannel);
            }

            return gameChannel.AddSubscription(peer, maxGameCount);
        }

       

        public void RemoveGameServer(IncomingGameServerPeer gameServer)
        {
            // find games belonging to the game server instance
            var instanceGames = new List<GameState>();
            foreach (var gameState in this.GameDict.Values)
            {
                if (gameState.GameServer == gameServer)
                {
                    instanceGames.Add(gameState);
                }
            }

            // remove game server instance games
            foreach (GameState gameState in instanceGames)
            {
                this.RemoveGameState(gameState.Id);
            }
        }

        public bool RemoveGameState(string gameId)
        {
            this.Lobby.Application.RemoveGame(gameId);

            GameState gameState;
            if (!this.GameDict.TryGetValue(gameId, out gameState))
            {
                return false;
            }

            if (log.IsDebugEnabled)
            {
                LogGameState("RemoveGameState:", gameState);
            }

            foreach (GameChannel channel in this.GameChannels.Values)
            {
                channel.OnGameRemoved(gameState);
            }

            this.GameDict.Remove(gameId);
            return true;
        }

        public bool TryGetGame(string gameId, out GameState gameState)
        {
            return this.GameDict.TryGetValue(gameId, out gameState);
        }

        public ErrorCode TryGetRandomGame(JoinRandomType joinType, ILobbyPeer peer, Hashtable gameProperties, string query, out GameState gameState, out string message)
        {
            message = null;

            foreach (GameState game in this.GameDict.Values)
            {
                if (game.IsOpen && game.IsVisible && game.IsCreatedOnGameServer && (game.MaxPlayer <= 0 || game.PlayerCount < game.MaxPlayer))
                {
                    if (gameProperties != null && game.MatchGameProperties(gameProperties) == false)
                    {
                        continue;
                    }

                    gameState = game;
                    return ErrorCode.Ok;
                }
            }

            gameState = null;
            return ErrorCode.NoMatchFound;
        }

        public bool UpdateGameState(UpdateGameEvent updateOperation, IncomingGameServerPeer gameServerPeer, out GameState gameState)
        {
            // try to get the game state 
            if (this.GameDict.TryGetValue(updateOperation.GameId, out gameState) == false)
            {
                if (updateOperation.Reinitialize)
                {
                    if (log.IsDebugEnabled)
                    {
                        log.DebugFormat("Reinitialize: Add Game State {0}", updateOperation.GameId);
                    }

                    gameState = new GameState(this.Lobby, updateOperation.GameId, 0, gameServerPeer);
                    this.GameDict.Add(updateOperation.GameId, gameState);
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

            if (!changed)
            {
                return false;
            }

            if (log.IsDebugEnabled)
            {
                LogGameState("UpdateGameState: ", gameState);
            }

            if (gameState.IsVisbleInLobby)
            {
                foreach (GameChannel channel in this.GameChannels.Values)
                {
                    channel.OnGameUpdated(gameState);
                }

                return true;
            }

            if (oldVisible)
            {
                foreach (GameChannel channel in this.GameChannels.Values)
                {
                    channel.OnGameRemoved(gameState);
                }
            }

            return true;
        }

        public void PublishGameChanges()
        {
            foreach (var channel in this.GameChannels.Values)
            {
                channel.PublishGameChanges();
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

        #endregion
    }
}