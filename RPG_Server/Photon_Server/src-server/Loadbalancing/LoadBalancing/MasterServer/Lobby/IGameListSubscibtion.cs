// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGameListSubscibtion.cs" company="">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.MasterServer.Lobby
{
    using System;
    using System.Collections;

    public interface IGameListSubscibtion : IDisposable
    {
        Hashtable GetGameList();
    }
}
