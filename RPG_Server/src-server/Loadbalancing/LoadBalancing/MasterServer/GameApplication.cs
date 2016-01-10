
namespace Photon.LoadBalancing.MasterServer
{
    using System.Collections.Generic;

    using ExitGames.Logging;

    using Photon.LoadBalancing.LoadBalancer;
    using Photon.LoadBalancing.MasterServer.GameServer;
    using Photon.LoadBalancing.MasterServer.Lobby;
    using Photon.LoadBalancing.ServerToServer.Events;

    public class GameApplication
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public readonly string ApplicationId;

        public readonly LoadBalancer<IncomingGameServerPeer> LoadBalancer;

        public readonly PlayerCache PlayerOnlineCache;

        public LobbyFactory LobbyFactory { get; protected set; }

        private readonly Dictionary<string, GameState> gameDict = new Dictionary<string, GameState>();
 
        public GameApplication(string applicationId, LoadBalancer<IncomingGameServerPeer> loadBalancer)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Creating application: appId={0}", applicationId);
            }

            this.ApplicationId = applicationId;
            this.LoadBalancer = loadBalancer;
            this.PlayerOnlineCache = new PlayerCache();
            this.LobbyFactory = new LobbyFactory(this);        
        }

        public virtual void OnClientConnected(MasterClientPeer peer)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("OnClientConnect: peerId={0}, appId={1}", peer.ConnectionId, this.ApplicationId);
            }

           // remove from player cache
            if (this.PlayerOnlineCache != null && string.IsNullOrEmpty(peer.UserId) == false)
            {
                this.PlayerOnlineCache.OnConnectedToMaster(peer.UserId);
            }
        }

        public virtual void OnClientDisconnected(MasterClientPeer peer)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("OnClientDisconnect: peerId={0}, appId={1}", peer.ConnectionId, this.ApplicationId);
            }

            // remove from player cache
            if (this.PlayerOnlineCache != null && string.IsNullOrEmpty(peer.UserId) == false)
            {
                this.PlayerOnlineCache.OnDisconnectFromMaster(peer.UserId);
            }
        }

        public bool GetOrCreateGame(string gameId, AppLobby lobby, IGameList gameList, byte maxPlayer, IncomingGameServerPeer gameServerPeer, out GameState gameState)
        {
            lock (this.gameDict)
            {
                if (this.gameDict.TryGetValue(gameId, out gameState))
                {
                    return false;
                }

                gameState = new GameState(lobby, gameId, maxPlayer, gameServerPeer);
                this.gameDict.Add(gameId, gameState);
                return true;
            }
        }

        public bool TryCreateGame(string gameId, AppLobby lobby, IGameList gameList, byte maxPlayer, IncomingGameServerPeer gameServerPeer, out GameState gameState)
        {
            bool result = false;

            lock (this.gameDict)
            {
                if (this.gameDict.TryGetValue(gameId, out gameState) == false)
                {
                    gameState = new GameState(lobby, gameId, maxPlayer, gameServerPeer);
                    this.gameDict.Add(gameId, gameState);
                    result = true;
                }
            }

            if (result)
            {
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("Created game: gameId={0}, appId={1}", gameId, this.ApplicationId);
                }
            }

            return result;
        }

        public bool TryGetGame(string gameId, out GameState gameState)
        {
            lock (this.gameDict)
            {
                return this.gameDict.TryGetValue(gameId, out gameState);
            }
        }

        public void OnGameUpdateOnGameServer(UpdateGameEvent updateGameEvent, IncomingGameServerPeer gameServerPeer)
        {
            GameState gameState;

            lock (this.gameDict)
            {
                if (!this.gameDict.TryGetValue(updateGameEvent.GameId, out gameState))
                {
                    if (updateGameEvent.Reinitialize)
                    {
                        AppLobby lobby;
                        if (!this.LobbyFactory.GetOrCreateAppLobby(updateGameEvent.LobbyId, (AppLobbyType)updateGameEvent.LobbyType, out lobby))
                        {
                            if (log.IsDebugEnabled)
                            {
                                log.DebugFormat("Could not create lobby: name={0}, type={1}", updateGameEvent.LobbyId, (AppLobbyType)updateGameEvent.LobbyType);
                                return;
                            }
                        }

                        gameState = new GameState(lobby, updateGameEvent.GameId, updateGameEvent.MaxPlayers.GetValueOrDefault(0), gameServerPeer);
                        this.gameDict.Add(updateGameEvent.GameId, gameState);
                    }
                }
            }

            if (gameState != null)
            {
                if (gameState.GameServer != gameServerPeer)
                {
                    return;
                }

                gameState.Lobby.UpdateGameState(updateGameEvent, gameServerPeer);
                return;
            }

            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Game to update not found: {0}", updateGameEvent.GameId);
            }
        }

        public void OnGameRemovedOnGameServer(string gameId)
        {
            bool found;
            GameState gameState;

            lock (this.gameDict)
            {
                found = this.gameDict.TryGetValue(gameId, out gameState);
            }

            if (found)
            {
                gameState.Lobby.RemoveGame(gameId);
            }
            else if (log.IsDebugEnabled)
            {
                log.DebugFormat("Game to remove not found: gameid={0}, appId={1}", gameId, this.ApplicationId);
            }
        }

        public bool RemoveGame(string gameId)
        {
            bool removed;

            lock (this.gameDict)
            {
                removed = this.gameDict.Remove(gameId);
            }

            if (log.IsDebugEnabled)
            {
                if (removed)
                {
                    log.DebugFormat("Removed game: gameId={0}, appId={1}", gameId, this.ApplicationId);
                }
                else
                {
                    log.DebugFormat("Game to remove not found: gameId={0}, appId={1}", gameId, this.ApplicationId);
                }
            }

            return removed;
        }

        public virtual void OnGameServerRemoved(IncomingGameServerPeer gameServer)
        {
            this.LobbyFactory.OnGameServerRemoved(gameServer);
        }
    }
}
