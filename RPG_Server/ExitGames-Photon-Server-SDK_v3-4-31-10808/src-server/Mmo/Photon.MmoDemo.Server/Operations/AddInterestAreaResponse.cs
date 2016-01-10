// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddInterestAreaResponse.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This operation is allowed AFTER having entered an <see cref="MmoWorld" /> with operation <see cref="EnterWorld" />.
//   See <see cref="MmoActor.OperationAddInterestArea">MmoActor.OperationAddInterestArea</see> for more information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Operations
{
    using Photon.MmoDemo.Common;
    using Photon.SocketServer.Mmo;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// This operation is allowed AFTER having entered an <see cref="MmoWorld"/> with operation <see cref="EnterWorld"/>.
    /// See <see cref="MmoActor.OperationAddInterestArea">MmoActor.OperationAddInterestArea</see> for more information.
    /// </summary>
    public class AddInterestAreaResponse
    {
        /// <summary>
        /// Gets or sets the id of the new <see cref="InterestArea"/>.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.InterestAreaId)]
        public byte InterestAreaId { get; set; }

        /// <summary>
        /// Gets or sets the id of an <see cref="Item"/> the new <see cref="InterestArea"/> should be attached to.
        /// This parameter is optional.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemId, IsOptional = true)]
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the type of an <see cref="Item"/> the new <see cref="InterestArea"/> should be attached to.
        /// This parameter is optional.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemType, IsOptional = true)]
        public byte? ItemType { get; set; }
    }
}