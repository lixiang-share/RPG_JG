// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CounterOperations.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This class handles the operations <see cref="SubscribeCounter">SubscribeCounter</see> and <see cref="UnsubscribeCounter">UnsubscribeCounter</see>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server
{
    using System.Collections.Generic;
    using System.Linq;

    using ExitGames.Diagnostics.Monitoring;

    using Photon.MmoDemo.Common;
    using Photon.MmoDemo.Server.Events;
    using Photon.MmoDemo.Server.Operations;
    using Photon.SocketServer;

    /// <summary>
    /// This class handles the operations <see cref="SubscribeCounter">SubscribeCounter</see> and <see cref="UnsubscribeCounter">UnsubscribeCounter</see>.
    /// </summary>
    public static class CounterOperations
    {
        /// <summary>
        /// Handles operation <see cref="SubscribeCounter"/>: The client receives counter updates from the <see cref="PhotonApplication.CounterPublisher"/>.
        /// </summary>
        /// <param name="peer">
        /// The client peer.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// An <see cref="OperationResponse"/> with error code <see cref="ReturnCode.Ok"/> or <see cref="ReturnCode.InvalidOperation"/> if already subscribed.
        /// </returns>
        public static OperationResponse SubscribeCounter(PeerBase peer, OperationRequest request)
        {
            var operation = new SubscribeCounter(peer.Protocol, request);
            if (!operation.IsValid)
            {
                return new OperationResponse(request.OperationCode) { ReturnCode = (int)ReturnCode.InvalidOperationParameter, DebugMessage = operation.GetErrorMessage() };
            }

            var mmoPeer = (MmoPeer)peer;
            if (mmoPeer.CounterSubscription == null)
            {
                mmoPeer.CounterSubscription = PhotonApplication.CounterPublisher.Channel.SubscribeToBatch(
                    peer.RequestFiber, m => PublishCounterData(peer, m), operation.ReceiveInterval);

                return operation.GetOperationResponse(MethodReturnValue.Ok);
            }

            return operation.GetOperationResponse((int)ReturnCode.InvalidOperation, "already subscribed");
        }

        /// <summary>
        /// Handles operation <see cref="UnsubscribeCounter"/>: The client stops receiving counter updates from the <see cref="PhotonApplication.CounterPublisher"/>.
        /// </summary>
        /// <param name="peer">
        /// The client peer.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// An <see cref="OperationResponse"/> with error code <see cref="ReturnCode.Ok"/> or <see cref="ReturnCode.InvalidOperation"/> if not yet subscribed.
        /// </returns>
        public static OperationResponse UnsubscribeCounter(PeerBase peer, OperationRequest request)
        {
            var mmoPeer = (MmoPeer)peer;
            if (mmoPeer.CounterSubscription == null)
            {
                return new OperationResponse(request.OperationCode) { ReturnCode = (int)ReturnCode.InvalidOperation, DebugMessage = "not subscribed" };
            }

            mmoPeer.CounterSubscription.Dispose();
            mmoPeer.CounterSubscription = null;
            return new OperationResponse(request.OperationCode);
        }

        /// <summary>
        /// The publish counter data.
        /// </summary>
        /// <param name="peer">
        /// The client peer.
        /// </param>
        /// <param name="counterSamples">
        /// The counter samples.
        /// </param>
        private static void PublishCounterData(PeerBase peer, ICollection<CounterSampleMessage> counterSamples)
        {
            IEnumerable<CounterAggregation> aggregations = CounterAggregation.Create(counterSamples);
            foreach (CounterAggregation aggregation in aggregations)
            {
                var @event = new CounterDataEvent
                    {
                        Name = aggregation.CounterName,
                        TimeStamps = aggregation.Timestamps.ToArray(),
                        Values = aggregation.Values.ToArray()
                    };

                var eventData = new EventData((byte)EventCode.CounterData, @event);

                // already in right fiber, we would use peer.SendEvent otherwise
                peer.SendEvent(eventData, new SendParameters { ChannelId = Settings.DiagnosticsEventChannel });
            }
        }

        /// <summary>
        /// The counter aggregation.
        /// </summary>
        private class CounterAggregation
        {
            /// <summary>
            /// Gets the CounterName.
            /// </summary>
            public string CounterName { get; private set; }

            /// <summary>
            /// Gets the counter Timestamps.
            /// </summary>
            public List<long> Timestamps { get; private set; }

            /// <summary>
            /// Gets the counter Values.
            /// </summary>
            public List<float> Values { get; private set; }

            /// <summary>
            /// The create.
            /// </summary>
            /// <param name="dataSamples">
            /// The data samples.
            /// </param>
            /// <returns>
            /// an array of counter samples
            /// </returns>
            public static IEnumerable<CounterAggregation> Create(ICollection<CounterSampleMessage> dataSamples)
            {
                if (dataSamples.Count == 0)
                {
                    return new CounterAggregation[0];
                }

                var result = new Dictionary<string, CounterAggregation>();

                // add other values
                foreach (CounterSampleMessage sample in dataSamples)
                {
                    CounterAggregation aggregation;
                    if (result.TryGetValue(sample.CounterName, out aggregation) == false)
                    {
                        aggregation = new CounterAggregation { CounterName = sample.CounterName, Timestamps = new List<long>(), Values = new List<float>() };

                        result.Add(aggregation.CounterName, aggregation);
                    }

                    aggregation.Timestamps.Add(sample.CounterSample.Timestamp.ToBinary());
                    aggregation.Values.Add(sample.CounterSample.Value);
                }

                return result.Values.ToArray();
            }
        }
    }
}