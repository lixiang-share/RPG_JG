// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetProperties.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This operation is allowed AFTER having entered an <see cref="MmoWorld" /> with operation <see cref="EnterWorld" />.
//   This is usually the first operation to call after reiveing event <see cref="ItemSubscribed" /> with an unknown properties revision.
//   See <see cref="MmoActor.OperationGetProperties">MmoActor.OperationGetProperties</see> for more information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Operations
{
    using Photon.MmoDemo.Common;
    using Photon.MmoDemo.Server.Events;
    using Photon.SocketServer;
    using Photon.SocketServer.Mmo;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// This operation is allowed AFTER having entered an <see cref="MmoWorld"/> with operation <see cref="EnterWorld"/>.
    /// This is usually the first operation to call after reiveing event <see cref="ItemSubscribed"/> with an unknown properties revision.
    /// See <see cref="MmoActor.OperationGetProperties">MmoActor.OperationGetProperties</see> for more information.
    /// </summary>
    public class GetPropertiesResponse
    {
        /// <summary>
        /// Gets or sets the target <see cref="Item"/> id.
        /// The request parameter is mandatory.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemId)]
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the target <see cref="Item"/> type.
        /// The request parameter is mandatory.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemType)]
        public byte ItemType { get; set; }
    }
}