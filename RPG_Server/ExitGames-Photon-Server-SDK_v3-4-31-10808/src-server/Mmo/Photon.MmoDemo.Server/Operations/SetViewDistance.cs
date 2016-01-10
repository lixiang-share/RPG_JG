// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetViewDistance.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This operation is allowed AFTER having entered an <see cref="MmoWorld" /> with operation <see cref="EnterWorld" />.
//   See <see cref="MmoActor.OperationSetViewDistance">MmoActor.OperationSetViewDistance</see> for more information.
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
    /// See <see cref="MmoActor.OperationSetViewDistance">MmoActor.OperationSetViewDistance</see> for more information.
    /// </summary>
    public class SetViewDistance : Operation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetViewDistance"/> class.
        /// </summary>
        /// <param name="protocol">
        /// The protocol.
        /// </param>
        /// <param name="request">
        /// The request. 
        /// </param>
        public SetViewDistance(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request)
        {
        }

        /// <summary>
        /// Gets or sets the id of the <see cref="InterestArea"/> to change.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.InterestAreaId, IsOptional = true)]
        public byte InterestAreaId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="InterestArea.ViewDistanceEnter">interest area's minimum view distance (the item subscribe threshold)</see>.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ViewDistanceEnter)]
        public float[] ViewDistanceEnter { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="InterestArea.ViewDistanceExit">interest area's maximum view distance (the item unsubscribe threshold)</see>.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ViewDistanceExit)]
        public float[] ViewDistanceExit { get; set; }

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
            var responseObject = new SetViewDistanceResponse { InterestAreaId = this.InterestAreaId };
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