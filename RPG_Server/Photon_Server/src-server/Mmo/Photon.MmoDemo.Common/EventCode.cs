// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventCode.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This enumeration contains all known event codes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Common
{
    /// <summary>
    /// This enumeration contains all known event codes.
    /// </summary>
    public enum EventCode : byte
    {
        /// <summary>
        /// The actor leave.
        /// </summary>
        ItemDestroyed = 1, 

        /// <summary>
        /// The actor move.
        /// </summary>
        ItemMoved, 

        /// <summary>
        /// The item properties set.
        /// </summary>
        ItemPropertiesSet, 

        /// <summary>
        /// The item generic.
        /// </summary>
        ItemGeneric, 

        /// <summary>
        /// The world exited.
        /// </summary>
        WorldExited, 

        /// <summary>
        /// The item subscribed.
        /// </summary>
        ItemSubscribed, 

        /// <summary>
        /// The item unsubscribed.
        /// </summary>
        ItemUnsubscribed, 

        /// <summary>
        /// The item properties.
        /// </summary>
        ItemProperties, 

        /// <summary>
        /// The radar update.
        /// </summary>
        RadarUpdate, 

        /// <summary>
        /// The counter data.
        /// </summary>
        CounterData
    }
}