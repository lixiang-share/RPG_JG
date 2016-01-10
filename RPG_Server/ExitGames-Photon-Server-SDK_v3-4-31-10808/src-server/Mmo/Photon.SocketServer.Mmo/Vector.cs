// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Vector.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Represents a 3D coordinate in the <see cref="IWorld">world</see>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.SocketServer.Mmo
{
    using System;

    /// <summary>
    ///   Represents a 3D coordinate in the <see cref = "IWorld">world</see>.
    /// </summary>
    public struct Vector : IEquatable<Vector>
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the X value.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        ///   Gets or sets Y value.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        ///   Gets or sets Z value.
        /// </summary>
        public int Z { get; set; }

        #endregion

        #region Operators

        /// <summary>
        ///   Adds on Vector to the other.
        /// </summary>
        /// <param name = "a">
        ///   The a.
        /// </param>
        /// <param name = "b">
        ///   The b.
        /// </param>
        /// <returns>
        ///   The sum
        /// </returns>
        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector { X = a.X + b.X, Y = a.Y + b.Y, Z = a.Z + b.Z };
        }

        /// <summary>
        ///   Devides each value of the vector by a value.
        /// </summary>
        /// <param name = "a">
        ///   The a.
        /// </param>
        /// <param name = "b">
        ///   The b.
        /// </param>
        /// <returns>
        ///   A new vector
        /// </returns>
        public static Vector operator /(Vector a, int b)
        {
            return new Vector { X = a.X / b, Y = a.Y / b, Z = a.Z / b };
        }

        /// <summary>
        ///   Compares to vectors.
        /// </summary>
        /// <param name = "coordinate1">
        ///   The coordinate 1.
        /// </param>
        /// <param name = "coordinate2">
        ///   The coordinate 2.
        /// </param>
        /// <returns>
        ///   true if x,y and z of both coordinates are equal
        /// </returns>
        public static bool operator ==(Vector coordinate1, Vector coordinate2)
        {
            return coordinate1.Equals(coordinate2);
        }

        /// <summary>
        ///   Compares to Vectors.
        /// </summary>
        /// <param name = "coordinate1">
        ///   The coordinate 1.
        /// </param>
        /// <param name = "coordinate2">
        ///   The coordinate 2.
        /// </param>
        /// <returns>
        ///   true if X, Y or Z of the coorindates are different.
        /// </returns>
        public static bool operator !=(Vector coordinate1, Vector coordinate2)
        {
            return coordinate1.Equals(coordinate2) == false;
        }

        /// <summary>
        ///   Multiples each value of the vector with a value.
        /// </summary>
        /// <param name = "a">
        ///   The a.
        /// </param>
        /// <param name = "b">
        ///   The b.
        /// </param>
        /// <returns>
        ///   A new vector
        /// </returns>
        public static Vector operator *(Vector a, int b)
        {
            return new Vector { X = a.X * b, Y = a.Y * b, Z = a.Z * b };
        }

        /// <summary>
        ///   Substracts one Vector from the other.
        /// </summary>
        /// <param name = "a">
        ///   The a.
        /// </param>
        /// <param name = "b">
        ///   The b.
        /// </param>
        /// <returns>
        ///   A new vector
        /// </returns>
        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector { X = a.X - b.X, Y = a.Y - b.Y, Z = a.Z - b.Z };
        }

        /// <summary>
        ///   Negates a vector
        /// </summary>
        /// <param name = "a">
        ///   The a.
        /// </param>
        /// <returns>
        ///   A new Vector
        /// </returns>
        public static Vector operator -(Vector a)
        {
            return new Vector { X = -a.X, Y = -a.Y, Z = -a.Z };
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///   Gets the max values from the input vectors.
        /// </summary>
        /// <param name = "value1">
        ///   The value 1.
        /// </param>
        /// <param name = "value2">
        ///   The value 2.
        /// </param>
        /// <returns>
        ///   A new vector.
        /// </returns>
        public static Vector Max(Vector value1, Vector value2)
        {
            return new Vector { X = Math.Max(value1.X, value2.X), Y = Math.Max(value1.Y, value2.Y), Z = Math.Max(value1.Z, value2.Z) };
        }

        /// <summary>
        ///   Gets the min values from the input vectors.
        /// </summary>
        /// <param name = "value1">
        ///   The value 1.
        /// </param>
        /// <param name = "value2">
        ///   The value 2.
        /// </param>
        /// <returns>
        ///   A new vector.
        /// </returns>
        public static Vector Min(Vector value1, Vector value2)
        {
            return new Vector { X = Math.Min(value1.X, value2.X), Y = Math.Min(value1.Y, value2.Y), Z = Math.Min(value1.Z, value2.Z) };
        }

        /// <summary>
        ///   Compares the Vector to an object.
        /// </summary>
        /// <param name = "obj">
        ///   The object to compare.
        /// </param>
        /// <returns>
        ///   true if obj is equal to current instance.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Vector)
            {
                var other = (Vector)obj;
                return this.Equals(other);
            }

            return false;
        }

        /// <summary>
        ///   Calculates the distance to another Vector.
        /// </summary>
        /// <param name = "vector">
        ///   The vector.
        /// </param>
        /// <returns>
        ///   The distance.
        /// </returns>
        public double GetDistance(Vector vector)
        {
            return Math.Sqrt(Math.Pow(vector.X - this.X, 2) + Math.Pow(vector.Y - this.Y, 2) + Math.Pow(vector.Z - this.Z, 2));
        }

        /// <summary>
        ///   Get the hash code.
        /// </summary>
        /// <returns>
        ///   XOR from X, Y and Z.
        /// </returns>
        public override int GetHashCode()
        {
            int result = this.X.GetHashCode();
            result ^= this.Y.GetHashCode();
            result ^= this.Z.GetHashCode();
            return result;
        }

        /// <summary>
        ///   Gets Magnitude.
        /// </summary>
        /// <returns>
        ///   The magnitude.
        /// </returns>
        public double GetMagnitude()
        {
            return Math.Sqrt((this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z));
        }

        /// <summary>
        ///   Build a string showing X, Y and Z.
        /// </summary>
        /// <returns>
        ///   string represenation of this vector.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}({1},{2},{3})", base.ToString(), this.X, this.Y, this.Z);
        }

        #endregion

        #region Implemented Interfaces

        #region IEquatable<Vector>

        /// <summary>
        ///   Compares X, Y and Z to another vector.
        /// </summary>
        /// <param name = "other">
        ///   The other vector.
        /// </param>
        /// <returns>
        ///   True if X, Y and Z equal in both vectors.
        /// </returns>
        public bool Equals(Vector other)
        {
            return this.X.Equals(other.X) && this.Y.Equals(other.Y) && this.Z.Equals(other.Z);
        }

        #endregion

        #endregion
    }
}