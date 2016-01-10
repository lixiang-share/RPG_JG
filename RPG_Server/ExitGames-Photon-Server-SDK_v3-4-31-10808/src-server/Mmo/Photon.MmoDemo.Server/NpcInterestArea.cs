// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NpcInterestArea.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The npc interest area.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server
{
    using Common;

    using ExitGames.Logging;

    using SocketServer.Mmo;
    using SocketServer.Mmo.Messages;

    /// <summary>
    /// The npc interest area.
    /// </summary>
    public class NpcInterestArea : InterestArea
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The npc id.
        /// </summary>
        private readonly string npcId;

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcInterestArea"/> class.
        /// </summary>
        /// <param name="npcId">
        /// The npc Id.
        /// </param>
        /// <param name="world">
        /// The world.
        /// </param>
        public NpcInterestArea(string npcId, IWorld world)
            : base(0, world)
        {
            this.npcId = npcId;
        }

        /// <summary>
        /// The on item subscribed.
        /// </summary>
        /// <param name="snapshot">
        /// The item snapshot.
        /// </param>
        protected override void OnItemSubscribed(ItemSnapshot snapshot)
        {
            base.OnItemSubscribed(snapshot);

            Item item = snapshot.Source;
            log.InfoFormat("Npc {2}: I see item {0}({1})", item.Id, (ItemType)item.Type, this.npcId);
        }

        /// <summary>
        /// The on item unsubscribed.
        /// </summary>
        /// <param name="item">
        /// The mmo item.
        /// </param>
        protected override void OnItemUnsubscribed(Item item)
        {
            base.OnItemUnsubscribed(item);

            log.InfoFormat("Npc {2}: I don't see item {0}({1}) anymore", item.Id, (ItemType)item.Type, this.npcId);
        }
    }
}