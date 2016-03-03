// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationCode.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the OperationCode type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.Operations
{
    public enum OperationCode : byte
    {
        // operation codes inherited from lite
        //Join = 255 (Join is not used the in load balancing project)
        Leave = Lite.Operations.OperationCode.Leave,
        RaiseEvent = Lite.Operations.OperationCode.RaiseEvent,
        SetProperties = Lite.Operations.OperationCode.SetProperties,
        GetProperties = Lite.Operations.OperationCode.GetProperties,
        Ping = Lite.Operations.OperationCode.Ping,

        // operation codes in load the balancing project
        ChangeGroups = 248,
        Authenticate = 230, 
        JoinLobby = 229, 
        LeaveLobby = 228, 
        CreateGame = 227, 
        JoinGame = 226, 
        JoinRandomGame = 225, 
        // CancelJoinRandomGame = 224, currently not used 
        DebugGame = 223,
        FiendFriends = 222,
    }
}