// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MasterApplication.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the MasterApplication type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.MasterServer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Runtime.InteropServices;

    using ExitGames.Logging;
    using ExitGames.Logging.Log4Net;

    using log4net;
    using log4net.Config;

    using Photon.LoadBalancing.Common;
    using Photon.LoadBalancing.LoadBalancer;
    using Photon.LoadBalancing.MasterServer.GameServer;
    using Photon.SocketServer;

    using LogManager = ExitGames.Logging.LogManager;

    public class MasterApplication : ApplicationBase
    {
        #region Constants and Fields

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private NodesReader reader;

        #endregion

        #region Properties

        public static ApplicationStats AppStats { get; protected set; }

        public GameServerCollection GameServers { get; protected set; }

        public bool IsMaster
        {
            get
            {
                return this.MasterNodeId == this.LocalNodeId;
            }
        }

        public LoadBalancer<IncomingGameServerPeer> LoadBalancer { get; protected set; }

        protected byte LocalNodeId
        {
            get
            {
                return this.reader.CurrentNodeId;
            }
        }

        protected byte MasterNodeId { get; set; }

        public GameApplication DefaultApplication { get; private set; }

        #endregion

        #region Public Methods

        public IPAddress GetInternalMasterNodeIpAddress()
        {
            return this.reader.GetIpAddress(this.MasterNodeId);
        }

        public virtual void RemoveGameServerFromLobby(IncomingGameServerPeer gameServerPeer)
        {
            this.DefaultApplication.OnGameServerRemoved(gameServerPeer);
        }

        #endregion

        #region Methods

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            if (this.IsGameServerPeer(initRequest))
            {
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("Received init request from game server");
                }

                return new IncomingGameServerPeer(initRequest, this);
            }

            if (this.LocalNodeId == this.MasterNodeId)
            {
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("Received init request from game client on leader node");
                }

                return new MasterClientPeer(initRequest);
            }

            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Received init request from game client on slave node");
            }

            return new RedirectedClientPeer(initRequest.Protocol, initRequest.PhotonPeer);
        }

        protected virtual void Initialize()
        {
            this.GameServers = new GameServerCollection();
            this.LoadBalancer = new LoadBalancer<IncomingGameServerPeer>(Path.Combine(this.ApplicationRootPath, "LoadBalancer.config"));

            this.DefaultApplication = new GameApplication("{Default}", this.LoadBalancer);

            if (MasterServerSettings.Default.AppStatsPublishInterval > 0)
            {
                AppStats = new ApplicationStats(MasterServerSettings.Default.AppStatsPublishInterval);
            }

            this.InitResolver();
        }

        protected virtual bool IsGameServerPeer(InitRequest initRequest)
        {
            return initRequest.LocalPort == MasterServerSettings.Default.IncomingGameServerPeerPort;
        }

        protected override void OnStopRequested()
        {
            // in case of application restarts, we need to disconnect all GS peers to force them to reconnect. 
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("OnStopRequested... going to disconnect {0} GS peers", this.GameServers.Count);
            }

            // copy to prevent changes of the underlying enumeration
            if (this.GameServers != null)
            {
                var gameServers = new Dictionary<string, IncomingGameServerPeer>(this.GameServers);

                foreach (var gameServer in gameServers)
                {
                    var peer = gameServer.Value;
                    if (log.IsDebugEnabled)
                    {
                        log.DebugFormat("Disconnecting GS peer {0}:{1}", peer.RemoteIP, peer.RemotePort);
                    }

                    peer.Disconnect();
                }
            }
        }

        protected override void Setup()
        {
            LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
            GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(this.ApplicationRootPath, "log");
            GlobalContext.Properties["LogFileName"] = "MS" + this.ApplicationName;
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(this.BinaryPath, "log4net.config")));

            Protocol.AllowRawCustomValues = true;
            this.SetUnmanagedDllDirectory();
            this.Initialize();
        }


        protected override void TearDown()
        {
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetDllDirectory(string lpPathName);

        /// <summary>
        /// Adds a directory to the search path used to locate the 32-bit or 64-bit version 
        /// for unmanaged DLLs used in the application.
        /// </summary>
        /// <remarks>
        /// Assemblies having references to unmanaged libraries (like SqlLite) require either a
        /// 32-Bit or a 64-Bit version of the library depending on the current process.
        /// </remarks>
        private void SetUnmanagedDllDirectory()
        {
            string unmanagedDllDirectory = Path.Combine(this.BinaryPath, IntPtr.Size == 8 ? "x64" : "x86");
            bool result = SetDllDirectory(unmanagedDllDirectory);

            if (result == false)
            {
                log.WarnFormat("Failed to set unmanaged dll directory to path {0}", unmanagedDllDirectory);
            }
        }

        private void InitResolver()
        {
            string nodesFileName = CommonSettings.Default.NodesFileName;
            if (string.IsNullOrEmpty(nodesFileName))
            {
                nodesFileName = "Nodes.txt";
            }

            this.reader = new NodesReader(this.ApplicationRootPath, nodesFileName);

            // TODO: remove Proxy code completly
            //if (this.IsResolver && MasterServerSettings.Default.EnableProxyConnections)
            //{
            //    // setup for proxy connections
            //    this.reader.NodeAdded += this.NodesReader_OnNodeAdded;
            //    this.reader.NodeChanged += this.NodesReader_OnNodeChanged;
            //    this.reader.NodeRemoved += this.NodesReader_OnNodeRemoved;
            //    log.Info("Proxy connections enabled");
            //}

            this.reader.Start();

            // use local host id if nodes.txt does not exist or if line ending with 'Y' does not exist, otherwise use fixed node #1
             this.MasterNodeId = (byte)(this.LocalNodeId == 0 ? 0 : 1);

            log.InfoFormat(
             "Current Node (nodeId={0}) is {1}the active master (leader)", 
             this.reader.CurrentNodeId, 
             this.MasterNodeId == this.reader.CurrentNodeId ? string.Empty : "NOT ");
        }

        #endregion
    }
}