// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemCache.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   A cache for <see cref="Item">items</see>. Each <see cref="IWorld" /> has one item cache.
//   It uses an <see cref="ReaderWriterLockSlim" /> to ensure thread safety.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.SocketServer.Mmo
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using ExitGames.Threading;

    /// <summary>
    ///   A cache for <see cref = "Item">items</see>. Each <see cref = "IWorld" /> has one item cache.
    ///   It uses an <see cref = "ReaderWriterLockSlim" /> to ensure thread safety.
    /// </summary>
    /// <remarks>
    ///   All members are thread safe.
    /// </remarks>
    public class ItemCache : IDisposable
    {
        #region Constants and Fields

        /// <summary>
        ///   The item caches.
        /// </summary>
        private readonly Dictionary<byte, ItemCacheL2> itemCaches;

        /// <summary>
        ///   The max lock milliseconds.
        /// </summary>
        private readonly int maxLockMilliseconds;

        /// <summary>
        ///   The reader writer lock.
        /// </summary>
        private readonly ReaderWriterLockSlim readerWriterLock;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ItemCache" /> class.
        /// </summary>
        /// <param name = "maxLockMilliseconds">
        ///   The max Lock Milliseconds.
        /// </param>
        public ItemCache(int maxLockMilliseconds)
        {
            this.maxLockMilliseconds = maxLockMilliseconds;
            this.itemCaches = new Dictionary<byte, ItemCacheL2>();
            this.readerWriterLock = new ReaderWriterLockSlim();
        }

        /// <summary>
        ///   Finalizes an instance of the <see cref = "ItemCache" /> class.
        /// </summary>
        ~ItemCache()
        {
            this.Dispose(false);
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///   Adds an <see cref = "Item" />.
        /// </summary>
        /// <param name = "item">
        ///   The new item.
        /// </param>
        /// <returns>
        ///   true if item added.
        /// </returns>
        public bool AddItem(Item item)
        {
            ItemCacheL2 level2Cache = this.GetLevel2Cache(item.Type);
            return level2Cache.AddItem(item);
        }

        /// <summary>
        ///   Removes an <see cref = "Item" />.
        /// </summary>
        /// <param name = "itemType">
        ///   The item Type.
        /// </param>
        /// <param name = "itemId">
        ///   The item id.
        /// </param>
        /// <returns>
        ///   true if item removed.
        /// </returns>
        public bool RemoveItem(byte itemType, string itemId)
        {
            ItemCacheL2 level2Cache = this.GetLevel2Cache(itemType);
            return level2Cache.RemoveItem(itemId);
        }

        /// <summary>
        ///   Tries to retrieve an <see cref = "Item" />.
        /// </summary>
        /// <param name = "itemType">
        ///   The item Type.
        /// </param>
        /// <param name = "itemId">
        ///   The item id.
        /// </param>
        /// <param name = "item">
        ///   The found item.
        /// </param>
        /// <returns>
        ///   true if item was found.
        /// </returns>
        public bool TryGetItem(byte itemType, string itemId, out Item item)
        {
            ItemCacheL2 level2Cache = this.GetLevel2Cache(itemType);
            return level2Cache.TryGetItem(itemId, out item);
        }

        #endregion

        #region Implemented Interfaces

        #region IDisposable

        /// <summary>
        ///   The dispose.
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
        ///   Clears the cache and disposes the rw lock.
        /// </summary>
        /// <param name = "disposing">
        ///   The disposing.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.readerWriterLock.Dispose();

                foreach (ItemCacheL2 level2Cache in this.itemCaches.Values)
                {
                    level2Cache.Dispose();
                }

                this.itemCaches.Clear();
            }
        }

        /// <summary>
        ///   The get level 2 cache.
        /// </summary>
        /// <param name = "itemType">
        ///   The item type.
        /// </param>
        /// <returns>
        ///   the level2 cache for the item type
        /// </returns>
        private ItemCacheL2 GetLevel2Cache(byte itemType)
        {
            ItemCacheL2 result;
            using (ReadLock.TryEnter(this.readerWriterLock, this.maxLockMilliseconds))
            {
                if (this.itemCaches.TryGetValue(itemType, out result))
                {
                    return result;
                }
            }

            using (WriteLock.TryEnter(this.readerWriterLock, this.maxLockMilliseconds))
            {
                if (false == this.itemCaches.TryGetValue(itemType, out result))
                {
                    result = new ItemCacheL2(this.maxLockMilliseconds);
                    this.itemCaches.Add(itemType, result);
                }

                return result;
            }
        }

        #endregion

        /// <summary>
        ///   The item cache l 2.
        /// </summary>
        private class ItemCacheL2 : IDisposable
        {
            #region Constants and Fields

            /// <summary>
            ///   The items.
            /// </summary>
            private readonly Dictionary<string, Item> items;

            /// <summary>
            ///   The max lock milliseconds.
            /// </summary>
            private readonly int maxLockMilliseconds;

            /// <summary>
            ///   The reader writer lock.
            /// </summary>
            private readonly ReaderWriterLockSlim readerWriterLock;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            ///   Initializes a new instance of the <see cref = "ItemCache.ItemCacheL2" /> class.
            /// </summary>
            /// <param name = "maxLockMilliseconds">
            ///   The max Lock Milliseconds.
            /// </param>
            public ItemCacheL2(int maxLockMilliseconds)
            {
                this.maxLockMilliseconds = maxLockMilliseconds;
                this.items = new Dictionary<string, Item>();
                this.readerWriterLock = new ReaderWriterLockSlim();
            }

            /// <summary>
            ///   Finalizes an instance of the <see cref = "ItemCache.ItemCacheL2" /> class.
            /// </summary>
            ~ItemCacheL2()
            {
                this.DisposeReaderWriterLock();
            }

            #endregion

            #region Public Methods

            /// <summary>
            ///   The add item.
            /// </summary>
            /// <param name = "item">
            ///   The new item.
            /// </param>
            /// <returns>
            ///   true if item added.
            /// </returns>
            public bool AddItem(Item item)
            {
                using (WriteLock.TryEnter(this.readerWriterLock, this.maxLockMilliseconds))
                {
                    if (this.items.ContainsKey(item.Id))
                    {
                        return false;
                    }

                    this.items.Add(item.Id, item);
                    return true;
                }
            }

            /// <summary>
            ///   The remove item.
            /// </summary>
            /// <param name = "itemId">
            ///   The item id.
            /// </param>
            /// <returns>
            ///   true if item removed.
            /// </returns>
            public bool RemoveItem(string itemId)
            {
                using (WriteLock.TryEnter(this.readerWriterLock, this.maxLockMilliseconds))
                {
                    return this.items.Remove(itemId);
                }
            }

            /// <summary>
            ///   The try get item.
            /// </summary>
            /// <param name = "itemId">
            ///   The item id.
            /// </param>
            /// <param name = "item">
            ///   The found item.
            /// </param>
            /// <returns>
            ///   true if item was found.
            /// </returns>
            public bool TryGetItem(string itemId, out Item item)
            {
                using (ReadLock.TryEnter(this.readerWriterLock, this.maxLockMilliseconds))
                {
                    return this.items.TryGetValue(itemId, out item);
                }
            }

            #endregion

            #region Implemented Interfaces

            #region IDisposable

            /// <summary>
            ///   The dispose.
            /// </summary>
            public void Dispose()
            {
                this.DisposeReaderWriterLock();
                GC.SuppressFinalize(this);
            }

            #endregion

            #endregion

            #region Methods

            /// <summary>
            ///   The dispose reader writer lock.
            /// </summary>
            private void DisposeReaderWriterLock()
            {
                this.readerWriterLock.Dispose();
            }

            #endregion
        }
    }
}