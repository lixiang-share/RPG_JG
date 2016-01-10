// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemSubscribed.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Client receive this event when an <see cref="MmoClientInterestArea" /> subscribes to an <see cref="Item" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Events
{
    using Photon.MmoDemo.Common;
    using Photon.MmoDemo.Server.Operations;
    using Photon.SocketServer.Mmo;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// Client receive this event when an <see cref="MmoClientInterestArea"/> subscribes to an <see cref="Item"/>.
    /// </summary>
    public class ItemSubscribed
    {
        /// <summary>
        /// Gets or sets the id of the <see cref="InterestArea"/> that subscribed.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.InterestAreaId)]
        public byte InterestAreaId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the subscribed <see cref="Item"/>.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemId)]
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the type of the subscribed <see cref="Item"/>.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemType)]
        public byte ItemType { get; set; }

        /// <summary>
        /// Gets or sets the position of the subscribed <see cref="Item"/>.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.Position)]
        public float[] Position { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the subscribed <see cref="Item"/>.
        /// This event parameter is optional.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.Rotation, IsOptional = true)]
        public float[] Rotation { get; set; }

        /// <summary>
        /// Gets or sets the properties revision number of the subscribed <see cref="Item"/>.
        /// The client compares to his cached properties and can then decide to update them with operation <see cref="GetProperties"/>.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.PropertiesRevision)]
        public int PropertiesRevision { get; set; }
    }
}