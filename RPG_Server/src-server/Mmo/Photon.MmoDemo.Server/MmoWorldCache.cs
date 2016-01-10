// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MmoWorldCache.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The square grid cache.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using ExitGames.Threading;

    using Photon.SocketServer.Mmo;

    /// <summary>
    /// This is a cache for <see cref="MmoWorld">MmoWorlds</see> that have a unique name.
    /// </summary>
    public sealed class MmoWorldCache : IDisposable
    {
        /// <summary>
        /// The instance.
        /// </summary>
        public static readonly MmoWorldCache Instance = new MmoWorldCache();

        /// <summary>
        /// Dictionary used to store cached values.
        /// </summary>
        private readonly Dictionary<string, MmoWorld> dict;

        /// <summary>
        /// An <see cref="ReaderWriterLockSlim"/> instance used to synchronize access to the cache.
        /// </summary>
        private readonly ReaderWriterLockSlim readWriteLock;

        /// <summary>
        /// Prevents a default instance of the <see cref="MmoWorldCache"/> class from being created. 
        /// Initializes a new instance of the <see cref="MmoWorldCache"/> class. 
        /// </summary>
        private MmoWorldCache()
        {
            this.dict = new Dictionary<string, MmoWorld>();
            this.readWriteLock = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="MmoWorldCache"/> class. 
        /// </summary>
        ~MmoWorldCache()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Removes all cached elements from this instance.
        /// </summary>
        public void Clear()
        {
            using (WriteLock.TryEnter(this.readWriteLock, Settings.MaxLockWaitTimeMilliseconds))
            {
                this.dict.Clear();
            }
        }

        /// <summary>
        /// The try create.
        /// </summary>
        /// <param name="name">
        /// The world name.
        /// </param>
        /// <param name="topLeftCorner">
        /// The top left corner.
        /// </param>
        /// <param name="bottomRightCorner">
        /// The bottom right corner.
        /// </param>
        /// <param name="tileDimensions">
        /// The tile dimensions.
        /// </param>
        /// <param name="world">
        /// The world.
        /// </param>
        /// <returns>
        /// true if create.
        /// </returns>
        public bool TryCreate(string name, Vector topLeftCorner, Vector bottomRightCorner, Vector tileDimensions, out MmoWorld world)
        {
            using (WriteLock.TryEnter(this.readWriteLock, Settings.MaxLockWaitTimeMilliseconds))
            {
                if (this.dict.TryGetValue(name, out world))
                {
                    return false;
                }

                world = new MmoWorld(name, topLeftCorner, bottomRightCorner, tileDimensions);
                this.dict.Add(name, world);
                return true;
            }
        }

        /// <summary>
        /// The try get.
        /// </summary>
        /// <param name="name">
        /// The world name.
        /// </param>
        /// <param name="world">
        /// The world.
        /// </param>
        /// <returns>
        /// true if found.
        /// </returns>
        public bool TryGet(string name, out MmoWorld world)
        {
            using (ReadLock.TryEnter(this.readWriteLock, Settings.MaxLockWaitTimeMilliseconds))
            {
                return this.dict.TryGetValue(name, out world);
            }
        }

        #region Implemented Interfaces

        #region IDisposable

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #endregion

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (MmoWorld world in this.dict.Values)
                {
                    world.Dispose();
                }
            }

            this.readWriteLock.Dispose();
        }
    }
}