// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnsubscribeItem.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This operation is allowed AFTER having entered an <see cref="MmoWorld" /> with operation <see cref="EnterWorld" />.
//   It is used to unsubscribe an <see cref="InterestArea" /> from an <see cref="Item" />'s <see cref="Item.EventChannel" /> after subscribring with operation <see cref="SubscribeItem" />.
//   See <see cref="MmoActor.OperationUnsubscribeItem">MmoActor.OperationUnsubscribeItem</see> for more information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Operations
{
    using Photon.MmoDemo.Common;
    using Photon.SocketServer.Mmo;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// This operation is allowed AFTER having entered an <see cref="MmoWorld"/> with operation <see cref="EnterWorld"/>.
    /// It is used to unsubscribe an <see cref="InterestArea"/> from an <see cref="Item"/>'s <see cref="Item.EventChannel"/> after subscribring with operation <see cref="SubscribeItem"/>.
    /// See <see cref="MmoActor.OperationUnsubscribeItem">MmoActor.OperationUnsubscribeItem</see> for more information.
    /// </summary>
    public class UnsubscribeItemResponse
    {
        /// <summary>
        /// Gets or sets the id of the <see cref="InterestArea"/> that is subscribed.
        /// This request parameter is optional. Default: 0.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.InterestAreaId)]
        public byte InterestAreaId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the <see cref="Item"/> to unsubscribe from.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemId)]
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the type of the <see cref="Item"/> to unsubscribe from.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemType)]
        public byte ItemType { get; set; }
    }
}