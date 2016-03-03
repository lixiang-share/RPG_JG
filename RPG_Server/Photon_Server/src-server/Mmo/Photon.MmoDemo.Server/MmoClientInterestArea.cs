// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MmoClientInterestArea.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This <see cref="ClientInterestArea" /> subclass overrides <see cref="OnItemSubscribed">OnItemSubscribed</see> and <see cref="OnItemUnsubscribed">OnItemUnsubscribed</see> in order to send events <see cref="ItemSubscribed" /> and <see cref="ItemUnsubscribed" /> to the client.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server
{
    using Common;

    using Events;

    using Messages;

    using SocketServer;
    using SocketServer.Mmo;
    using SocketServer.Mmo.Messages;
    
    /// <summary>
    /// This <see cref="ClientInterestArea"/> subclass overrides <see cref="OnItemSubscribed">OnItemSubscribed</see> and <see cref="OnItemUnsubscribed">OnItemUnsubscribed</see> in order to send events <see cref="ItemSubscribed"/> and <see cref="ItemUnsubscribed"/> to the client.
    /// </summary>
    public class MmoClientInterestArea : ClientInterestArea
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MmoClientInterestArea"/> class. 
        /// </summary>
        /// <param name="peer">
        /// The peer state.
        /// </param>
        /// <param name="id">
        /// The interesta area id.
        /// </param>
        /// <param name="world">
        /// The world.
        /// </param>
        public MmoClientInterestArea(PeerBase peer, byte id, IWorld world)
            : base(peer, id, world, peer.RequestFiber)
        {
        }

        /// <summary>
        /// Calls <see cref="ClientInterestArea.OnItemSubscribed">ClientInterestArea.OnItemSubscribed</see> and sends event <see cref="ItemSubscribed"/> to the client.
        /// </summary>
        /// <param name="snapshot">
        /// The item snapshot.
        /// </param>
        protected override void OnItemSubscribed(ItemSnapshot snapshot)
        {
            base.OnItemSubscribed(snapshot);

            var mmoSnapshot = (MmoItemSnapshot)snapshot;
            var item = snapshot.Source;

            var subscribeEvent = new ItemSubscribed
                {
                    ItemId = item.Id,
                    ItemType = item.Type,
                    Position = mmoSnapshot.Coordinate,
                    PropertiesRevision = snapshot.PropertiesRevision,
                    InterestAreaId = this.Id,
                    Rotation = mmoSnapshot.Rotation
                };

            var eventData = new EventData((byte)EventCode.ItemSubscribed, subscribeEvent);
            this.Peer.SendEvent(eventData, new SendParameters { ChannelId = Settings.ItemEventChannel });
        }

        /// <summary>
        /// Calls <see cref="ClientInterestArea.OnItemUnsubscribed">ClientInterestArea.OnItemUnsubscribed</see> and sends event <see cref="ItemUnsubscribed"/> to the client.
        /// </summary>
        /// <param name="item">
        /// The mmo item.
        /// </param>
        protected override void OnItemUnsubscribed(Item item)
        {
            base.OnItemUnsubscribed(item);

            var unsubscribeEvent = new ItemUnsubscribed { ItemId = item.Id, ItemType = item.Type, InterestAreaId = this.Id };
            var eventData = new EventData((byte)EventCode.ItemUnsubscribed, unsubscribeEvent);
            this.Peer.SendEvent(eventData, new SendParameters { ChannelId = Settings.ItemEventChannel });
        }
    }
}