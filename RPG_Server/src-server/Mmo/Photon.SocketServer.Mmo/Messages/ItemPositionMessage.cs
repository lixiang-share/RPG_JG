// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemPositionMessage.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This message contains the current position of the <see cref="Item" />.
//   This type of message is published by <see cref="Item">items</see> through the <see cref="Item.PositionUpdateChannel" />.
//   <para>
//   <see cref="InterestArea">Interest areas</see> that receive this message use it to either follow the sender (the attached item) or to decide whether to unsubscribe.
//   </para>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.SocketServer.Mmo.Messages
{
    /// <summary>
    ///   This message contains the current position of the <see cref = "Item" />. 
    ///   This type of message is published by <see cref = "Item">items</see> through the <see cref = "Item.PositionUpdateChannel" />. 
    ///   <para>
    ///     <see cref = "InterestArea">Interest areas</see> that receive this message use it to either follow the sender (the attached item) or to decide whether to unsubscribe.
    ///   </para>
    /// </summary>
    public class ItemPositionMessage
    {
        #region Constants and Fields

        /// <summary>
        ///   The position.
        /// </summary>
        private readonly Vector position;

        /// <summary>
        ///   The source.
        /// </summary>
        private readonly Item source;

        /// <summary>
        ///   The world region.
        /// </summary>
        private readonly Region worldRegion;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ItemPositionMessage" /> class.
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
        public ItemPositionMessage(Item source, Vector position, Region worldRegion)
        {
            this.source = source;
            this.position = position;
            this.worldRegion = worldRegion;
        }

        #endregion

        #region Properties

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

        #endregion
    }
}