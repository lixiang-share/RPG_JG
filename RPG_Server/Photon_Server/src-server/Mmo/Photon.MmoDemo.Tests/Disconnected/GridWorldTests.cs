// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GridWorldTests.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The grid world tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Tests.Disconnected
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    using Server;

    using SocketServer.Mmo;

    /// <summary>
    /// The grid world tests.
    /// </summary>
    [TestFixture]
    public class GridWorldTests
    {
        /// <summary>
        /// Test for <see cref="GridWorld.GetRegionsExcept"/>
        /// </summary>
        [Test]
        public void GetRegionsExcept()
        {
            var topLeftCorner = new Vector { X = 1, Y = 1 };
            var bottomRightCorner = new Vector { X = 100, Y = 100 };
            var tileDimensions = new Vector { X = 10, Y = 10 };
            var world = new GridWorld(topLeftCorner, bottomRightCorner, tileDimensions, new MmoItemCache());

            var a = new BoundingBox { Min = new Vector { X = 5, Y = 5 }, Max = new Vector { X = 15, Y = 15 } };
            a = world.GetRegionAlignedBoundingBox(a);
            Assert.AreEqual(topLeftCorner, a.Min);
            Assert.AreEqual(tileDimensions * 2, a.Max);

            var b = new BoundingBox { Min = new Vector { X = 12, Y = 12 }, Max = new Vector { X = 22, Y = 22 } };
            b = world.GetRegionAlignedBoundingBox(b);
            Assert.AreEqual(tileDimensions + topLeftCorner, b.Min);
            Assert.AreEqual(tileDimensions * 3, b.Max);

            HashSet<Region> regionsA = world.GetRegions(a);
            Assert.AreEqual(4, regionsA.Count);
            HashSet<Region> regionsB = world.GetRegions(b);
            Assert.AreEqual(4, regionsB.Count);
            HashSet<Region> regions = world.GetRegionsExcept(a, b);
            Assert.AreEqual(3, regions.Count);
            Region region = world.GetRegion(topLeftCorner);
            Assert.IsTrue(regions.Contains(region));
            region = world.GetRegion(topLeftCorner + new Vector { X = world.TileDimensions.X });
            Assert.IsTrue(regions.Contains(region));
            region = world.GetRegion(topLeftCorner + new Vector { Y = world.TileDimensions.Y });
            Assert.IsTrue(regions.Contains(region));
            region = world.GetRegion(topLeftCorner + world.TileDimensions);
            Assert.IsFalse(regions.Contains(region));

            b = new BoundingBox { Min = new Vector { X = 30, Y = 30 }, Max = new Vector { X = 40, Y = 40 } };
            b = world.GetRegionAlignedBoundingBox(b);
            regions = world.GetRegionsExcept(a, b);
            Assert.AreEqual(4, regions.Count);
        }

        /// <summary>
        /// The region indexes and borders 1.
        /// </summary>
        [Test]
        public void RegionIndexesAndBorders1()
        {
            var topLeftCorner = new Vector { X = 1, Y = 1 };
            var bottomRightCorner = new Vector { X = 99, Y = 999 };
            var tileDimensions = new Vector { X = 10, Y = 10 };
            var world = new GridWorld(topLeftCorner, bottomRightCorner, tileDimensions, new MmoItemCache());

            RegionIndexesAndBorders(world);
        }

        /// <summary>
        /// The region indexes and borders 2.
        /// </summary>
        [Test]
        public void RegionIndexesAndBorders2()
        {
            var topLeftCorner = new Vector { X = -100, Y = -100 };
            var bottomRightCorner = new Vector { X = -1, Y = -1 };
            var tileDimensions = new Vector { X = 10, Y = 10 };
            var world = new GridWorld(topLeftCorner, bottomRightCorner, tileDimensions, new MmoItemCache());

            RegionIndexesAndBorders(world);
        }

        /// <summary>
        /// The region indexes and borders 3.
        /// </summary>
        [Test]
        public void RegionIndexesAndBorders3()
        {
            var topLeftCorner = new Vector { X = -100, Y = -100 };
            var bottomRightCorner = new Vector { X = 100, Y = 100 };
            var tileDimensions = new Vector { X = 10, Y = 10 };
            var world = new GridWorld(topLeftCorner, bottomRightCorner, tileDimensions, new MmoItemCache());

            RegionIndexesAndBorders(world);
        }

        /// <summary>
        /// The region indexes and borders 4.
        /// </summary>
        [Test]
        public void RegionIndexesAndBorders4()
        {
            var topLeftCorner = new Vector { X = 0, Y = 0 };
            var bottomRightCorner = new Vector { X = 100, Y = 100 };
            var tileDimensions = new Vector { X = 1, Y = 1 };
            var world = new GridWorld(topLeftCorner, bottomRightCorner, tileDimensions, new MmoItemCache());

            RegionIndexesAndBorders(world);
        }

        /// <summary>
        /// The region indexes and borders 5.
        /// </summary>
        [Test]
        public void RegionIndexesAndBorders5()
        {
            var topLeftCorner = new Vector { X = 0, Y = 0 };
            var bottomRightCorner = new Vector { X = 100, Y = 100 };
            var tileDimensions = new Vector { X = -1, Y = -1 };
            var world = new GridWorld(topLeftCorner, bottomRightCorner, tileDimensions, new MmoItemCache());

            RegionIndexesAndBorders(world);
        }

        /// <summary>
        /// The tester.
        /// </summary>
        /// <param name="world">
        /// The world.
        /// </param>
        private static void RegionIndexesAndBorders(GridWorld world)
        {
            Region region;
            var current = new Vector();
            for (current.X = world.Area.Min.X; current.X < world.Area.Max.X; current.X++)
            {
                for (current.Y = world.Area.Min.Y; current.Y < world.Area.Max.Y; current.Y++)
                {
                    Assert.IsNotNull(world.GetRegion(current), current.ToString());
                }
            }

            try
            {
                for (current.Y = world.Area.Min.Y; current.Y <= world.Area.Max.Y; current.Y += world.TileDimensions.Y)
                {
                    // current.Y = (float)Math.Round(current.Y, 2);
                    for (current.X = world.Area.Min.X; current.X <= world.Area.Max.X; current.X += world.TileDimensions.X)
                    {
                        // current.X = (float)Math.Round(current.X, 2);
                        Assert.IsNotNull(region = world.GetRegion(current), "null at {0}", current);
                        Assert.AreEqual(current, region.Coordinate, current.ToString());
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine(current);
                throw;
            }

            current.X = world.Area.Min.X - 1;
            Assert.IsNull(world.GetRegion(current));
            current.Y = world.Area.Min.Y - 1;
            Assert.IsNull(world.GetRegion(current));
            current.X = world.Area.Min.X;
            Assert.IsNull(world.GetRegion(current));

            current.Y = world.Area.Max.Y + 1;
            Assert.IsNull(world.GetRegion(current));
            current.X = world.Area.Max.X + 1;
            Assert.IsNull(world.GetRegion(current));
            current.Y = world.Area.Max.Y;
            Assert.IsNull(world.GetRegion(current));

            Assert.NotNull(world.GetRegion(world.Area.Min));
            Assert.NotNull(world.GetRegion(world.Area.Max));

            world.GetRegions(world.Area);
        }
    }
}