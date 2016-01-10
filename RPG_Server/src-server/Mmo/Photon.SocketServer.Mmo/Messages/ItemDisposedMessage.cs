// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemDisposedMessage.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This type of message is pubished by <see cref="Item">items</see> sent through the item <see cref="Item.DisposeChannel" />.
//   <see cref="InterestArea">Interest areas</see> unsubscribe the sender when they receive this message.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.SocketServer.Mmo.Messages
{
    /// <summary>
    ///   This type of message is pubished by <see cref = "Item">items</see> sent through the item <see cref = "Item.DisposeChannel" />. 
    ///   <see cref = "InterestArea">Interest areas</see> unsubscribe the sender when they receive this message.
    /// </summary>
    public sealed class ItemDisposedMessage
    {
        #region Constants and Fields

        /// <summary>
        ///   The source.
        /// </summary>
        private readonly Item source;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ItemDisposedMessage" /> class.
        /// </summary>
        /// <param name = "source">
        ///   The source.
        /// </param>
        public ItemDisposedMessage(Item source)
        {
            this.source = source;
        }

        #endregion

        #region Properties

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

        #endregion
    }
}