// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Move.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This operation is allowed AFTER having entered an <see cref="MmoWorld" /> with operation <see cref="EnterWorld" />.
//   It is used to move an existing <see cref="Item" />.
//   See <see cref="MmoActor.OperationMove">MmoActor.OperationMove</see> for more information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Operations
{
    using Photon.MmoDemo.Common;
    using Photon.SocketServer;
    using Photon.SocketServer.Mmo;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// This operation is allowed AFTER having entered an <see cref="MmoWorld"/> with operation <see cref="EnterWorld"/>.
    /// It is used to move an existing <see cref="Item"/>. 
    /// See <see cref="MmoActor.OperationMove">MmoActor.OperationMove</see> for more information.
    /// </summary>
    public class MoveResponse 
    {
        /// <summary>
        /// Gets or sets the selected <see cref="Item"/> Id.
        /// This request parameter is optional. If not submitted the <see cref="Actor.Avatar"/> is selected.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemId)]
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the selected <see cref="Item"/> type.
        /// This request parameter is optional. If not submitted the <see cref="Actor.Avatar"/> is selected.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemType)]
        public byte? ItemType { get; set; }
    }
}