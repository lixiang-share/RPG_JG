// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RaiseGenericEvent.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This operation is allowed AFTER having entered an <see cref="MmoWorld" /> with operation <see cref="EnterWorld" />.
//   It is used to send an <see cref="ItemGeneric" /> event either to the <see cref="Item" />'s owner or to item's <see cref="Item.EventChannel" /> subscriber.
//   See <see cref="MmoActor.OperationRaiseGenericEvent">MmoActor.OperationRaiseGenericEvent</see> for more information.
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
    /// It is used to send an <see cref="ItemGeneric"/> event either to the <see cref="Item"/>'s owner or to item's <see cref="Item.EventChannel"/> subscriber. 
    /// See <see cref="MmoActor.OperationRaiseGenericEvent">MmoActor.OperationRaiseGenericEvent</see> for more information.
    /// </summary>
    public class RaiseGenericEvent : Operation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RaiseGenericEvent"/> class.
        /// </summary>
        /// <param name="protocol">
        /// The protocol.
        /// </param>
        /// <param name="request">
        /// The request. 
        /// </param>
        public RaiseGenericEvent(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request)
        {
        }

        /// <summary>
        /// Gets or sets the custom event code.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.CustomEventCode)]
        public byte CustomEventCode { get; set; }

        /// <summary>
        /// Gets or sets the optional event content.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.EventData, IsOptional = true)]
        public object EventData { get; set; }

        /// <summary>
        /// Gets or sets the event receiver.
        /// See enum <see cref="Common.EventReceiver"/> for possible values.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.EventReceiver)]
        public byte EventReceiver { get; set; }

        /// <summary>
        /// Gets or sets the event reliability.
        /// See enum <see cref="Reliability"/> for possible values.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.EventReliability)]
        public byte EventReliability { get; set; }

        /// <summary>
        /// Gets or sets the id of the selected <see cref="Item"/>.
        /// If not submitted the <see cref="Actor.Avatar"/> is selected.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemId, IsOptional = true)]
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the type of the selected <see cref="Item"/>.
        /// If not submitted the <see cref="Actor.Avatar"/> is selected.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemType, IsOptional = true)]
        public byte? ItemType { get; set; }

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
            var responseObject = new RaiseGenericEventResponse { ItemId = this.ItemId, ItemType = this.ItemType };
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