// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkloadController.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the WorkloadController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.LoadShedding
{
    #region using directives

    using System;
    using System.Net;

    using ExitGames.Concurrency.Fibers;
    using ExitGames.Diagnostics.Counter;
    using ExitGames.Logging;

    using Photon.LoadBalancing.Common;
    using Photon.LoadBalancing.GameServer;
    using Photon.LoadBalancing.LoadShedding.Diagnostics;
    using Photon.SocketServer;

    using PhotonHostRuntimeInterfaces;

    #endregion

    public class WorkloadController
    {
        #region Constants and Fields

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private const int AverageHistoryLength = 100;

        private readonly ApplicationBase application;

        private readonly string applicationName;

        private readonly AverageCounterReader businessLogicQueueCounter;

        private readonly AverageCounterReader bytesInCounter;

        private readonly AverageCounterReader bytesOutCounter;

        private readonly AverageCounterReader cpuCounter;

        private readonly AverageCounterReader enetQueueCounter;

        private readonly AverageCounterReader timeSpentInServerInCounter;

        private readonly AverageCounterReader timeSpentInServerOutCounter;

        private readonly AverageCounterReader enetThreadsProcessingCounter;
        
        private readonly AverageCounterReader tcpPeersCounter;

        private readonly AverageCounterReader tcpDisconnectsPerSecondCounter;

        private readonly AverageCounterReader tcpClientDisconnectsPerSecondCounter;

        private readonly AverageCounterReader udpPeersCounter;

        private readonly AverageCounterReader udpDisconnectsPerSecondCounter;

        private readonly AverageCounterReader udpClientDisconnectsPerSecondCounter;
        
        private readonly PerformanceCounterReader enetThreadsActiveCounter; 

        private readonly IFeedbackControlSystem feedbackControlSystem;

        private readonly PoolFiber fiber;

        private readonly byte latencyOperationCode;

        private readonly IPEndPoint remoteEndPointTcp;

        private readonly IPEndPoint remoteEndPointUdp;

        private readonly long updateIntervalInMs;

        private LatencyMonitor latencyMonitorTcp;

        private LatencyMonitor latencyMonitorUdp; 

        private IDisposable timerControl;

        private ServerState serverState = ServerState.Normal;

        #endregion

        #region Constructors and Destructors

        public WorkloadController(
            ApplicationBase application, string instanceName, string applicationName, IPEndPoint latencyEndpointTcp, IPEndPoint latencyEndpointUdp, byte latencyOperationCode, long updateIntervalInMs)
        {
            this.latencyOperationCode = latencyOperationCode;
            this.updateIntervalInMs = updateIntervalInMs;
            this.FeedbackLevel = FeedbackLevel.Normal;
            this.application = application;
            this.applicationName = applicationName;

            this.fiber = new PoolFiber();
            this.fiber.Start();

            this.remoteEndPointTcp = latencyEndpointTcp;
            this.remoteEndPointUdp = latencyEndpointUdp;

            this.cpuCounter = new AverageCounterReader(AverageHistoryLength, "Processor", "% Processor Time", "_Total");
            if (!this.cpuCounter.InstanceExists)
            {
                log.WarnFormat("Did not find counter {0}", this.cpuCounter.Name);
            }

            this.businessLogicQueueCounter = new AverageCounterReader(AverageHistoryLength, "Photon Socket Server: Threads and Queues", "Business Logic Queue", instanceName);
            if (!this.businessLogicQueueCounter.InstanceExists)
            {
                log.WarnFormat("Did not find counter {0}", this.businessLogicQueueCounter.Name);
            }

            this.enetQueueCounter = new AverageCounterReader(AverageHistoryLength, "Photon Socket Server: Threads and Queues", "ENet Queue", instanceName);
            if (!this.enetQueueCounter.InstanceExists)
            {
                log.WarnFormat("Did not find counter {0}", this.enetQueueCounter.Name);
            }

            // amazon instances do not have counter for network interfaces
            this.bytesInCounter = new AverageCounterReader(AverageHistoryLength, "Photon Socket Server", "bytes in/sec", instanceName);
            if (!this.bytesInCounter.InstanceExists)
            {
                log.WarnFormat("Did not find counter {0}", this.bytesInCounter.Name);
            }

            this.bytesOutCounter = new AverageCounterReader(AverageHistoryLength, "Photon Socket Server", "bytes out/sec", instanceName);
            if (!this.bytesOutCounter.InstanceExists)
            {
                log.WarnFormat("Did not find counter {0}", this.bytesOutCounter.Name);
            }

            this.enetThreadsProcessingCounter = new AverageCounterReader(AverageHistoryLength, "Photon Socket Server: Threads and Queues", "ENet Threads Processing", instanceName);
            if (!this.enetThreadsProcessingCounter.InstanceExists)
            {
                log.WarnFormat("Did not find counter {0}", this.enetThreadsProcessingCounter.Name);
            }

            this.enetThreadsActiveCounter = new PerformanceCounterReader("Photon Socket Server: Threads and Queues", "ENet Threads Active", instanceName);
            if (!this.enetThreadsActiveCounter.InstanceExists)
            {
                log.WarnFormat("Did not find counter {0}", this.enetThreadsActiveCounter.Name);
            }

            this.timeSpentInServerInCounter = new AverageCounterReader(AverageHistoryLength, "Photon Socket Server: ENet", "Time Spent In Server: In (ms)", instanceName);
            if (!this.timeSpentInServerInCounter.InstanceExists)
            {
                log.WarnFormat("Did not find counter {0}", this.timeSpentInServerInCounter.Name);
            }

            this.timeSpentInServerOutCounter = new AverageCounterReader(AverageHistoryLength, "Photon Socket Server: ENet", "Time Spent In Server: Out (ms)", instanceName);
            if (!this.timeSpentInServerOutCounter.InstanceExists)
            {
                log.WarnFormat("Did not find counter {0}", this.timeSpentInServerOutCounter.Name);
            }
            
            this.tcpDisconnectsPerSecondCounter = new AverageCounterReader(AverageHistoryLength, "Photon Socket Server: TCP", "TCP: Disconnected Peers +/sec", instanceName); 
            if (!this.tcpDisconnectsPerSecondCounter.InstanceExists)
            {
                log.WarnFormat("Did not find counter {0}", this.tcpDisconnectsPerSecondCounter.Name);
            }

            this.tcpClientDisconnectsPerSecondCounter = new AverageCounterReader(AverageHistoryLength, "Photon Socket Server: TCP", "TCP: Disconnected Peers (C) +/sec", instanceName);
            if (!this.tcpClientDisconnectsPerSecondCounter.InstanceExists)
            {
                log.WarnFormat("Did not find counter {0}", this.tcpClientDisconnectsPerSecondCounter.Name);
            }


            this.tcpPeersCounter = new AverageCounterReader(AverageHistoryLength, "Photon Socket Server: TCP", "TCP: Peers", instanceName);
            if (!this.tcpPeersCounter.InstanceExists)
            {
                log.WarnFormat("Did not find counter {0}", this.tcpPeersCounter.Name);
            }

            this.udpDisconnectsPerSecondCounter = new AverageCounterReader(AverageHistoryLength, "Photon Socket Server: UDP", "UDP: Disconnected Peers +/sec", instanceName);
            if (!this.udpDisconnectsPerSecondCounter.InstanceExists)
            {
                log.WarnFormat("Did not find counter {0}", this.udpDisconnectsPerSecondCounter.Name);
            }

            this.udpClientDisconnectsPerSecondCounter = new AverageCounterReader(AverageHistoryLength, "Photon Socket Server: UDP", "UDP: Disconnected Peers (C) +/sec", instanceName);
            if (!this.udpClientDisconnectsPerSecondCounter.InstanceExists)
            {
                log.WarnFormat("Did not find counter {0}", this.udpClientDisconnectsPerSecondCounter.Name);
            }

            this.udpPeersCounter = new AverageCounterReader(AverageHistoryLength, "Photon Socket Server: UDP", "UDP: Peers", instanceName);
            if (!this.udpPeersCounter.InstanceExists)
            {
                log.WarnFormat("Did not find counter {0}", this.udpPeersCounter.Name);
            }

            this.feedbackControlSystem = new FeedbackControlSystem(1000, this.application.ApplicationRootPath);
        }

        #endregion

        #region Events

        public event EventHandler FeedbacklevelChanged;

        #endregion

        #region Properties

        public FeedbackLevel FeedbackLevel { get; private set; }

        public ServerState ServerState
        {
            get
            {
                return this.serverState;
            }

            set
            {
                if (value != this.serverState)
                {
                    var oldValue = this.serverState;
                    this.serverState = value;
                    Counter.ServerState.RawValue = (long)this.ServerState;
                    this.RaiseFeedbacklevelChanged();

                    if (log.IsInfoEnabled)
                    {
                        log.InfoFormat("ServerState changed: old={0}, new={1}", oldValue, this.serverState);
                    }

                }
            }
        }

        #endregion

        #region Public Methods

        public void OnLatencyMonitorConnectFailed(IPEndPoint endPoint, int errorCode, string errorMessage)
        {
            log.ErrorFormat("Latency monitor connection to {0} failed with err {1}: {2}, serverId={3}", endPoint, errorCode, errorMessage, GameApplication.ServerId);

            if (endPoint.Equals(this.remoteEndPointTcp))
            {
                this.fiber.Schedule(this.StartTcpLatencyMonitor, 1000);
            }
            else if (endPoint.Equals(this.remoteEndPointUdp))
            {
                this.fiber.Schedule(this.StartUdpLatencyMonitor, 1000);
            }
        }

        public void OnLatencyConnectClosed(IPEndPoint endPoint)
        {
            if (endPoint.Equals(this.remoteEndPointTcp))
            {
                this.fiber.Schedule(this.StartTcpLatencyMonitor, 1000);
            }
            else if (endPoint.Equals(this.remoteEndPointUdp))
            {
                this.fiber.Schedule(this.StartUdpLatencyMonitor, 1000);
            }
        }

        public LatencyMonitor OnLatencyMonitorPeerConnected(InitResponse initResponse)
        {
            var monitor = new LatencyMonitor(initResponse.Protocol, initResponse.PhotonPeer, this.latencyOperationCode, AverageHistoryLength, 500, this); 
            var peerType = initResponse.PhotonPeer.GetPeerType();

            if (peerType == PeerType.ENetPeer || peerType == PeerType.ENetOutboundPeer)
            {
                this.latencyMonitorUdp = monitor; 
            }
            else
            {
                this.latencyMonitorTcp = monitor; 
            }

            return monitor;
        }

        /// <summary>
        ///   Starts the workload controller with a specified update interval in milliseconds.
        /// </summary>
        public void Start()
        {
            if (this.timerControl == null)
            {
                this.timerControl = this.fiber.ScheduleOnInterval(this.Update, 100, this.updateIntervalInMs);
            }

            if (GameServerSettings.Default.EnableLatencyMonitor)
            {
                this.StartTcpLatencyMonitor();
                this.StartUdpLatencyMonitor();
            }
        }

        public void Stop()
        {
            if (this.timerControl != null)
            {
                this.timerControl.Dispose();
            }

            if (this.latencyMonitorUdp != null)
            {
                this.latencyMonitorUdp.Dispose();
            }

            if (this.latencyMonitorTcp != null)
            {
                this.latencyMonitorTcp.Dispose(); 
            }
        }

        #endregion

        #region Methods

        private void StartTcpLatencyMonitor()
        {
            // TCP Latency monitor:
            if (this.remoteEndPointTcp != null)
            {
                if (this.application.ConnectToServerTcp(
                    this.remoteEndPointTcp, this.applicationName, this.remoteEndPointTcp))
                {
                    if (log.IsDebugEnabled)
                    {
                        log.DebugFormat(
                            "Connecting TCP latency monitor to {0}:{1}, serverId={2}",
                            this.remoteEndPointTcp.Address,
                            this.remoteEndPointTcp.Port,
                            GameApplication.ServerId);
                    }
                }
                else
                {
                    log.WarnFormat(
                        "TCP Latency monitor connection refused on {0}:{1}, serverId={2}",
                        this.remoteEndPointTcp.Address,
                        this.remoteEndPointTcp.Port,
                        GameApplication.ServerId);
                }
            }
        }
    
        private void StartUdpLatencyMonitor()
        {
            // UDP Latency monitor: 
            if (this.remoteEndPointUdp != null)
            {
                if (this.application.ConnectToServerUdp(
                    this.remoteEndPointUdp, this.applicationName, this.remoteEndPointUdp, 1, null))
                {
                    if (log.IsDebugEnabled)
                    {
                        log.DebugFormat(
                            "Connecting UDP latency monitor to {0}:{1}",
                            this.remoteEndPointUdp.Address,
                            this.remoteEndPointUdp.Port);
                    }
                }
                else
                {
                    log.WarnFormat(
                        "UDP Latency monitor connection refused on {0}:{1}",
                        this.remoteEndPointUdp.Address,
                        this.remoteEndPointUdp.Port);
                }
            }
        }

        private void Update()
        {
            FeedbackLevel oldValue = this.feedbackControlSystem.Output;

            if (this.cpuCounter.InstanceExists)
            {
                var cpuUsage = (int)this.cpuCounter.GetNextAverage();
                Counter.CpuAvg.RawValue = cpuUsage;
                this.feedbackControlSystem.SetCpuUsage(cpuUsage);
            }

            if (this.businessLogicQueueCounter.InstanceExists)
            {
                var businessLogicQueue = (int)this.businessLogicQueueCounter.GetNextAverage();
                Counter.BusinessQueueAvg.RawValue = businessLogicQueue;
                this.feedbackControlSystem.SetBusinessLogicQueueLength(businessLogicQueue);
            }

            if (this.enetQueueCounter.InstanceExists)
            {
                var enetQueue = (int)this.enetQueueCounter.GetNextAverage();
                Counter.EnetQueueAvg.RawValue = enetQueue;
                this.feedbackControlSystem.SetENetQueueLength(enetQueue);
            }

            if (this.bytesInCounter.InstanceExists && this.bytesOutCounter.InstanceExists)
            {
                int bytes = (int)this.bytesInCounter.GetNextAverage() + (int)this.bytesOutCounter.GetNextAverage();
                Counter.BytesInAndOutAvg.RawValue = bytes;
                this.feedbackControlSystem.SetBandwidthUsage(bytes);
            }

            if (this.enetThreadsProcessingCounter.InstanceExists && this.enetThreadsActiveCounter.InstanceExists)
            {
                try
                {
                    var enetThreadsProcessingAvg = this.enetThreadsProcessingCounter.GetNextAverage();
                    var enetThreadsActiveAvg = this.enetThreadsActiveCounter.GetNextValue();

                    int enetThreadsProcessing;
                    if (enetThreadsActiveAvg > 0)
                    {
                        enetThreadsProcessing = (int)(enetThreadsProcessingAvg / enetThreadsActiveAvg * 100);
                    }
                    else
                    {
                        enetThreadsProcessing = 0;
                    }

                    Counter.EnetThreadsProcessingAvg.RawValue = enetThreadsProcessing;
                    this.feedbackControlSystem.SetEnetThreadsProcessing(enetThreadsProcessing); 
                }
                catch (DivideByZeroException)
                {
                    log.WarnFormat("Could not calculate Enet Threads processing quotient: Enet Threads Active is 0");
                }
            }


            if (this.tcpPeersCounter.InstanceExists && this.tcpDisconnectsPerSecondCounter.InstanceExists && this.tcpClientDisconnectsPerSecondCounter.InstanceExists)
            {
                try
                {
                    var tcpDisconnectsTotal = this.tcpDisconnectsPerSecondCounter.GetNextAverage();
                    var tcpDisconnectsClient = this.tcpClientDisconnectsPerSecondCounter.GetNextAverage();
                    var tcpDisconnectsWithoutClientDisconnects = tcpDisconnectsTotal - tcpDisconnectsClient;
                    var tcpPeerCount = this.tcpPeersCounter.GetNextAverage();

                    int tcpDisconnectRate;
                    if (tcpPeerCount > 0)
                    {
                        tcpDisconnectRate = (int)(tcpDisconnectsWithoutClientDisconnects / tcpPeerCount * 1000);
                    }
                    else
                    {
                        tcpDisconnectRate = 0;
                    }

                    Counter.TcpDisconnectRateAvg.RawValue = tcpDisconnectRate;

                    //this.feedbackControlSystem.SetEnetThreadsProcessing(enetThreadsProcessing);
                }
                catch (DivideByZeroException)
                {
                    log.WarnFormat("Could not calculate TCP Disconnect Rate: TCP Peers is 0");
                }
            }

            if (this.udpPeersCounter.InstanceExists && this.udpDisconnectsPerSecondCounter.InstanceExists && this.udpClientDisconnectsPerSecondCounter.InstanceExists)
            {
                try
                {
                    var udpDisconnectsTotal = this.udpDisconnectsPerSecondCounter.GetNextAverage();
                    var udpDisconnectsClient = this.udpClientDisconnectsPerSecondCounter.GetNextAverage();
                    var udpDisconnectsWithoutClientDisconnects = udpDisconnectsTotal - udpDisconnectsClient;
                    var udpPeerCount = this.udpPeersCounter.GetNextAverage();

                    int udpDisconnectRate;
                    if (udpPeerCount > 0)
                    {
                        udpDisconnectRate = (int)(udpDisconnectsWithoutClientDisconnects / udpPeerCount * 1000);
                    }
                    else
                    {
                        udpDisconnectRate = 0;
                    }

                    Counter.UdpDisconnectRateAvg.RawValue = udpDisconnectRate;

                    this.feedbackControlSystem.SetDisconnectRateUdp(udpDisconnectRate);
                }
                catch (DivideByZeroException)
                {
                    log.WarnFormat("Could not calculate UDP Disconnect Rate: UDP Peers is 0");
                }
            }

            if (this.timeSpentInServerInCounter.InstanceExists && this.timeSpentInServerOutCounter.InstanceExists)
            {
                var timeSpentInServer = (int)this.timeSpentInServerInCounter.GetNextAverage() + (int)this.timeSpentInServerOutCounter.GetNextAverage();
                Counter.TimeInServerInAndOutAvg.RawValue = timeSpentInServer;
                this.feedbackControlSystem.SetTimeSpentInServer(timeSpentInServer); 
            }

            if (this.latencyMonitorUdp != null)
            {
                var latencyUdpAvg = this.latencyMonitorUdp.AverageLatencyMs;
                Counter.LatencyUdpAvg.RawValue = latencyUdpAvg;
                this.feedbackControlSystem.SetLatencyUdp(latencyUdpAvg);
            }

            if (this.latencyMonitorTcp != null)
            {
                var latencyTcpAvg = this.latencyMonitorTcp.AverageLatencyMs;
                Counter.LatencyTcpAvg.RawValue = latencyTcpAvg;
                this.feedbackControlSystem.SetLatencyTcp(latencyTcpAvg);
            }

            this.FeedbackLevel = this.feedbackControlSystem.Output;
            Counter.LoadLevel.RawValue = (byte)this.FeedbackLevel; 

            if (oldValue != this.FeedbackLevel)
            {
                if (log.IsInfoEnabled)
                {
                    log.InfoFormat("FeedbackLevel changed: old={0}, new={1}", oldValue, this.FeedbackLevel);
                }

                this.RaiseFeedbacklevelChanged();
            }
        }

        private void RaiseFeedbacklevelChanged()
        {
            var e = this.FeedbacklevelChanged;
            if (e != null)
            {
                e(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}


