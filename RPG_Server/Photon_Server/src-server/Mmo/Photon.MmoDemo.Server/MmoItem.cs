// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MmoItem.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This <see cref="Item" /> subclass overrides <see cref="OnDestroy">OnDestroy</see> in order to publish event <see cref="ItemDestroyed" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server
{
    using System.Collections;

    using Photon.MmoDemo.Common;
    using Photon.MmoDemo.Server.Events;
    using Photon.MmoDemo.Server.Messages;
    using Photon.SocketServer;
    using Photon.SocketServer.Mmo;
    using Photon.SocketServer.Mmo.Messages;

    /// <summary>
    ///   This <see cref = "Item" /> subclass overrides <see cref = "OnDestroy">OnDestroy</see> in order to publish event <see cref = "ItemDestroyed" />.
    /// </summary>
    public class MmoItem : Item, IMmoItem
    {
        #region Constants and Fields

        /// <summary>
        ///   The owner.
        /// </summary>
        private readonly MmoActor owner;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "MmoItem" /> class.
        /// </summary>
        /// <param name = "world">
        ///   The world.
        /// </param>
        /// <param name = "coordinate">
        ///   The coordinate.
        /// </param>
        /// <param name = "rotation">
        ///   The rotation.
        /// </param>
        /// <param name = "properties">
        ///   The properties.
        /// </param>
        /// <param name = "owner">
        ///   The owner.
        /// </param>
        /// <param name = "itemId">
        ///   The item Id.
        /// </param>
        /// <param name = "itemType">
        ///   The item Type.
        /// </param>
        public MmoItem(IWorld world, float[] coordinate, float[] rotation, Hashtable properties, MmoActor owner, string itemId, byte itemType)
            : base(coordinate.ToVector(), properties, itemId, itemType, world, owner.Peer.RequestFiber)
        {
            this.owner = owner;
            this.Rotation = rotation;
            this.Coordinate = coordinate;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the coordinate.
        /// </summary>
        public float[] Coordinate { get; private set; }

        /// <summary>
        ///   Gets the <see cref = "MmoActor" /> owner.
        /// </summary>
        public MmoActor Owner
        {
            get
            {
                return this.owner;
            }
        }

        /// <summary>
        ///   Gets or sets the rotation.
        /// </summary>
        public float[] Rotation { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        ///   Moves the item.
        /// </summary>
        /// <param name = "coordinate">
        ///   The coordinate.
        /// </param>
        public void Move(float[] coordinate)
        {
            this.Coordinate = coordinate;
            this.Position = coordinate.ToVector();
            this.UpdateInterestManagement();
        }

        /// <summary>
        ///   Spawns the item.
        /// </summary>
        /// <param name = "coordinate">
        ///   The coordinate.
        /// </param>
        public void Spawn(float[] coordinate)
        {
            this.Coordinate = coordinate;
            this.Position = coordinate.ToVector();
            this.UpdateInterestManagement();
        }

        #endregion

        #region Implemented Interfaces

        #region IMmoItem

        /// <summary>
        ///   Checks wheter the <paramref name = "actor" /> is allowed to change the item.
        /// </summary>
        /// <param name = "actor">
        ///   The accessing actor.
        /// </param>
        /// <returns>
        ///   True if the <paramref name = "actor" /> equals the <see cref = "Owner" />.
        /// </returns>
        public bool GrantWriteAccess(MmoActor actor)
        {
            return this.owner == actor;
        }

        /// <summary>
        ///   Sends the event to the owner peer.
        /// </summary>
        /// <param name = "eventData">
        ///   The event data.
        /// </param>
        /// <param name = "sendParameters">
        ///   The send Parameters.
        /// </param>
        /// <returns>
        ///   Always true.
        /// </returns>
        public bool ReceiveEvent(EventData eventData, SendParameters sendParameters)
        {
            this.owner.Peer.SendEvent(eventData, sendParameters);
            return true;
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        ///   Override to include the <see cref = "Rotation" /> and <see cref = "Coordinate" /> on item subscribe.
        /// </summary>
        /// <returns>
        ///   An instance of <see cref = "MmoItemSnapshot" />.
        /// </returns>
        protected override ItemSnapshot GetItemSnapshot()
        {
            return new MmoItemSnapshot(this, this.Position, this.CurrentWorldRegion, this.PropertiesRevision, this.Rotation, this.Coordinate);
        }

        /// <summary>
        ///   Override to include the <see cref = "Coordinate" /> for the <see cref = "MmoRadar" />.
        /// </summary>
        /// <param name = "position">
        ///   The position.
        /// </param>
        /// <param name = "region">
        ///   The region.
        /// </param>
        /// <returns>
        ///   An instance of <see cref = "MmoItemPositionUpdate" />.
        /// </returns>
        protected override ItemPositionMessage GetPositionUpdateMessage(Vector position, Region region)
        {
            return new MmoItemPositionUpdate(this, position, region, this.Coordinate);
        }

        /// <summary>
        ///   Publishes event <see cref = "ItemDestroyed" /> in the <see cref = "Item.EventChannel" />.
        /// </summary>
        protected override void OnDestroy()
        {
            var eventInstance = new ItemDestroyed { ItemId = this.Id, ItemType = this.Type };
            var eventData = new EventData((byte)EventCode.ItemDestroyed, eventInstance);
            var message = new ItemEventMessage(this, eventData, new SendParameters { ChannelId = Settings.ItemEventChannel });
            this.EventChannel.Publish(message);
        }

        #endregion
    }
}