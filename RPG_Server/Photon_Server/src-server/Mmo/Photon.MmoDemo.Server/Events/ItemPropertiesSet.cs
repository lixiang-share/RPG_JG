// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemPropertiesSet.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Clients receive this event after executing operation <see cref="SetProperties" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Events
{
    using System.Collections;

    using Photon.MmoDemo.Common;
    using Photon.MmoDemo.Server.Operations;
    using Photon.SocketServer.Mmo;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// Clients receive this event after executing operation <see cref="SetProperties"/>.
    /// </summary>
    public class ItemPropertiesSet
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
        /// Gets or sets the properties revision number.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.PropertiesRevision)]
        public int PropertiesRevision { get; set; }

        /// <summary>
        /// Gets or sets the new and/or changed properties.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.PropertiesSet, IsOptional = true)]
        public Hashtable PropertiesSet { get; set; }

        /// <summary>
        /// Gets or sets the keys of the removed properties.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.PropertiesUnset, IsOptional = true)]
        public ArrayList PropertiesUnset { get; set; }
    }
}