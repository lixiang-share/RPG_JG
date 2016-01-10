// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameApplication.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the GameApplication type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.GameServer
{
    #region using directives

    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    using ExitGames.Concurrency.Fibers;
    using ExitGames.Logging;
    using ExitGames.Logging.Log4Net;

    using Lite;
    using Lite.Messages;

    using log4net;
    using log4net.Config;

    using Photon.LoadBalancing.Common;
    using Photon.LoadBalancing.LoadShedding;
    using Photon.LoadBalancing.LoadShedding.Diagnostics;
    using Photon.SocketServer;
    using Photon.SocketServer.Diagnostics;
    using Photon.SocketServer.ServerToServer;

    using ConfigurationException = ExitGames.Configuration.ConfigurationException;
    using LogManager = ExitGames.Logging.LogManager;

    #endregion

    public class GameApplication : ApplicationBase
    {
        #region Constants and Fields

        public static readonly Guid ServerId = Guid.NewGuid();

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private static GameApplication instance;

        private static OutgoingMasterServerPeer masterPeer;

        private readonly NodesReader reader;

        private PoolFiber executionFiber;

        private byte isReconnecting;

        private Timer masterConnectRetryTimer;

        #endregion

        #region Constructors and Destructors

        public GameApplication()
        {
            UpdateMasterEndPoint();

            this.GamingTcpPort = GameServerSettings.Default.GamingTcpPort;
            this.GamingUdpPort = GameServerSettings.Default.GamingUdpPort;
            this.GamingWebSocketPort = GameServerSettings.Default.GamingWebSocketPort;

            this.ConnectRetryIntervalSeconds = GameServerSettings.Default.ConnectReytryInterval;

            this.reader = new NodesReader(this.ApplicationRootPath, CommonSettings.Default.NodesFileName);
        }

        #endregion

        #region Public Properties

        public static new GameApplication Instance
        {
            get
            {
                return instance;
            }

            protected set
            {
                Interlocked.Exchange(ref instance, value);
            }
        }

        public int? GamingTcpPort { get; protected set; }

        public int? GamingUdpPort { get; protected set; }

        public int? GamingWebSocketPort { get; protected set; }

        public IPEndPoint MasterEndPoint { get; protected set; }

        public ApplicationStatsPublisher AppStatsPublisher { get; protected set; }

        public OutgoingMasterServerPeer MasterPeer
        {
            get
            {
                return masterPeer;
            }

            protected set
            {
                Interlocked.Exchange(ref masterPeer, value);
            }
        }

        public IPAddress PublicIpAddress { get; protected set; }

        public WorkloadController WorkloadController { get; protected set; }

        #endregion

        #region Properties

        protected int ConnectRetryIntervalSeconds { get; set; }

        #endregion

        #region Public Methods

        public void ConnectToMaster(IPEndPoint endPoint)
        {
            if (this.Running == false)
            {
                return;
            }

            if (this.ConnectToServerTcp(endPoint, "Master", endPoint))
            {
                if (log.IsInfoEnabled)
                {
                    log.InfoFormat("Connecting to master at {0}, serverId={1}", endPoint, ServerId);
                }
            }
            else
            {
                log.WarnFormat("master connection refused - is the process shutting down ? {0}", ServerId);
            }
        }

        public void ConnectToMaster()
        {
            if (this.Running == false)
            {
                return;
            }

            UpdateMasterEndPoint();
            ConnectToMaster(MasterEndPoint);
        }

        public byte GetCurrentNodeId()
        {
            return this.reader.ReadCurrentNodeId();
        }

        public void ReconnectToMaster()
        {
            if (this.Running == false)
            {
                return;
            }

            Thread.VolatileWrite(ref this.isReconnecting, 1);
            this.masterConnectRetryTimer = new Timer(o => this.ConnectToMaster(), null, this.ConnectRetryIntervalSeconds * 1000, 0);
        }

        #endregion

        #region Methods

        private void UpdateMasterEndPoint()
        {
            IPAddress masterAddress;
            if (!IPAddress.TryParse(GameServerSettings.Default.MasterIPAddress, out masterAddress))
            {
                var hostEntry = Dns.GetHostEntry(GameServerSettings.Default.MasterIPAddress);
                if (hostEntry.AddressList == null || hostEntry.AddressList.Length == 0)
                {
                    throw new ConfigurationException(
                        "MasterIPAddress setting is neither an IP nor an DNS entry: "
                        + GameServerSettings.Default.MasterIPAddress);
                }

                masterAddress =
                    hostEntry.AddressList.First(address => address.AddressFamily == AddressFamily.InterNetwork); 

                if (masterAddress == null)
                {
                    throw new ConfigurationException(
                        "MasterIPAddress does not resolve to an IPv4 address! Found: "
                        + string.Join(", ", hostEntry.AddressList.Select(a => a.ToString()).ToArray()));
                }
            }

            int masterPort = GameServerSettings.Default.OutgoingMasterServerPeerPort;
            this.MasterEndPoint = new IPEndPoint(masterAddress, masterPort);
        }

        /// <summary>
        ///   Sanity check to verify that game states are cleaned up correctly
        /// </summary>
        protected virtual void CheckGames()
        {
            var roomNames = GameCache.Instance.GetRoomNames();

            foreach (var roomName in roomNames)
            {
                Room room;
                GameCache.Instance.TryGetRoomWithoutReference(roomName, out room);
                room.EnqueueMessage(new RoomMessage((byte)GameMessageCodes.CheckGame));
            }
        }

        protected virtual PeerBase CreateGamePeer(InitRequest initRequest)
        {
            return new GameClientPeer(initRequest, this);
        }

        protected virtual OutgoingMasterServerPeer CreateMasterPeer(InitResponse initResponse)
        {
            return new OutgoingMasterServerPeer(initResponse.Protocol, initResponse.PhotonPeer, this);
        }

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("CreatePeer for {0}", initRequest.ApplicationId);
            }

            // Game server latency monitor connects to self
            if (initRequest.ApplicationId == "LatencyMonitor")
            {
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat(
                        "incoming latency peer at {0}:{1} from {2}:{3}, serverId={4}", 
                        initRequest.LocalIP, 
                        initRequest.LocalPort, 
                        initRequest.RemoteIP, 
                        initRequest.RemotePort, 
                        ServerId);
                }

                return new LatencyPeer(initRequest.Protocol, initRequest.PhotonPeer);
            }

            if (log.IsDebugEnabled)
            {
                log.DebugFormat(
                    "incoming game peer at {0}:{1} from {2}:{3}", 
                    initRequest.LocalIP, 
                    initRequest.LocalPort, 
                    initRequest.RemoteIP, 
                    initRequest.RemotePort);
            }

            return this.CreateGamePeer(initRequest);
        }

        protected override ServerPeerBase CreateServerPeer(InitResponse initResponse, object state)
        {
            if (initResponse.ApplicationId == "LatencyMonitor")
            {
                // latency monitor
                LatencyMonitor peer = this.WorkloadController.OnLatencyMonitorPeerConnected(initResponse);
                return peer;
            }

            // master
            Thread.VolatileWrite(ref this.isReconnecting, 0);
            return this.MasterPeer = this.CreateMasterPeer(initResponse);
        }

        protected virtual void InitLogging()
        {
            LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
            GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(this.ApplicationRootPath, "log");
            GlobalContext.Properties["LogFileName"] = "GS" + this.ApplicationName;
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(this.BinaryPath, "log4net.config")));
        }

        protected override void OnServerConnectionFailed(int errorCode, string errorMessage, object state)
        {
            var ipEndPoint = state as IPEndPoint;
            if (ipEndPoint == null)
            {
                log.ErrorFormat("Unknown connection failed with err {0}: {1}", errorCode, errorMessage);
                return;
            }

            if (ipEndPoint.Equals(this.MasterEndPoint))
            {
                if (this.isReconnecting == 0)
                {
                    log.ErrorFormat(
                        "Master connection failed with err {0}: {1}, serverId={2}", errorCode, errorMessage, ServerId);
                }
                else if (log.IsWarnEnabled)
                {
                    log.WarnFormat(
                        "Master connection failed with err {0}: {1}, serverId={2}", errorCode, errorMessage, ServerId);
                }

                this.ReconnectToMaster();
                return;
            }

            this.WorkloadController.OnLatencyMonitorConnectFailed(ipEndPoint, errorCode, errorMessage);
        }

        protected override void OnStopRequested()
        {
            log.InfoFormat("OnStopRequested: serverid={0}", ServerId);

            if (this.masterConnectRetryTimer != null)
            {
                this.masterConnectRetryTimer.Dispose();
            }

            if (this.WorkloadController != null)
            {
                this.WorkloadController.Stop();
            }

            if (this.MasterPeer != null)
            {
                this.MasterPeer.Disconnect();
            }

            base.OnStopRequested();
        }

        protected override void Setup()
        {
            Instance = this;
            this.InitLogging();

            log.InfoFormat("Setup: serverId={0}", ServerId);

            Protocol.AllowRawCustomValues = true;

            this.PublicIpAddress = PublicIPAddressReader.ParsePublicIpAddress(GameServerSettings.Default.PublicIPAddress);

            bool isMaster =
                PublicIPAddressReader.IsLocalIpAddress(this.MasterEndPoint.Address)
                || this.MasterEndPoint.Address.Equals(this.PublicIpAddress);

            Counter.IsMasterServer.RawValue = isMaster ? 1 : 0; 

            this.SetupFeedbackControlSystem();
            this.ConnectToMaster();

            if (GameServerSettings.Default.AppStatsPublishInterval > 0)
            {
                this.AppStatsPublisher = new ApplicationStatsPublisher(GameServerSettings.Default.AppStatsPublishInterval);
            }

            CounterPublisher.DefaultInstance.AddStaticCounterClass(
                typeof(Lite.Diagnostics.Counter), this.ApplicationName);
            CounterPublisher.DefaultInstance.AddStaticCounterClass(typeof(Counter), this.ApplicationName);

            this.executionFiber = new PoolFiber();
            this.executionFiber.Start();
            this.executionFiber.ScheduleOnInterval(this.CheckGames, 60000, 60000);
        }

        protected void SetupFeedbackControlSystem()
        {
            var latencyEndpointTcp = this.GetLatencyEndpointTcp();
            var latencyEndpointUdp = this.GetLatencyEndpointUdp();

            this.WorkloadController = new WorkloadController(
                this, this.PhotonInstanceName, "LatencyMonitor", latencyEndpointTcp, latencyEndpointUdp, 1, 1000);

            this.WorkloadController.Start();
        }

        protected override void TearDown()
        {
            log.InfoFormat("TearDown: serverId={0}", ServerId);

            if (this.WorkloadController != null)
            {
                this.WorkloadController.Stop();
            }

            if (this.MasterPeer != null)
            {
                this.MasterPeer.Disconnect();
            }
        }

        private IPEndPoint GetLatencyEndpointTcp()
        {
            if (!GameServerSettings.Default.EnableLatencyMonitor)
            {
                return null;
            }

            IPEndPoint latencyEndpointTcp;
            if (string.IsNullOrEmpty(GameServerSettings.Default.LatencyMonitorAddress))
            {
                if (this.GamingTcpPort.HasValue == false)
                {
                    if (log.IsWarnEnabled)
                    {
                        log.Error(
                            "Could not start latency monitor because no tcp port is specified in the application configuration.");
                    }

                    return null;
                }

                if (this.PublicIpAddress == null)
                {
                    if (log.IsWarnEnabled)
                    {
                        log.Error("Could not latency monitor because public ip adress could not be resolved.");
                    }

                    return null;
                }

                int? tcpPort = GameServerSettings.Default.RelayPortTcp == 0
                                   ? this.GamingTcpPort
                                   : GameServerSettings.Default.RelayPortTcp + this.GetCurrentNodeId() - 1;
                latencyEndpointTcp = new IPEndPoint(this.PublicIpAddress, tcpPort.Value);
            }
            else
            {
                if (Global.TryParseIpEndpoint(GameServerSettings.Default.LatencyMonitorAddress, out latencyEndpointTcp)
                    == false)
                {
                    if (log.IsWarnEnabled)
                    {
                        log.ErrorFormat(
                            "Could not start latency monitor because an invalid endpoint ({0}) is specified in the application configuration.", 
                            GameServerSettings.Default.LatencyMonitorAddress);
                    }

                    return latencyEndpointTcp;
                }
            }

            return latencyEndpointTcp;
        }

        private IPEndPoint GetLatencyEndpointUdp()
        {
            if (!GameServerSettings.Default.EnableLatencyMonitor)
            {
                return null;
            }

            IPEndPoint latencyEndpointUdp;
            if (string.IsNullOrEmpty(GameServerSettings.Default.LatencyMonitorAddressUdp))
            {
                if (this.GamingUdpPort.HasValue == false)
                {
                    if (log.IsWarnEnabled)
                    {
                        log.Error(
                            "Could not latency monitor because no udp port is specified in the application configuration.");
                    }

                    return null;
                }

                if (this.PublicIpAddress == null)
                {
                    if (log.IsWarnEnabled)
                    {
                        log.Error("Could not latency monitor because public ip adress could not be resolved.");
                    }

                    return null;
                }

                int? udpPort = GameServerSettings.Default.RelayPortUdp == 0
                                   ? this.GamingUdpPort
                                   : GameServerSettings.Default.RelayPortUdp + this.GetCurrentNodeId() - 1;
                latencyEndpointUdp = new IPEndPoint(this.PublicIpAddress, udpPort.Value);
            }
            else
            {
                if (Global.TryParseIpEndpoint(
                    GameServerSettings.Default.LatencyMonitorAddressUdp, out latencyEndpointUdp) == false)
                {
                    if (log.IsWarnEnabled)
                    {
                        log.ErrorFormat(
                            "Coud not start latency monitor because an invalid endpoint ({0}) is specified in the application configuration.", 
                            GameServerSettings.Default.LatencyMonitorAddressUdp);
                    }

                    return latencyEndpointUdp;
                }
            }

            return latencyEndpointUdp;
        }

        #endregion
    }
}