// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemDestroyed.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Client receive this event after executing operation <see cref="DestroyItem" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Events
{
    using Photon.MmoDemo.Common;
    using Photon.MmoDemo.Server.Operations;
    using Photon.SocketServer.Mmo;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// Client receive this event after executing operation <see cref="DestroyItem"/>.
    /// </summary>
    public class ItemDestroyed
    {
        /// <summary>
        /// Gets or sets the id of the affected <see cref="Item"/>.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemId)]
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the type of the affected <see cref="Item"/>.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemType)]
        public byte ItemType { get; set; }
    }
}