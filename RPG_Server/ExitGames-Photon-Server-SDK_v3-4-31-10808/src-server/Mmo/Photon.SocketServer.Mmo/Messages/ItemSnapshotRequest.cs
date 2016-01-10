// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemSnapshotRequest.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   <see cref="InterestArea" />s send this <see cref="RegionMessage" /> through newly subscribed <see cref="Region" />s to all <see cref="Item" />s in these regions.
//   Receiving <see cref="Item" />s answer with an <see cref="ItemSnapshot" />.
//   <para>
//   Receiving <see cref="InterestArea" />s ignore this type of message.
//   </para>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.SocketServer.Mmo.Messages
{
    /// <summary>
    ///   <see cref = "InterestArea" />s send this <see cref = "RegionMessage" /> through newly subscribed <see cref = "Region" />s to all <see cref = "Item" />s in these regions. 
    ///   Receiving <see cref = "Item" />s answer with an <see cref = "ItemSnapshot" />.
    ///   <para>
    ///     Receiving <see cref = "InterestArea" />s ignore this type of message.
    ///   </para>
    /// </summary>
    public sealed class ItemSnapshotRequest : RegionMessage
    {
#if MissingSubscribeDebug
        private static readonly ExitGames.Logging.ILogger log = ExitGames.Logging.LogManager.GetCurrentClassLogger();
#endif

        /// <summary>
        ///   The source.
        /// </summary>
        private readonly InterestArea source;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ItemSnapshotRequest" /> class.
        /// </summary>
        /// <param name = "source">
        ///   The source.
        /// </param>
        public ItemSnapshotRequest(InterestArea source)
        {
            this.source = source;
        }

        /// <summary>
        ///   Gets the source.
        /// </summary>
        public InterestArea Source
        {
            get
            {
                return this.source;
            }
        }

        /// <summary>
        ///   Called by the <see cref = "InterestArea" /> when received.
        ///   Increments <see cref = "MessageCounters.CounterReceive" />.
        /// </summary>
        /// <param name = "interestArea">
        ///   The calling interest area.
        /// </param>
        public override void OnInterestAreaReceive(InterestArea interestArea)
        {
            MessageCounters.CounterReceive.Increment();
        }

        /// <summary>
        ///   Called by the <see cref = "Item" /> when received.
        ///   Increments <see cref = "MessageCounters.CounterReceive" /> and publishes an <see cref = "ItemPositionMessage" /> in the <paramref name = "item" />'s <see cref = "Item.CurrentWorldRegion" />.
        /// </summary>
        /// <param name = "item">
        ///   The calling item.
        /// </param>
        public override void OnItemReceive(Item item)
        {
            MessageCounters.CounterReceive.Increment();

            ItemSnapshot itemSnapshot = item.GetItemSnapshot();
            this.source.ReceiveItemSnapshot(itemSnapshot);

#if MissingSubscribeDebug
                    if (log.IsDebugEnabled)
                    {
                        log.DebugFormat(
                            "{0} sent snap shot to interest area {1} - region {2}", 
                            item.Id, 
                            this.source.GetHashCode(), 
                            itemSnapshot.WorldRegion.Coordinate);
                    }
#endif
        }
    }
}