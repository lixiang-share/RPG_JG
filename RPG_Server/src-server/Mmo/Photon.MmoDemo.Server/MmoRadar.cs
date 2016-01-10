// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MmoRadar.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Subscribers of the MmoRadar's <see cref="Channel" /> receive event <see cref="RadarUpdate" /> for all moving <see cref="Item">Items</see> in the <see cref="IWorld" />.
//   The receive interval is configured with <see cref="Settings.RadarUpdateInterval" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server
{
    using System;
    using System.Collections.Generic;

    using ExitGames.Concurrency.Fibers;

    using Messages;

    using Photon.MmoDemo.Common;

    using Photon.MmoDemo.Server.Events;

    using Photon.SocketServer;
    using Photon.SocketServer.Concurrency;
    using Photon.SocketServer.Mmo;
    using Photon.SocketServer.Mmo.Messages;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// Subscribers of the MmoRadar's <see cref="Channel"/> receive event <see cref="RadarUpdate"/> for all moving <see cref="Item">Items</see> in the <see cref="IWorld"/>.
    /// The receive interval is configured with <see cref="Settings.RadarUpdateInterval"/>.
    /// </summary>
    public sealed class MmoRadar : IDisposable
    {
        /// <summary>
        /// The action queue.
        /// </summary>
        private readonly ActionQueue actionQueue;

        /// <summary>
        /// The channel.
        /// </summary>
        private readonly MessageChannel<ItemEventMessage> channel;

        /// <summary>
        /// The fiber.
        /// </summary>
        private readonly IFiber fiber;

        /// <summary>
        /// The item positions.
        /// </summary>
        private readonly Dictionary<Item, float[]> itemPositions;

        /// <summary>
        /// The item subscriptions.
        /// </summary>
        private readonly Dictionary<Item, IDisposable> itemSubscriptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="MmoRadar"/> class.
        /// </summary>
        public MmoRadar()
        {
            this.fiber = new PoolFiber();
            this.fiber.Start();
            this.channel = new MessageChannel<ItemEventMessage>(ItemEventMessage.CounterEventSend);
            this.itemPositions = new Dictionary<Item, float[]>();
            this.itemSubscriptions = new Dictionary<Item, IDisposable>();
            this.actionQueue = new ActionQueue(this, this.fiber);
        }

        /// <summary>
        /// Gets the channel that publishes event <see cref="RadarUpdate"/>.
        /// </summary>
        public MessageChannel<ItemEventMessage> Channel
        {
            get
            {
                return this.channel;
            }
        }

        /// <summary>
        /// Registers an <see cref="Item"/> with the radar.
        /// The radar will receive position changes from the item and publish them with his <see cref="Channel"/>.
        /// The publish interval can be configured with <see cref="Settings.RadarUpdateInterval">Settings.RadarUpdateInterval</see>.
        /// </summary>
        /// <param name="item">
        /// The new item.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        public void AddItem(Item item, float[] position)
        {
            this.actionQueue.EnqueueAction(
                () =>
                {
                    this.itemPositions.Add(item, position);

                    // update radar every 10 seconds
                    IDisposable positionUpdates = item.PositionUpdateChannel.SubscribeToLast(
                        this.fiber, this.UpdatePosition, Settings.RadarUpdateInterval);
                    IDisposable disposeMessage = item.DisposeChannel.Subscribe(this.fiber, this.RemoveItem);
                    var unsubscriber = new UnsubscriberCollection(positionUpdates, disposeMessage);
                    this.itemSubscriptions.Add(item, unsubscriber);

                    this.PublishUpdate(item, position, true);
                });
        }

        /// <summary>
        /// Send event <see cref="RadarUpdate"/> for all registered <see cref="Item">Items</see> to the peer.
        /// </summary>
        /// <param name="peer">
        /// The client peer.
        /// </param>
        public void SendContentToPeer(MmoPeer peer)
        {
            this.actionQueue.EnqueueAction(() => this.PublishAll(peer));
        }

        #region Implemented Interfaces

        #region IDisposable

        /// <summary>
        /// Disposes the fiber and clears all subscriptions and dictionaries.
        /// </summary>
        public void Dispose()
        {
            this.fiber.Dispose();
            this.Channel.ClearSubscribers();
            foreach (IDisposable unsubscriber in this.itemSubscriptions.Values)
            {
                unsubscriber.Dispose();
            }

            this.itemSubscriptions.Clear();
            this.itemPositions.Clear();
        }

        #endregion

        #endregion

        /// <summary>
        /// The get update event.
        /// </summary>
        /// <param name="item">
        /// The updated item.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <returns>
        /// a new radar update event
        /// </returns>
        private static RadarUpdate GetUpdateEvent(Item item, float[] position)
        {
            return new RadarUpdate { ItemId = item.Id, ItemType = item.Type, Position = position };
        }

        /// <summary>
        /// The publish all.
        /// </summary>
        /// <param name="receiver">
        /// The receiver.
        /// </param>
        private void PublishAll(Peer receiver)
        {
            foreach (KeyValuePair<Item, float[]> entry in this.itemPositions)
            {
                RadarUpdate message = GetUpdateEvent(entry.Key, entry.Value);
                var eventData = new EventData((byte)EventCode.RadarUpdate, message);
                receiver.SendEvent(eventData, new SendParameters { Unreliable = true, ChannelId = Settings.RadarEventChannel });
            }
        }

        /// <summary>
        /// The publish update.
        /// </summary>
        /// <param name="item">
        /// The publisher.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="unreliable">
        /// The unreliable.
        /// </param>
        private void PublishUpdate(Item item, float[] position, bool unreliable)
        {
            RadarUpdate updateEvent = GetUpdateEvent(item, position);
            IEventData eventData = new EventData((byte)EventCode.RadarUpdate, updateEvent);
            var message = new ItemEventMessage(item, eventData, new SendParameters { Unreliable = unreliable, ChannelId = Settings.RadarEventChannel });
            this.channel.Publish(message);
        }

        /// <summary>
        /// The remove item.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        private void RemoveItem(ItemDisposedMessage message)
        {
            ////log.InfoFormat("remove item {0}", message.Source.Id);
            Item item = message.Source;
            this.itemPositions.Remove(item);
            this.itemSubscriptions[item].Dispose();
            this.itemSubscriptions.Remove(item);

            this.PublishUpdate(item, null, false);
        }

        /// <summary>
        /// The update position.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        private void UpdatePosition(ItemPositionMessage message)
        {
            var positionUpdate = (MmoItemPositionUpdate)message;
            Item item = message.Source;
            if (this.itemPositions.ContainsKey(item))
            {
                this.itemPositions[item] = positionUpdate.Coordinate;
                this.PublishUpdate(item, positionUpdate.Coordinate, true);
            }
        }
    }
}