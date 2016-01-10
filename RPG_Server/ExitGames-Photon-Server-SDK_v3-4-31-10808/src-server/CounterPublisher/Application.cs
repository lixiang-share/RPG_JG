// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Application.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The application.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.CounterPublisher
{
    #region using directives

    using System;
    using System.IO;

    using ExitGames.Logging;
    using ExitGames.Logging.Log4Net;

    using log4net.Config;

    using Photon.SocketServer;
    using Photon.SocketServer.Diagnostics;
    using Photon.SocketServer.Diagnostics.Configuration;

    #endregion

    /// <summary>
    ///   The application.
    /// </summary>
    public class Application : ApplicationBase
    {
        /// <summary>Logger for debug.</summary>
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        // ReSharper disable UnaccessedField.Local
        #region Constants and Fields

        /// <summary>
        ///   The counter publisher.
        /// </summary>
        private CounterPublisher counterPublisher;

        #endregion

        // ReSharper restore UnaccessedField.Local
        #region Methods

        /// <summary>
        ///   Returns null: No connections allowed.
        /// </summary>
        /// <param name = "initRequest">
        ///   The initialization request sent by the peer.
        /// </param>
        /// <returns>
        ///   Always null.
        /// </returns>
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return null;
        }

        /// <summary>
        ///   The setup.
        /// </summary>
        protected override void Setup()
        {
            // log4net
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(this.ApplicationRootPath, "log");
            string path = Path.Combine(this.BinaryPath, "log4net.config");
            var file = new FileInfo(path);
            if (file.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(file);
            }

            try
            {
                if (PhotonSettings.Default.CounterPublisher.Enabled)
                {
                    this.counterPublisher = new CounterPublisher(PhotonSettings.Default.CounterPublisher);
                    this.counterPublisher.AddStaticCounterClass(typeof(SystemCounter), "System");
                    this.counterPublisher.AddStaticCounterClass(typeof(SocketServerCounter), "SocketServer");

                    if (log.IsDebugEnabled)
                    {
                        log.Debug("Starting counter publisher");
                    }

                    this.counterPublisher.Start();
                }
            }
            catch (Exception ex)
            {                
                log.Error(ex);
            }
        }

        /// <summary>
        ///   The tear down.
        /// </summary>
        protected override void TearDown()
        {
        }

        #endregion
    }
}