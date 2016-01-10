// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActorParameter.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the ActorParameter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.Operations
{
    /// <summary>
    /// Well known actor properties (used as byte keys in actor-property hashtables).
    /// </summary>
    public enum ActorParameter : byte
    {
        PlayerName = 255
    }
}