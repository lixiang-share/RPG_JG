// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemMoved.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Clients receive this event after executing operation <see cref="Move" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Events
{
    using Photon.MmoDemo.Common;
    using Photon.MmoDemo.Server.Operations;
    using Photon.SocketServer.Rpc;

    using SocketServer.Mmo;

    /// <summary>
    /// Clients receive this event after executing operation <see cref="Move"/>.
    /// </summary>
    public class ItemMoved
    {
        /// <summary>
        /// Gets or sets the source <see cref="Item"/> Id.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemId)]
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the source <see cref="Item"/> type.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemType)]
        public byte ItemType { get; set; }

        /// <summary>
        /// Gets or sets the source <see cref="Item"/>'s former known position.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.OldPosition)]
        public float[] OldPosition { get; set; }

        /// <summary>
        /// Gets or sets the source <see cref="Item"/>'s new position.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.Position)]
        public float[] Position { get; set; }

        /// <summary>
        /// Gets or sets the source <see cref="Item"/>'s new rotation.
        /// This parameter is optional.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.Rotation, IsOptional = true)]
        public float[] Rotation { get; set; }

        /// <summary>
        /// Gets or sets the source <see cref="Item"/>'s old rotation.
        /// This parameter is optional.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.OldRotation, IsOptional = true)]
        public float[] OldRotation { get; set; }
    }
}