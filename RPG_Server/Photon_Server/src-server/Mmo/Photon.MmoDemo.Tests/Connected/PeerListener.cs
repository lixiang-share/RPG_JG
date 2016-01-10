// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PeerListener.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The peer listener.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Tests.Connected
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using ExitGames.Client.Photon;
    using ExitGames.Logging;

    using Photon.MmoDemo.Common;

    /// <summary>
    ///   The peer listener.
    /// </summary>
    public class PeerListener : IPhotonPeerListener
    {
        #region Constants and Fields

        /// <summary>
        ///   The logger.
        /// </summary>
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///   The connect handler.
        /// </summary>
        private readonly Action<PeerListener, bool> connectHandler;

        /// <summary>
        ///   The event queue.
        /// </summary>
        private readonly List<EventData> eventList = new List<EventData>();

        /// <summary>
        ///   The response list.
        /// </summary>
        private readonly List<OperationResponse> responseList = new List<OperationResponse>();

        /// <summary>
        ///   The username.
        /// </summary>
        private readonly string username;

        /// <summary>
        ///   The events received.
        /// </summary>
        private static long eventsReceived;

        /// <summary>
        ///   The response received.
        /// </summary>
        private static long responseReceived;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PeerListener" /> class.
        /// </summary>
        /// <param name = "username">
        ///   The username.
        /// </param>
        /// <param name = "connectHandler">
        ///   The on Connect Handler.
        /// </param>
        public PeerListener(string username, Action<PeerListener, bool> connectHandler)
        {
            this.username = username;
            this.connectHandler = connectHandler;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets EventsReceived.
        /// </summary>
        public static long EventsReceived
        {
            get
            {
                return Interlocked.Read(ref eventsReceived);
            }
        }

        /// <summary>
        ///   Gets EventList.
        /// </summary>
        public List<EventData> EventList
        {
            get
            {
                return this.eventList;
            }
        }

        /// <summary>
        ///   Gets ResponseList.
        /// </summary>
        public List<OperationResponse> ResponseList
        {
            get
            {
                return this.responseList;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///   The reset stats.
        /// </summary>
        public static void ResetStats()
        {
            Interlocked.Exchange(ref eventsReceived, 0);
            Interlocked.Exchange(ref responseReceived, 0);
        }

        #endregion

        #region Implemented Interfaces

        #region IPhotonPeerListener

        /// <summary>
        ///   The debug return.
        /// </summary>
        /// <param name = "logLevel">
        ///   The log Level.
        /// </param>
        /// <param name = "message">
        ///   The message.
        /// </param>
        public void DebugReturn(DebugLevel logLevel, string message)
        {
            switch (logLevel)
            {
                case DebugLevel.ALL:
                    {
                        if (log.IsDebugEnabled)
                        {
                            log.Debug(message);
                        }

                        break;
                    }

                case DebugLevel.INFO:
                    {
                        if (log.IsInfoEnabled)
                        {
                            log.Info(message);
                        }

                        break;
                    }

                case DebugLevel.ERROR:
                    {
                        if (log.IsErrorEnabled)
                        {
                            log.Error(message);
                        }

                        break;
                    }

                case DebugLevel.WARNING:
                    {
                        if (log.IsWarnEnabled)
                        {
                            log.Warn(message);
                        }

                        break;
                    }
            }
        }

        public void OnEvent(EventData ev)
        {
            // service() is executed in event fiber 
            this.AddEvent(ev);
        }

        /// <summary>
        ///   The operation result.
        /// </summary>
        /// <param name = "operationResponse">
        ///   The operation Response.
        /// </param>
        public void OnOperationResponse(OperationResponse operationResponse)
        {
            this.AddResponse(operationResponse);
        }

        public void OnStatusChanged(StatusCode statusCode)
        {
            switch (statusCode)
            {
                case StatusCode.Connect:
                    {
                        if (log.IsDebugEnabled)
                        {
                            log.Debug("PeerStatusCallback: Connect");
                        }

                        this.connectHandler(this, true);
                        return;
                    }

                case StatusCode.Disconnect:
                    {
                        if (log.IsDebugEnabled)
                        {
                            log.Debug("PeerStatusCallback: Disconnect");
                        }

                        return;
                    }
            }

            log.ErrorFormat("PeerStatusCallback: {0}", statusCode);
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        ///   The add event.
        /// </summary>
        /// <param name = "eventData">
        ///   The event data.
        /// </param>
        private void AddEvent(EventData eventData)
        {
            Interlocked.Increment(ref eventsReceived);
            this.eventList.Add(eventData);

            if (log.IsDebugEnabled)
            {
                if (eventData.Parameters.ContainsKey((byte)ParameterCode.ItemId))
                {
                    log.DebugFormat(
                        "{0} receives event, {1} total - code {2}, source {3}", 
                        this.username, 
                        this.eventList.Count, 
                        (EventCode)eventData.Code, 
                        eventData[(byte)ParameterCode.ItemId]);
                }
                else
                {
                    log.DebugFormat("{0} receives event, {1} total - code {2}", this.username, this.eventList.Count, (EventCode)eventData.Code);
                }
            }
        }

        /// <summary>
        ///   The add response.
        /// </summary>
        /// <param name = "response">
        ///   The response.
        /// </param>
        private void AddResponse(OperationResponse response)
        {
            Interlocked.Increment(ref responseReceived);
            this.responseList.Add(response);

            if (response.ReturnCode != (int)ReturnCode.Ok)
            {
                log.ErrorFormat("ERR {0}, OP {1}, DBG {2}", (ReturnCode)(int)response.ReturnCode, (OperationCode)response.OperationCode, response.DebugMessage);
            }
            else if (log.IsDebugEnabled)
            {
                if (response.Parameters.ContainsKey((byte)ParameterCode.ItemId))
                {
                    log.DebugFormat(
                        "{0} receives response, {1} total - code {2}, source {3}", 
                        this.username, 
                        this.responseList.Count, 
                        (EventCode)response.OperationCode, 
                        response[(byte)ParameterCode.ItemId]);
                }
                else
                {
                    log.DebugFormat("{0} receives response, {1} total - code {2}", this.username, this.responseList.Count, (EventCode)response.OperationCode);
                }
            }
        }

        #endregion
    }
}