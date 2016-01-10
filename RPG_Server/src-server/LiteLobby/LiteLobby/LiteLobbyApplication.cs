// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LiteLobbyApplication.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Main photon application. This application is started from the photon server.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteLobby
{
    #region using directives

    using Lite;

    using Photon.SocketServer;

    #endregion

    /// <summary>
    /// Main photon application that is started from the photon server.
    /// This <see cref="LiteApplication"/> subclass creates a <see cref="LiteLobbyPeer"/> instead of a <see cref="LitePeer"/>, therefore the <see cref="LiteLobbyPeer"/> dispatches incoming <see cref="OperationRequest"/>s.
    /// </summary>
    public class LiteLobbyApplication : LiteApplication
    {
        /// <summary>
        /// Creates a <see cref="LiteLobbyPeer"/>.
        /// </summary>
        /// <param name="initRequest">
        /// The initialization request sent by the peer.
        /// </param>
        /// <returns>
        /// A new <see cref="LiteLobbyPeer"/> instance.
        /// </returns>
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new LiteLobbyPeer(initRequest.Protocol, initRequest.PhotonPeer);
        }
    }
}