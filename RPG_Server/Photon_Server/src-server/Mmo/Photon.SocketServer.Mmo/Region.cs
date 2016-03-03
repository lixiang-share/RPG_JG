// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Region.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Represents a region used for region-based interest management.
//   A Region is a subclass of <see cref="MessageChannel{T}" /> and requires messages of type <see cref="RegionMessage" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.SocketServer.Mmo
{
    using Photon.SocketServer.Concurrency;
    using Photon.SocketServer.Mmo.Messages;

    /// <summary>
    ///   Represents a region used for region-based interest management. 
    ///   A Region is a subclass of <see cref = "MessageChannel{T}" /> and requires messages of type <see cref = "RegionMessage" />.
    /// </summary>
    public class Region : MessageChannel<RegionMessage>
    {
        #region Constants and Fields

        /// <summary>
        ///   The coordinate.
        /// </summary>
        private readonly Vector coordinate;

        /// <summary>
        ///   Yhe hash code.
        /// </summary>
        private readonly int hashCode;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Region" /> class.
        /// </summary>
        /// <param name = "coordinate">
        ///   The coordinate.
        /// </param>
        public Region(Vector coordinate)
            : base(MessageCounters.CounterSend)
        {
            this.coordinate = coordinate;
            this.hashCode = this.coordinate.GetHashCode();
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the coordinate.
        /// </summary>
        public Vector Coordinate
        {
            get
            {
                return this.coordinate;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///   Compares with another object.
        /// </summary>
        /// <param name = "obj">
        ///   The obj.
        /// </param>
        /// <returns>
        ///   True if <paramref name = "obj" /> is the same instance.
        /// </returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        ///   Gets the hash code.
        /// </summary>
        /// <returns>
        ///   The hash code.
        /// </returns>
        public override int GetHashCode()
        {
            return this.hashCode;
        }

        #endregion
    }
}