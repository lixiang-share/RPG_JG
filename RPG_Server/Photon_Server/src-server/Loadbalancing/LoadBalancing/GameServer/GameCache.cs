// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameCache.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the GameCache type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.GameServer
{
    #region using directives

    using Lite;
    using Lite.Caching;

    #endregion

    public class GameCache : RoomCacheBase
    {
        public static readonly GameCache Instance = new GameCache();

        protected override Room CreateRoom(string roomId, params object[] args)
        {
            return new Game(roomId, this, GameServerSettings.Default.EmptyRoomLiveTime);
        }
    }
}