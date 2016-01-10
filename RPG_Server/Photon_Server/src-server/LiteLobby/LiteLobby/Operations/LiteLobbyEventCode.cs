// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LiteLobbyEventCode.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The lite lobby event code.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteLobby.Operations
{
    /// <summary>
    /// defines the event codes used by the lite lobby application.
    /// </summary>
    public enum LiteLobbyEventCode : byte
    {
        /// <summary>
        /// Event code for the game list event.
        /// </summary>
        GameList = 252, 

        /// <summary>
        /// event code for the update game list event.
        /// </summary>
        GameListUpdate = 251, 
    }
}