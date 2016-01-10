// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FeedbackName.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the FeedbackName type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.LoadShedding
{
    internal enum FeedbackName
    {
        CpuUsage, 
        BusinessLogicQueueLength, 
        ENetQueueLength, 
        PeerCount, 
        Bandwidth,
        LatencyTcp,
        LatencyUdp,
        OutOfRotation,
        EnetThreadsProcessing,
        TimeSpentInServer,
        DisconnectRateUdp
    }
}