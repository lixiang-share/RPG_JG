// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegionMessage.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Abstract class for messages that are sent through <see cref="Region">regions</see>.
//   These messages are received by <see cref="Item">items</see> and <see cref="InterestArea">interest areas</see>.
//   The receiver does not know what kind of message he receives and calls either <see cref="OnItemReceive" /> or <see cref="OnInterestAreaReceive" /> to dispatch it.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.SocketServer.Mmo.Messages
{
    /// <summary>
    ///   Abstract class for messages that are sent through <see cref = "Region">regions</see>. 
    ///   These messages are received by <see cref = "Item">items</see> and <see cref = "InterestArea">interest areas</see>. 
    ///   The receiver does not know what kind of message he receives and calls either <see cref = "OnItemReceive" /> or <see cref = "OnInterestAreaReceive" /> to dispatch it.
    /// </summary>
    public abstract class RegionMessage
    {
        #region Public Methods

        /// <summary>
        ///   Called by the <see cref = "InterestArea" /> when received.
        /// </summary>
        /// <param name = "interestArea">
        ///   The calling interest area.
        /// </param>
        public abstract void OnInterestAreaReceive(InterestArea interestArea);

        /// <summary>
        ///   Called by the <see cref = "Item" /> when received.
        /// </summary>
        /// <param name = "item">
        ///   The calling item.
        /// </param>
        public abstract void OnItemReceive(Item item);

        #endregion
    }
}