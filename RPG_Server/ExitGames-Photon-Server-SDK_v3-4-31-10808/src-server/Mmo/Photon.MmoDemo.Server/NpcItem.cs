// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NpcItem.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The npc item.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server
{
    using System.Collections;

    using ExitGames.Concurrency.Fibers;

    using Messages;

    using SocketServer;
    using SocketServer.Mmo;
    using SocketServer.Mmo.Messages;

    /// <summary>
    /// The npc item.
    /// </summary>
    public class NpcItem : Item, IMmoItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NpcItem"/> class.
        /// </summary>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="properties">
        /// The properties.
        /// </param>
        /// <param name="id">
        /// The item id.
        /// </param>
        /// <param name="type">
        /// The item type.
        /// </param>
        /// <param name="world">
        /// The world.
        /// </param>
        /// <param name="fiber">
        /// The fiber.
        /// </param>
        public NpcItem(Vector position, Hashtable properties, string id, byte type, IWorld world, IFiber fiber)
            : base(position, properties, id, type, world, fiber)
        {
        }

        #region Implemented Interfaces

        #region IMmoItem

        /// <summary>
        /// Checks wheter the <paramref name="actor"/> is allowed to change the item.
        /// </summary>
        /// <param name="actor">
        /// The accessing actor.
        /// </param>
        /// <returns>
        /// Always false.
        /// </returns>
        public bool GrantWriteAccess(MmoActor actor)
        {
            return false;
        }

        /// <summary>
        /// Receives an event.
        /// Disabled for npc items.
        /// </summary>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        /// <returns>
        /// Always false.
        /// </returns>
        public bool ReceiveEvent(EventData eventData)
        {
            return false;
        }

        #endregion

        #endregion

        /// <summary>
        /// Override to include the rotation and float[] coordinate on item subscribe.
        /// </summary>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="region">
        /// The region.
        /// </param>
        /// <returns>
        /// An instance of <see cref="MmoItemSnapshot"/>.
        /// </returns>
        protected override ItemSnapshot GetItemSnapshot(Vector position, Region region)
        {
            return new MmoItemSnapshot(this, position, region, this.PropertiesRevision, null, this.Position.ToFloatArray());
        }

        /// <summary>
        /// Override to include the float[] coordinate for the <see cref="MmoRadar"/>.
        /// </summary>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="region">
        /// The region.
        /// </param>
        /// <returns>
        /// An instance of <see cref="MmoItemPositionUpdate"/>.
        /// </returns>
        protected override ItemPositionMessage GetPositionUpdateMessage(Vector position, Region region)
        {
            return new MmoItemPositionUpdate(this, position, region, this.Position.ToFloatArray());
        }
    }
}