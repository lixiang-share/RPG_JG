// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RadarTabPage.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The radar tab page.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Client.WinGrid
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// The radar tab page.
    /// </summary>
    public partial class RadarTabPage : TabPage
    {
        /// <summary>
        /// The item positions.
        /// </summary>
        private readonly Dictionary<string, float[]> itemPositions = new Dictionary<string, float[]>();

        /// <summary>
        /// The world data.
        /// </summary>
        private WorldData worldData;

        /// <summary>
        /// Initializes a new instance of the <see cref="RadarTabPage"/> class.
        /// </summary>
        public RadarTabPage()
        {
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        /// <summary>
        /// Gets or sets BackgroundImage.
        /// </summary>
        public override Image BackgroundImage
        {
            get
            {
                return Settings.IslandImage;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets BackgroundImageLayout.
        /// </summary>
        public override ImageLayout BackgroundImageLayout
        {
            get
            {
                return ImageLayout.Stretch;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets a value indicating whether Initialized.
        /// </summary>
        public bool Initialized { get; private set; }

        /// <summary>
        /// The initialize.
        /// </summary>
        /// <param name="world">
        /// The world.
        /// </param>
        public void Initialize(WorldData world)
        {
            this.worldData = world;
            this.Initialized = true;
        }

        /// <summary>
        /// The on radar update.
        /// </summary>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item Type.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        public void OnRadarUpdate(string itemId, byte itemType, float[] position)
        {
            itemId += itemType;
            if (position == null)
            {
                this.itemPositions.Remove(itemId);
                return;
            }

            if (!this.itemPositions.ContainsKey(itemId))
            {
                this.itemPositions.Add(itemId, position);
                return;
            }

            this.itemPositions[itemId] = position;
        }

        /// <summary>
        /// The on paint.
        /// </summary>
        /// <param name="e">
        /// The event args.
        /// </param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.worldData != null)
            {
                var boardSize = new[] { e.ClipRectangle.Width, e.ClipRectangle.Height };
                GameTabPage.PaintGrid(e.Graphics, boardSize, this.worldData);

                WorldData world = this.worldData;
                float width = world.Width;
                float height = world.Height;
                float w = boardSize[0] / width;
                float h = boardSize[1] / height;
                float minW = Math.Max(4, w);
                float minH = Math.Max(4, h);

                Color color = Color.DeepPink;
                var brush = new SolidBrush(color);

                foreach (float[] entry in this.itemPositions.Values)
                {
                    float x = ((this.worldData.BottomRightCorner[0] - entry[0]) * w) - 1;
                    float y = (entry[1] * h) - 1;

                    e.Graphics.FillRectangle(brush, x - (minW / 2) + 1, y - (minH / 2) + 1, minW - 1, minH - 1);
                }

                string rttString = string.Format("Online: {0}", this.itemPositions.Count);
                e.Graphics.DrawString(rttString, this.Font, Settings.IslandTextBrush, 0, 0);
            }
        }
    }
}