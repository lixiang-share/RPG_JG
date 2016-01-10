// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoundingBox.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The bounding box is a 2d or 3d shape.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.SocketServer.Mmo
{
    using System;

    /// <summary>
    ///   The bounding box is a 2d or 3d shape.
    /// </summary>
    public struct BoundingBox : IEquatable<BoundingBox>
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the Max coordinate.
        /// </summary>
        public Vector Max { get; set; }

        /// <summary>
        ///   Gets or sets the Min coordinate.
        /// </summary>
        public Vector Min { get; set; }

        #endregion

        #region Operators

        /// <summary>
        ///   Compares two bounding boxes.
        /// </summary>
        /// <param name = "a">
        ///   The a.
        /// </param>
        /// <param name = "b">
        ///   The b.
        /// </param>
        /// <returns>
        ///   True if a and b are equal.
        /// </returns>
        public static bool operator ==(BoundingBox a, BoundingBox b)
        {
            return a.Equals(b);
        }

        /// <summary>
        ///   Compares two bounding boxes.
        /// </summary>
        /// <param name = "a">
        ///   The a.
        /// </param>
        /// <param name = "b">
        ///   The b.
        /// </param>
        /// <returns>
        ///   True if a and b are not equal.
        /// </returns>
        public static bool operator !=(BoundingBox a, BoundingBox b)
        {
            return !a.Equals(b);
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///   Creates a bounding box from a polygon.
        /// </summary>
        /// <param name = "points">
        ///   The points.
        /// </param>
        /// <returns>
        ///   A bounding box.
        /// </returns>
        /// <exception cref = "ArgumentNullException">
        ///   <paramref name = "points" /> is null.
        /// </exception>
        /// <exception cref = "ArgumentException">
        ///   <paramref name = "points" /> has a length of 0.
        /// </exception>
        public static BoundingBox CreateFromPoints(params Vector[] points)
        {
            if (points == null)
            {
                throw new ArgumentNullException("points");
            }

            if (points.Length == 0)
            {
                throw new ArgumentException("points");
            }

            Vector min = points[0];
            Vector max = points[0];
            for (int index = 1; index < points.Length; index++)
            {
                Vector tmp = points[index];
                if (tmp.X < min.X)
                {
                    min.X = tmp.X;
                }

                if (tmp.Y < min.Y)
                {
                    min.Y = tmp.Y;
                }

                if (tmp.Z < min.Z)
                {
                    min.Z = tmp.Z;
                }

                if (tmp.X > max.X)
                {
                    max.X = tmp.X;
                }

                if (tmp.Y > max.Y)
                {
                    max.Y = tmp.Y;
                }

                if (tmp.Z > max.Z)
                {
                    max.Z = tmp.Z;
                }
            }

            return new BoundingBox { Min = min, Max = max };
        }

        /// <summary>
        ///   Checks whether <paramref name = "point" /> exists within bounding box borders.
        /// </summary>
        /// <param name = "point">
        ///   The point.
        /// </param>
        /// <returns>
        ///   True if <paramref name = "point" /> exists inside the box.
        /// </returns>
        public bool Contains(Vector point)
        {
            // not outside of box?
            return (point.X < this.Min.X || point.X > this.Max.X || point.Y < this.Min.Y || point.Y > this.Max.Y || point.Z < this.Min.Z || point.Z > this.Max.Z) ==
                   false;
        }

        /// <summary>
        ///   Checks whether <paramref name = "obj" /> is a bounding box with equal values.
        /// </summary>
        /// <param name = "obj">
        ///   The obj.
        /// </param>
        /// <returns>
        ///   True if <paramref name = "obj" /> has equal values.
        /// </returns>
        public override bool Equals(object obj)
        {
            return (obj is BoundingBox) ? this.Equals((BoundingBox)obj) : false;
        }

        /// <summary>
        ///   Gets all 4 corners of a 2D bounding box with the minimum Z.
        /// </summary>
        /// <returns>
        ///   Four <see cref = "Vector" />s.
        /// </returns>
        public Vector[] GetCorners2D()
        {
            return new[]
                {
                    new Vector { X = this.Min.X, Y = this.Min.Y, Z = this.Min.Z }, new Vector { X = this.Max.X, Y = this.Min.Y, Z = this.Min.Z }, 
                    new Vector { X = this.Min.X, Y = this.Max.Y, Z = this.Min.Z }, new Vector { X = this.Max.X, Y = this.Max.Y, Z = this.Min.Z }
                };
        }

        /// <summary>
        ///   Gets all 8 corners of a 3D bounding box.
        /// </summary>
        /// <returns>
        ///   Eight <see cref = "Vector" />s.
        /// </returns>
        public Vector[] GetCorners3D()
        {
            return new[]
                {
                    new Vector { X = this.Min.X, Y = this.Min.Y, Z = this.Min.Z }, new Vector { X = this.Max.X, Y = this.Min.Y, Z = this.Min.Z }, 
                    new Vector { X = this.Min.X, Y = this.Max.Y, Z = this.Min.Z }, new Vector { X = this.Max.X, Y = this.Max.Y, Z = this.Min.Z }, 
                    new Vector { X = this.Min.X, Y = this.Min.Y, Z = this.Max.Z }, new Vector { X = this.Max.X, Y = this.Min.Y, Z = this.Max.Z }, 
                    new Vector { X = this.Min.X, Y = this.Max.Y, Z = this.Max.Z }, new Vector { X = this.Max.X, Y = this.Max.Y, Z = this.Max.Z }, 
                };
        }

        /// <summary>
        ///   Gets the hash code.
        /// </summary>
        /// <returns>
        ///   The hash code.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Min.GetHashCode() + this.Max.GetHashCode();
        }

        /// <summary>
        ///   Intersects this instance with another bounding box.
        /// </summary>
        /// <param name = "other">
        ///   The other.
        /// </param>
        /// <returns>
        ///   A new bounding box.
        /// </returns>
        public BoundingBox IntersectWith(BoundingBox other)
        {
            return new BoundingBox { Min = Vector.Max(this.Min, other.Min), Max = Vector.Min(this.Max, other.Max) };
        }

        /////// <summary>
        /////// Checks whether the bounding boxes intersect on the X and Y axes.
        /////// </summary>
        /////// <param name="box">
        /////// The box.
        /////// </param>
        /////// <returns>
        /////// True if <paramref name="box"/> overlaps with this instance on the X and Y axes.
        /////// </returns>
        ////public bool Intersects2D(BoundingBox box)
        ////{
        ////    return (this.Max.X < box.Min.X || this.Min.X > box.Max.X || this.Max.Y < box.Min.Y || this.Min.Y > box.Max.Y) == false;
        ////}

        /////// <summary>
        /////// Checks whether the bounding boxes intersect on the X, Y and Z axes.
        /////// </summary>
        /////// <param name="box">
        /////// The box.
        /////// </param>
        /////// <returns>
        /////// True if <paramref name="box"/> overlaps with this instance on the X, Y and Z axes.
        /////// </returns>
        ////public bool Intersects3D(BoundingBox box)
        ////{
        ////    return (this.Max.X < box.Min.X || this.Min.X > box.Max.X || this.Max.Y < box.Min.Y || this.Min.Y > box.Max.Y || this.Max.Z < box.Min.Z ||
        ////            this.Min.Z > box.Max.Z) == false;
        ////}

        /////// <summary>
        /////// Checks whether the bounding boxes intersect on the X axes.
        /////// </summary>
        /////// <param name="box">
        /////// The box.
        /////// </param>
        /////// <returns>
        /////// True if <paramref name="box"/> overlaps with this instance on the X axes.
        /////// </returns>
        ////public bool IntersectsX(BoundingBox box)
        ////{
        ////    return (this.Max.X < box.Min.X || this.Min.X > box.Max.X) == false;
        ////}

        /////// <summary>
        /////// Checks whether the bounding boxes intersect on the Y axes.
        /////// </summary>
        /////// <param name="box">
        /////// The box.
        /////// </param>
        /////// <returns>
        /////// True if <paramref name="box"/> overlaps with this instance on the Y axes.
        /////// </returns>
        ////public bool IntersectsY(BoundingBox box)
        ////{
        ////    return (this.Max.Y < box.Min.Y || this.Min.Y > box.Max.Y) == false;
        ////}

        /////// <summary>
        /////// Checks whether the bounding boxes intersect on the Z axes.
        /////// </summary>
        /////// <param name="box">
        /////// The box.
        /////// </param>
        /////// <returns>
        /////// True if <paramref name="box"/> overlaps with this instance on the Z axes.
        /////// </returns>
        ////public bool IntersectsZ(BoundingBox box)
        ////{
        ////    return (this.Max.Z < box.Min.Z || this.Min.Z > box.Max.Z) == false;
        ////}

        /// <summary>
        ///   Checks whether <see cref = "Max" /> and <see cref = "Min" /> span a valid (positive) area.
        /// </summary>
        /// <returns>
        ///   True if all values of <see cref = "Max" /> are larger than or equal to all values of <see cref = "Min" />.
        /// </returns>
        public bool IsValid()
        {
            return (this.Max.X < this.Min.X || this.Max.Y < this.Min.Y || this.Max.Z < this.Min.Z) == false;
        }

        /// <summary>
        ///   Unites two bounding boxes.
        /// </summary>
        /// <param name = "other">
        ///   The additional.
        /// </param>
        /// <returns>
        ///   A new bounding box.
        /// </returns>
        public BoundingBox UnionWith(BoundingBox other)
        {
            return new BoundingBox { Min = Vector.Min(this.Min, other.Min), Max = Vector.Max(this.Max, other.Max) };
        }

        #endregion

        #region Implemented Interfaces

        #region IEquatable<BoundingBox>

        /// <summary>
        ///   Compares this instance to another bounding box.
        /// </summary>
        /// <param name = "other">
        ///   The other.
        /// </param>
        /// <returns>
        ///   True if both bounding boxes have the same values.
        /// </returns>
        public bool Equals(BoundingBox other)
        {
            return (this.Min == other.Min) && (this.Max == other.Max);
        }

        #endregion

        #endregion
    }
}