// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RadarSubscribe.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This operation subscribes to an <see cref="MmoRadar" />. It can be executed any time.
//   See <see cref="MmoPeer.OperationRadarSubscribe">MmoPeer.OperationRadarSubscribe</see> for more information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Operations
{
    using Photon.MmoDemo.Common;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// This operation subscribes to an <see cref="MmoRadar"/>. It can be executed any time.
    /// See <see cref="MmoPeer.OperationRadarSubscribe">MmoPeer.OperationRadarSubscribe</see> for more information.
    /// </summary>
    public class RadarSubscribe : Operation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RadarSubscribe"/> class.
        /// </summary>
        /// <param name="protocol">
        /// The protocol.
        /// </param>
        /// <param name="request">
        /// The request. 
        /// </param>
        public RadarSubscribe(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request)
        {
        }

        /// <summary>
        /// Gets or sets the name of the selected <see cref="MmoWorld"/>.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.WorldName)]
        public string WorldName { get; set; }

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
            var responseObject = new RadarSubscribeResponse { WorldName = this.WorldName };
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