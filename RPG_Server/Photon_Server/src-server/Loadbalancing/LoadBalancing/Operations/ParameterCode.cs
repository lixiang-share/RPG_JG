// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterCode.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the ParameterCode type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.Operations
{
    public enum ParameterCode : byte
    {
        // parameters inherited from lite
        GameId = Lite.Operations.ParameterKey.GameId,
        ActorNr = Lite.Operations.ParameterKey.ActorNr,
        TargetActorNr = Lite.Operations.ParameterKey.TargetActorNr,
        Actors = Lite.Operations.ParameterKey.Actors,
        Properties = Lite.Operations.ParameterKey.Properties,
        Broadcast = Lite.Operations.ParameterKey.Broadcast,
        ActorProperties = Lite.Operations.ParameterKey.ActorProperties,
        GameProperties = Lite.Operations.ParameterKey.GameProperties,
        Cache = Lite.Operations.ParameterKey.Cache,
        ReceiverGroup = Lite.Operations.ParameterKey.ReceiverGroup,
        Data = Lite.Operations.ParameterKey.Data,
        Code = Lite.Operations.ParameterKey.Code,
        Flush = Lite.Operations.ParameterKey.Flush,
        DeleteCacheOnLeave = Lite.Operations.ParameterKey.DeleteCacheOnLeave,
        Group = Lite.Operations.ParameterKey.Group,
        GroupsForRemove = Lite.Operations.ParameterKey.GroupsForRemove,
        GroupsForAdd = Lite.Operations.ParameterKey.GroupsForAdd,
        SuppressRoomEvents = Lite.Operations.ParameterKey.SuppressRoomEvents,

        // load balancing project specific parameters
        Address = 230,
        PeerCount = 229,
        GameCount = 228,
        MasterPeerCount = 227,
        UserId = 225,
        ApplicationId = 224,
        Position = 223,
        GameList = 222,
        Secret = 221,
        AppVersion = 220,
        NodeId = 219,
        Info = 218,
        ClientAuthenticationType = 217,
        ClientAuthenticationParams = 216,
        CreateIfNotExists = 215,
        JoinMode = 215,
        ClientAuthenticationData = 214,
        LobbyName = 213,
        LobbyType = 212,
    }
}