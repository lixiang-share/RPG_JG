// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWorld.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Represents a virtual world that exists within the boundaries of a <see cref="BoundingBox" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.SocketServer.Mmo
{
    using System.Collections.Generic;

    /// <summary>
    ///   Represents a virtual world that exists within the boundaries of a <see cref = "BoundingBox" />.
    /// </summary>
    public interface IWorld
    {
        #region Properties

        /// <summary>
        ///   Gets the underlying area.
        /// </summary>
        BoundingBox Area { get; }

        /// <summary>
        ///   Gets the cache for all <see cref = "Item">Items</see> in this world.
        /// </summary>
        ItemCache ItemCache { get; }

        #endregion

        #region Public Methods

        /// <summary>
        ///   Get the region at the <paramref name = "position" />/
        /// </summary>
        /// <param name = "position">
        ///   The position.
        /// </param>
        /// <returns>
        ///   A <see cref = "Region" /> or null.
        /// </returns>
        Region GetRegion(Vector position);

        /// <summary>
        ///   Gets the bounding box of the overlapping regions.
        /// </summary>
        /// <param name = "area">
        ///   The area that overlaps with the same regions as the result.
        /// </param>
        /// <returns>
        ///   A region aligned bounding box.
        /// </returns>
        BoundingBox GetRegionAlignedBoundingBox(BoundingBox area);

        /// <summary>
        ///   Gets all <see cref = "Region">regions</see> overlapping with a specific <paramref name = "area" />.
        /// </summary>
        /// <param name = "area">
        ///   The area to return the <see cref = "Region">regions</see> for.
        /// </param>
        /// <returns>
        ///   The <see cref = "Region">regions</see> overlapping with the <paramref name = "area" />.
        /// </returns>
        HashSet<Region> GetRegions(BoundingBox area);

        /// <summary>
        ///   Gets the regions that overlap with the <paramref name = "area" /> except the ones that do also overlap with <paramref name = "except" />.
        /// </summary>
        /// <param name = "area">
        ///   The area that overlaps with the result regions.
        /// </param>
        /// <param name = "except">
        ///   Regions overlapping with this parameter are not returned.
        /// </param>
        /// <returns>
        ///   A collection of regions.
        /// </returns>
        HashSet<Region> GetRegionsExcept(BoundingBox area, BoundingBox except);

        #endregion
    }
}