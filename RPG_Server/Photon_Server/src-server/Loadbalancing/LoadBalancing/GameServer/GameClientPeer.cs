// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameClientPeer.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the GamePeer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.GameServer
{
    #region using directives

    using System;

    using ExitGames.Logging;

    using Lite;
    using Lite.Caching;
    using Lite.Messages;
    using Lite.Operations;

    using Photon.LoadBalancing.Operations;
    using Photon.SocketServer;

    using OperationCode = Photon.LoadBalancing.Operations.OperationCode;

    #endregion

    public class GameClientPeer : LitePeer
    {
        #region Constants and Fields

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly GameApplication application;

        #endregion

        #region Constructors and Destructors

        public GameClientPeer(InitRequest initRequest, GameApplication application)
            : base(initRequest.Protocol, initRequest.PhotonPeer)
        {
            this.application = application;
            this.PeerId = string.Empty;

            if (GameApplication.Instance.AppStatsPublisher != null)
            {
                GameApplication.Instance.AppStatsPublisher.IncrementPeerCount();
            }
        }

        #endregion

        #region Properties

        public string PeerId { get; protected set; }

        public DateTime LastActivity { get; protected set; }

        public byte LastOperation { get; protected set; }

        #endregion

        #region Public Methods

        public void OnJoinFailed(ErrorCode result)
        {
            this.RequestFiber.Enqueue(() => this.OnJoinFailedInternal(result));
        }

        public override string ToString()
        {
            return string.Format(
                "{0}: {1}",
                this.GetType().Name,
                string.Format(
                    "PID {0}, IsConnected: {1}, IsDisposed: {2}, Last Activity: Operation {3} at UTC {4} in Room {7}, IP {5}:{6}, ",
                    this.ConnectionId,
                    this.Connected,
                    this.Disposed,
                    this.LastOperation,
                    this.LastActivity,
                    this.RemoteIP,
                    this.RemotePort,
                    this.RoomReference == null ? string.Empty : this.RoomReference.Room.Name)); 
        }

        #endregion

        #region Methods

        protected override RoomReference GetRoomReference(JoinRequest joinRequest)
        {
            throw new NotSupportedException("Use TryGetRoomReference or TryCreateRoomReference instead.");
        }

        protected virtual void HandleCreateGameOperation(OperationRequest operationRequest, SendParameters sendParameters)
        {
            // The JoinRequest from the Lite application is also used for create game operations to support all feaures 
            // provided by Lite games. 
            // The only difference is the operation code to prevent games created by a join operation. 
            // On "LoadBalancing" game servers games must by created first by the game creator to ensure that no other joining peer 
            // reaches the game server before the game is created.
            var createRequest = new JoinGameRequest(this.Protocol, operationRequest);
            if (this.ValidateOperation(createRequest, sendParameters) == false)
            {
                return;
            }

            // remove peer from current game
            this.RemovePeerFromCurrentRoom();

            // try to create the game
            RoomReference gameReference;
            if (this.TryCreateRoom(createRequest.GameId, out gameReference) == false)
            {
                var response = new OperationResponse
                    {
                        OperationCode = (byte)OperationCode.CreateGame,
                        ReturnCode = (short)ErrorCode.GameIdAlreadyExists,
                        DebugMessage = "Game already exists"
                    };

                this.SendOperationResponse(response, sendParameters);
                return;
            }

            // save the game reference in the peers state                    
            this.RoomReference = gameReference;

            // finally enqueue the operation into game queue
            gameReference.Room.EnqueueOperation(this, operationRequest, sendParameters);
        }

        /// <summary>
        ///   Handles the <see cref = "JoinRequest" /> to enter a <see cref = "Game" />.
        ///   This method removes the peer from any previously joined room, finds the room intended for join
        ///   and enqueues the operation for it to handle.
        /// </summary>
        /// <param name = "operationRequest">
        ///   The operation request to handle.
        /// </param>
        /// <param name = "sendParameters">
        ///   The send Parameters.
        /// </param>
        protected virtual void HandleJoinGameOperation(OperationRequest operationRequest, SendParameters sendParameters)
        {
            // create join operation
            var joinRequest = new JoinGameRequest(this.Protocol, operationRequest);
            if (this.ValidateOperation(joinRequest, sendParameters) == false)
            {
                return;
            }

            // remove peer from current game
            this.RemovePeerFromCurrentRoom();

            // try to get the game reference from the game cache 
            RoomReference gameReference;
            if (joinRequest.CreateIfNotExists)
            {
                gameReference = this.GetOrCreateRoom(joinRequest.GameId);
            }
            else
            {
                if (this.TryGetRoomReference(joinRequest.GameId, out gameReference) == false)
                {
                    this.OnRoomNotFound(joinRequest.GameId);

                    var response = new OperationResponse
                    {
                        OperationCode = (byte)OperationCode.JoinGame,
                        ReturnCode = (short)ErrorCode.GameIdNotExists,
                        DebugMessage = "Game does not exists"
                    };

                    this.SendOperationResponse(response, sendParameters);
                    return;
                }
            }

            // save the game reference in the peers state                    
            this.RoomReference = gameReference;

            // finally enqueue the operation into game queue
            gameReference.Room.EnqueueOperation(this, operationRequest, sendParameters);
        }

        protected virtual void HandleDebugGameOperation(OperationRequest operationRequest, SendParameters sendParameters)
        {
            var debugRequest = new DebugGameRequest(this.Protocol, operationRequest);
            if (this.ValidateOperation(debugRequest, sendParameters) == false)
            {
                return;
            }

            string debug = string.Format("DebugGame called from PID {0}. {1}", this.ConnectionId, this.GetRoomCacheDebugString(debugRequest.GameId));
            operationRequest.Parameters.Add((byte)ParameterCode.Info, debug);


            if (this.RoomReference == null)
            {
                Room room; 
                // get a room without obtaining a reference:
                if (!this.TryGetRoomWithoutReference(debugRequest.GameId, out room))
                {
                    var response = new OperationResponse
                        {
                            OperationCode = (byte)OperationCode.DebugGame,
                            ReturnCode = (short)ErrorCode.GameIdNotExists,
                            DebugMessage = "Game does not exist in RoomCache"
                        };


                    this.SendOperationResponse(response, sendParameters);
                    return;
                }

                room.EnqueueOperation(this, operationRequest, sendParameters);
            }
            else
            {
                this.RoomReference.Room.EnqueueOperation(this, operationRequest, sendParameters);
            }
        }

        protected virtual void OnRoomNotFound(string gameId)
        {
            this.application.MasterPeer.RemoveGameState(gameId);
        }

        protected override void OnDisconnect(PhotonHostRuntimeInterfaces.DisconnectReason reasonCode, string reasonDetail)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("OnDisconnect: conId={0}, reason={1}, detail={2}", this.ConnectionId, reasonCode, reasonDetail);
            }

            if (GameApplication.Instance.AppStatsPublisher != null)
            {
                GameApplication.Instance.AppStatsPublisher.DecrementPeerCount();
            }

            if (this.RoomReference == null)
            {
                return;
            }

            var message = new RoomMessage((byte)GameMessageCodes.RemovePeerFromGame, this);
            this.RoomReference.Room.EnqueueMessage(message);
            this.RoomReference.Dispose();
            this.RoomReference = null;
        }

        protected override void OnOperationRequest(OperationRequest request, SendParameters sendParameters)
        {
            if (log.IsDebugEnabled)
            {
                if (request.OperationCode != (byte)Lite.Operations.OperationCode.RaiseEvent)
                {
                    log.DebugFormat("OnOperationRequest: conId={0}, opCode={1}", this.ConnectionId, request.OperationCode);
                }
            }

            this.LastActivity = DateTime.UtcNow;
            this.LastOperation = request.OperationCode;


            switch (request.OperationCode)
            {
                case (byte)OperationCode.Authenticate:
                    this.HandleAuthenticateOperation(request, sendParameters);
                    return;

                case (byte)OperationCode.CreateGame:
                    this.HandleCreateGameOperation(request, sendParameters);
                    return;

                case (byte)OperationCode.JoinGame:
                    this.HandleJoinGameOperation(request, sendParameters);
                    return;

                case (byte)Lite.Operations.OperationCode.Leave:
                    this.HandleLeaveOperation(request, sendParameters);
                    return;

                case (byte)Lite.Operations.OperationCode.Ping:
                    this.HandlePingOperation(request, sendParameters);
                    return;

                case (byte)OperationCode.DebugGame:
                    this.HandleDebugGameOperation(request, sendParameters);
                    return;

                case (byte)Lite.Operations.OperationCode.RaiseEvent:
                case (byte)Lite.Operations.OperationCode.GetProperties:
                case (byte)Lite.Operations.OperationCode.SetProperties:
                case (byte)Lite.Operations.OperationCode.ChangeGroups:
                    this.HandleGameOperation(request, sendParameters);
                    return;
            }

            string message = string.Format("Unknown operation code {0}", request.OperationCode);
            var response = new OperationResponse { ReturnCode = (short)ErrorCode.OperationDenied, DebugMessage = message, OperationCode = request.OperationCode };
            this.SendOperationResponse(response, sendParameters);
        }

        protected virtual RoomReference GetOrCreateRoom(string gameId)
        {
            return GameCache.Instance.GetRoomReference(gameId, this);
        }

        protected virtual bool TryCreateRoom(string gameId, out RoomReference roomReference)
        {
            return GameCache.Instance.TryCreateRoom(gameId, this, out roomReference);
        }

        protected virtual bool TryGetRoomReference(string gameId, out RoomReference roomReference)
        {
            return GameCache.Instance.TryGetRoomReference(gameId, this, out roomReference);
        }

        protected virtual bool TryGetRoomWithoutReference(string gameId, out Room room)
        {
            return GameCache.Instance.TryGetRoomWithoutReference(gameId, out room); 
        }

        public virtual string GetRoomCacheDebugString(string gameId)
        {
            return GameCache.Instance.GetDebugString(gameId); 
        }

        protected virtual void HandleAuthenticateOperation(OperationRequest operationRequest, SendParameters sendParameters)
        {
            var request = new AuthenticateRequest(this.Protocol, operationRequest);
            if (this.ValidateOperation(request, sendParameters) == false)
            {
                return;
            }

            if (request.UserId != null)
            {
                this.PeerId = request.UserId;
            }

            var response = new OperationResponse { OperationCode = operationRequest.OperationCode };
            this.SendOperationResponse(response, sendParameters);
        }

        private void OnJoinFailedInternal(ErrorCode result)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("OnJoinFailed: {0}", result);
            }

            // if join operation failed -> release the reference to the room
            if (result != ErrorCode.Ok && this.RoomReference != null)
            {
                this.RoomReference.Dispose();
                this.RoomReference = null;
            }
        }
        #endregion
    }
}