// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemEventMessage.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This message type contains <see cref="EventData" /> to be sent to clients.
//   <see cref="ItemEventMessage">ItemEventMessages</see> are published through the item <see cref="Item.EventChannel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.SocketServer.Mmo.Messages
{
    using ExitGames.Diagnostics.Counter;

    /// <summary>
    ///   This message type contains <see cref = "EventData" /> to be sent to clients.
    ///   <see cref = "ItemEventMessage">ItemEventMessages</see> are published through the item <see cref = "Item.EventChannel" />.
    /// </summary>
    public class ItemEventMessage
    {
        #region Constants and Fields

        /// <summary>
        ///   The counter event receive.
        /// </summary>
        public static readonly CountsPerSecondCounter CounterEventReceive = new CountsPerSecondCounter("ItemEventMessage.Receive");

        /// <summary>
        ///   The counter event send.
        /// </summary>
        public static readonly CountsPerSecondCounter CounterEventSend = new CountsPerSecondCounter("ItemEventMessage.Send");

        /// <summary>
        ///   The event data.
        /// </summary>
        private readonly IEventData eventData;

        /// <summary>
        ///   The send parameters.
        /// </summary>
        private readonly SendParameters sendParameters;

        /// <summary>
        ///   The source.
        /// </summary>
        private readonly Item source;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ItemEventMessage" /> class.
        /// </summary>
        /// <param name = "source">
        ///   The source.
        /// </param>
        /// <param name = "eventData">
        ///   The event Data.
        /// </param>
        /// <param name = "sendParameters">
        ///   The send Options.
        /// </param>
        public ItemEventMessage(Item source, IEventData eventData, SendParameters sendParameters)
        {
            this.source = source;
            this.eventData = eventData;
            this.sendParameters = sendParameters;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the <see cref = "EventData" /> to be sent to the client.
        /// </summary>
        public IEventData EventData
        {
            get
            {
                return this.eventData;
            }
        }

        /// <summary>
        ///   Gets the send parameters.
        /// </summary>
        public SendParameters SendParameters
        {
            get
            {
                return this.sendParameters;
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

        #endregion
    }
}