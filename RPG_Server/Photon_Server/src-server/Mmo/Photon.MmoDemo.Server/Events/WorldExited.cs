// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorldExited.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Clients receive this event after exiting a <see cref="IWorld" />,
//   either because of operation <see cref="OperationCode.ExitWorld" /> or
//   because another client with the same username executes operation <see cref="EnterWorld" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Events
{
    using Photon.MmoDemo.Common;
    using Photon.MmoDemo.Server.Operations;
    using Photon.SocketServer.Mmo;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// Clients receive this event after exiting a <see cref="IWorld"/>, 
    /// either because of operation <see cref="OperationCode.ExitWorld"/> or 
    /// because another client with the same username executes operation <see cref="EnterWorld"/>. 
    /// </summary>
    public class WorldExited
    {
        /// <summary>
        /// Gets or sets the name of the world that was exited.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.WorldName)]
        public string WorldName { get; set; }
    }
}