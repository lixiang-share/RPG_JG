// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubscribeItem.cs" company="Exit Games GmbH">
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
    using Photon.MmoDemo.Server.Events;
    using Photon.SocketServer;
    using Photon.SocketServer.Mmo;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// This operation is allowed AFTER having entered an <see cref="MmoWorld"/> with operation <see cref="EnterWorld"/>.
    /// It is used to subscribe an <see cref="InterestArea"/> to an <see cref="Item"/>'s <see cref="Item.EventChannel"/>. 
    /// See <see cref="MmoActor.OperationSubscribeItem">MmoActor.OperationSubscribeItem</see> for more information.
    /// </summary>
    public class SubscribeItem : Operation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscribeItem"/> class.
        /// </summary>
        /// <param name="protocol">
        /// The protocol.
        /// </param>
        /// <param name="request">
        /// The request. 
        /// </param>
        public SubscribeItem(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request)
        {
        }

        /// <summary>
        /// Gets or sets the id of the <see cref="InterestArea"/> selected for subscription.
        /// This request parameter is optional. Default: 0.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.InterestAreaId, IsOptional = true)]
        public byte InterestAreaId { get; set; }

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

        /// <summary>
        /// Gets or sets the known properties revision.
        /// If the value is null or smaller than the <see cref="Item.PropertiesRevision">Item PropertiesRevision</see> event <see cref="ItemProperties"/> is sent to the client.
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
            var responseObject = new SubscribeItemResponse { ItemId = this.ItemId, ItemType = this.ItemType };
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