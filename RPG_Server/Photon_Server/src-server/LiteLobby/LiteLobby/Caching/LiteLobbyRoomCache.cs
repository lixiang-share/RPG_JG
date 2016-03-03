// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LiteLobbyRoomCache.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The lite lobby room cache.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteLobby.Caching
{
    using Lite;
    using Lite.Caching;

    /// <summary>
    /// The lite lobby room cache.
    /// </summary>
    public class LiteLobbyRoomCache : RoomCacheBase
    {
        /// <summary>
        /// The instance.
        /// </summary>
        public static readonly LiteLobbyRoomCache Instance = new LiteLobbyRoomCache();

        /// <summary>
        /// The create room.
        /// </summary>
        /// <param name="roomId">
        /// The room id.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// a <see cref="LiteLobbyRoom"/>  
        /// </returns>
        protected override Room CreateRoom(string roomId, params object[] args)
        {
            return new LiteLobbyRoom(roomId, this);
        }
    }
}