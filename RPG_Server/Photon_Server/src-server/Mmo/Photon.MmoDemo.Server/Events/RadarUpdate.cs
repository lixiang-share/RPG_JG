// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RadarUpdate.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Clients receive this event frequently after executing operation <see cref="RadarSubscribe" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Events
{
    using Photon.MmoDemo.Common;
    using Photon.MmoDemo.Server.Operations;
    using Photon.SocketServer.Rpc;

    using SocketServer.Mmo;

    /// <summary>
    /// Clients receive this event frequently after executing operation <see cref="RadarSubscribe"/>.
    /// </summary>
    public class RadarUpdate
    {
        /// <summary>
        /// Gets or sets the Id of the <see cref="Item"/> that changed the position.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemId)]
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the type of the <see cref="Item"/> that changed the position.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemType)]
        public byte ItemType { get; set; }

        /// <summary>
        /// Gets or sets the new position of the <see cref="Item"/>.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.Position, IsOptional = true)]
        public float[] Position { get; set; }
    }
}