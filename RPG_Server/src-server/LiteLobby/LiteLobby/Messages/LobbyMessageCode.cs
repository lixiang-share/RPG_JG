// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LobbyMessageCode.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines message codes for <see cref="LiteLobbyRoom" /> rooms.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteLobby.Messages
{
    /// <summary>
    /// Defines message codes for <see cref="LiteLobbyRoom"/> rooms.
    /// </summary>
    public enum LobbyMessageCode : byte
    {
        /// <summary>
        /// The add game.
        /// </summary>
        AddGame = 4, 

        /// <summary>
        /// The remove game.
        /// </summary>
        RemoveGame = 5, 

        /// <summary>
        /// The publish change list.
        /// </summary>
        PublishChangeList = 6, 
    }
}