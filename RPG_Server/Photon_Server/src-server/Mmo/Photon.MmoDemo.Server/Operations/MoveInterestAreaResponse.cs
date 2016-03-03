// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MoveInterestAreaResponse.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This operation is allowed AFTER having entered an <see cref="MmoWorld" /> with operation <see cref="EnterWorld" />.
//   It is used to move an existing <see cref="InterestArea" />.
//   See <see cref="MmoActor.OperationMoveInterestArea">MmoActor.OperationMoveInterestArea</see> for more information.
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
    /// It is used to move an existing <see cref="InterestArea"/>. 
    /// See <see cref="MmoActor.OperationMoveInterestArea">MmoActor.OperationMoveInterestArea</see> for more information.
    /// </summary>
    public class MoveInterestAreaResponse
    {
        /// <summary>
        /// Gets or sets the Id of the selected <see cref="InterestArea"/>.
        /// This request parameter is optional. Default: #0.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.InterestAreaId)]
        public byte InterestAreaId { get; set; }
    }
}