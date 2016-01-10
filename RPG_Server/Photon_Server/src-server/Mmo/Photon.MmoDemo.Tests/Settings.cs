// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Tests
{
    /// <summary>
    /// The settings.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// The application id.
        /// </summary>
        public static readonly string ApplicationId = "MmoDemo";

        /// <summary>
        /// The connect timeout milliseconds.
        /// </summary>
        public static readonly int ConnectTimeoutMilliseconds = 4000;

        /// <summary>
        /// The server address.
        /// </summary>
        public static readonly string ServerAddress = "127.0.0.1:5055";
        ////public static readonly string ServerAddress = "127.0.0.1:4530";

        /// <summary>
        /// The use tcp.
        /// </summary>
        public static readonly bool UseTcp = false;

        /// <summary>
        /// The wait time.
        /// </summary>
        public static readonly int WaitTime = 5000;
    }
}