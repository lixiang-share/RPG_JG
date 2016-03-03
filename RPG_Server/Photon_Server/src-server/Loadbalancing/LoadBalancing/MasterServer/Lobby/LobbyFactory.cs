
namespace Photon.LoadBalancing.MasterServer.Lobby
{
    using System.Collections.Generic;

    using ExitGames.Logging;

    using Photon.LoadBalancing.MasterServer.GameServer;

    public class LobbyFactory
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<KeyValuePair<string, AppLobbyType>, AppLobby> lobbyDict = new Dictionary<KeyValuePair<string, AppLobbyType>, AppLobby>();

        private readonly GameApplication application;

        private readonly AppLobby defaultLobby;

        public LobbyFactory(GameApplication application)
        {
            this.application = application;
            this.defaultLobby = new AppLobby(this.application, string.Empty, AppLobbyType.Default);
        }

        public LobbyFactory(GameApplication application, AppLobbyType defaultLobbyType)
        {
            this.application = application;
            this.defaultLobby = new AppLobby(this.application, string.Empty, defaultLobbyType);
        }

        public bool GetOrCreateAppLobby(string lobbyName, AppLobbyType lobbyType , out AppLobby lobby)
        {
            if (string.IsNullOrEmpty(lobbyName))
            {
                lobby = this.defaultLobby;
                return true;
            }

            var key = new KeyValuePair<string, AppLobbyType>(lobbyName, lobbyType);

            lock (this.lobbyDict)
            {
                if (this.lobbyDict.TryGetValue(key, out lobby))
                {
                    return true;
                }

                lobby = new AppLobby(this.application, lobbyName, lobbyType);
                this.lobbyDict.Add(key, lobby);
            }

            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Created lobby: name={0}, type={1}", lobbyName, lobbyType);
            }

            return true;
        }

        public void OnGameServerRemoved(IncomingGameServerPeer gameServerPeer)
        {
            this.defaultLobby.RemoveGameServer(gameServerPeer);

            lock (this.lobbyDict)
            {
                foreach (var lobby in this.lobbyDict.Values)
                {
                    lobby.RemoveGameServer(gameServerPeer);
                }
            }
        }
    }
}
