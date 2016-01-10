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
    public class GetProperties : Operation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetProperties"/> class.
        /// </summary>
        /// <param name="protocol">
        /// The protocol.
        /// </param>
        /// <param name="request">
        /// The request. 
        /// </param>
        public GetProperties(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request)
        {
        }

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

        /// <summary>
        /// Gets or sets the known properties revision.
        /// The request parameter is optional.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.PropertiesRevision, IsOptional = true)]
        public int? PropertiesRevision { get; set; }

        /// <summary>
        /// Gets the operation response.
        /// </summary>
        /// <param name="errorCode">
        /// The error code.
        /// </param>
        /// <param name="debugMessage">
        /// The debug message.
        /// </param>
        /// <returns>
        /// A new operation response.
        /// </returns>
        public OperationResponse GetOperationResponse(short errorCode, string debugMessage)
        {
            var responseObject = new GetPropertiesResponse { ItemId = this.ItemId, ItemType = this.ItemType };
            return new OperationResponse(this.OperationRequest.OperationCode, responseObject) { ReturnCode = errorCode, DebugMessage = debugMessage };
        }

        /// <summary>
        /// Gets the operation response.
        /// </summary>
        /// <param name="returnValue">
        /// The return value.
        /// </param>
        /// <returns>
        /// A new operation response.
        /// </returns>
        public OperationResponse GetOperationResponse(MethodReturnValue returnValue)
        {
            return this.GetOperationResponse(returnValue.Error, returnValue.Debug);
        }
    }
}