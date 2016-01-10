// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemUnsubscribed.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Client receive this event when an <see cref="MmoClientInterestArea" /> unsubscribes from an <see cref="Item" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Events
{
    using Photon.MmoDemo.Common;
    using Photon.SocketServer.Mmo;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// Client receive this event when an <see cref="MmoClientInterestArea"/> unsubscribes from an <see cref="Item"/>.
    /// </summary>
    public class ItemUnsubscribed
    {
        /// <summary>
        /// Gets or sets the id of the <see cref="InterestArea"/> that unsubscribed.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.InterestAreaId)]
        public byte InterestAreaId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the unsubscribed <see cref="Item"/>.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemId)]
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the type of the unsubscribed <see cref="Item"/>.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemType)]
        public byte ItemType { get; set; }
    }
}