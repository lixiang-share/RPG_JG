// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Item.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Represents an entity in a <see cref="IWorld">world</see>.
//   Items are event publisher and the counterpart to <see cref="InterestArea">InterestAreas</see>.
//   <para>
//   Items have
//   <list type="bullet">
//   <item>
//   a <see cref="Type" />,
//   </item>
//   <item>
//   a per type unique <see cref="Id" />,
//   </item>
//   <item>
//   a <see cref="Position" />,
//   </item>
//   <item>
//   <see cref="Properties" /> with a <see cref="PropertiesRevision">revision number</see>
//   </item>
//   <item>
//   and 3 different <see cref="MessageChannel{T}">MessageChannels</see>:
//   <list type="bullet">
//   <item>
//   <see cref="EventChannel" />: <see cref="EventData" /> for <see cref="ClientInterestArea">interest areas</see>.
//   </item>
//   <item>
//   <see cref="PositionUpdateChannel" />: Position updates for attached and subscribed <see cref="InterestArea">interest areas</see>.
//   Attached <see cref="InterestArea">interest areas</see> move to the same position and subscribed <see cref="InterestArea">interest areas</see>
//   unsubscribe when the item leaves the outer threshold (one position update is analyzed every few seconds).
//   </item>
//   <item>
//   <see cref="DisposeChannel" />: Subscribed <see cref="InterestArea">interest areas</see> are informed when the item is disposed in order to unsubscribe.
//   </item>
//   </list>
//   </item>
//   </list>
//   </para>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.SocketServer.Mmo
{
    using System;
    using System.Collections;

    using ExitGames.Concurrency.Fibers;

    using Photon.SocketServer.Concurrency;
    using Photon.SocketServer.Mmo.Messages;

    /// <summary>
    ///   Represents an entity in a <see cref = "IWorld">world</see>. 
    ///   Items are event publisher and the counterpart to <see cref = "InterestArea">InterestAreas</see>.
    ///   <para>
    ///     Items have
    ///     <list type = "bullet">
    ///       <item>
    ///         a <see cref = "Type" />,
    ///       </item>
    ///       <item>
    ///         a per type unique <see cref = "Id" />,
    ///       </item>
    ///       <item>
    ///         a <see cref = "Position" />,
    ///       </item>
    ///       <item>
    ///         <see cref = "Properties" /> with a <see cref = "PropertiesRevision">revision number</see>
    ///       </item>
    ///       <item>
    ///         and 3 different <see cref = "MessageChannel{T}">MessageChannels</see>: 
    ///         <list type = "bullet">
    ///           <item>
    ///             <see cref = "EventChannel" />: <see cref = "EventData" /> for <see cref = "ClientInterestArea">interest areas</see>.
    ///           </item>
    ///           <item>
    ///             <see cref = "PositionUpdateChannel" />: Position updates for attached and subscribed <see cref = "InterestArea">interest areas</see>. 
    ///             Attached <see cref = "InterestArea">interest areas</see> move to the same position and subscribed <see cref = "InterestArea">interest areas</see> 
    ///             unsubscribe when the item leaves the outer threshold (one position update is analyzed every few seconds).
    ///           </item>
    ///           <item>
    ///             <see cref = "DisposeChannel" />: Subscribed <see cref = "InterestArea">interest areas</see> are informed when the item is disposed in order to unsubscribe. 
    ///           </item>
    ///         </list>
    ///       </item>
    ///     </list>
    ///   </para>
    /// </summary>
    /// <remarks>
    ///   Item accessing operations are required to be invoked on the item's <see cref = "Fiber" />.
    /// </remarks>
    public class Item : IDisposable
    {
#if MissingSubscribeDebug
        private static readonly ExitGames.Logging.ILogger log = ExitGames.Logging.LogManager.GetCurrentClassLogger();
#endif

        /// <summary>
        ///   The dispose channel.
        /// </summary>
        private readonly MessageChannel<ItemDisposedMessage> disposeChannel;

        /// <summary>
        ///   The item eventChannel.
        /// </summary>
        private readonly MessageChannel<ItemEventMessage> eventChannel;

        /// <summary>
        ///   The fiber.
        /// </summary>
        private readonly IFiber fiber;

        /// <summary>
        ///   The id.
        /// </summary>
        private readonly string id;

        /// <summary>
        ///   The position region.
        /// </summary>
        private readonly MessageChannel<ItemPositionMessage> positionUpdateChannel;

        /// <summary>
        ///   The properties.
        /// </summary>
        private readonly Hashtable properties;

        /// <summary>
        ///   The type.
        /// </summary>
        private readonly byte type;

        /// <summary>
        ///   The world.
        /// </summary>
        private readonly IWorld world;

        /// <summary>
        ///   The disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Item" /> class.
        /// </summary>
        /// <param name = "position">
        ///   The position.
        /// </param>
        /// <param name = "properties">
        ///   The properties.
        /// </param>
        /// <param name = "id">
        ///   The id.
        /// </param>
        /// <param name = "type">
        ///   The type.
        /// </param>
        /// <param name = "world">
        ///   The world.
        /// </param>
        /// <param name = "fiber">
        ///   The fiber. Typically identical to the owner's <see cref = "PeerBase.RequestFiber">request fiber</see>.
        /// </param>
        public Item(Vector position, Hashtable properties, string id, byte type, IWorld world, IFiber fiber)
        {
            this.Position = position;
            this.eventChannel = new MessageChannel<ItemEventMessage>(ItemEventMessage.CounterEventSend);
            this.disposeChannel = new MessageChannel<ItemDisposedMessage>(MessageCounters.CounterSend);
            this.positionUpdateChannel = new MessageChannel<ItemPositionMessage>(MessageCounters.CounterSend);
            this.properties = properties ?? new Hashtable();
            if (properties != null)
            {
                this.PropertiesRevision++;
            }

            this.fiber = fiber;
            this.id = id;
            this.world = world;
            this.type = type;
        }

        /// <summary>
        ///   Finalizes an instance of the <see cref = "Item" /> class. 
        ///   Suppressed by Dispose.
        /// </summary>
        ~Item()
        {
            this.Dispose(false);
        }

        /// <summary>
        ///   Gets the item fiber.
        /// </summary>
        public IFiber Fiber
        {
            get
            {
                return this.fiber;
            }
        }

        /// <summary>
        ///   Gets the <see cref = "Region" /> where at the item's current <see cref = "Position" />.
        /// </summary>
        public Region CurrentWorldRegion { get; private set; }

        /// <summary>
        ///   Gets the channel that is used to publish <see cref = "ItemDisposedMessage">dispose messages</see>.
        ///   Subscribed <see cref = "InterestArea">interest areas</see> unsubscribe when receiving the message.
        ///   <see cref = "Dispose(bool)" /> publishes the <see cref = "ItemDisposedMessage" /> message.
        /// </summary>
        public MessageChannel<ItemDisposedMessage> DisposeChannel
        {
            get
            {
                return this.disposeChannel;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this item has been disposed.
        ///   Actions that where enqueued on the <see cref = "Fiber" /> could arrive after the item has been disposed.
        ///   Check this property to ensure that your operation is legal.
        /// </summary>
        public bool Disposed
        {
            get
            {
                return this.disposed;
            }
        }

        /// <summary>
        ///   Gets the channel that is used to publish <see cref = "ItemEventMessage">ItemEventMessages</see>.
        ///   <see cref = "ClientInterestArea">ClientInterestAreas</see> subscribe this channel to forward all received <see cref = "EventData">events</see> to the client <see cref = "PeerBase" />.
        ///   <see cref = "ItemEventMessage">ItemEventMessages</see> are published by the developer's application.
        /// </summary>
        public MessageChannel<ItemEventMessage> EventChannel
        {
            get
            {
                return this.eventChannel;
            }
        }

        /// <summary>
        ///   Gets the item's Id.
        ///   Unique per <see cref = "Type" /> and <see cref = "ItemCache" />.
        /// </summary>
        public string Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        ///   Gets or sets the item's current position.
        ///   The position is used for interest management internal calculations.
        /// </summary>
        public Vector Position { get; set; }

        /// <summary>
        ///   Gets the channel that is used to publish <see cref = "ItemPositionMessage">ItemPositionMessages</see>.
        ///   Subscribed <see cref = "InterestArea">interest areas</see> use this channel to determine when to unsubscribe.
        ///   Attached <see cref = "InterestArea">interest areas</see> use this channel to update their current position accordingly.
        ///   <see cref = "ItemPositionMessage" /> are published with <see cref = "UpdateInterestManagement" />.
        /// </summary>
        public MessageChannel<ItemPositionMessage> PositionUpdateChannel
        {
            get
            {
                return this.positionUpdateChannel;
            }
        }

        /// <summary>
        ///   Gets the item properties.
        ///   Set with <see cref = "SetProperties">SetProperties</see>.
        /// </summary>
        public Hashtable Properties
        {
            get
            {
                return this.properties;
            }
        }

        /// <summary>
        ///   Gets or sets the current properties revision number.
        ///   Incremented with <see cref = "SetProperties">SetProperties</see>.
        /// </summary>
        public int PropertiesRevision { get; set; }

        /// <summary>
        ///   Gets the item type.
        ///   This is for the client to distinguish what kind of item to display.
        /// </summary>
        public byte Type
        {
            get
            {
                return this.type;
            }
        }

        /// <summary>
        ///   Gets the world the item is member of.
        /// </summary>
        public IWorld World
        {
            get
            {
                return this.world;
            }
        }

        /// <summary>
        ///   Gets or sets CurrentWorldRegionSubscription.
        /// </summary>
        private IDisposable CurrentWorldRegionSubscription { get; set; }

        /// <summary>
        ///   Does nothing but calling <see cref = "OnDestroy" />.
        /// </summary>
        public void Destroy()
        {
            this.OnDestroy();
        }

        /// <summary>
        ///   Publishes a <see cref = "ItemPositionMessage" /> in the <see cref = "PositionUpdateChannel" /> 
        ///   and in the current <see cref = "Region" /> if it changes
        ///   and then updates the <see cref = "CurrentWorldRegion" />.
        /// </summary>
        public void UpdateInterestManagement()
        {
            Region newRegion = this.World.GetRegion(this.Position);

            // inform attached and auto subscribed (delayed) interest areas
            ItemPositionMessage message = this.GetPositionUpdateMessage(this.Position, newRegion);
            this.positionUpdateChannel.Publish(message);

            if (this.SetCurrentWorldRegion(newRegion))
            {
                // inform unsubscribed interest areas in new region
                ItemSnapshot snapshot = this.GetItemSnapshot();
                newRegion.Publish(snapshot);

#if MissingSubscribeDebug
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("{0} sent snap shot to region {1}", this.id, newRegion.Coordinate);
                }
#endif
            }
        }

        /// <summary>
        ///   Sets the <see cref = "Position" /> and calls <see cref = "UpdateInterestManagement" />.
        /// </summary>
        /// <param name = "position">
        ///   The position.
        /// </param>
        [Obsolete("Use Position_set and UpdateInterestManagement() instead")]
        public void Move(Vector position)
        {
            this.Position = position;
            this.UpdateInterestManagement();
        }

        /// <summary>
        ///   Updates the <see cref = "Properties" /> and increments the <see cref = "PropertiesRevision" />.
        /// </summary>
        /// <param name = "propertiesSet">
        ///   The properties to set.
        /// </param>
        /// <param name = "propertiesUnset">
        ///   The property keys to unset.
        /// </param>
        public void SetProperties(Hashtable propertiesSet, ArrayList propertiesUnset)
        {
            if (propertiesSet != null)
            {
                foreach (DictionaryEntry entry in propertiesSet)
                {
                    this.properties[entry.Key] = entry.Value;
                }
            }

            if (propertiesUnset != null)
            {
                foreach (object key in propertiesUnset)
                {
                    this.properties.Remove(key);
                }
            }

            this.PropertiesRevision++;
        }

        /// <summary>
        ///   Sets the <see cref = "Position" /> and calls <see cref = "UpdateInterestManagement" />.
        /// </summary>
        /// <param name = "position">
        ///   The new position.
        /// </param>
        [Obsolete("Use Position_set and UpdateInterestManagement() instead")]
        public void Spawn(Vector position)
        {
            this.Position = position;
            this.UpdateInterestManagement();
        }

        #region Implemented Interfaces

        #region IDisposable

        /// <summary>
        ///   Calls <see cref = "Dispose(bool)" /> and suppresses finalization.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #endregion

        /// <summary>
        ///   Requests an <see cref = "ItemSnapshot" />.
        /// </summary>
        /// <param name = "snapShotRequest">
        ///   The snap shot request.
        /// </param>
        internal void EnqueueItemSnapshotRequest(ItemSnapshotRequest snapShotRequest)
        {
            this.fiber.Enqueue(
                () =>
                    {
                        if (this.disposed)
                        {
                            return;
                        }

                        snapShotRequest.OnItemReceive(this);
                    });
        }

        /// <summary>
        ///   Creates an <see cref = "ItemSnapshot" /> with a snapshot of the current attributes.
        ///   Override this method to return a subclass of <see cref = "ItemSnapshot" /> that includes more data.
        ///   The return value is published through the <see cref = "CurrentWorldRegion" /> or sent directly to an <see cref = "InterestArea" />.
        /// </summary>
        /// <returns>
        ///   A new <see cref = "ItemSnapshot" />.
        /// </returns>
        protected internal virtual ItemSnapshot GetItemSnapshot()
        {
            return new ItemSnapshot(this, this.Position, this.CurrentWorldRegion, this.PropertiesRevision);
        }

        /// <summary>
        ///   Publishes a <see cref = "ItemDisposedMessage" /> through the <see cref = "DisposeChannel" /> and disposes all channels.
        ///   <see cref = "Disposed" /> is set to true.
        /// </summary>
        /// <param name = "disposing">
        ///   True if called from <see cref = "Dispose()" />, false if called from the finalizer.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.SetCurrentWorldRegion(null);
                this.disposeChannel.Publish(new ItemDisposedMessage(this));
                this.eventChannel.Dispose();
                this.disposeChannel.Dispose();
                this.positionUpdateChannel.Dispose();

                this.disposed = true;
            }
        }

        /// <summary>
        ///   Creates an <see cref = "ItemPositionMessage" /> with the current position and region.
        ///   The return value is published through the <see cref = "PositionUpdateChannel" />.
        /// </summary>
        /// <param name = "position">
        ///   The position.
        /// </param>
        /// <param name = "region">
        ///   The region.
        /// </param>
        /// <returns>
        ///   An instance of <see cref = "ItemPositionMessage" />.
        /// </returns>
        protected virtual ItemPositionMessage GetPositionUpdateMessage(Vector position, Region region)
        {
            return new ItemPositionMessage(this, position, region);
        }

        /// <summary>
        ///   Called from <see cref = "Destroy" />.
        ///   Does nothing.
        /// </summary>
        protected virtual void OnDestroy()
        {
        }

        /// <summary>
        ///   The set current world region.
        /// </summary>
        /// <param name = "newRegion">
        ///   The new region.
        /// </param>
        /// <returns>
        ///   True if the current region changed.
        /// </returns>
        protected bool SetCurrentWorldRegion(Region newRegion)
        {
            // out of bounds
            if (newRegion == null)
            {
                // was not out of bounce before
                if (this.CurrentWorldRegion != null)
                {
#if MissingSubscribeDebug
                    if (log.IsDebugEnabled)
                    {
                        log.DebugFormat("{0} unsubscribed from region {1}", this.id, this.CurrentWorldRegion.Coordinate);
                    }

#endif
                    this.CurrentWorldRegion = null;
                    this.CurrentWorldRegionSubscription.Dispose();
                    this.CurrentWorldRegionSubscription = null;
                }

#if MissingSubscribeDebug
                else if (log.IsDebugEnabled)
                {
                    log.DebugFormat("{0} out of bounds", this.id);
                }
#endif

                return false;
            }

            // was out of bounce before
            if (this.CurrentWorldRegion == null)
            {
                this.CurrentWorldRegionSubscription = newRegion.Subscribe(this.fiber, this.Region_OnReceive);
#if MissingSubscribeDebug
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("{0} subscribed to region {1} - before null", this.id, newRegion.Coordinate);
                }
#endif
                this.CurrentWorldRegion = newRegion;
                return true;
            }

            // current region changed
            if (newRegion != this.CurrentWorldRegion)
            {
                IDisposable newSubscription = newRegion.Subscribe(this.fiber, this.Region_OnReceive);
#if MissingSubscribeDebug
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("{0} subscribed to region {1} - before {2}", this.id, newRegion.Coordinate, this.CurrentWorldRegion.Coordinate);
                }
#endif
                this.CurrentWorldRegionSubscription.Dispose();
                this.CurrentWorldRegionSubscription = newSubscription;
                this.CurrentWorldRegion = newRegion;
                return true;
            }

            return false;
        }

        /// <summary>
        ///   The on region message receive.
        /// </summary>
        /// <param name = "message">
        ///   The message.
        /// </param>
        private void Region_OnReceive(RegionMessage message)
        {
            if (this.disposed)
            {
                return;
            }

            message.OnItemReceive(this);
        }
    }
}