// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server
{
    using Events;

    using Photon.SocketServer;

    using SocketServer.Mmo;

    /// <summary>
    /// The settings.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Initializes static members of the <see cref="Settings"/> class.
        /// </summary>
        static Settings()
        {
            ItemAutoUnsubcribeDelay = 5000;

            // just the radar
            RadarEventChannel = 2;

            // movement etc
            ItemEventChannel = 0;

            MaxLockWaitTimeMilliseconds = 1000;
            RadarUpdateInterval = 10000;

            DiagnosticsEventChannel = 2;
            DiagnosticsEventReliability = Reliability.Reliable;
        }

        /// <summary>
        /// Gets or sets the diagnostics channel number.
        /// This property determines which enet channel to use when sending event <see cref="CounterDataEvent"/> to the client.
        /// Default value is #2.
        /// </summary>
        public static byte DiagnosticsEventChannel { get; set; }

        /// <summary>
        /// Gets or sets the diagnostics event reliability.
        /// This property determines if event <see cref="CounterDataEvent"/> is sent reliable or unreliable to the client.
        /// Defaut value is <see cref="Reliability.Reliable"/>.
        /// </summary>
        public static Reliability DiagnosticsEventReliability { get; set; }

        /// <summary>
        /// Gets or sets the maximum unsubscribe delay of items that leave the outer view threshold.
        /// Default value is 5 seconds.
        /// </summary>
        public static int ItemAutoUnsubcribeDelay { get; set; }

        /// <summary>
        /// Gets or sets the enet channel used for events that are published with the <see cref="Item.EventChannel">Item.EventChannel</see>.
        /// Default value is 0.
        /// </summary>
        public static byte ItemEventChannel { get; set; }

        /// <summary>
        /// Gets or sets the maxium lock wait time for the lock protected dictionaries <see cref="MmoItemCache"/> and <see cref="MmoWorldCache"/>.
        /// Default is 1 second.
        /// </summary>
        public static int MaxLockWaitTimeMilliseconds { get; set; }
   
        /// <summary>
        /// Gets or sets the enet channel used for event that are published with the <see cref="MmoRadar"/>.
        /// Default is 2.
        /// </summary>
        public static byte RadarEventChannel { get; set; }

        /// <summary>
        /// Gets or sets the interval the <see cref="MmoRadar"/> uses to publish position changes with a <see cref="RadarUpdate"/> event.
        /// Default is 10 seconds.
        /// </summary>
        public static int RadarUpdateInterval { get; set; }
    }
}