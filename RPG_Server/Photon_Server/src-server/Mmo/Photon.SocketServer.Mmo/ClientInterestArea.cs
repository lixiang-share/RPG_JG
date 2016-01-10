// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientInterestArea.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This <see cref="InterestArea" /> subclass automatically subscribes to the <see cref="Item.EventChannel" />
//   of every subscribed <see cref="Item" /> and forwards the received events to the <see cref="PeerBase" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.SocketServer.Mmo
{
    using System;
    using System.Collections.Generic;

    using ExitGames.Concurrency.Fibers;

    using Photon.SocketServer.Mmo.Messages;

    /// <summary>
    ///   This <see cref = "InterestArea" /> subclass automatically subscribes to the <see cref = "Item.EventChannel" />
    ///   of every subscribed <see cref = "Item" /> and forwards the received events to the <see cref = "PeerBase" />.
    /// </summary>
    public class ClientInterestArea : InterestArea
    {
        #region Constants and Fields

        /// <summary>
        ///   The event channel subscriptions.
        /// </summary>
        private readonly Dictionary<Item, IDisposable> eventChannelSubscriptions;

        /// <summary>
        ///   The fiber for event processing.
        /// </summary>
        private readonly IFiber fiber;

        /// <summary>
        ///   The peer.
        /// </summary>
        private readonly PeerBase peer;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ClientInterestArea" /> class.
        /// </summary>
        /// <param name = "peer">
        ///   The peer.
        /// </param>
        /// <param name = "id">
        ///   The id for this interest area. 
        ///   Unique per <see cref = "Actor" />.
        /// </param>
        /// <param name = "world">
        ///   The <see cref = "IWorld" /> this interest area is watching.
        /// </param>
        /// <param name = "fiber">
        ///   The fiber this intereast receives events on.
        /// </param>
        public ClientInterestArea(PeerBase peer, byte id, IWorld world, IFiber fiber)
            : base(id, world)
        {
            this.peer = peer;
            this.eventChannelSubscriptions = new Dictionary<Item, IDisposable>();
            this.fiber = fiber;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the owner's <see cref = "PeerBase" />.
        /// </summary>
        public PeerBase Peer
        {
            get
            {
                return this.peer;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Subscribes the <see cref = "PeerBase" /> to the item's <see cref = "Item.EventChannel" />.
        /// </summary>
        /// <param name = "itemSnapshot">
        ///   The item snapshot message.
        /// </param>
        protected override void OnItemSubscribed(ItemSnapshot itemSnapshot)
        {
            Item item = itemSnapshot.Source;

            // publish event messages
            IDisposable messageReceiver = item.EventChannel.Subscribe(this.fiber, this.SubscribedItem_OnItemEvent);
            this.eventChannelSubscriptions.Add(item, messageReceiver);
        }

        /// <summary>
        ///   Unsubscribes the <see cref = "PeerBase" /> from the item's <see cref = "Item.EventChannel" />.
        /// </summary>
        /// <param name = "item">
        ///   The item.
        /// </param>
        protected override void OnItemUnsubscribed(Item item)
        {
            IDisposable messageReceiver = this.eventChannelSubscriptions[item];
            this.eventChannelSubscriptions.Remove(item);
            messageReceiver.Dispose();
        }

        /// <summary>
        ///   The subscribed item event.
        /// </summary>
        /// <param name = "message">
        ///   The message.
        /// </param>
        private void SubscribedItem_OnItemEvent(ItemEventMessage message)
        {
            ItemEventMessage.CounterEventReceive.Increment();
            this.peer.SendEvent(message.EventData, message.SendParameters);
        }

        #endregion
    }
}