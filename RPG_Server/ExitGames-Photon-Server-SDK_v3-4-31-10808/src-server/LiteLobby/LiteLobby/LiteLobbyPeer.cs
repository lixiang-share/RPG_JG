// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LiteLobbyPeer.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This <see cref="LitePeer" /> subclass handles join operations with different operation implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteLobby
{
    using Lite;
    using Lite.Caching;

    using LiteLobby.Caching;
    using LiteLobby.Operations;

    using Photon.SocketServer;

    using PhotonHostRuntimeInterfaces;

    /// <summary>
    ///   This <see cref = "LitePeer" /> subclass handles join operations with different operation implementation.
    /// </summary>
    public class LiteLobbyPeer : LitePeer
    {
        #region Constants and Fields

        /// <summary>
        ///   Games with this suffix will be handled as lobby-type.
        /// </summary>
        public static readonly string LobbySuffix = LobbySettings.Default.LobbySuffix;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "LiteLobbyPeer" /> class.
        /// </summary>
        /// <param name = "rpcProtocol">
        ///   The rpc Protocol.
        /// </param>
        /// <param name = "photonPeer">
        ///   The photon peer.
        /// </param>
        public LiteLobbyPeer(IRpcProtocol rpcProtocol, IPhotonPeer photonPeer)
            : base(rpcProtocol, photonPeer)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Joins the peer to a <see cref = "LiteLobbyGame" />.
        ///   Called by <see cref = "HandleJoinOperation">HandleJoinOperation</see>.
        /// </summary>
        /// <param name = "joinOperation">
        ///   The join operation.
        /// </param>
        /// <param name = "sendParameters">
        ///   The send Parameters.
        /// </param>
        protected virtual void HandleJoinGameWithLobby(JoinRequest joinOperation, SendParameters sendParameters)
        {
            // remove the peer from current game if the peer
            // allready joined another game
            this.RemovePeerFromCurrentRoom();

            RoomReference gameReference = null;
            if (joinOperation.ActorNr == 0)
            {
                // get a game reference from the game cache 
                // the game will be created by the cache if it does not exists allready
                gameReference = LiteLobbyGameCache.Instance.GetRoomReference(joinOperation.GameId, this, joinOperation.LobbyId);

            }
            else
            {
                if (!LiteLobbyGameCache.Instance.TryGetRoomReference(joinOperation.GameId, this, out gameReference))
                {
                    SendOperationResponse(
                        new OperationResponse
                        {
                            OperationCode = joinOperation.OperationRequest.OperationCode,
                            ReturnCode = -1,
                            DebugMessage = "the specified game is not exists."
                        },
                        sendParameters);
                    return;
                }
            }
            // save the game reference in peers state                    
            this.RoomReference = gameReference;

            // enqueue the operation request into the games execution queue
            gameReference.Room.EnqueueOperation(this, joinOperation.OperationRequest, sendParameters);
        }

        /// <summary>
        ///   Joins the peer to a <see cref = "LiteLobby" />.
        ///   Called by <see cref = "HandleJoinOperation">HandleJoinOperation</see>.
        /// </summary>
        /// <param name = "joinRequest">
        ///   The join operation.
        /// </param>
        /// <param name = "sendParameters">
        ///   The send Parameters.
        /// </param>
        protected virtual void HandleJoinLobby(JoinRequest joinRequest, SendParameters sendParameters)
        {
            // remove the peer from current game if the peer
            // allready joined another game
            this.RemovePeerFromCurrentRoom();

            // get a lobby reference from the game cache 
            // the lobby will be created by the cache if it does not exists allready
            RoomReference lobbyReference = LiteLobbyRoomCache.Instance.GetRoomReference(joinRequest.GameId, this);

            // save the lobby(room) reference in peers state                    
            this.RoomReference = lobbyReference;

            // enqueue the operation request into the games execution queue
            lobbyReference.Room.EnqueueOperation(this, joinRequest.OperationRequest, sendParameters);
        }

        /// <summary>
        ///   This override replaces the lite <see cref = "Lite.Operations.JoinRequest" /> with the lobby <see cref = "JoinRequest" /> and enables lobby support.
        /// </summary>
        /// <param name = "operationRequest">
        ///   The operation request.
        /// </param>
        /// <param name = "sendParameters">
        ///   The send Parameters.
        /// </param>
        protected override void HandleJoinOperation(OperationRequest operationRequest, SendParameters sendParameters)
        {
            // create join operation from the operation request
            var joinRequest = new JoinRequest(this.Protocol, operationRequest);
            if (!this.ValidateOperation(joinRequest, sendParameters))
            {
                return;
            }

            // check the type of join operation
            if (joinRequest.GameId.EndsWith(LobbySuffix))
            {
                // the game name ends with the lobby suffix
                // the client wants to join a lobby
                this.HandleJoinLobby(joinRequest, sendParameters);
            }
            else if (string.IsNullOrEmpty(joinRequest.LobbyId) == false)
            {
                // the lobbyId is set
                // the client wants to join a game with a lobby
                this.HandleJoinGameWithLobby(joinRequest, sendParameters);
            }
            else
            {
                base.HandleJoinOperation(operationRequest, sendParameters);
            }
        }

        #endregion
    }
}