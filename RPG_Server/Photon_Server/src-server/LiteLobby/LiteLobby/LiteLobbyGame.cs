// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LiteLobbyGame.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   A <see cref="LiteGame" /> that updates the <see cref="LiteLobbyRoom" /> when a <see cref="LiteLobbyPeer" /> joins or leaves.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteLobby
{
    #region using directives

    using Lite;
    using Lite.Caching;
    using Lite.Messages;
    using Lite.Operations;

    using LiteLobby.Caching;
    using LiteLobby.Messages;

    using Photon.SocketServer;

    #endregion

    /// <summary>
    ///   A <see cref = "LiteGame" /> that updates the <see cref = "LiteLobbyRoom" /> when a <see cref = "LiteLobbyPeer" /> joins or leaves.
    /// </summary>
    public class LiteLobbyGame : LiteGame
    {
        #region Constants and Fields

        /// <summary>
        ///   This <see cref = "RoomReference" /> is the link to the <see cref = "LiteLobbyRoom" /> that needs to be updated when players join or leave.
        /// </summary>
        private readonly RoomReference lobbyReference;
        
        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "LiteLobbyGame" /> class.
        /// </summary>
        /// <param name = "gameName">
        ///   The name of the game.
        /// </param>
        /// <param name = "lobbyName">
        ///   The name of the lobby for the game.
        /// </param>
        public LiteLobbyGame(string gameName, string lobbyName, RoomCacheBase parent)
            : base(gameName, parent)
        {
            // get the reference to the lobby
            this.lobbyReference = LiteLobbyRoomCache.Instance.GetRoomReference(lobbyName, null);
        }

        #endregion

        #region Properties
     
        /// <summary>
        ///   Gets the lobby for this game instance.
        /// </summary>
        /// <value>The lobby.</value>
        protected Room Lobby
        {
            get
            {
                return this.lobbyReference.Room;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Disposes the <see cref = "lobbyReference" />.
        /// </summary>
        /// <param name = "disposing">
        ///   The disposing.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.lobbyReference.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        ///   Updates the lobby when an <see cref = "Actor" /> joins.
        /// </summary>
        /// <param name = "peer">
        ///   The peer.
        /// </param>
        /// <param name = "joinRequest">
        ///   The join operation.
        /// </param>
        /// <param name = "sendParamters">
        ///   The send Paramters.
        /// </param>
        /// <returns>
        ///   The newly created (joined) <see cref = "Actor" />.
        /// </returns>
        protected override Actor HandleJoinOperation(LitePeer peer, JoinRequest joinRequest, SendParameters sendParamters)
        {
            Actor actor = base.HandleJoinOperation(peer, joinRequest, sendParamters);
            if (actor != null)
            {
                this.UpdateLobby();
            }

            return actor;
        }

        /// <summary>
        ///   Updates the lobby when an <see cref = "Actor" /> leaves (disconnect, <see cref = "LeaveRequest" />, <see cref = "JoinRequest" /> for another game).
        /// </summary>
        /// <param name = "peer">
        ///   The <see cref = "LitePeer" /> to remove.
        /// </param>
        /// <param name="leaveRequest">
        ///   The <see cref="LeaveRequest"/> sent by the peer or null if the peer have been disconnected without sending a leave request.
        /// </param>
        /// <returns>
        ///   The actore number of the removed actor.
        ///   If the specified peer does not exists -1 will be returned.
        /// </returns>
        protected override int RemovePeerFromGame(LitePeer peer, LeaveRequest leaveRequest)
        {
            int actorNr = base.RemovePeerFromGame(peer, leaveRequest);
            this.UpdateLobby();
            return actorNr;
        }

        /// <summary>
        ///   Updates the lobby if necessary.
        /// </summary>
        private void UpdateLobby()
        {
            if (this.lobbyReference == null)
            {
                return;
            }

            // if a game is listed, find the lobby game and send it a message to 
            // de-list or update the list info
            RoomMessage message = this.Actors.Count == 0
                                      ? new RoomMessage((byte)LobbyMessageCode.RemoveGame, new[] { this.Name, "0" })
                                      : new RoomMessage((byte)LobbyMessageCode.AddGame, new[] { this.Name, this.Actors.Count.ToString() });

            this.lobbyReference.Room.EnqueueMessage(message);
        }

        #endregion
    }
}