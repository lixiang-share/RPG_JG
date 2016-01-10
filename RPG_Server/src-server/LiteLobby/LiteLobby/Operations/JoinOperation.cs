// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JoinOperation.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The join operation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteLobby.Operations
{
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    /// <summary>
    ///   The join operation.
    /// </summary>
    public class JoinRequest : Lite.Operations.JoinRequest
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "JoinRequest" /> class.
        /// </summary>
        /// <param name = "protocol">
        ///   The protocol.
        /// </param>
        /// <param name = "operationRequest">
        ///   Operation request containing the operation parameters.
        /// </param>
        public JoinRequest(IRpcProtocol protocol, OperationRequest operationRequest)
            : base(protocol, operationRequest)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "JoinRequest" /> class.
        /// </summary>
        public JoinRequest()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets a lobby-room to the game that's joined.
        /// </summary>
        [DataMember(Code = (byte)LobbyParameterKeys.LobbyId, IsOptional = true)]
        public string LobbyId { get; set; }

        #endregion
    }
}