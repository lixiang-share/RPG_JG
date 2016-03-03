// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CounterDataEvent.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Clients receive this event after executing operation <see cref="SubscribeCounter" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Events
{
    using Photon.MmoDemo.Common;
    using Photon.MmoDemo.Server.Operations;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// Clients receive this event after executing operation <see cref="SubscribeCounter"/>.
    /// </summary>
    public class CounterDataEvent
    {
        /// <summary>
        /// Gets or sets the counter name.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.CounterName)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the timestamps of the counter values.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.CounterTimeStamps)]
        public long[] TimeStamps { get; set; }

        /// <summary>
        /// Gets or sets the counter values.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.CounterValues)]
        public float[] Values { get; set; }
    }
}