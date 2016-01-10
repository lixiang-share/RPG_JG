// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubscribeCounter.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This operation subscribes to the <see cref="PhotonApplication.CounterPublisher" />. It can be executed any time.
//   See <see cref="CounterOperations.SubscribeCounter" /> for more information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Operations
{
    using Photon.MmoDemo.Common;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// This operation subscribes to the <see cref="PhotonApplication.CounterPublisher"/>. It can be executed any time.
    /// See <see cref="CounterOperations.SubscribeCounter"/> for more information.
    /// </summary>
    public class SubscribeCounter : Operation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscribeCounter"/> class.
        /// </summary>
        /// <param name="protocol">
        /// The protocol.
        /// </param>
        /// <param name="request">
        /// The request. 
        /// </param>
        public SubscribeCounter(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request)
        {
        }

        /// <summary>
        /// Gets or sets the interval to receive the counter values with.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.CounterReceiveInterval)]
        public int ReceiveInterval { get; set; }

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
            return new OperationResponse(this.OperationRequest.OperationCode) { ReturnCode = errorCode, DebugMessage = debugMessage };
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