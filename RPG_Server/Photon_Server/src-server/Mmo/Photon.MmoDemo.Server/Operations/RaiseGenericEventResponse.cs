// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RaiseGenericEventResponse.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This operation is allowed AFTER having entered an <see cref="MmoWorld" /> with operation <see cref="EnterWorld" />.
//   It is used to send an <see cref="ItemGeneric" /> event either to the <see cref="Item" />'s owner or to item's <see cref="Item.EventChannel" /> subscriber.
//   See <see cref="MmoActor.OperationRaiseGenericEvent">MmoActor.OperationRaiseGenericEvent</see> for more information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Operations
{
    using Photon.MmoDemo.Common;
    using Photon.MmoDemo.Server.Events;
    using Photon.SocketServer.Mmo;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// This operation is allowed AFTER having entered an <see cref="MmoWorld"/> with operation <see cref="EnterWorld"/>.
    /// It is used to send an <see cref="ItemGeneric"/> event either to the <see cref="Item"/>'s owner or to item's <see cref="Item.EventChannel"/> subscriber. 
    /// See <see cref="MmoActor.OperationRaiseGenericEvent">MmoActor.OperationRaiseGenericEvent</see> for more information.
    /// </summary>
    public class RaiseGenericEventResponse
    {
        /// <summary>
        /// Gets or sets the id of the selected <see cref="Item"/>.
        /// If not submitted the <see cref="Actor.Avatar"/> is selected.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemId)]
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the type of the selected <see cref="Item"/>.
        /// If not submitted the <see cref="Actor.Avatar"/> is selected.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemType)]
        public byte? ItemType { get; set; }
    }
}