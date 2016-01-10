// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Client.WinGrid
{
    using System.Drawing;

    /// <summary>
    /// The settings.
    /// </summary>
    public class Settings : Client.Settings
    {
        /// <summary>
        /// The island image.
        /// </summary>
        internal static readonly Image IslandImage;

        /// <summary>
        /// The island text brush.
        /// </summary>
        internal static readonly Brush IslandTextBrush = Brushes.Yellow;

        /// <summary>
        /// Initializes static members of the <see cref="Settings"/> class.
        /// </summary>
        static Settings()
        {
            IslandImage = new Bitmap(typeof(GameTabPage), "Island.png");
            IslandImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
        }

        /// <summary>
        /// Gets or sets a value indicating whether AutoMove.
        /// </summary>
        public bool AutoMove { get; set; }

        /// <summary>
        /// Gets or sets IntervalMove.
        /// </summary>
        public int AutoMoveInterval { get; set; }

        /// <summary>
        /// Gets or sets Velocity.
        /// </summary>
        public int AutoMoveVelocity { get; set; }

        /// <summary>
        /// Gets or sets IntervalDraw.
        /// </summary>
        public int DrawInterval { get; set; }

        /// <summary>
        /// Gets or sets IntervalSend.
        /// </summary>
        public int SendInterval { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether UseTcp.
        /// </summary>
        public bool UseTcp { get; set; }
    }
}