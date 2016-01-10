// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageCounters.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Contains counters that keep track of the amount of messages sent and received from <see cref="Item" /> channels.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.SocketServer.Mmo.Messages
{
    using ExitGames.Diagnostics.Counter;

    /// <summary>
    ///   Contains counters that keep track of the amount of messages sent and received from <see cref = "Item" /> channels.
    /// </summary>
    public static class MessageCounters
    {
        #region Constants and Fields

        /// <summary>
        ///   Used to count how many messages were received by <see cref = "InterestArea">interest areas</see> (and sometimes <see cref = "Item">items</see>).
        ///   Name: "ItemMessage.Receive"
        /// </summary>
        public static readonly CountsPerSecondCounter CounterReceive = new CountsPerSecondCounter("ItemMessage.Receive");

        /// <summary>
        ///   Used to count how many messages were sent by <see cref = "Item">items</see> (and sometimes <see cref = "InterestArea">interest areas</see>).
        ///   Name: "ItemMessage.Send"
        /// </summary>
        public static readonly CountsPerSecondCounter CounterSend = new CountsPerSecondCounter("ItemMessage.Send");

        #endregion
    }
}