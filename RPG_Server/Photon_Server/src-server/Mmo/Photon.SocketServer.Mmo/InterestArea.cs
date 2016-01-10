// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InterestArea.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Provides a mechanism to subscribe <see cref="Item">items</see> and <see cref="Region">regions</see> in a <see cref="IWorld">world</see>.
//   <para>
//   Interest areas have
//   <list type="bullet">
//   <item>
//   an <see cref="Id" /> that is unique per <see cref="Actor" />
//   </item>
//   <item>
//   a <see cref="Position" />
//   </item>
//   <item>
//   a subscribe threshold (<see cref="ViewDistanceEnter" />)
//   </item>
//   <item>
//   an unsubscribe threshold (<see cref="ViewDistanceExit" />)
//   </item>
//   </list>
//   The InterestArea subscribes to <see cref="Region">regions</see> that overlap with the inner view radius.
//   </para>
//   <para>
//   Whenever an <see cref="Item" /> moves into a new <see cref="Region" /> it sends an <see cref="ItemSnapshot" /> into the region.
//   Interest areas that receive this message automatically subscribe to the <see cref="Item" /> (unless <see cref="AutoSubscribeItem">AutoSubscribeItem</see> returns false).
//   </para>
//   <para>
//   If the interest area moves and subscribes to a new <see cref="Region" /> it sends a <see cref="ItemSnapshotRequest" /> into the region.
//   Receiving <see cref="Item">items</see> answer with an <see cref="ItemSnapshot" />.
//   </para>
//   <para>
//   The interest area analyzes one <see cref="ItemPositionMessage" /> every few seconds from each subscribed item <see cref="Item.PositionUpdateChannel" />
//   and then checks if the item moved out of the interest area’s outer view radius.
//   The check interval is configured with <see cref="ItemAutoUnsubcribeDelayMilliseconds" />.
//   </para>
//   <para>
//   Interest areas can be attached to an <see cref="Item" />.
//   Once an interest area is attached it moves wherever the item by receiving all position updates from the item’s <see cref="Item.PositionUpdateChannel" />.
//   Per default interest areas do not auto subscribe their <see cref="AttachedItem" /> (see <see cref="AutoSubscribeItem">AutoSubscribeItem</see>).
//   </para>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.SocketServer.Mmo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ExitGames.Concurrency.Fibers;

    using Photon.SocketServer.Concurrency;
    using Photon.SocketServer.Mmo.Messages;

    /// <summary>
    ///   Provides a mechanism to subscribe <see cref = "Item">items</see> and <see cref = "Region">regions</see> in a <see cref = "IWorld">world</see>.  
    ///   <para>
    ///     Interest areas have
    ///     <list type = "bullet">
    ///       <item>
    ///         an <see cref = "Id" /> that is unique per <see cref = "Actor" />
    ///       </item>
    ///       <item>
    ///         a <see cref = "Position" />
    ///       </item>
    ///       <item>
    ///         a subscribe threshold (<see cref = "ViewDistanceEnter" />)
    ///       </item>
    ///       <item>
    ///         an unsubscribe threshold (<see cref = "ViewDistanceExit" />)
    ///       </item>
    ///     </list>
    ///     The InterestArea subscribes to <see cref = "Region">regions</see> that overlap with the inner view radius. 
    ///   </para>
    ///   <para>
    ///     Whenever an <see cref = "Item" /> moves into a new <see cref = "Region" /> it sends an <see cref = "ItemSnapshot" /> into the region. 
    ///     Interest areas that receive this message automatically subscribe to the <see cref = "Item" /> (unless <see cref = "AutoSubscribeItem">AutoSubscribeItem</see> returns false). 
    ///   </para>
    ///   <para>
    ///     If the interest area moves and subscribes to a new <see cref = "Region" /> it sends a <see cref = "ItemSnapshotRequest" /> into the region. 
    ///     Receiving <see cref = "Item">items</see> answer with an <see cref = "ItemSnapshot" />.
    ///   </para>
    ///   <para>
    ///     The interest area analyzes one <see cref = "ItemPositionMessage" /> every few seconds from each subscribed item <see cref = "Item.PositionUpdateChannel" /> 
    ///     and then checks if the item moved out of the interest area’s outer view radius. 
    ///     The check interval is configured with <see cref = "ItemAutoUnsubcribeDelayMilliseconds" />.
    ///   </para>
    ///   <para>
    ///     Interest areas can be attached to an <see cref = "Item" />. 
    ///     Once an interest area is attached it moves wherever the item by receiving all position updates from the item’s <see cref = "Item.PositionUpdateChannel" />. 
    ///     Per default interest areas do not auto subscribe their <see cref = "AttachedItem" /> (see <see cref = "AutoSubscribeItem">AutoSubscribeItem</see>). 
    ///   </para>
    /// </summary>
    /// <remarks>
    ///   Thread safety: All instance members require a lock on <see cref = "SyncRoot" />.
    /// </remarks>
    public class InterestArea : IDisposable
    {
        #region Constants and Fields

        /// <summary>
        ///   Locking the sync root guarantees thread safe access.
        /// </summary>
        public readonly object SyncRoot = new object();

        /// <summary>
        ///   The subscribed items.
        /// </summary>
        private readonly Dictionary<Item, ItemAutoSubscription> autoManagedItemSubscriptions;

        /// <summary>
        ///   The id.
        /// </summary>
        private readonly byte id;

        /// <summary>
        ///   The manual managed item subscriptions.
        /// </summary>
        private readonly Dictionary<Item, IDisposable> manualManagedItemSubscriptions;

        /// <summary>
        ///   The item snap shot request
        /// </summary>
        private readonly ItemSnapshotRequest snapShotRequest;

        /// <summary>
        ///   The subscribedWorldRegions.
        /// </summary>
        private readonly Dictionary<Region, IDisposable> subscribedWorldRegions;

        /// <summary>
        ///   The subscription management fiber.
        /// </summary>
        private readonly IFiber subscriptionManagementFiber;

        /// <summary>
        ///   The world.
        /// </summary>
        private readonly IWorld world;

        /// <summary>
        ///   The world area
        /// </summary>
        private readonly BoundingBox worldArea;

        /// <summary>
        ///   The current inner focus (region boundaries)
        /// </summary>
        private BoundingBox currentRegionInnerFocus;

        /// <summary>
        ///   The current outer focus (region boundaries)
        /// </summary>
        private BoundingBox currentRegionOuterFocus;

        /// <summary>
        ///   The item movement subscription.
        /// </summary>
        private IDisposable itemMovementSubscription;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes static members of the <see cref = "InterestArea" /> class.
        /// </summary>
        static InterestArea()
        {
            ItemAutoUnsubcribeDelayMilliseconds = 5000;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "InterestArea" /> class.
        /// </summary>
        /// <param name = "id">
        ///   The id for this interest area. 
        ///   Unique per <see cref = "Actor" />.
        /// </param>
        /// <param name = "world">
        ///   The <see cref = "IWorld" /> this interest area is watching.
        /// </param>
        protected InterestArea(byte id, IWorld world)
        {
            this.id = id;
            this.world = world;
            this.snapShotRequest = new ItemSnapshotRequest(this);
            this.subscribedWorldRegions = new Dictionary<Region, IDisposable>();
            this.autoManagedItemSubscriptions = new Dictionary<Item, ItemAutoSubscription>();
            this.manualManagedItemSubscriptions = new Dictionary<Item, IDisposable>();
            this.subscriptionManagementFiber = new PoolFiber();
            this.subscriptionManagementFiber.Start();

            this.worldArea = world.Area;

            // make invalid
            this.currentRegionInnerFocus = new BoundingBox { Max = this.worldArea.Min, Min = this.worldArea.Max };
            this.currentRegionOuterFocus = this.currentRegionInnerFocus;
        }

        /// <summary>
        ///   Finalizes an instance of the <see cref = "InterestArea" /> class.
        /// </summary>
        ~InterestArea()
        {
            this.Dispose(false);
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets ItemAutoUnsubcribeDelayMilliseconds.
        ///   Default: 5000ms.
        /// </summary>
        public static int ItemAutoUnsubcribeDelayMilliseconds { get; set; }

        /// <summary>
        ///   Gets the attached <see cref = "Item" />.
        ///   Set with <see cref = "AttachToItem">AttachToItem</see>.
        /// </summary>
        public Item AttachedItem { get; private set; }

        /// <summary>
        ///   Gets the interest area Id.
        ///   Unique per <see cref = "Actor" />.
        /// </summary>
        public byte Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        ///   Gets or sets the interest area <see cref = "Vector" /> position.
        ///   This value is used for internal  management calculations.
        /// </summary>
        public Vector Position { get; set; }

        /// <summary>
        ///   Gets or sets the inner view distance (the item subscribe threshold).
        /// </summary>
        public Vector ViewDistanceEnter { get; set; }

        /// <summary>
        ///   Gets or sets the outer view distance (the item unsubscribe threshold).
        /// </summary>
        public Vector ViewDistanceExit { get; set; }

        /// <summary>
        ///   Gets the <see cref = "IWorld" /> the interest area looks at.
        /// </summary>
        public IWorld World
        {
            get
            {
                return this.world;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///   Attaching an <see cref = "Item" /> to the interest area automatically updates the interest area's <see cref = "Position" /> when the <see cref = "Item" /> moves.
        ///   Attached item's are excluded from the auto-subscription mechanism.
        ///   Detach the item with <see cref = "Detach">Detach</see>.
        /// </summary>
        /// <remarks>
        ///   Thread safety: Requires enqueuing on the item's <see cref = "Item.Fiber" /> and like all instance members a lock on <see cref = "SyncRoot" />.
        /// </remarks>
        /// <param name = "item">
        ///   The newly attached item.
        /// </param>
        public void AttachToItem(Item item)
        {
            if (this.AttachedItem != null)
            {
                throw new InvalidOperationException();
            }

            this.AttachedItem = item;
            this.Position = item.Position;

            if (this.AutoSubscribeItem(item) == false)
            {
                ItemAutoSubscription autoSubscription;
                if (this.autoManagedItemSubscriptions.TryGetValue(item, out autoSubscription))
                {
                    this.AutoUnsubscribeItem(autoSubscription);
                }
            }

            IDisposable disposeSubscription = item.DisposeChannel.Subscribe(this.subscriptionManagementFiber, this.AttachedItem_OnItemDisposed);

            // move camera when item moves
            IDisposable positionSubscription = item.PositionUpdateChannel.Subscribe(this.subscriptionManagementFiber, this.AttachedItem_OnItemPosition);
            this.itemMovementSubscription = new UnsubscriberCollection(disposeSubscription, positionSubscription);
        }

        /// <summary>
        ///   Detaches the interest area from an <see cref = "Item" /> that was attached with <see cref = "AttachToItem">AttachToItem</see>.
        /// </summary>
        public void Detach()
        {
            if (this.AttachedItem != null)
            {
                this.itemMovementSubscription.Dispose();
                this.itemMovementSubscription = null;

                Item item = this.AttachedItem;
                this.AttachedItem = null;

                if (!this.autoManagedItemSubscriptions.ContainsKey(item) && this.AutoSubscribeItem(item))
                {
                    // ask for snapshot of previously attached item in order to subscribe it
                    item.EnqueueItemSnapshotRequest(this.snapShotRequest);
                }

                // following code does the same as line above, but notifies all items instead of just this one
                ////Region region;
                ////if (this.World.GetRegion(this.Position, out region))
                ////{
                ////    // ask for properties of previously attached item in order to subscribe it
                ////    region.Publish(this.snapShotRequest);
                ////}
            }
        }

        /// <summary>
        ///   Subscribes an <see cref = "Item" /> manually. 
        ///   Manually subscribed items are excluded from the auto-unsubscribe mechanism. 
        ///   Leads to <see cref = "OnItemSubscribed">OnItemSubscribed</see>.
        ///   Unsubscribe with <see cref = "UnsubscribeItem">UnsubscribeItem</see>.
        /// </summary>
        /// <remarks>
        ///   Thread safety: Requires enqueuing on the item's <see cref = "Item.Fiber" /> and like all instance members a lock on <see cref = "SyncRoot" />.
        /// </remarks>
        /// <param name = "item">
        ///   The item to subscribe.
        /// </param>
        /// <returns>
        ///   false if item has been subscribed before.
        /// </returns>
        public bool SubscribeItem(Item item)
        {
            if (this.manualManagedItemSubscriptions.ContainsKey(item))
            {
                return false;
            }

            // unsubscribe if item is disposed 
            IDisposable managementListener = item.DisposeChannel.Subscribe(this.subscriptionManagementFiber, this.SubscribedItem_OnItemDisposed);
            this.manualManagedItemSubscriptions.Add(item, managementListener);

            ItemAutoSubscription oldSubscription;
            if (this.autoManagedItemSubscriptions.TryGetValue(item, out oldSubscription))
            {
                oldSubscription.Dispose();
                this.autoManagedItemSubscriptions.Remove(item);
            }
            else
            {
                this.OnItemSubscribed(item.GetItemSnapshot());
            }

            return true;
        }

        /// <summary>
        ///   Unsubscribe an <see cref = "Item" /> that was manually subscribed with <see cref = "SubscribeItem">SubscribeItem</see>.
        /// </summary>
        /// <param name = "item">
        ///   The item.
        /// </param>
        /// <returns>
        ///   true if item had been subscribed.
        /// </returns>
        public bool UnsubscribeItem(Item item)
        {
            IDisposable subscription;
            if (this.manualManagedItemSubscriptions.TryGetValue(item, out subscription))
            {
                subscription.Dispose();
                this.manualManagedItemSubscriptions.Remove(item);
                this.OnItemUnsubscribed(item);
                return true;
            }

            return false;
        }

        /// <summary>
        ///   Updates the <see cref = "Region" /> subscriptions that are used to detect <see cref = "Item">Items</see> in the nearby <see cref = "IWorld" />.
        ///   This method should be called after changing the interest area's <see cref = "Position" />.
        /// </summary>
        public void UpdateInterestManagement()
        {
            // update unsubscribe area
            BoundingBox focus = BoundingBox.CreateFromPoints(this.Position - this.ViewDistanceExit, this.Position + this.ViewDistanceExit);
            BoundingBox outerFocus = focus.IntersectWith(this.worldArea);

            // get subscribe area
            focus = new BoundingBox { Min = this.Position - this.ViewDistanceEnter, Max = this.Position + this.ViewDistanceEnter };
            BoundingBox innerFocus = focus.IntersectWith(this.worldArea);

            innerFocus = this.world.GetRegionAlignedBoundingBox(innerFocus);
            if (innerFocus != this.currentRegionInnerFocus)
            {
                if (innerFocus.IsValid())
                {
                    HashSet<Region> regions = this.currentRegionInnerFocus.IsValid()
                                                  ? this.world.GetRegionsExcept(innerFocus, this.currentRegionInnerFocus)
                                                  : this.world.GetRegions(innerFocus);
                    this.SubscribeRegions(regions);
                }

                this.currentRegionInnerFocus = innerFocus;
            }

            outerFocus = this.world.GetRegionAlignedBoundingBox(outerFocus);
            if (outerFocus != this.currentRegionOuterFocus)
            {
                if (outerFocus.IsValid())
                {
                    IEnumerable<Region> regions = this.currentRegionOuterFocus.IsValid()
                                                      ? (IEnumerable<Region>)this.world.GetRegionsExcept(this.currentRegionOuterFocus, outerFocus)
                                                      : this.subscribedWorldRegions.Keys.Where(r => !outerFocus.Contains(r.Coordinate)).ToArray();
                    this.currentRegionOuterFocus = outerFocus;
                    this.UnsubscribeRegions(regions);
                }
                else
                {
                    this.currentRegionOuterFocus = outerFocus;
                }
            }
        }

        /// <summary>
        ///   Obsolete. Calls <see cref = "UpdateInterestManagement" />.
        /// </summary>
        [Obsolete("Use UpdateInterestManagement() instead")]
        public void UpdateRegionSubscriptions()
        {
            this.UpdateInterestManagement();
        }

        #endregion

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

        #region Methods

        /// <summary>
        ///   Receives the <see cref = "ItemSnapshot" />.
        ///   auto subscribes item if necessary.
        /// </summary>
        /// <param name = "message">
        ///   The message.
        /// </param>
        internal void ReceiveItemSnapshot(ItemSnapshot message)
        {
            lock (this.SyncRoot)
            {
                // auto subscribe item?
                if (this.AutoSubscribeItem(message.Source) == false)
                {
                    return;
                }

                ItemAutoSubscription subscription;

                // already subscribed
                if (this.autoManagedItemSubscriptions.TryGetValue(message.Source, out subscription))
                {
                    // dropped out of world, unsubscribe
                    if (message.WorldRegion == null)
                    {
                        this.AutoUnsubscribeItem(subscription);
                        return;
                    }

                    // update position
                    subscription.ItemPosition = message.Position;
                    subscription.WorldRegion = message.WorldRegion;
                    return;
                }

                // already subscribed
                if (this.manualManagedItemSubscriptions.ContainsKey(message.Source))
                {
                    return;
                }

                // item not in view
                if (message.WorldRegion == null || this.subscribedWorldRegions.ContainsKey(message.WorldRegion) == false)
                {
                    return;
                }

                // unsubscribe if item is disposed
                IDisposable disposeListener = message.Source.DisposeChannel.Subscribe(this.subscriptionManagementFiber, this.AutoSubscribedItem_OnItemDisposed);

                // unsubscribe if item moves out of range
                IDisposable itemPositionEvaluator = message.Source.PositionUpdateChannel.SubscribeToLast(
                    this.subscriptionManagementFiber, this.AutoSubscribedItem_OnItemPosition, ItemAutoUnsubcribeDelayMilliseconds);

                var itemSubscription = new ItemAutoSubscription(
                    message.Source, message.Position, message.WorldRegion, new UnsubscriberCollection(disposeListener, itemPositionEvaluator));
                this.autoManagedItemSubscriptions.Add(message.Source, itemSubscription);

                this.OnItemSubscribed(message);
            }
        }

        /// <summary>
        ///   Checks whether to auto subscribe the <paramref name = "item" />.
        ///   The default implementation ignores the <see cref = "AttachedItem" />.
        ///   Override to change or extend this behavior.
        /// </summary>
        /// <param name = "item">
        ///   The item to subscribe.
        /// </param>
        /// <returns>
        ///   True if the <paramref name = "item" /> is not the <see cref = "AttachedItem" />, otherwise false.
        /// </returns>
        protected virtual bool AutoSubscribeItem(Item item)
        {
            return item != this.AttachedItem;
        }

        /// <summary>
        ///   The clear auto subscriptions.
        /// </summary>
        protected void ClearAutoSubscriptions()
        {
            foreach (ItemAutoSubscription subscription in this.autoManagedItemSubscriptions.Values)
            {
                subscription.Dispose();
                this.OnItemUnsubscribed(subscription.Item);
            }

            this.autoManagedItemSubscriptions.Clear();
        }

        /// <summary>
        ///   The clear manual subscriptions.
        /// </summary>
        protected void ClearManualSubscriptions()
        {
            foreach (KeyValuePair<Item, IDisposable> pair in this.manualManagedItemSubscriptions)
            {
                pair.Value.Dispose();
                this.OnItemUnsubscribed(pair.Key);
            }

            this.manualManagedItemSubscriptions.Clear();
        }

        /// <summary>
        ///   The clear focus.
        /// </summary>
        protected void ClearRegionSubscriptions()
        {
            foreach (IDisposable subscription in this.subscribedWorldRegions.Values)
            {
                subscription.Dispose();
            }

            this.subscribedWorldRegions.Clear();
        }

        /// <summary>
        ///   Disposes the fiber used to manage the subscriptions, detaches any attached item and resolves all existing channel subscriptions.
        /// </summary>
        /// <param name = "disposing">
        ///   True if called from <see cref = "Dispose()" />, false if called from the finalizer.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.subscriptionManagementFiber.Dispose();

                // detach
                if (this.AttachedItem != null)
                {
                    this.itemMovementSubscription.Dispose();
                    this.itemMovementSubscription = null;
                    this.AttachedItem = null;
                }

                this.ClearRegionSubscriptions();
                this.ClearAutoSubscriptions();
                this.ClearManualSubscriptions();
            }
        }

        /// <summary>
        ///   Does nothing.
        ///   Called after subscribing an <see cref = "Item" />, either manually (<see cref = "SubscribeItem">SubscribeItem</see>) or automatically.
        /// </summary>
        /// <param name = "itemSnapshot">
        ///   The hearbeat message from the subscribed item.
        /// </param>
        /// <remarks>
        ///   Thread Safety: This method does not provide thread safe access to the <see cref = "Item" />. 
        ///   Instead of accessing the item directly override <see cref = "Item.GetItemSnapshot" /> subsclass that contains a copy of the required values.
        /// </remarks>
        /// <remarks>
        ///   Thread Safety: This method is executed while having an exclusive lock on <see cref = "SyncRoot" />.
        /// </remarks>
        protected virtual void OnItemSubscribed(ItemSnapshot itemSnapshot)
        {
        }

        /// <summary>
        ///   Does nothing.
        ///   Called after subscribing an <see cref = "Item" />, either manually (<see cref = "UnsubscribeItem">UnsubscribeItem</see>, <see cref = "Dispose(bool)">Dispose</see>, <see cref = "AttachToItem">AttachToItem</see>) or automatically.
        /// </summary>
        /// <param name = "item">
        ///   The item.
        /// </param>
        /// <remarks>
        ///   Thread Safety: This method does not provide thread safe access to the <see cref = "Item" />, but is executed while having an exclusive lock on <see cref = "SyncRoot" />.
        /// </remarks>
        protected virtual void OnItemUnsubscribed(Item item)
        {
        }

        /// <summary>
        ///   The attached item disposed.
        /// </summary>
        /// <param name = "message">
        ///   The message.
        /// </param>
        private void AttachedItem_OnItemDisposed(ItemDisposedMessage message)
        {
            MessageCounters.CounterReceive.Increment();

            lock (this.SyncRoot)
            {
                if (message.Source == this.AttachedItem)
                {
                    this.Detach();
                }
            }
        }

        /// <summary>
        ///   The on attached item position update.
        /// </summary>
        /// <param name = "message">
        ///   The message.
        /// </param>
        private void AttachedItem_OnItemPosition(ItemPositionMessage message)
        {
            MessageCounters.CounterReceive.Increment();

            lock (this.SyncRoot)
            {
                if (this.AttachedItem == message.Source)
                {
                    this.Position = message.Position;
                    this.UpdateInterestManagement();
                }
            }
        }

        /// <summary>
        ///   The on auto subscribed item disposed.
        /// </summary>
        /// <param name = "message">
        ///   The message.
        /// </param>
        private void AutoSubscribedItem_OnItemDisposed(ItemDisposedMessage message)
        {
            MessageCounters.CounterReceive.Increment();

            lock (this.SyncRoot)
            {
                ItemAutoSubscription subscription;
                if (this.autoManagedItemSubscriptions.TryGetValue(message.Source, out subscription))
                {
                    this.AutoUnsubscribeItem(subscription);
                }
            }
        }

        /// <summary>
        ///   The on auto subscribed item position update.
        ///   unsubscribes item if too far away
        /// </summary>
        /// <param name = "message">
        ///   The message.
        /// </param>
        private void AutoSubscribedItem_OnItemPosition(ItemPositionMessage message)
        {
            MessageCounters.CounterReceive.Increment();

            lock (this.SyncRoot)
            {
                ItemAutoSubscription subscription;

                // not subscribed
                if (false == this.autoManagedItemSubscriptions.TryGetValue(message.Source, out subscription))
                {
                    return;
                }

                subscription.ItemPosition = message.Position;

                // dropped out of world, unsubscribe
                if (message.WorldRegion == null)
                {
                    this.AutoUnsubscribeItem(subscription);
                    return;
                }

                // region is still the same, don't evaluate further
                if (message.WorldRegion == subscription.WorldRegion)
                {
                    return;
                }

                subscription.WorldRegion = message.WorldRegion;

                if (this.subscribedWorldRegions.ContainsKey(subscription.WorldRegion) == false)
                {
                    // unsubscribe if item is out of range
                    this.AutoUnsubscribeDistantItem(subscription);
                }
            }
        }

        /// <summary>
        ///   The auto unsubscribe distant item.
        /// </summary>
        /// <param name = "subscription">
        ///   The subscription.
        /// </param>
        private void AutoUnsubscribeDistantItem(ItemAutoSubscription subscription)
        {
            ////IArea area = this.World.Area.GetAreaWithRadius(this.Position, this.ViewDistanceExit);
            ////var regions = this.World.GetRegions(area);
            ////if (regions.Contains(subscription.WorldRegion) == false)
            if (false == this.currentRegionOuterFocus.Contains(subscription.ItemPosition))
            {
                this.AutoUnsubscribeItem(subscription);
            }
        }

        /// <summary>
        ///   The auto unsubscribe item.
        /// </summary>
        /// <param name = "subscription">
        ///   The subscription.
        /// </param>
        private void AutoUnsubscribeItem(ItemAutoSubscription subscription)
        {
            subscription.Dispose();
            this.autoManagedItemSubscriptions.Remove(subscription.Item);
            this.OnItemUnsubscribed(subscription.Item);
        }

        /// <summary>
        ///   The region receive message.
        /// </summary>
        /// <param name = "message">
        ///   The message.
        /// </param>
        private void Region_OnReceive(RegionMessage message)
        {
            message.OnInterestAreaReceive(this);
        }

        /// <summary>
        ///   Subscribes the <paramref name = "regions" />.
        /// </summary>
        /// <param name = "regions">
        ///   The regions.
        /// </param>
        private void SubscribeRegions(IEnumerable<Region> regions)
        {
            foreach (Region region in regions)
            {
                if (this.subscribedWorldRegions.ContainsKey(region) == false)
                {
                    IDisposable subscription = region.Subscribe(this.subscriptionManagementFiber, this.Region_OnReceive);
                    this.subscribedWorldRegions.Add(region, subscription);
                    region.Publish(this.snapShotRequest);
                }
            }
        }

        /// <summary>
        ///   The on subscribed item disposed.
        /// </summary>
        /// <param name = "itemDisposeMessage">
        ///   The item dispose message.
        /// </param>
        private void SubscribedItem_OnItemDisposed(ItemDisposedMessage itemDisposeMessage)
        {
            MessageCounters.CounterReceive.Increment();

            // interest areas have to be locked 
            lock (this.SyncRoot)
            {
                this.UnsubscribeItem(itemDisposeMessage.Source);
            }
        }

        /// <summary>
        ///   Unsubscribe the <paramref name = "regions" />.
        /// </summary>
        /// <param name = "regions">
        ///   The regions.
        /// </param>
        private void UnsubscribeRegions(IEnumerable<Region> regions)
        {
            foreach (Region region in regions)
            {
                IDisposable subscription;
                if (this.subscribedWorldRegions.TryGetValue(region, out subscription))
                {
                    subscription.Dispose();
                    this.subscribedWorldRegions.Remove(region);
                }
            }

            List<ItemAutoSubscription> itemSubscriptions = this.autoManagedItemSubscriptions.Values.Where(i => regions.Contains(i.WorldRegion)).ToList();
            foreach (ItemAutoSubscription subscription in itemSubscriptions)
            {
                this.AutoUnsubscribeDistantItem(subscription);
            }
        }

        #endregion
    }
}