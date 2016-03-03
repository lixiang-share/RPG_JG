// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MmoItemPositionUpdate.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the MmoItemPositionUpdate type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Messages
{
    using SocketServer.Mmo;
    using SocketServer.Mmo.Messages;

    /// <summary>
    /// This class extends <see cref="ItemPositionMessage"/> to provide access to the client coordinate for the <see cref="MmoRadar"/>.
    /// </summary>
    public class MmoItemPositionUpdate : ItemPositionMessage
    {
        /// <summary>
        /// The coordinate.
        /// </summary>
        private readonly float[] coordinate;

        /// <summary>
        /// Initializes a new instance of the <see cref="MmoItemPositionUpdate"/> class.
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
        /// <param name="coordinate">
        /// The coordinate.
        /// </param>
        public MmoItemPositionUpdate(Item source, Vector position, Region worldRegion, float[] coordinate)
            : base(source, position, worldRegion)
        {
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
    }
}