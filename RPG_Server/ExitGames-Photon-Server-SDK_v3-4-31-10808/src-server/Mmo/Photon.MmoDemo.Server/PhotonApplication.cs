// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhotonApplication.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This <see cref="Application" /> subclass creates <see cref="MmoPeer">MmoPeers</see> and has a <see cref="CounterSamplePublisher" /> that is used to send diagnostic values to the client.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server
{
    using System;
    using System.IO;

    using ExitGames.Diagnostics.Monitoring;
    using ExitGames.Logging;
    using ExitGames.Logging.Log4Net;

    using log4net.Config;

    using SocketServer;
    using SocketServer.Diagnostics;
    
    /// <summary>
    /// This <see cref="ApplicationBase"/> subclass creates <see cref="MmoPeer">MmoPeers</see> and has a <see cref="CounterSamplePublisher"/> that is used to send diagnostic values to the client.
    /// </summary>
    public class PhotonApplication : ApplicationBase
    {
        /// <summary>
        /// The <see cref="CounterSamplePublisher"/> used to publish diagnostic counters.
        /// </summary>
        public static readonly CounterSamplePublisher CounterPublisher;

        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes static members of the <see cref="PhotonApplication"/> class.
        /// </summary>
        static PhotonApplication()
        {
            CounterPublisher = new CounterSamplePublisher(1);
        }

        /// <summary>
        /// Initializes the application.
        /// </summary>
        public static void Initialize()
        {
            ////// counter event channel
            ////Settings.DiagnosticsEventChannel = 2;
            ////Settings.DiagnosticsEventReliability = Reliability.Reliable;
            CounterPublisher.AddCounter(new CpuUsageCounterReader(), "Cpu");
            CounterPublisher.AddCounter(PhotonCounter.EventSentPerSec, "Events/sec");
            CounterPublisher.AddCounter(PhotonCounter.InitPerSec, "Connections/sec");
            CounterPublisher.AddCounter(PhotonCounter.OperationReceivePerSec, "Operations/sec");

            ////CounterPublisher.AddCounter("Ops < 50ms", Peer.OperationsFast);
            CounterPublisher.AddCounter(PhotonCounter.OperationsMiddle, "Ops > 50ms");
            CounterPublisher.AddCounter(PhotonCounter.OperationsSlow, "Ops > 200ms");
            CounterPublisher.AddCounter(OperationsMaxTimeCounter.Instance, "Ops max ms");

            // debug
            ////CounterPublisher.AddCounter("Msg sent", ItemMessage.CounterSend);
            ////CounterPublisher.AddCounter("Msg received", ItemMessage.CounterReceive);
            ////CounterPublisher.AddCounter("Events published", ItemEventMessage.CounterEventSend);

            // almost equal to Event/sec
            ////CounterPublisher.AddCounter("int. event msg rec.", ItemEventMessage.CounterEventReceive);
            CounterPublisher.Start();
        }

        /// <summary>
        /// Creates a new <see cref="MmoPeer"/>.
        /// </summary>
        /// <param name="initRequest">
        /// The init Request.
        /// </param>
        /// <returns>
        /// A new <see cref="MmoPeer"/>
        /// </returns>
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new MmoPeer(initRequest.Protocol, initRequest.PhotonPeer);
        }

        /// <summary>
        /// Initializes log4net and counters for the application.
        /// </summary>
        protected override void Setup()
        {
            // log4net
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(this.ApplicationRootPath, "log");
            var configFileInfo = new FileInfo(Path.Combine(this.BinaryPath, "log4net.config"));
            if (configFileInfo.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(configFileInfo);
            }

            AppDomain.CurrentDomain.UnhandledException += AppDomain_OnUnhandledException;

            Initialize();
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        protected override void TearDown()
        {
        }
       
        /// <summary>
        /// Logs unhandled exceptions.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private static void AppDomain_OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            log.Error(e.ExceptionObject);
        }
    }
}