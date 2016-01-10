// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemSnapshot.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   An <see cref="Item" /> sends this <see cref="RegionMessage" /> through the <see cref="Region" /> at the current position to the <see cref="InterestArea" />s overlapping this region.
//   It contains a the current position and other property snapshots of the <see cref="Item" />.
//   <para>
//   <see cref="InterestArea" />s that receive this message check whether to subscribe to the sender (see <see cref="InterestArea.AutoSubscribeItem">InterestArea.AutoSubscribeItem</see>).
//   </para>
//   <para>
//   Receiving <see cref="Item" />s ignore this type of message.
//   </para>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.SocketServer.Mmo.Messages
{
    /// <summary>
    ///   An <see cref = "Item" /> sends this <see cref = "RegionMessage" /> through the <see cref = "Region" /> at the current position to the <see cref = "InterestArea" />s overlapping this region. 
    ///   It contains a the current position and other property snapshots of the <see cref = "Item" />. 
    ///   <para>
    ///     <see cref = "InterestArea" />s that receive this message check whether to subscribe to the sender (see <see cref = "InterestArea.AutoSubscribeItem">InterestArea.AutoSubscribeItem</see>). 
    ///   </para>
    ///   <para>
    ///     Receiving <see cref = "Item" />s ignore this type of message.
    ///   </para>
    /// </summary>
    public class ItemSnapshot : RegionMessage
    {
#if MissingSubscribeDebug
        private static readonly ExitGames.Logging.ILogger log = ExitGames.Logging.LogManager.GetCurrentClassLogger();
#endif

        /// <summary>
        ///   The position.
        /// </summary>
        private readonly Vector position;

        /// <summary>
        ///   The properties revision.
        /// </summary>
        private readonly int propertiesRevision;

        /// <summary>
        ///   The source.
        /// </summary>
        private readonly Item source;

        /// <summary>
        ///   The world region.
        /// </summary>
        private readonly Region worldRegion;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ItemSnapshot" /> class.
        /// </summary>
        /// <param name = "source">
        ///   The source.
        /// </param>
        /// <param name = "position">
        ///   The position.
        /// </param>
        /// <param name = "worldRegion">
        ///   The world Region.
        /// </param>
        /// <param name = "propertiesRevision">
        ///   The properties Revision.
        /// </param>
        public ItemSnapshot(Item source, Vector position, Region worldRegion, int propertiesRevision)
        {
            this.source = source;
            this.position = position;
            this.propertiesRevision = propertiesRevision;
            this.worldRegion = worldRegion;
        }

        /// <summary>
        ///   Gets the <see cref = "Source" /> item's position.
        /// </summary>
        public Vector Position
        {
            get
            {
                return this.position;
            }
        }

        /// <summary>
        ///   Gets the <see cref = "Source" /> item's properties revision number.
        /// </summary>
        public int PropertiesRevision
        {
            get
            {
                return this.propertiesRevision;
            }
        }

        /// <summary>
        ///   Gets the source <see cref = "Item" />.
        /// </summary>
        public Item Source
        {
            get
            {
                return this.source;
            }
        }

        /// <summary>
        ///   Gets current <see cref = "Region" /> where the <see cref = "Source" /> item is located.
        /// </summary>
        public Region WorldRegion
        {
            get
            {
                return this.worldRegion;
            }
        }

        /// <summary>
        ///   Increments <see cref = "MessageCounters.CounterReceive" /> and subscribes the <see cref = "InterestArea" /> to the <see cref = "Source" /> item if it not already subscribed or attached.
        ///   Called by the <see cref = "InterestArea" /> when received.
        /// </summary>
        /// <param name = "interestArea">
        ///   The calling interest area.
        /// </param>
        public override void OnInterestAreaReceive(InterestArea interestArea)
        {
            MessageCounters.CounterReceive.Increment();

            interestArea.ReceiveItemSnapshot(this);
#if MissingSubscribeDebug
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("{1} received snap shot from item {0} - region {2}", 
                        this.source.Id, 
                        interestArea.GetHashCode(), 
                        this.WorldRegion.Coordinate);
                }
#endif
        }

        /// <summary>
        ///   Increments <see cref = "MessageCounters.CounterReceive" />.
        ///   Called by the <see cref = "Item" /> when received.
        /// </summary>
        /// <param name = "item">
        ///   The calling item.
        /// </param>
        public override void OnItemReceive(Item item)
        {
            MessageCounters.CounterReceive.Increment();
        }
    }
}