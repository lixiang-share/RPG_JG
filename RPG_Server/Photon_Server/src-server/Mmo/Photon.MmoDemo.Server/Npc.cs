// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Npc.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The mmo npc.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server
{
    using System;
    using System.Collections;

    using ExitGames.Concurrency.Fibers;

    using Photon.SocketServer.Mmo;

    /// <summary>
    /// The mmo npc.
    /// </summary>
    public sealed class Npc : IDisposable
    {
        /// <summary>
        /// The fiber.
        /// </summary>
        private readonly IFiber fiber;

        /// <summary>
        /// The interest area.
        /// </summary>
        private readonly NpcInterestArea interestArea;

        /// <summary>
        /// The representation.
        /// </summary>
        private readonly NpcItem representation;

        /// <summary>
        /// Initializes a new instance of the <see cref="Npc"/> class.
        /// </summary>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="properties">
        /// The properties.
        /// </param>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item Type.
        /// </param>
        /// <param name="world">
        /// The world.
        /// </param>
        /// <param name="viewDistanceEnter">
        /// The view Distance Enter.
        /// </param>
        /// <param name="viewDistanceExit">
        /// The view Distance Exit.
        /// </param>
        public Npc(Vector position, Hashtable properties, string itemId, byte itemType, IWorld world, Vector viewDistanceEnter, Vector viewDistanceExit)
        {
            this.fiber = new PoolFiber();
            this.representation = new NpcItem(position, properties, itemId, itemType, world, this.fiber);
            if (!world.ItemCache.AddItem(this.representation))
            {
                throw new InvalidOperationException();
            }

            this.fiber.Start();

            this.interestArea = new NpcInterestArea(itemId, world);
            this.interestArea.AttachToItem(this.representation);
            this.interestArea.ViewDistanceEnter = viewDistanceEnter;
            this.interestArea.ViewDistanceExit = viewDistanceExit;
            this.interestArea.UpdateInterestManagement();
            this.representation.Position = position;
            this.representation.UpdateInterestManagement();
        }

        #region Implemented Interfaces

        #region IDisposable

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.fiber.Dispose();
        }

        #endregion

        #endregion
    }
}