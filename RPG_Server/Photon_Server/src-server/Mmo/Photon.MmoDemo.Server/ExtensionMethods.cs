// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionMethods.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The extension methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server
{
    using System;

    using Photon.SocketServer.Mmo;

    /// <summary>
    ///   The extension methods.
    /// </summary>
    public static class ExtensionMethods
    {
        #region Public Methods

        /// <summary>
        ///   Converts a <see cref = "Vector" /> to a float array.
        ///   Each value is devided by 100.
        /// </summary>
        /// <param name = "vector">
        ///   The vector.
        /// </param>
        /// <param name = "dimensions">
        ///   The dimensions.
        /// </param>
        /// <returns>
        ///   A new float array.
        /// </returns>
        public static float[] ToFloatArray(this Vector vector, byte dimensions)
        {
            switch (dimensions)
            {
                case 0:
                    {
                        return new float[0];
                    }

                case 1:
                    {
                        return new[] { Convert.ToSingle(vector.X) / 100 };
                    }

                case 2:
                    {
                        return new[] { Convert.ToSingle(vector.X) / 100, Convert.ToSingle(vector.Y) / 100 };
                    }

                case 3:
                    {
                        return new[] { Convert.ToSingle(vector.X) / 100, Convert.ToSingle(vector.Y) / 100, Convert.ToSingle(vector.Z) / 100 };
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException("dimensions");
                    }
            }
        }

        /// <summary>
        ///   Converts a float array to a <see cref = "Vector" />.
        ///   Each value is multiplied with 100.
        /// </summary>
        /// <param name = "vector">
        ///   The vector.
        /// </param>
        /// <returns>
        ///   A new <see cref = "Vector" />.
        /// </returns>
        public static Vector ToVector(this float[] vector)
        {
            switch (vector.Length)
            {
                case 0:
                    {
                        return new Vector();
                    }

                case 1:
                    {
                        return new Vector { X = Convert.ToInt32(vector[0] * 100) };
                    }

                case 2:
                    {
                        return new Vector { X = Convert.ToInt32(vector[0] * 100), Y = Convert.ToInt32(vector[1] * 100) };
                    }

                default:
                    {
                        return new Vector { X = Convert.ToInt32(vector[0] * 100), Y = Convert.ToInt32(vector[1] * 100), Z = Convert.ToInt32(vector[2] * 100) };
                    }
            }
        }

        #endregion
    }
}