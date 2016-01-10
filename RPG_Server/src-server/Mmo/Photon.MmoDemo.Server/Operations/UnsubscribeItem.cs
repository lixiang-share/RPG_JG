// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnsubscribeItem.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This operation is allowed AFTER having entered an <see cref="MmoWorld" /> with operation <see cref="EnterWorld" />.
//   It is used to unsubscribe an <see cref="InterestArea" /> from an <see cref="Item" />'s <see cref="Item.EventChannel" /> after subscribring with operation <see cref="SubscribeItem" />.
//   See <see cref="MmoActor.OperationUnsubscribeItem">MmoActor.OperationUnsubscribeItem</see> for more information.
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
    /// It is used to unsubscribe an <see cref="InterestArea"/> from an <see cref="Item"/>'s <see cref="Item.EventChannel"/> after subscribring with operation <see cref="SubscribeItem"/>.
    /// See <see cref="MmoActor.OperationUnsubscribeItem">MmoActor.OperationUnsubscribeItem</see> for more information.
    /// </summary>
    public class UnsubscribeItem : Operation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsubscribeItem"/> class.
        /// </summary>
        /// <param name="protocol">
        /// The protocol.
        /// </param>
        /// <param name="request">
        /// The request. 
        /// </param>
        public UnsubscribeItem(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request)
        {
        }

        /// <summary>
        /// Gets or sets the id of the <see cref="InterestArea"/> that is subscribed.
        /// This request parameter is optional. Default: 0.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.InterestAreaId, IsOptional = true)]
        public byte InterestAreaId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the <see cref="Item"/> to unsubscribe from.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemId)]
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the type of the <see cref="Item"/> to unsubscribe from.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemType)]
        public byte ItemType { get; set; }

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
            var responseObject = new UnsubscribeItemResponse { ItemId = this.ItemId, ItemType = this.ItemType, InterestAreaId = this.InterestAreaId };
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