// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JoinGameRequest.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the JoinGameRequest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Photon.LoadBalancing.Operations
{
    #region using directives

    using Lite.Operations;

    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    #endregion

    public enum JoinMode : byte
    {
        Default = 0,
        CreateIfNotExists = 1,
        Rejoin = 2,
    }

    public class JoinGameRequest : JoinRequest
    {
        public JoinGameRequest(IRpcProtocol protocol, OperationRequest operationRequest)
            : base(protocol, operationRequest)
        {
        }

        public JoinGameRequest()
        {
        }

        [DataMember(Code = (byte)ParameterCode.LobbyName, IsOptional = true)]
        public string LobbyName { get; set; }

        [DataMember(Code = (byte)ParameterCode.LobbyType, IsOptional = true)]
        public byte LobbyType { get; set; }

        //[DataMember(Code = (byte)ParameterCode.CreateIfNotExists, IsOptional = true)]
        //public bool CreateIfNotExists { get; set; }
        private object internalJoinMode;
        [DataMember(Code = (byte)ParameterCode.JoinMode, IsOptional = true)]
        internal object InternalJoinMode
        {
            get
            {
                return this.internalJoinMode;
            }
            set
            {
                this.internalJoinMode = value;
                var type = value.GetType();
                if (type == typeof(bool))
                {
                    if (this.JoinMode == JoinMode.Default && (bool)value)
                    {
                        this.JoinMode = JoinMode.CreateIfNotExists;
                    }
                    return;
                }

                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.SByte:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        this.JoinMode = (JoinMode)(System.Convert.ToInt32(value));
                        return;
                }
            }
        }

        // no ParameterKey:
        // for backward compatibility this is set through InternalJoinMode
        public JoinMode JoinMode { get; set; }

        // no ParameterKey:
        // for backward compatibility this is set through InternalJoinMode
        public bool CreateIfNotExists
        {
            get
            {
                return this.JoinMode > 0;
            }

            set
            {
                this.internalJoinMode = value;
                if (this.JoinMode == JoinMode.Default && value)
                {
                    this.JoinMode = JoinMode.CreateIfNotExists;
                }
            }
        }
    }
}