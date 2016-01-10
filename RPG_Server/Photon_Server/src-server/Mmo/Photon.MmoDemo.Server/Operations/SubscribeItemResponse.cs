// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubscribeItemResponse.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This operation is allowed AFTER having entered an <see cref="MmoWorld" /> with operation <see cref="EnterWorld" />.
//   It is used to subscribe an <see cref="InterestArea" /> to an <see cref="Item" />'s <see cref="Item.EventChannel" />.
//   See <see cref="MmoActor.OperationSubscribeItem">MmoActor.OperationSubscribeItem</see> for more information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Operations
{
    using Photon.MmoDemo.Common;
    using Photon.SocketServer.Mmo;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// This operation is allowed AFTER having entered an <see cref="MmoWorld"/> with operation <see cref="EnterWorld"/>.
    /// It is used to subscribe an <see cref="InterestArea"/> to an <see cref="Item"/>'s <see cref="Item.EventChannel"/>. 
    /// See <see cref="MmoActor.OperationSubscribeItem">MmoActor.OperationSubscribeItem</see> for more information.
    /// </summary>
    public class SubscribeItemResponse
    {
        /// <summary>
        /// Gets or sets the Id of the <see cref="Item"/> to subscribe to.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemId)]
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the type of the <see cref="Item"/> to subscribe to.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemType)]
        public byte ItemType { get; set; }
    }
}