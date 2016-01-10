// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebugGameResponse.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.Operations
{
    using Photon.SocketServer.Rpc;

    public class DebugGameResponse
    {

        [DataMember(Code = (byte)ParameterCode.Address, IsOptional = true)]
        public string Address { get; set; }

        [DataMember(Code = (byte)ParameterCode.NodeId, IsOptional = true)]
        public byte NodeId { get; set; }

        [DataMember(Code = (byte)ParameterCode.Info)]
        public string Info { get; set; }
    }
}
