// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NpcMmoWorld.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This <see cref="GridWorld" /> subclass has a <see cref="Name" /> and a <see cref="MmoRadar" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server
{
    using System;
    using System.Collections;
    using System.Reflection;

    using log4net;

    using Photon.MmoDemo.Common;
    using Photon.SocketServer.Mmo;

    /// <summary>
    /// This <see cref="GridWorld"/> subclass has a <see cref="Name"/> and a <see cref="MmoRadar"/>.
    /// </summary>
    public class MmoWorld : GridWorld
    {
        /// <summary>
        /// The radar.
        /// </summary>
        public readonly MmoRadar Radar = new MmoRadar();

        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The world name.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The npc 1.
        /// </summary>
        private readonly Npc npc1;

        /// <summary>
        /// The npc 2.
        /// </summary>
        private readonly Npc npc2;

        /// <summary>
        /// The npc 3.
        /// </summary>
        private readonly Npc npc3;

        /// <summary>
        /// The npc 4.
        /// </summary>
        private readonly Npc npc4;

        /// <summary>
        /// Initializes a new instance of the <see cref="MmoWorld"/> class. 
        /// </summary>
        /// <param name="name">
        /// The world name.
        /// </param>
        /// <param name="topLeftCorner">
        /// The top left corner.
        /// </param>
        /// <param name="bottomRightCorner">
        /// The bottom right corner.
        /// </param>
        /// <param name="tileDimensions">
        /// The tile dimensions.
        /// </param>
        public MmoWorld(string name, Vector topLeftCorner, Vector bottomRightCorner, Vector tileDimensions)
            : base(topLeftCorner, bottomRightCorner, tileDimensions, new MmoItemCache())
        {
            this.name = name;

            log.InfoFormat("created world {0}", name);

            const string PropertyKeyColor = "color";
            const string PropertyKeyInterestAreaAttached = "attached";
            const string PropertyKeyText = "text";
            const string PropertyKeyViewDistanceEnter = "enter";
            const string PropertyKeyViewDistanceExit = "exit";

            var viewDistanceEnter = new Vector { X = (this.TileDimensions.X / 2) + 1, Y = (this.TileDimensions.Y / 2) + 1 };
            var viewDistanceExit = new Vector
                {
                    X = Math.Max(viewDistanceEnter.X + this.TileDimensions.X, (int)(1.5f * viewDistanceEnter.X)), 
                    Y = Math.Max(viewDistanceEnter.Y + this.TileDimensions.Y, (int)(1.5f * viewDistanceEnter.Y))
                };
            int color;
            unchecked
            {
                color = (int)0xFFFFFFFF;
            }

            var properties = new Hashtable
                {
                    { PropertyKeyInterestAreaAttached, true }, 
                    { PropertyKeyViewDistanceEnter, viewDistanceEnter.ToFloatArray() }, 
                    { PropertyKeyViewDistanceExit, viewDistanceExit.ToFloatArray() }, 
                    { PropertyKeyColor, color }, 
                    { PropertyKeyText, "NPC" }
                };

            // left and right are swapped in island demo
            this.npc1 = new Npc(topLeftCorner, properties, "top right npc", (byte)ItemType.Avatar, this, viewDistanceEnter, viewDistanceExit);
            this.npc2 = new Npc(bottomRightCorner, properties, "bottom left npc", (byte)ItemType.Avatar, this, viewDistanceEnter, viewDistanceExit);
            var bottomLeftCorner = new Vector { X = topLeftCorner.X, Y = bottomRightCorner.Y };
            this.npc3 = new Npc(
                bottomLeftCorner, 
                properties, 
                "bottom right npc", 
                (byte)ItemType.Avatar, 
                this, 
                viewDistanceEnter, 
                viewDistanceExit);
            var topRightCorner = new Vector { X = bottomRightCorner.X, Y = topLeftCorner.Y };
            this.npc4 = new Npc(
                topRightCorner, 
                properties, 
                "top left npc", 
                (byte)ItemType.Avatar, 
                this, 
                viewDistanceEnter, 
                viewDistanceExit);
        }

        /// <summary>
        /// Gets Name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }
}