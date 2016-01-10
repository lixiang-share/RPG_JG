// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GridWorld.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This <see cref="IWorld" /> implementation uses a grid to divide the <see cref="IWorld">world</see>.
//   It contains <see cref="Region" />s.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.SocketServer.Mmo
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///   This <see cref = "IWorld" /> implementation uses a grid to divide the <see cref = "IWorld">world</see>.
    ///   It contains <see cref = "Region" />s.
    /// </summary>
    public class GridWorld : IWorld, IDisposable
    {
        #region Constants and Fields

        /// <summary>
        ///   The item cache.
        /// </summary>
        private readonly ItemCache itemCache;

        /// <summary>
        ///   The rectangle area.
        /// </summary>
        private readonly BoundingBox rectangleArea;

        /// <summary>
        ///   The tile dimensions.
        /// </summary>
        private readonly Vector tileDimensions;

        /// <summary>
        ///   The tile dimensions minus 1
        /// </summary>
        private readonly Vector tileSize;

        /// <summary>
        ///   The world regions.
        /// </summary>
        private readonly Region[][] worldRegions;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "GridWorld" /> class.
        /// </summary>
        /// <param name = "corner1">
        ///   One corner of the world.
        /// </param>
        /// <param name = "corner2">
        ///   The other corner of the world.
        /// </param>
        /// <param name = "tileDimensions">
        ///   The tile dimensions.
        /// </param>
        /// <param name = "itemCache">
        ///   The cache for all <see cref = "Item">Items</see> in this world.
        /// </param>
        public GridWorld(Vector corner1, Vector corner2, Vector tileDimensions, ItemCache itemCache)
            : this(BoundingBox.CreateFromPoints(corner1, corner2), tileDimensions, itemCache)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "GridWorld" /> class.
        /// </summary>
        /// <param name = "boundingBox">
        ///   The bounding box defines the world's size.
        /// </param>
        /// <param name = "tileDimensions">
        ///   The tile dimensions.
        /// </param>
        /// <param name = "itemCache">
        ///   The cache for all <see cref = "Item">Items</see> in this world.
        /// </param>
        public GridWorld(BoundingBox boundingBox, Vector tileDimensions, ItemCache itemCache)
        {
            // 2D grid: extend Z to max possible
            this.rectangleArea = new BoundingBox
                {
                    Min = new Vector { X = boundingBox.Min.X, Y = boundingBox.Min.Y, Z = int.MinValue }, 
                    Max = new Vector { X = boundingBox.Max.X, Y = boundingBox.Max.Y, Z = int.MaxValue }
                };

            var size = new Vector { X = boundingBox.Max.X - boundingBox.Min.X + 1, Y = boundingBox.Max.Y - boundingBox.Min.Y + 1 };
            if (tileDimensions.X <= 0)
            {
                tileDimensions.X = size.X;
            }

            if (tileDimensions.Y <= 0)
            {
                tileDimensions.Y = size.Y;
            }

            this.tileDimensions = tileDimensions;
            this.tileSize = new Vector { X = tileDimensions.X - 1, Y = tileDimensions.Y - 1 };
            this.itemCache = itemCache;

            var regionsX = (int)Math.Ceiling(size.X / (double)tileDimensions.X);
            var regionsY = (int)Math.Ceiling(size.Y / (double)tileDimensions.Y);

            this.worldRegions = new Region[regionsX][];
            Vector current = boundingBox.Min;
            for (int x = 0; x < regionsX; x++)
            {
                this.worldRegions[x] = new Region[regionsY];
                for (int y = 0; y < regionsY; y++)
                {
                    this.worldRegions[x][y] = new Region(current);
                    current.Y += tileDimensions.Y;
                }

                current.X += tileDimensions.X;
                current.Y = boundingBox.Min.Y;
            }
        }

        /// <summary>
        ///   Finalizes an instance of the <see cref = "GridWorld" /> class.
        /// </summary>
        ~GridWorld()
        {
            this.Dispose(false);
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the area.
        /// </summary>
        public BoundingBox Area
        {
            get
            {
                return this.rectangleArea;
            }
        }

        /// <summary>
        ///   Gets the cache for all <see cref = "Item">Items</see> in this world.
        /// </summary>
        public ItemCache ItemCache
        {
            get
            {
                return this.itemCache;
            }
        }

        /// <summary>
        ///   Gets the size of the used tiles (size of each <see cref = "Region" />).
        /// </summary>
        public Vector TileDimensions
        {
            get
            {
                return this.tileDimensions;
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IDisposable

        /// <summary>
        ///   Disposes all used <see cref = "Region">regions</see>.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IWorld

        /// <summary>
        ///   Gets a region at the given <paramref name = "position" />.
        /// </summary>
        /// <param name = "position">
        ///   The position.
        /// </param>
        /// <returns>
        ///   A <see cref = "Region" /> or null.
        /// </returns>
        public Region GetRegion(Vector position)
        {
            if (this.rectangleArea.Contains(position))
            {
                return this.GetRegionAt(position);
            }

            return null;
        }

        /// <summary>
        ///   Returns a bounding box that is aligned with the Grid.
        /// </summary>
        /// <param name = "area">
        ///   The area.
        /// </param>
        /// <returns>
        ///   A bounding box that 
        /// </returns>
        public BoundingBox GetRegionAlignedBoundingBox(BoundingBox area)
        {
            area = this.rectangleArea.IntersectWith(area);
            if (area.IsValid())
            {
                var result = new BoundingBox { Min = this.GetRegionAt(area.Min).Coordinate, Max = this.GetRegionAt(area.Max).Coordinate + this.tileSize, };
                return result;
            }

            return area;
        }

        /// <summary>
        ///   Gets all <see cref = "Region">regions</see> overlapping with a specific <paramref name = "area" />.
        /// </summary>
        /// <param name = "area">
        ///   The area to return the <see cref = "Region">regions</see> for.
        /// </param>
        /// <returns>
        ///   The <see cref = "Region">regions</see> overlapping with the <paramref name = "area" />.
        /// </returns>
        public HashSet<Region> GetRegions(BoundingBox area)
        {
            return new HashSet<Region>(this.GetRegionEnumerable(area));
        }

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
        public HashSet<Region> GetRegionsExcept(BoundingBox area, BoundingBox except)
        {
            var result = new HashSet<Region>();

            // min x
            if (area.Min.X < except.Min.X)
            {
                // get all left to except
                var box = new BoundingBox { Min = area.Min, Max = new Vector { X = Math.Min(area.Max.X, except.Min.X - 1), Y = area.Max.Y } };
                result.UnionWith(this.GetRegionEnumerable(box));
            }

            // min y
            if (area.Min.Y < except.Min.Y)
            {
                // get all above except
                var box = new BoundingBox { Min = area.Min, Max = new Vector { X = area.Max.X, Y = Math.Min(area.Max.Y, except.Min.Y - 1) } };
                result.UnionWith(this.GetRegionEnumerable(box));
            }

            // max x
            if (area.Max.X > except.Max.X)
            {
                // get all right to except
                var box = new BoundingBox { Min = new Vector { X = Math.Max(area.Min.X, except.Max.X + 1), Y = area.Min.Y }, Max = area.Max };
                result.UnionWith(this.GetRegionEnumerable(box));
            }

            // max y
            if (area.Max.Y > except.Max.Y)
            {
                // get all below except
                var box = new BoundingBox { Min = new Vector { X = area.Min.X, Y = Math.Max(area.Min.Y, except.Max.Y + 1) }, Max = area.Max };
                result.UnionWith(this.GetRegionEnumerable(box));
            }

            return result;
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        ///   Disposes all <see cref = "Region" />s if <paramref name = "disposing" /> is true.
        /// </summary>
        /// <param name = "disposing">
        ///   True if called from <see cref = "Dispose()" />, false if called from destructor.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (Region[] regions in this.worldRegions)
                {
                    foreach (Region region in regions)
                    {
                        region.Dispose();
                    }
                }
            }
        }

        /// <summary>
        ///   The get region at.
        /// </summary>
        /// <param name = "coordinate">
        ///   The coordinate.
        /// </param>
        /// <returns>
        ///   The region.
        /// </returns>
        private Region GetRegionAt(Vector coordinate)
        {
            Vector relativePoint = coordinate - this.rectangleArea.Min;
            int indexX = relativePoint.X / this.tileDimensions.X;
            int indexY = relativePoint.Y / this.tileDimensions.Y;
            return this.worldRegions[indexX][indexY];
        }

        /// <summary>
        ///   Gets all overlapping regions.
        /// </summary>
        /// <param name = "area">
        ///   The area.
        /// </param>
        /// <returns>
        ///   An enumerable of regions.
        /// </returns>
        private IEnumerable<Region> GetRegionEnumerable(BoundingBox area)
        {
            BoundingBox overlap = this.rectangleArea.IntersectWith(area);

            Vector current = overlap.Min;
            while (current.Y <= overlap.Max.Y)
            {
                foreach (Region region in this.GetRegionsForY(overlap, current))
                {
                    yield return region;
                }

                // go stepwise to the bottom
                current.Y += this.tileDimensions.Y;
            }

            if (current.Y > overlap.Max.Y)
            {
                current.Y = overlap.Max.Y;
                foreach (Region region in this.GetRegionsForY(overlap, current))
                {
                    yield return region;
                }
            }

            yield break;
        }

        /// <summary>
        ///   The get region index for y.
        /// </summary>
        /// <param name = "overlap">
        ///   The overlap.
        /// </param>
        /// <param name = "current">
        ///   The current.
        /// </param>
        /// <returns>
        ///   The enumerable.
        /// </returns>
        private IEnumerable<Region> GetRegionsForY(BoundingBox overlap, Vector current)
        {
            // start on left side
            current.X = overlap.Min.X;
            while (current.X <= overlap.Max.X)
            {
                yield return this.GetRegionAt(current);

                // go stepwise to the right
                current.X += this.tileDimensions.X;
            }

            if (current.X > overlap.Max.X)
            {
                current.X = overlap.Max.X;
                yield return this.GetRegionAt(current);
            }
        }

        #endregion
    }
}