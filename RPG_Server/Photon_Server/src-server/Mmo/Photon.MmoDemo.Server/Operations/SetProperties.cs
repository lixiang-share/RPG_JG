// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetProperties.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This operation is allowed AFTER having entered an <see cref="MmoWorld" /> with operation <see cref="EnterWorld" />.
//   It is used to set the properties of an existing <see cref="Item" />.
//   See <see cref="MmoActor.OperationSetProperties">MmoActor.OperationSetProperties</see> for more information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Operations
{
    using System.Collections;

    using Photon.MmoDemo.Common;
    using Photon.SocketServer;
    using Photon.SocketServer.Mmo;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// This operation is allowed AFTER having entered an <see cref="MmoWorld"/> with operation <see cref="EnterWorld"/>.
    /// It is used to set the properties of an existing <see cref="Item"/>. 
    /// See <see cref="MmoActor.OperationSetProperties">MmoActor.OperationSetProperties</see> for more information.
    /// </summary>
    public class SetProperties : Operation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetProperties"/> class.
        /// </summary>
        /// <param name="protocol">
        /// The protocol.
        /// </param>
        /// <param name="request">
        /// The request. 
        /// </param>
        public SetProperties(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request)
        {
        }

        /// <summary>
        /// Gets or sets the selected <see cref="Item"/> Id.
        /// This request parameter is optional. If not submitted the <see cref="Actor.Avatar"/> is selected.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemId, IsOptional = true)]
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the selected <see cref="Item"/> type.
        /// This request parameter is optional. If not submitted the <see cref="Actor.Avatar"/> is selected.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemType, IsOptional = true)]
        public byte? ItemType { get; set; }

        /// <summary>
        /// Gets or sets the properties to add or change.
        /// This request parameter is optional.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.PropertiesSet, IsOptional = true)]
        public Hashtable PropertiesSet { get; set; }

        /// <summary>
        /// Gets or sets the property keys to remove.
        /// This request parameter is optional.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.PropertiesUnset, IsOptional = true)]
        public ArrayList PropertiesUnset { get; set; }

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
            var responseObject = new SetPropertiesResponse { ItemId = this.ItemId, ItemType = this.ItemType };
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