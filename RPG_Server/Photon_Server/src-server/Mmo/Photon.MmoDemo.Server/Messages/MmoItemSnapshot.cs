// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MmoItemSnapshot.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   <see cref="ItemSnapshot" /> subclass that includes the <see cref="Rotation" /> and the <see cref="Coordinate" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Messages
{
    using Photon.SocketServer.Mmo;
    using Photon.SocketServer.Mmo.Messages;

    /// <summary>
    /// <see cref="ItemSnapshot"/> subclass that includes the <see cref="Rotation"/> and the <see cref="Coordinate"/>.
    /// </summary>
    public sealed class MmoItemSnapshot : ItemSnapshot
    {
        /// <summary>
        /// The item coordinate.
        /// </summary>
        private readonly float[] coordinate;

        /// <summary>
        /// The item rotation.
        /// </summary>
        private readonly float[] rotation;

        /// <summary>
        /// Initializes a new instance of the <see cref="MmoItemSnapshot"/> class.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="worldRegion">
        /// The world region.
        /// </param>
        /// <param name="propertiesRevision">
        /// The properties revision.
        /// </param>
        /// <param name="rotation">
        /// The rotation.
        /// </param>
        /// <param name="coordinate">
        /// The coordinate.
        /// </param>
        public MmoItemSnapshot(Item source, Vector position, Region worldRegion, int propertiesRevision, float[] rotation, float[] coordinate)
            : base(source, position, worldRegion, propertiesRevision)
        {
            this.rotation = rotation;
            this.coordinate = coordinate;
        }

        /// <summary>
        /// Gets the coordinate.
        /// </summary>
        public float[] Coordinate
        {
            get
            {
                return this.coordinate;
            }
        }

        /// <summary>
        /// Gets the rotation.
        /// </summary>
        public float[] Rotation
        {
            get
            {
                return this.rotation;
            }
        }
    }
}