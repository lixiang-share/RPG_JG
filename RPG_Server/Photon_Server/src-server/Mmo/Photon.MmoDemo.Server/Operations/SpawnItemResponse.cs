// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpawnItemResponse.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This operation is allowed AFTER having entered an <see cref="MmoWorld" /> with operation <see cref="EnterWorld" />.
//   It is used to create a new <see cref="Item" />.
//   See <see cref="MmoActor.OperationSpawnItem">MmoActor.OperationSpawnItem</see> for more information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Operations
{
    using Photon.MmoDemo.Common;
    using Photon.SocketServer.Mmo;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// This operation is allowed AFTER having entered an <see cref="MmoWorld"/> with operation <see cref="EnterWorld"/>.
    /// It is used to create a new <see cref="Item"/>. 
    /// See <see cref="MmoActor.OperationSpawnItem">MmoActor.OperationSpawnItem</see> for more information.
    /// </summary>
    public class SpawnItemResponse
    {
        /// <summary>
        /// Gets or sets an <see cref="InterestArea"/> Id for immediate subscription.
        /// This request parameter is optional.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.InterestAreaId, IsOptional = true)]
        public byte? InterestAreaId { get; set; }

        /// <summary>
        /// Gets or sets thew new <see cref="Item"/> id.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemId)]
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets thew new <see cref="Item"/> type.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemType)]
        public byte ItemType { get; set; }
    }
}