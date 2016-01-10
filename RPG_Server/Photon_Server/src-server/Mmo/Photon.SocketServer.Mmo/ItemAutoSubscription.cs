// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemAutoSubscription.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The item subscription.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.SocketServer.Mmo
{
    using System;

    /// <summary>
    ///   The item subscription.
    /// </summary>
    internal class ItemAutoSubscription : IDisposable
    {
        #region Constants and Fields

        /// <summary>
        ///   The subscribed item.
        /// </summary>
        private readonly Item item;

        /// <summary>
        ///   The subscription.
        /// </summary>
        private readonly IDisposable subscription;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ItemAutoSubscription" /> class.
        /// </summary>
        /// <param name = "item">
        ///   The subscribed item.
        /// </param>
        /// <param name = "itemPosition">
        ///   The item position.
        /// </param>
        /// <param name = "itemRegion">
        ///   The item Region.
        /// </param>
        /// <param name = "subscription">
        ///   The subscription.
        /// </param>
        public ItemAutoSubscription(Item item, Vector itemPosition, Region itemRegion, IDisposable subscription)
        {
            this.ItemPosition = itemPosition;
            this.item = item;
            this.subscription = subscription;
            this.WorldRegion = itemRegion;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets Item.
        /// </summary>
        public Item Item
        {
            get
            {
                return this.item;
            }
        }

        /// <summary>
        ///   Gets or sets ItemPosition.
        /// </summary>
        public Vector ItemPosition { get; set; }

        /// <summary>
        ///   Gets or sets WorldRegion.
        /// </summary>
        public Region WorldRegion { get; set; }

        #endregion

        #region Implemented Interfaces

        #region IDisposable

        /// <summary>
        ///   The dispose.
        /// </summary>
        public void Dispose()
        {
            this.subscription.Dispose();
        }

        #endregion

        #endregion
    }
}