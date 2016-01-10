// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LiteLobbyGameCache.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The lite lobby game cache.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteLobby.Caching
{
    using Lite;
    using Lite.Caching;

    /// <summary>
    /// The lite lobby game cache.
    /// </summary>
    public class LiteLobbyGameCache : RoomCacheBase
    {
        /// <summary>
        /// The instance.
        /// </summary>
        public static readonly LiteLobbyGameCache Instance = new LiteLobbyGameCache();

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
        /// a <see cref="LiteLobbyGame"/>
        /// </returns>
        protected override Room CreateRoom(string roomId, params object[] args)
        {
            var lobbyName = (string)args[0];
            return new LiteLobbyGame(roomId, lobbyName, this);
        }
    }
}