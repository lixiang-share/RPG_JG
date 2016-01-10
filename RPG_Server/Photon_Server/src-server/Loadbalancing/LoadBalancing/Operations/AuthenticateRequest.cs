// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthenticateRequest.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the AuthenticateRequest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.Operations
{
    #region using directives

    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    #endregion

    public class AuthenticateRequest : Operation
    {
        #region Constructors and Destructors

        public AuthenticateRequest(IRpcProtocol protocol, OperationRequest operationRequest)
            : base(protocol, operationRequest)
        {
        }

        public AuthenticateRequest()
        {
        }

        #endregion

        #region Properties

        [DataMember(Code = (byte)ParameterCode.ApplicationId, IsOptional = true)]
        public string ApplicationId { get; set; }

        [DataMember(Code = (byte)ParameterCode.AppVersion, IsOptional = true)]
        public string ApplicationVersion { get; set; }

        [DataMember(Code = (byte)ParameterCode.Secret, IsOptional = true)]
        public string Secret { get; set; }

        [DataMember(Code = (byte)ParameterCode.UserId, IsOptional = true)]
        public string UserId { get; set; }

        [DataMember(Code = (byte)ParameterCode.ClientAuthenticationType, IsOptional = true)]
        public byte ClientAuthenticationType { get; set; }

        [DataMember(Code = (byte)ParameterCode.ClientAuthenticationParams, IsOptional = true)]
        public string ClientAuthenticationParams { get; set; }

        [DataMember(Code = (byte)ParameterCode.ClientAuthenticationData, IsOptional = true)]
        public object ClientAuthenticationData { get; set; }

        #endregion
    }
}