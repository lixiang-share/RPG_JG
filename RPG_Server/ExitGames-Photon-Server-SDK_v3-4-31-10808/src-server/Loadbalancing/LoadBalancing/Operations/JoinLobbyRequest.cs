// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JoinLobbyRequest.cs" company="">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.Operations
{
    using System.Collections;

    using Lite.Operations;

    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    public class JoinLobbyRequest : Operation
    {
        public JoinLobbyRequest()
        {
        }

        public JoinLobbyRequest(IRpcProtocol protocol, OperationRequest operationRequest)
            : base(protocol, operationRequest)
        {
        }

        [DataMember(Code = (byte)ParameterCode.GameCount, IsOptional = true)]
        public int GameListCount { get; set; }
 
        [DataMember(Code = (byte)ParameterKey.GameProperties, IsOptional = true)]
        public Hashtable GameProperties { get; set; }

        [DataMember(Code = (byte)ParameterCode.LobbyName, IsOptional = true)]
        public string LobbyName { get; set; }

        [DataMember(Code = (byte)ParameterCode.LobbyType, IsOptional = true)]
        public byte LobbyType { get; set; }
    }
}
