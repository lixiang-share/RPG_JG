// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LiteLobbyRoom.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The lite lobby room.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteLobby
{
    #region using directives

    using System;
using System.Collections;
using Lite;
using Lite.Caching;
using Lite.Events;
using Lite.Messages;
using Lite.Operations;
using LiteLobby.Messages;
using LiteLobby.Operations;
using Photon.SocketServer;
using JoinRequest = LiteLobby.Operations.JoinRequest;

    #endregion

    /// <summary>
    /// A <see cref="LiteGame"/> that does not support event exchange between the joined <see cref="Actor"/>s.
    /// Instead it tracks all existing <see cref="LiteLobbyGame"/>s and informs joined actors about it. 
    /// </summary>
    public class LiteLobbyRoom : LiteGame
    {
        /// <summary>Hashtable containing all roomes for this lobby.</summary>
        private readonly Hashtable roomList;

        /// <summary>Hashtable containing all roomes which changed since last update.</summary>
        private Hashtable changedRoomList;

        // ReSharper disable UnaccessedField.Local

        /// <summary>
        /// The schedule.
        /// </summary>
        private IDisposable schedule;

        // ReSharper restore UnaccessedField.Local

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteLobbyRoom"/> class.
        /// </summary>
        /// <param name="lobbyName">
        /// Name of the lobby.
        /// </param>
        /// <param name="roomCache">
        ///   The <see cref="RoomCacheBase"/> instance to which the room belongs.
        /// </param>
        public LiteLobbyRoom(string lobbyName, RoomCacheBase roomCache)
            : base(lobbyName, roomCache)
        {
            this.roomList = new Hashtable();
            this.changedRoomList = new Hashtable();

            // schedule sending the change list
            this.SchedulePublishChanges();
        }

        /// <summary>
        /// Dispatches the <see cref="JoinRequest"/> different from the base <see cref="LiteGame.ExecuteOperation">LiteGame.ExecuteOperation</see>.
        /// </summary>
        /// <param name="peer">
        /// The peer.
        /// </param>
        /// <param name="operationRequest">
        /// The operation request to execute.
        /// </param>
        /// <param name="sendParameters">
        /// The send Parameters.
        /// </param>
        protected override void ExecuteOperation(LitePeer peer, OperationRequest operationRequest, SendParameters sendParameters)
        {
            switch ((OperationCode)operationRequest.OperationCode)
            {
                case OperationCode.Join:
                    var joinOperation = new JoinRequest(peer.Protocol, operationRequest);
                    if (peer.ValidateOperation(joinOperation, sendParameters) == false)
                    {
                        return;
                    }

                    this.HandleJoinOperation(peer, joinOperation, sendParameters);
                    return;
            }

            base.ExecuteOperation(peer, operationRequest, sendParameters);
        }

        /// <summary>
        /// Executes the base <see cref="LiteGame.HandleJoinOperation">HandleJoinOperation</see> and then sends an updated game list to all <see cref="Actor"/>s in the lobby.
        /// </summary>
        /// <param name="peer">
        /// The peer.
        /// </param>
        /// <param name="joinRequest">
        /// The join request.
        /// </param>
        /// <param name="sendParameters">
        /// The send Parameters.
        /// </param>
        /// <returns>
        /// The new actor
        /// </returns>
        protected virtual Actor HandleJoinOperation(LitePeer peer, JoinRequest joinRequest, SendParameters sendParameters)
        {
            Actor actor = base.HandleJoinOperation(peer, joinRequest, sendParameters);
            if (actor != null)
            {
                this.PublishGameList(actor);
            }

            return actor;
        }

        /// <summary>
        /// Adds 3 new <see cref="IMessage.Action">message actions</see> to the <see cref="LiteGame"/>:
        /// <list type="bullet">
        /// <item><see cref="LobbyMessageCode.AddGame"/>: Sent from <see cref="LiteLobbyGame"/> when a player joins.</item>
        /// <item><see cref="LobbyMessageCode.RemoveGame"/>: Sent from <see cref="LiteLobbyGame"/> when a player leaves.</item>
        /// <item><see cref="LobbyMessageCode.PublishChangeList"/>: Frequenyly sent with a timer to keep the clients updated.</item>
        /// </list>
        /// </summary>
        /// <param name="message">
        /// Message to process.
        /// </param>
        protected override void ProcessMessage(IMessage message)
        {
            // this switch only handles the Lobby-specific messages.
            // all other messages will be handled by the base class.
            switch ((LobbyMessageCode)message.Action)
            {
                case LobbyMessageCode.AddGame:
                    this.GameListAddOrUpdateGameId((string[])message.Message);
                    return;

                case LobbyMessageCode.RemoveGame:
                    this.GameListRemoveGameId((string[])message.Message);
                    return;

                case LobbyMessageCode.PublishChangeList:
                    this.PublishChangeList();
                    return;
            }

            base.ProcessMessage(message);
        }

        /// <summary>
        /// This override disables the event publishing.
        /// </summary>
        /// <param name="peer">
        /// The peer.
        /// </param>
        /// <param name="joinRequest">
        /// The join request.
        /// </param>
        protected override void PublishJoinEvent(LitePeer peer, Lite.Operations.JoinRequest joinRequest)
        {
            // lobbies don't publish a join event to the clients
        }

        /// <summary>
        /// This override disables the event publishing.
        /// </summary>
        /// <param name="peer">
        /// The peer.
        /// </param>
        /// <param name="leaveOperation">
        /// The leave operation.
        /// </param>
        protected override void PublishLeaveEvent(Actor actor, LeaveRequest leaveOperation)
        {
            // lobbies don't publish a leave event to the clients
        }

        /// <summary>
        /// This override disables the event publishing.
        /// </summary>
        /// <param name="peer">
        /// The peer.
        /// </param>
        /// <param name="leaveRequest">
        /// The <see cref="LeaveRequest"/> sent by the peer or null if the peer have been disconnected without sending a leave request.
        /// </param>
        /// <returns>
        /// the actor number.
        /// </returns>
        protected override int RemovePeerFromGame(LitePeer peer, LeaveRequest leaveRequest)
        {
            Actor actor = this.Actors.GetActorByPeer(peer);

            if (this.Actors.Remove(actor) == false)
            {
                if (Log.IsWarnEnabled)
                {
                    Log.WarnFormat("RemovePeerFromGame - Actor to remove not found for peer: {0}", peer.ConnectionId);
                }

                return 0;
            }

            return actor.ActorNr;
        }

        /// <summary>
        /// Copies gameInfo into a dictionary that is used as a list of known games.
        /// </summary>
        /// <param name="gameData">
        /// The list of known games.
        /// </param>
        /// <param name="gameInfo">
        /// The array content but the first item are saved in <paramref name="gameData"/>. 
        /// The key is the first item in gameInfo.
        /// </param>
        private static void AddInfoToList(IDictionary gameData, string[] gameInfo)
        {
            if (gameInfo.Length <= 2)
            {
                gameData[gameInfo[0]] = gameInfo[1];
            }
            else
            {
                var info = new string[gameInfo.Length - 1];
                Array.Copy(gameInfo, 1, info, 0, info.Length);
                gameData[gameInfo[0]] = info;
            }
        }

        /// <summary>
        /// Adds the new <paramref name="gameInfo"/> information to the known rooms list.
        /// </summary>
        /// <param name="gameInfo">
        /// The new or updated game info.
        /// </param>
        private void GameListAddOrUpdateGameId(string[] gameInfo)
        {
            AddInfoToList(this.roomList, gameInfo);
            AddInfoToList(this.changedRoomList, gameInfo); // this is a change, store it and send as update to clients
        }

        /// <summary>
        /// Removes info about a <see cref="LiteLobbyGame"/> from the known rooms list.
        /// </summary>
        /// <param name="gameInfo">
        /// The game info.
        /// </param>
        private void GameListRemoveGameId(string[] gameInfo)
        {
            this.roomList.Remove(gameInfo[0]);

            // this is a change, store it and send as update to clients
            AddInfoToList(this.changedRoomList, gameInfo);
        }

        /// <summary>
        /// Sends the change list to all users in the lobby and then clears it.
        /// </summary>
        private void PublishChangeList()
        {
            if (this.changedRoomList.Count > 0 && this.Actors.Count > 0)
            {
                var customEvent = new CustomEvent(0, (byte)LiteLobbyEventCode.GameListUpdate, this.changedRoomList);

                // PublishEvent(hashtable) uses a reference to hashtable, which means you can not clear it (see below)
                this.PublishEvent(customEvent, this.Actors, new SendParameters { Unreliable = true });
            }

            this.changedRoomList = new Hashtable(); // creating a new hashtable, as PublishEvent() uses just a ref to the data to be sent
            this.SchedulePublishChanges();
        }

        /// <summary>
        /// Sends the complete list of games to one actor in the lobby.
        /// </summary>
        /// <param name="actor">
        /// The actor.
        /// </param>
        private void PublishGameList(Actor actor)
        {
            // Room list needs to be cloned because it is serialized in other thread and is changed at the same time
            var customEvent = new CustomEvent(actor.ActorNr, (byte)LiteLobbyEventCode.GameList, (Hashtable)this.roomList.Clone());
            this.PublishEvent(customEvent, actor, new SendParameters { Unreliable = true });
        }

        /// <summary>
        /// Schedules a broadcast of all changes.
        /// </summary>
        private void SchedulePublishChanges()
        {
            var message = new RoomMessage((byte)LobbyMessageCode.PublishChangeList);
            this.schedule = this.ScheduleMessage(message, LobbySettings.Default.LobbyUpdateIntervalMs);
        }
    }
}