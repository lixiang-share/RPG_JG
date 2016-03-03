// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationCode.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This enumeration contains known operation codes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Common
{
    /// <summary>
    /// This enumeration contains known operation codes.
    /// </summary>
    public enum OperationCode : byte
    {
        /// <summary>
        /// The nil (nothing).
        /// </summary>
        Nil = 0, 

        /// <summary>
        /// Create world operation code
        /// </summary>
        CreateWorld = 90, 

        /// <summary>
        /// The enter world.
        /// </summary>
        EnterWorld = 91, 

        /// <summary>
        /// The exit world.
        /// </summary>
        ExitWorld = 92, 

        /// <summary>
        /// The move code.
        /// </summary>
        Move = 93, 

        /// <summary>
        /// The raise generic event.
        /// </summary>
        RaiseGenericEvent = 94, 

        /// <summary>
        /// The set properties.
        /// </summary>
        SetProperties = 95, 

        /// <summary>
        /// The spawn item.
        /// </summary>
        SpawnItem = 96, 

        /// <summary>
        /// The destroy item.
        /// </summary>
        DestroyItem = 97, 

        /// <summary>
        /// The subscribe item.
        /// </summary>
        SubscribeItem = 98, 

        /// <summary>
        /// The unsubscribe item.
        /// </summary>
        UnsubscribeItem = 99, 

        /// <summary>
        /// The set view distance.
        /// </summary>
        SetViewDistance = 100, 

        /// <summary>
        /// The attach interest area.
        /// </summary>
        AttachInterestArea = 101, 

        /// <summary>
        /// The detach interest area.
        /// </summary>
        DetachInterestArea = 102, 

        /// <summary>
        /// The add interest area.
        /// </summary>
        AddInterestArea = 103, 

        /// <summary>
        /// The remove interest area.
        /// </summary>
        RemoveInterestArea = 104, 

        /// <summary>
        /// The get properties.
        /// </summary>
        GetProperties = 105, 

        /// <summary>
        /// The move interest area.
        /// </summary>
        MoveInterestArea = 106, 

        /// <summary>
        /// The radar subscribe.
        /// </summary>
        RadarSubscribe = 107, 

        /// <summary>
        /// The unsubscribe counter.
        /// </summary>
        UnsubscribeCounter = 108, 

        /// <summary>
        /// The subscribe counter.
        /// </summary>
        SubscribeCounter = 109
    }
}