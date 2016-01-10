// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttachInterestAreaResponse.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This operation is allowed AFTER having entered an <see cref="MmoWorld" /> with operation <see cref="EnterWorld" />.
//   See <see cref="MmoActor.OperationAttachInterestArea">MmoActor.OperationAttachInterestArea</see> for more information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Operations
{
    using Photon.MmoDemo.Common;
    using Photon.SocketServer.Mmo;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// This operation is allowed AFTER having entered an <see cref="MmoWorld"/> with operation <see cref="EnterWorld"/>.
    /// See <see cref="MmoActor.OperationAttachInterestArea">MmoActor.OperationAttachInterestArea</see> for more information.
    /// </summary>
    public class AttachInterestAreaResponse
    {
        /// <summary>
        /// Gets or sets the id of an existing <see cref="InterestArea"/>.
        /// If not submitted the default interest area #0 is selected.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.InterestAreaId)]
        public byte InterestAreaId { get; set; }

        /// <summary>
        /// Gets or sets the id of an existing <see cref="Item"/>.
        /// If not submitted the <see cref="Actor.Avatar"/> is selected.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemId)]
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the type of an existing <see cref="Item"/>.
        /// If not submitted the <see cref="Actor.Avatar"/> is selected.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemType)]
        public byte? ItemType { get; set; }
    }
}