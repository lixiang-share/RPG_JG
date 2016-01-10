// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventReceiver.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This enumeration contains all known event receivers.
//   Used for operation <see cref="OperationCode.RaiseGenericEvent" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Common
{
    /// <summary>
    /// This enumeration contains all known event receivers.
    /// Used for operation <see cref="OperationCode.RaiseGenericEvent"/>.
    /// </summary>
    public enum EventReceiver
    {
        /// <summary>
        /// The item subscriber.
        /// </summary>
        ItemSubscriber = 1, 

        /// <summary>
        /// The item owner.
        /// </summary>
        ItemOwner = 2
    }
}