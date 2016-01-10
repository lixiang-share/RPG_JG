// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MmoWorld.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This <see cref="GridWorld" /> subclass has a <see cref="Name" /> and a <see cref="MmoRadar" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server
{
    using ExitGames.Logging;

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
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The world name.
        /// </summary>
        private readonly string name;

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