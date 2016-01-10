// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Connected.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.UnitTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;

    using ExitGames.Client.Photon;
    using ExitGames.Client.Photon.LoadBalancing;

    using Lite.Operations;

    using NUnit.Framework;

    using Photon.LoadBalancing.Operations;
    using Photon.LoadBalancing.UnitTests.Client;

    using ErrorCode = ExitGames.Client.Photon.LoadBalancing.ErrorCode;
    using EventCode = ExitGames.Client.Photon.LoadBalancing.EventCode;
    using OperationCode = ExitGames.Client.Photon.LoadBalancing.OperationCode;
    using ParameterCode = ExitGames.Client.Photon.LoadBalancing.ParameterCode;

    /// <summary>
    ///   Test cases for the LoadBalancing applications. 
    ///   Requires that Photon's "LoadBalancing" is started with Master, Game1 and Game2 application.
    /// </summary>
    [TestFixture]
    public class ApiTests
    {
        #region Constants and Fields

        private const string AppId = "Master";

        //private static string appVersion = "1.0";

        private const ConnectionProtocol Protocol = ConnectionProtocol.Tcp;

        private static readonly IPEndPoint endPointGame1 = new IPEndPoint(IPAddress.Loopback, 4531);

        //private static readonly IPEndPoint endPointGame2 = new IPEndPoint(IPAddress.Loopback, 4532);

        private static readonly IPEndPoint endPointMaster = new IPEndPoint(IPAddress.Loopback, 4530);

        private const string Player1 = "Player1";

        private const string Player2 = "Player2";

        private const int WaitTime = 5000;

        #endregion

        //private AutoResetEvent connected;

        #region Public Methods

        [SetUp]
        [Test]
        public void ConnectionTest()
        {
            // master: 
            var client = new NunitClient(Protocol, AppId, WaitTime);
            client.Connect(endPointMaster);
            client.Dispose();
           
            // game1: 
            client = new NunitClient(Protocol, AppId, WaitTime);
            client.Connect(endPointGame1);
            client.Dispose();

            // game2: 
            ////client = new NunitClient(Protocol, waitTime, appId);
            ////client.Connect(endPointGame2);
            ////client.Dispose();
            
            // verify that games from previous tests are cleaned up / check lobby on master:
            CheckGameListCount(0);
        }

        private static void CheckGameListCount(int expectedGameCount, Hashtable gameList = null)
        {
            if (gameList == null)
            {
                var masterClient = new TestClient(Protocol);
                masterClient.Connect(endPointMaster.Address.ToString(), endPointMaster.Port, AppId);
                masterClient.WaitForConnect(WaitTime);

                Assert.IsTrue(masterClient.PhotonClient.OpJoinLobby());
                var ev = masterClient.WaitForEvent(EventCode.GameList, WaitTime);
                Assert.AreEqual(EventCode.GameList, ev.Code);
                gameList = (Hashtable)ev.Parameters[ParameterCode.GameList];

                masterClient.Close();
            }

            int openGames = gameList.Count;

            if (expectedGameCount > 0 && openGames == 0)
            {
                Assert.Fail("Expected {0} games listed in lobby, but got: 0", expectedGameCount);
            }

            if (openGames != expectedGameCount)
            {
                var gameNames = new string[gameList.Count];
                gameList.Keys.CopyTo(gameNames, 0);
                var msg = string.Format(
                    "Expected {0} open games, but got {1}: {2}", expectedGameCount, openGames, string.Join(",", gameNames));
                Assert.Fail(msg);
            }
        }

        [Test]
        public void CreateGameTwice()
        {
            NunitClient masterClient = null;

            try
            {
                masterClient = CreateMasterClientAndAuthenticate(null);
                string roomName = GenerateRandomizedRoomName("CreateGameTwice_");
                masterClient.CreateGame(roomName, Operations.ErrorCode.Ok);
                masterClient.CreateGame(roomName, Operations.ErrorCode.GameIdAlreadyExists);
            }
            finally
            {
                DisposeClients(masterClient);
            }
        }

        [Test]
        public void InvisibleGame()
        {
            NunitClient masterClient = null;
            NunitClient gameClient1 = null;
            NunitClient gameClient2 = null;

            try
            {
                string roomName = GenerateRandomizedRoomName("InvisibleGame_");

                // create room 
                gameClient1 = CreateGameOnGameServer(null, roomName, null, 0, false, true, 0, null, null);

                // connect 2nd client to master
                masterClient = CreateMasterClientAndAuthenticate(null);

                // try join random game should fail because the game is not visible
                var joinRandomGameRequest = new JoinRandomGameRequest { JoinRandomType = (byte)MatchmakingMode.FillRoom };
                masterClient.JoinRandomGame(joinRandomGameRequest, (short)Operations.ErrorCode.NoMatchFound);

                // join 2nd client on master - ok:
                var joinGameRequest = new JoinGameRequest { GameId = roomName };
                var joinResponse = masterClient.JoinGame(joinGameRequest, Operations.ErrorCode.Ok);
                masterClient.Dispose();

                // join directly on GS - game full:
                gameClient2 = new NunitClient(Protocol, AppId, WaitTime);
                gameClient2.ConnectAndAuthenticate(joinResponse.Address, null);
                gameClient2.JoinGame(joinGameRequest, Operations.ErrorCode.Ok);
            }
            finally
            {
                DisposeClients(masterClient, gameClient1, gameClient2);
            }
        }

        [Test]
        public void ClosedGame()
        {
            NunitClient masterClient = null;
            NunitClient gameClient1 = null;
            NunitClient gameClient2 = null;

            try
            {
                // create the game
                string roomName = GenerateRandomizedRoomName("ClosedGame_");
                gameClient1 = CreateGameOnGameServer(null, roomName, null, 0, true, false, 0, null, null);

                // join 2nd client on master - closed: 
                masterClient = CreateMasterClientAndAuthenticate(null);
                var joinGameRequest = new JoinGameRequest { GameId = roomName };
                masterClient.JoinGame(joinGameRequest, Operations.ErrorCode.GameClosed);
                masterClient.Dispose();
               
                // join directly on GS - game closed: 
                gameClient2 = new NunitClient(Protocol, AppId, WaitTime);
                gameClient2.ConnectAndAuthenticate(gameClient1.RemoteEndPoint, null);
                gameClient2.JoinGame(roomName, Operations.ErrorCode.GameClosed);

            }
            finally
            {
                DisposeClients(masterClient, gameClient1, gameClient2);
            }
        }

        [Test]
        public void MaxPlayers()
        {
            NunitClient masterClient = null;
            NunitClient gameClient1 = null;
            NunitClient gameClient2 = null;

            try
            {
                string roomName = GenerateRandomizedRoomName("MaxPlayers_");
                gameClient1 = CreateGameOnGameServer(null, roomName, null, 0, true, true, 1, null, null);

                // join 2nd client on master - full: 
                masterClient = CreateMasterClientAndAuthenticate(null);
                masterClient.JoinGame(roomName, Operations.ErrorCode.GameFull);

                // join random 2nd client on master - full: 
                var joinRequest = new JoinRandomGameRequest();
                masterClient.JoinRandomGame(joinRequest, (short)Operations.ErrorCode.NoMatchFound);
                joinRequest.JoinRandomType = (byte)MatchmakingMode.SerialMatching;
                masterClient.JoinRandomGame(joinRequest, (short)Operations.ErrorCode.NoMatchFound);
                joinRequest.JoinRandomType = (byte)MatchmakingMode.RandomMatching;
                masterClient.JoinRandomGame(joinRequest, (short)Operations.ErrorCode.NoMatchFound);
                masterClient.Dispose();

                // join directly on GS: 
                gameClient2 = new NunitClient(Protocol, AppId, WaitTime);
                gameClient2.ConnectAndAuthenticate(gameClient1.RemoteEndPoint, null);
                gameClient2.JoinGame(roomName, Operations.ErrorCode.GameFull);
            }
            finally
            {
                DisposeClients(masterClient, gameClient1, gameClient2);
            }
        }

        [Test]
        public void LobbyGameListEvents()
        {
            // previous tests could just have leaved games on the game server
            // so there might be AppStats or GameListUpdate event in schedule.
            // Just wait a second so this events can published before starting the test
            Thread.Sleep(1100);

            var masterClient1 = new TestClient(ConnectionProtocol.Tcp);
            var masterClient2 = new TestClient(ConnectionProtocol.Tcp);

            var gameClient1 = new TestClient(ConnectionProtocol.Tcp); 

            try
            {
                masterClient1.Connect(endPointMaster.Address.ToString(), endPointMaster.Port, AppId);
                masterClient1.WaitForConnect(WaitTime);

                Assert.IsTrue(masterClient1.PhotonClient.OpJoinLobby());
                var ev = masterClient1.WaitForEvent(EventCode.GameList, WaitTime);
                Assert.AreEqual(EventCode.GameList, ev.Code);
                var gameList = (Hashtable)ev.Parameters[ParameterCode.GameList];
                CheckGameListCount(0, gameList);

                masterClient2.Connect(endPointMaster.Address.ToString(), endPointMaster.Port, AppId);
                masterClient2.WaitForConnect(WaitTime);

                Assert.IsTrue(masterClient2.PhotonClient.OpJoinLobby());
                ev = masterClient2.WaitForEvent(EventCode.GameList, WaitTime);
                Assert.AreEqual(EventCode.GameList, ev.Code);
                gameList = (Hashtable)ev.Parameters[ParameterCode.GameList];
                CheckGameListCount(0, gameList);

                // join lobby again: 
                masterClient1.OperationResponseQueue.Clear();
                Assert.IsTrue(masterClient1.PhotonClient.OpJoinLobby());
                var operationResponse = masterClient1.WaitForOperationResponse(WaitTime);
                Assert.AreEqual(
                    Operations.OperationCode.JoinLobby, (Operations.OperationCode)operationResponse.OperationCode);
                Assert.AreEqual(
                    Operations.ErrorCode.Ok,
                    (Operations.ErrorCode)operationResponse.ReturnCode,
                    operationResponse.DebugMessage);


                masterClient1.EventQueue.Clear();
                masterClient2.EventQueue.Clear();

                // open game

                string roomName = "LobbyGamelistEvents_1_" + Guid.NewGuid().ToString().Substring(0, 6);
                string gameServerIp;
                int gameServerPort;
                
                CreateRoomOnGameServer(masterClient1, roomName, out gameServerIp, out gameServerPort, out gameClient1);


                var timeout = Environment.TickCount + 10000; 

                bool gameListUpdateReceived = false;
                bool appStatsReceived = false;  

                while (Environment.TickCount < timeout && (!gameListUpdateReceived || !appStatsReceived))
                {
                    try
                    {
                        ev = masterClient2.WaitForEvent(1000);

                        if (ev.Code == EventCode.AppStats)
                        {
                            appStatsReceived = true;
                            Assert.AreEqual(1, ev[ParameterCode.GameCount]);
                        }
                        else if (ev.Code == EventCode.GameListUpdate)
                        {
                            gameListUpdateReceived = true;
                            var roomList = (Hashtable)ev.Parameters[ParameterCode.GameList];
                            Assert.AreEqual(1, roomList.Count);
                            
                            var room = (Hashtable)roomList[roomName];
                            Assert.IsNotNull(room);
                            Assert.AreEqual(3, room.Count);

                            Assert.IsNotNull(room[GameProperties.IsOpen], "IsOpen");
                            Assert.IsNotNull(room[GameProperties.MaxPlayers], "MaxPlayers");
                            Assert.IsNotNull(room[GameProperties.PlayerCount], "PlayerCount");
                            
                            Assert.AreEqual(true, room[GameProperties.IsOpen]);
                            Assert.AreEqual(0, room[GameProperties.MaxPlayers]);
                            Assert.AreEqual(1, room[GameProperties.PlayerCount]);
                        }
                    }
                    catch (TimeoutException)
                    {
                    }
                }

                Assert.IsTrue(gameListUpdateReceived, "GameListUpdate event received");
                Assert.IsTrue(appStatsReceived, "AppStats event received");
                
                
                gameClient1.SendOperationRequest(new OperationRequest { OperationCode = OperationCode.Leave });

                gameListUpdateReceived = false;
                appStatsReceived = false;

                timeout = Environment.TickCount + 10000;
                while (Environment.TickCount < timeout && (!gameListUpdateReceived || !appStatsReceived))
                {
                    try
                    {
                        ev = masterClient2.WaitForEvent(1000);

                        if (ev.Code == EventCode.AppStats)
                        {
                            appStatsReceived = true;
                            Assert.AreEqual(0, ev[ParameterCode.GameCount]);
                        }

                        if (ev.Code == EventCode.GameListUpdate)
                        {
                            gameListUpdateReceived = true;
                            
                            var roomList = (Hashtable)ev.Parameters[ParameterCode.GameList];
                            Assert.AreEqual(1, roomList.Count);
                            var room = (Hashtable)roomList[roomName];
                            Assert.IsNotNull(room);

                            Assert.AreEqual(1, room.Count);
                            Assert.IsNotNull(room[GameProperties.Removed], "Removed");
                            Assert.AreEqual(true, room[GameProperties.Removed]);
                        }
                    }
                    catch (TimeoutException)
                    {
                    }
                }

                Assert.IsTrue(gameListUpdateReceived, "GameListUpdate event received");
                Assert.IsTrue(appStatsReceived, "AppStats event received");
                
                // leave lobby
                masterClient2.PhotonClient.OpLeaveLobby();

                gameListUpdateReceived = false;
                appStatsReceived = false;

                masterClient1.Connect(endPointMaster.Address.ToString(), endPointMaster.Port, AppId);
                masterClient1.WaitForConnect(WaitTime);

                roomName = "LobbyGamelistEvents_2_" + Guid.NewGuid().ToString().Substring(0, 6);

                CreateRoomOnGameServer(masterClient1, roomName, out gameServerIp, out gameServerPort, out gameClient1);

                timeout = Environment.TickCount + 10000;

                while (Environment.TickCount < timeout && (!gameListUpdateReceived || !appStatsReceived))
                {
                    try
                    {
                        ev = masterClient2.WaitForEvent(1000);

                        if (ev.Code == EventCode.AppStats)
                        {
                            appStatsReceived = true;
                            Assert.AreEqual(1, ev[ParameterCode.GameCount]);
                        }

                        if (ev.Code == EventCode.GameListUpdate)
                        {
                            gameListUpdateReceived = true;
                        }
                    }
                    catch (TimeoutException)
                    {
                    }

                }
                Assert.IsFalse(gameListUpdateReceived, "GameListUpdate event received");
                Assert.IsTrue(appStatsReceived, "AppStats event received"); 
            }
            finally
            {
                if (masterClient1.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    masterClient1.Close();
                }

                if (gameClient1.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    gameClient1.Close();
                }

                if (masterClient2.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    masterClient2.Close();
                }
            }
        }

        [Test]
        public void JoinNotExistingGame()
        {
            NunitClient client = null;

            try
            {
                string roomName = GenerateRandomizedRoomName("JoinNoMatchFound_");

                // try join game on master
                client = CreateMasterClientAndAuthenticate(null);
                client.JoinGame(roomName, Operations.ErrorCode.GameIdNotExists);
                client.Dispose();

                // try join game on gameServer
                client = new NunitClient(Protocol, AppId, WaitTime);
                client.ConnectAndAuthenticate(endPointGame1, null);
                client.JoinGame(roomName, Operations.ErrorCode.GameIdNotExists);
                client.Dispose();
            }
            finally
            {
                DisposeClients(client);
            }
        }


        [Test]
        public void JoinCreateIfNotExists()
        {
            NunitClient masterClient1 = null;
            NunitClient masterClient2 = null;

            NunitClient gameClient1 = null;
            NunitClient gameClient2 = null;

            try
            {
                string roomName = GenerateRandomizedRoomName("JoinCreateIfNotExists_");
                var joinRequest = new JoinGameRequest 
                { 
                    GameId = roomName, 
                    CreateIfNotExists = true 
                };

                masterClient1 = CreateMasterClientAndAuthenticate(null);
                masterClient2 = CreateMasterClientAndAuthenticate(null);

                // try to join a game which does not exists
                var joinResponse1 = masterClient1.JoinGame(joinRequest, Operations.ErrorCode.Ok);
                masterClient1.Dispose();
                
                // try to join a game which exists but is not created on the game server
                var joinResponse2 = masterClient2.JoinGame(joinRequest, Operations.ErrorCode.Ok);
                Assert.AreEqual(joinResponse1.Address, joinResponse2.Address);

                // try to join not existing game on the game server
                gameClient1 = new NunitClient(Protocol, AppId, WaitTime);
                gameClient1.Connect(joinResponse1.Address);
                gameClient1.JoinGame(joinRequest, Operations.ErrorCode.Ok);

                // try to join a game which exists and is created on the game server
                joinResponse2 = masterClient2.JoinGame(joinRequest, Operations.ErrorCode.Ok);
                Assert.AreEqual(joinResponse1.Address, joinResponse2.Address);
                masterClient2.Dispose();

                // try to join existing game on the game server
                gameClient2 = new NunitClient(Protocol, AppId, WaitTime);
                gameClient2.Connect(joinResponse2.Address);
                gameClient2.JoinGame(joinRequest, Operations.ErrorCode.Ok);
            }
            finally
            {
                DisposeClients(masterClient1, masterClient2);
                DisposeClients(gameClient1, gameClient2);
            }
        }

        [Test]
        public void JoinOnGameServer()
        {
            NunitClient masterClient1 = null;
            NunitClient masterClient2 = null;

            NunitClient gameClient1 = null;
            NunitClient gameClient2 = null;

            try
            {
                masterClient1 = CreateMasterClientAndAuthenticate(null);
                masterClient2 = CreateMasterClientAndAuthenticate(null);

                // create game
                string roomName = GenerateRandomizedRoomName("JoinOnGameServer_");
                var createResponse = masterClient1.CreateGame(roomName, Operations.ErrorCode.Ok);

                // join on master while the first client is not yet on GS:
                masterClient2.JoinGame(roomName, Operations.ErrorCode.GameIdNotExists);

                // move 1st client to GS: 
                masterClient1.Dispose();
                masterClient1 = null;

                var player1Properties = new Hashtable();
                player1Properties.Add("Name", Player1);
                var createRequest = new OperationRequest { OperationCode = OperationCode.CreateGame, Parameters = new Dictionary<byte, object>() };
                createRequest.Parameters[ParameterCode.RoomName] = roomName;
                createRequest.Parameters[ParameterCode.Broadcast] = true;
                createRequest.Parameters[ParameterCode.PlayerProperties] = player1Properties;

                gameClient1 = new NunitClient(Protocol, AppId, WaitTime);
                gameClient1.ConnectAndAuthenticate(createResponse.Address, null);
                gameClient1.SendRequestAndWaitForResponse(createRequest);

                // get own join event: 
                var ev = gameClient1.WaitForEvent(WaitTime);
                Assert.AreEqual(EventCode.Join, ev.Code);
                Assert.AreEqual(1, ev.Parameters[ParameterCode.ActorNr]);
                var playerProperties = ((Hashtable)ev.Parameters[ParameterCode.PlayerProperties]);
                Assert.AreEqual(Player1, playerProperties["Name"]);

                // join 2nd client on master - ok: 
                var joinResponse = masterClient2.JoinGame(roomName, Operations.ErrorCode.Ok);

                // disconnect and move 2nd client to GS: 
                masterClient2.PhotonClient.Disconnect();

                var player2Properties = new Hashtable();
                player2Properties.Add("Name", Player2);
                var joinRequest = new OperationRequest { OperationCode = OperationCode.JoinGame, Parameters = new Dictionary<byte, object>() };
                joinRequest.Parameters[ParameterCode.RoomName] = roomName;
                joinRequest.Parameters[ParameterCode.Broadcast] = true;
                joinRequest.Parameters[ParameterCode.PlayerProperties] = player2Properties;

                gameClient2 = new NunitClient(Protocol, AppId, WaitTime);
                gameClient2.ConnectAndAuthenticate(joinResponse.Address, null);
                gameClient2.SendRequestAndWaitForResponse(joinRequest);

                ev = gameClient1.WaitForEvent(WaitTime);
                Assert.AreEqual(EventCode.Join, ev.Code);
                Assert.AreEqual(2, ev.Parameters[ParameterCode.ActorNr]);
                playerProperties = ((Hashtable)ev.Parameters[ParameterCode.PlayerProperties]);
                Assert.AreEqual(Player2, playerProperties["Name"]);

                ev = gameClient2.WaitForEvent(WaitTime);
                Assert.AreEqual(EventCode.Join, ev.Code);
                Assert.AreEqual(2, ev.Parameters[ParameterCode.ActorNr]);
                playerProperties = ((Hashtable)ev.Parameters[ParameterCode.PlayerProperties]);
                Assert.AreEqual(Player2, playerProperties["Name"]);

                // TODO: continue implementation
                // raise event, leave etc.        
            }
            finally
            {
                DisposeClients(masterClient1, masterClient2, gameClient1, gameClient2);
            }
        }
        
        [Test]
        public void JoinRandomNoMatchFound()
        {
            var masterClient1 = new TestClient(ConnectionProtocol.Tcp);
            masterClient1.Connect(endPointMaster.Address.ToString(), endPointMaster.Port, AppId);
            masterClient1.WaitForConnect(WaitTime);

            try
            {
                Assert.IsTrue(
                    masterClient1.PhotonClient.OpJoinRandomRoom(
                        new Hashtable(), 0, new Hashtable(), MatchmakingMode.FillRoom));
                var operationResponse = masterClient1.WaitForOperationResponse(WaitTime);
                Assert.AreEqual(
                   Operations.OperationCode.JoinRandomGame, (Operations.OperationCode)operationResponse.OperationCode);
                Assert.AreEqual(Operations.ErrorCode.NoMatchFound, (Operations.ErrorCode)operationResponse.ReturnCode, operationResponse.DebugMessage);
            }
            finally
            {
                if (masterClient1.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    masterClient1.Close();
                }
            }
        }

        [Test]
        public void JoinRandomOnGameServer()
        {
            var masterClient1 = new TestClient(ConnectionProtocol.Tcp);
            masterClient1.Connect(endPointMaster.Address.ToString(), endPointMaster.Port, AppId);
            masterClient1.WaitForConnect(WaitTime);

            var masterClient2 = new TestClient(ConnectionProtocol.Tcp);
            masterClient2.Connect(endPointMaster.Address.ToString(), endPointMaster.Port, AppId);
            masterClient2.WaitForConnect(WaitTime);

            var gameClient1 = new TestClient(ConnectionProtocol.Tcp);
            var gameClient2 = new TestClient(ConnectionProtocol.Tcp);

            try
            {
                // create
                string roomName = "JoinRandomOnGameServer_" + Guid.NewGuid().ToString().Substring(0, 6);
                Assert.IsTrue(
                    masterClient1.PhotonClient.OpCreateRoom(
                        roomName, true, true, 0, new Hashtable(), new string[0], new Hashtable()));
                var operationResponse = masterClient1.WaitForOperationResponse(WaitTime);
                Assert.AreEqual(
                    Operations.OperationCode.CreateGame, (Operations.OperationCode)operationResponse.OperationCode);
                Assert.AreEqual(
                    Operations.ErrorCode.Ok,
                    (Operations.ErrorCode)operationResponse.ReturnCode,
                    operationResponse.DebugMessage);

                var gameServerAddress1 = (string)operationResponse.Parameters[(byte)Operations.ParameterCode.Address];
                Console.WriteLine("Match on GS: " + gameServerAddress1);

                // join on master while the first client is not yet on GS:
                Assert.IsTrue(
                    masterClient2.PhotonClient.OpJoinRandomRoom(
                        new Hashtable(), 0, new Hashtable(), MatchmakingMode.FillRoom));
                operationResponse = masterClient2.WaitForOperationResponse(WaitTime);
                Assert.AreEqual(
                    Operations.OperationCode.JoinRandomGame, (Operations.OperationCode)operationResponse.OperationCode);
                Assert.AreEqual(
                    Operations.ErrorCode.NoMatchFound,
                    (Operations.ErrorCode)operationResponse.ReturnCode,
                    operationResponse.DebugMessage);

                // move 1st client to GS: 
                masterClient1.PhotonClient.Disconnect();

                string[] split = gameServerAddress1.Split(':');
                gameClient1.Connect(split[0], int.Parse(split[1]), AppId);
                gameClient1.WaitForConnect(WaitTime);

                var player1Properties = new Hashtable();
                player1Properties.Add("Name", Player1);

                gameClient1.PhotonClient.OpCreateRoom(
                    roomName, true, true, 0, new Hashtable(), new string[0], player1Properties);
                operationResponse = gameClient1.WaitForOperationResponse(WaitTime);
                Assert.AreEqual(
                    Operations.OperationCode.CreateGame, (Operations.OperationCode)operationResponse.OperationCode);
                Assert.AreEqual(
                    Operations.ErrorCode.Ok,
                    (Operations.ErrorCode)operationResponse.ReturnCode,
                    operationResponse.DebugMessage);

                // get own join event: 
                var ev = gameClient1.WaitForEvent(WaitTime);
                Assert.AreEqual(EventCode.Join, ev.Code);
                Assert.AreEqual(1, ev.Parameters[ParameterCode.ActorNr]);
                var playerProperties = ((Hashtable)ev.Parameters[ParameterCode.PlayerProperties]);
                Assert.AreEqual(Player1, playerProperties["Name"]);

                // join 2nd client on master - ok: 
                Assert.IsTrue(
                    masterClient2.PhotonClient.OpJoinRandomRoom(
                        new Hashtable(), 0, new Hashtable(), MatchmakingMode.FillRoom));
                operationResponse = masterClient2.WaitForOperationResponse(WaitTime);
                Assert.AreEqual(
                    Operations.OperationCode.JoinRandomGame, (Operations.OperationCode)operationResponse.OperationCode);
                Assert.AreEqual(
                    Operations.ErrorCode.Ok,
                    (Operations.ErrorCode)operationResponse.ReturnCode,
                    operationResponse.DebugMessage);

                var gameServerAddress2 = (string)operationResponse.Parameters[(byte)Operations.ParameterCode.Address];
                Assert.AreEqual(gameServerAddress1, gameServerAddress2);

                var roomName2 = (string)operationResponse.Parameters[(byte)Operations.ParameterCode.GameId];
                Assert.AreEqual(roomName, roomName2);

                // disconnect and move 2nd client to GS: 
                masterClient2.PhotonClient.Disconnect();

                gameClient2.Connect(split[0], int.Parse(split[1]), AppId);
                gameClient2.WaitForConnect(WaitTime);

                // clean up - just in case: 
                gameClient1.OperationResponseQueue.Clear();
                gameClient2.OperationResponseQueue.Clear();

                gameClient1.EventQueue.Clear();
                gameClient2.EventQueue.Clear();

                // join 2nd client on GS: 
                var player2Properties = new Hashtable();
                player2Properties.Add("Name", Player2);

                Assert.IsTrue(gameClient2.PhotonClient.OpJoinRoom(roomName, player2Properties));
                operationResponse = gameClient2.WaitForOperationResponse(WaitTime);
                Assert.AreEqual(
                    Operations.OperationCode.JoinGame, (Operations.OperationCode)operationResponse.OperationCode);
                Assert.AreEqual(
                    Operations.ErrorCode.Ok,
                    (Operations.ErrorCode)operationResponse.ReturnCode,
                    operationResponse.DebugMessage);

                ev = gameClient1.WaitForEvent(WaitTime);
                Assert.AreEqual(EventCode.Join, ev.Code);
                Assert.AreEqual(2, ev.Parameters[ParameterCode.ActorNr]);
                playerProperties = ((Hashtable)ev.Parameters[ParameterCode.PlayerProperties]);
                Assert.AreEqual(Player2, playerProperties["Name"]);

                ev = gameClient2.WaitForEvent(WaitTime);
                Assert.AreEqual(EventCode.Join, ev.Code);
                Assert.AreEqual(2, ev.Parameters[ParameterCode.ActorNr]);
                playerProperties = ((Hashtable)ev.Parameters[ParameterCode.PlayerProperties]);
                Assert.AreEqual(Player2, playerProperties["Name"]);

                // disocnnect 2nd client
                gameClient2.Close();
                ev = gameClient1.WaitForEvent(WaitTime);
                Assert.AreEqual(EventCode.Leave, ev.Code);
                Assert.AreEqual(2, ev.Parameters[ParameterCode.ActorNr]);

                // TODO: continue implementation
                // raise event, leave etc.        
            }
            finally
            {
                if (masterClient1.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    masterClient1.Close();
                }
                if (masterClient2.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    masterClient2.Close();
                }

                if (gameClient1.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    gameClient1.Close();
                }
                if (gameClient2.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    gameClient2.Close();
                }
            }
        }

        [Test]
        public void ApplicationStats()
        {
            var masterClient1 = new TestClient(ConnectionProtocol.Tcp);
            masterClient1.Connect(endPointMaster.Address.ToString(), endPointMaster.Port, AppId);
            masterClient1.WaitForConnect(WaitTime);

            var masterClient2 = new TestClient(ConnectionProtocol.Tcp);
            masterClient2.Connect(endPointMaster.Address.ToString(), endPointMaster.Port, AppId);
            masterClient2.WaitForConnect(WaitTime);

            var masterClient3 = new TestClient(ConnectionProtocol.Tcp);
            masterClient3.Connect(endPointMaster.Address.ToString(), endPointMaster.Port, AppId);
            masterClient3.WaitForConnect(WaitTime);

            var gameClient1 = new TestClient(ConnectionProtocol.Tcp);

            var gameClient2 = new TestClient(ConnectionProtocol.Tcp);

            try
            {
                string gameServerIp;
                int gameServerPort;
                string roomName = "ApplicationStats_" + Guid.NewGuid().ToString().Substring(0, 6);
               
                
                // app stats
                var appStatsEvent = masterClient3.WaitForEvent(10000); 
                Assert.IsNotNull(appStatsEvent, "AppStatsEvent");
                Assert.AreEqual(EventCode.AppStats, appStatsEvent.Code, "Event Code");
                Assert.AreEqual(3, appStatsEvent.Parameters[ParameterCode.MasterPeerCount], "Peer Count on Master");
                Assert.AreEqual(0, appStatsEvent.Parameters[ParameterCode.PeerCount], "Peer Count on GS");
                Assert.AreEqual(0, appStatsEvent.Parameters[ParameterCode.GameCount], "Game Count");

                masterClient3.EventQueue.Clear();


                CreateRoomOnGameServer(
                 masterClient1, true, true, 10, roomName, out gameServerIp, out gameServerPort, out gameClient1);

                // app stats
                appStatsEvent = masterClient3.WaitForEvent(10000);
                Assert.IsNotNull(appStatsEvent, "AppStatsEvent");
                Assert.AreEqual(EventCode.AppStats, appStatsEvent.Code, "Event Code");
                Assert.AreEqual(2, appStatsEvent.Parameters[ParameterCode.MasterPeerCount], "Peer Count on Master");
                Assert.AreEqual(1, appStatsEvent.Parameters[ParameterCode.PeerCount], "Peer Count on GS");
                Assert.AreEqual(1, appStatsEvent.Parameters[ParameterCode.GameCount], "Game Count");
                masterClient3.EventQueue.Clear();
                

                Assert.IsTrue(
                 masterClient2.PhotonClient.OpJoinRandomRoom(
                     new Hashtable(), 0, new Hashtable(), MatchmakingMode.FillRoom));
                var operationResponse = masterClient2.WaitForOperationResponse(WaitTime);

                Assert.AreEqual(
                    Operations.OperationCode.JoinRandomGame, (Operations.OperationCode)operationResponse.OperationCode);
                Assert.AreEqual(
                    Operations.ErrorCode.Ok,
                    (Operations.ErrorCode)operationResponse.ReturnCode,
                    operationResponse.DebugMessage);

                var gameServerAddress2 = (string)operationResponse.Parameters[(byte)Operations.ParameterCode.Address];
                Assert.AreEqual(string.Format("{0}:{1}", gameServerIp, gameServerPort), gameServerAddress2);

                var roomName2 = (string)operationResponse.Parameters[(byte)Operations.ParameterCode.GameId];
                Assert.AreEqual(roomName, roomName2);

                // no app stats if nothing changed?!
                //appStatsEvent = masterClient3.WaitForEvent(10000);
                //Assert.IsNotNull(appStatsEvent, "AppStatsEvent");
                //Assert.AreEqual(EventCode.AppStats, appStatsEvent.Code, "Event Code");
                //Assert.AreEqual(2, appStatsEvent.Parameters[ParameterCode.MasterPeerCount], "Peer Count on Master");
                //Assert.AreEqual(1, appStatsEvent.Parameters[ParameterCode.PeerCount], "Peer Count on GS");
                //Assert.AreEqual(1, appStatsEvent.Parameters[ParameterCode.GameCount], "Game Count");

                //masterClient3.EventQueue.Clear();


                // disconnect and move 2nd client to GS: 
                masterClient2.PhotonClient.Disconnect();

                gameClient2.Connect(gameServerIp, gameServerPort, AppId);
                gameClient2.WaitForConnect(WaitTime);
                
                // join 2nd client on GS: 
                var player2Properties = new Hashtable();
                player2Properties.Add("Name", Player2);

                Assert.IsTrue(gameClient2.PhotonClient.OpJoinRoom(roomName, player2Properties));
                operationResponse = gameClient2.WaitForOperationResponse(WaitTime);
                Assert.AreEqual(
                    Operations.OperationCode.JoinGame, (Operations.OperationCode)operationResponse.OperationCode);
                Assert.AreEqual(
                    Operations.ErrorCode.Ok,
                    (Operations.ErrorCode)operationResponse.ReturnCode,
                    operationResponse.DebugMessage);


                // app stats: 
                appStatsEvent = masterClient3.WaitForEvent(10000);
                Assert.IsNotNull(appStatsEvent, "AppStatsEvent");
                Assert.AreEqual(EventCode.AppStats, appStatsEvent.Code, "Event Code");
                Assert.AreEqual(1, appStatsEvent.Parameters[ParameterCode.MasterPeerCount], "Peer Count on Master");
                Assert.AreEqual(2, appStatsEvent.Parameters[ParameterCode.PeerCount], "Peer Count on GS");
                Assert.AreEqual(1, appStatsEvent.Parameters[ParameterCode.GameCount], "Game Count");

                masterClient3.EventQueue.Clear();

                gameClient2.SendOperationRequest(new OperationRequest { OperationCode = OperationCode.Leave });
                operationResponse = gameClient2.WaitForOperationResponse(WaitTime);
                Assert.AreEqual(
                    OperationCode.Leave, operationResponse.OperationCode);
                Assert.AreEqual(
                    Operations.ErrorCode.Ok,
                    (Operations.ErrorCode)operationResponse.ReturnCode,
                    operationResponse.DebugMessage);


                gameClient1.SendOperationRequest(new OperationRequest { OperationCode = OperationCode.Leave });
                operationResponse = gameClient1.WaitForOperationResponse(WaitTime);
                Assert.AreEqual(
                    OperationCode.Leave, operationResponse.OperationCode);
                Assert.AreEqual(
                    Operations.ErrorCode.Ok,
                    (Operations.ErrorCode)operationResponse.ReturnCode,
                    operationResponse.DebugMessage);



                // app stats: 
                appStatsEvent = masterClient3.WaitForEvent(10000);
                Assert.IsNotNull(appStatsEvent, "AppStatsEvent");
                Assert.AreEqual(EventCode.AppStats, appStatsEvent.Code, "Event Code");
                Assert.AreEqual(1, appStatsEvent.Parameters[ParameterCode.MasterPeerCount], "Peer Count on Master");
                Assert.AreEqual(2, appStatsEvent.Parameters[ParameterCode.PeerCount], "Peer Count on GS");
                Assert.AreEqual(0, appStatsEvent.Parameters[ParameterCode.GameCount], "Game Count");
            }
            finally
            {
                if (masterClient1.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    masterClient1.Close();
                }
                if (masterClient2.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    masterClient2.Close();
                }
                if (masterClient3.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    masterClient3.Close();
                }


                if (gameClient1.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    gameClient1.Close();
                }
                if (gameClient2.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    gameClient2.Close();
                }
            }
        }

        [Test]
        public void BroadcastProperties()
        {
            var masterClient1 = new TestClient(ConnectionProtocol.Tcp);
            var masterClient2 = new TestClient(ConnectionProtocol.Tcp);

            var gameClient1 = new TestClient(ConnectionProtocol.Tcp);
            var gameClient2 = new TestClient(ConnectionProtocol.Tcp);

            try
            {
                masterClient1.Connect(endPointMaster.Address.ToString(), endPointMaster.Port, AppId);
                masterClient1.WaitForConnect(WaitTime);
                
                masterClient2.Connect(endPointMaster.Address.ToString(), endPointMaster.Port, AppId);
                masterClient2.WaitForConnect(WaitTime);

                masterClient1.EventQueue.Clear();
                masterClient2.EventQueue.Clear();

                masterClient1.OperationResponseQueue.Clear();
                masterClient2.OperationResponseQueue.Clear();

                // open game
                string roomName = "BroadcastProperties_" + Guid.NewGuid().ToString().Substring(0, 6);

                var player1Properties = new Hashtable();
                player1Properties.Add("Name", Player1);

                var gameProperties = new Hashtable();
                gameProperties["P1"] = 1;
                gameProperties["P2"] = 2;


                var lobbyProperties = new string[] { "L1", "L2", "L3" };


                Assert.IsTrue(
                    masterClient1.PhotonClient.OpCreateRoom(
                        roomName, true, true, 0, gameProperties, lobbyProperties, player1Properties));

                var operationResponse = masterClient1.WaitForOperationResponse(WaitTime);
                Assert.AreEqual(
                    Operations.OperationCode.CreateGame, (Operations.OperationCode)operationResponse.OperationCode);
                Assert.AreEqual(
                    Operations.ErrorCode.Ok,
                    (Operations.ErrorCode)operationResponse.ReturnCode,
                    operationResponse.DebugMessage);

                var gameServerAddress1 = (string)operationResponse.Parameters[(byte)Operations.ParameterCode.Address];
                Console.WriteLine("Created room " + roomName + " on GS: " + gameServerAddress1);

                // move 1st client to GS: 
                masterClient1.PhotonClient.Disconnect();

                string[] split = gameServerAddress1.Split(':');
                string gameServerIp = split[0];
                int gameServerPort = int.Parse(split[1]);

                gameClient1.Connect(gameServerIp, gameServerPort, AppId);
                gameClient1.WaitForConnect(WaitTime);


                gameClient1.PhotonClient.OpCreateRoom(
                    roomName, true, true, 0, gameProperties, lobbyProperties, player1Properties);
                operationResponse = gameClient1.WaitForOperationResponse(WaitTime);
                Assert.AreEqual(
                    Operations.OperationCode.CreateGame, (Operations.OperationCode)operationResponse.OperationCode);
                Assert.AreEqual(
                    Operations.ErrorCode.Ok,
                    (Operations.ErrorCode)operationResponse.ReturnCode,
                    operationResponse.DebugMessage);

                // move 2nd client to GS: 
                masterClient2.PhotonClient.Disconnect();
                
                gameClient2.Connect(gameServerIp, gameServerPort, AppId);
                gameClient2.WaitForConnect(WaitTime);

                var player2Properties = new Hashtable();
                player2Properties.Add("Name", Player2);

                Assert.IsTrue(gameClient2.PhotonClient.OpJoinRoom(roomName, player2Properties));
                operationResponse = gameClient2.WaitForOperationResponse(WaitTime);
                Assert.AreEqual(
                    Operations.OperationCode.JoinGame, (Operations.OperationCode)operationResponse.OperationCode);
                Assert.AreEqual(
                    Operations.ErrorCode.Ok,
                    (Operations.ErrorCode)operationResponse.ReturnCode,
                    operationResponse.DebugMessage);

                var room = (Hashtable)operationResponse.Parameters[ParameterCode.GameProperties]; 
                Assert.IsNotNull(room);
                Assert.AreEqual(5, room.Count);

                Assert.IsNotNull(room[GameProperties.IsOpen], "IsOpen");
                Assert.IsNotNull(room[GameProperties.IsVisible], "IsVisisble");
                Assert.IsNotNull(room[GameProperties.PropsListedInLobby], "PropertiesInLobby");
                Assert.IsNotNull(room["P1"], "P1");
                Assert.IsNotNull(room["P2"], "P2");


                Assert.AreEqual(true, room[GameProperties.IsOpen], "IsOpen");
                Assert.AreEqual(true, room[GameProperties.IsVisible], "IsVisisble");
                Assert.AreEqual(3, ((string[])room[GameProperties.PropsListedInLobby]).Length, "PropertiesInLobby");
                Assert.AreEqual("L1", ((string[])room[GameProperties.PropsListedInLobby])[0], "PropertiesInLobby");
                Assert.AreEqual("L2", ((string[])room[GameProperties.PropsListedInLobby])[1], "PropertiesInLobby");
                Assert.AreEqual("L3", ((string[])room[GameProperties.PropsListedInLobby])[2], "PropertiesInLobby");
                Assert.AreEqual(1, room["P1"], "P1");
                Assert.AreEqual(2, room["P2"], "P2");

                // set properties: 
                var setProperties = new OperationRequest
                    { OperationCode = OperationCode.SetProperties, Parameters = new Dictionary<byte, object>() };
                
                setProperties.Parameters[ParameterCode.Broadcast] = true; 
                setProperties.Parameters[ParameterCode.Properties] = new Hashtable { { "P3", 3 }, { "P1", null }, { "P2", 20 } };

                gameClient1.SendOperationRequest(setProperties); 
                operationResponse = gameClient1.WaitForOperationResponse(WaitTime);
                Assert.AreEqual(OperationCode.SetProperties, operationResponse.OperationCode);
                Assert.AreEqual(ErrorCode.Ok, operationResponse.ReturnCode, operationResponse.DebugMessage);
                
                var ev = gameClient2.WaitForEvent(EventCode.PropertiesChanged, WaitTime);
                
                room = (Hashtable)ev.Parameters[ParameterCode.Properties]; 
                Assert.IsNotNull(room);
                Assert.AreEqual(3, room.Count);

                Assert.IsNull(room["P1"], "P1");
                Assert.IsNotNull(room["P2"], "P2");
                Assert.IsNotNull(room["P3"], "P3");

                Assert.AreEqual(null, room["P1"], "P1");
                Assert.AreEqual(20, room["P2"], "P2");
                Assert.AreEqual(3, room["P3"], "P3");

                var getProperties = new OperationRequest { OperationCode = OperationCode.GetProperties, Parameters = new Dictionary<byte, object>()};
                getProperties.Parameters[ParameterCode.Properties] = PropertyType.Game; 
                
                gameClient2.SendOperationRequest(getProperties);
                operationResponse = gameClient2.WaitForOperationResponse(WaitTime);
                
                Assert.AreEqual(OperationCode.GetProperties, operationResponse.OperationCode);
                Assert.AreEqual(ErrorCode.Ok, operationResponse.ReturnCode, operationResponse.DebugMessage);

                room = (Hashtable)operationResponse.Parameters[ParameterCode.GameProperties];
                Assert.IsNotNull(room);
                Assert.AreEqual(6, room.Count);

                Assert.IsNotNull(room[GameProperties.IsOpen], "IsOpen");
                Assert.IsNotNull(room[GameProperties.IsVisible], "IsVisisble");
                Assert.IsNotNull(room[GameProperties.PropsListedInLobby], "PropertiesInLobby");
                Assert.IsNull(room["P1"], "P1");
                Assert.IsNotNull(room["P2"], "P2");
                Assert.IsNotNull(room["P3"], "P3");


                Assert.AreEqual(true, room[GameProperties.IsOpen], "IsOpen");
                Assert.AreEqual(true, room[GameProperties.IsVisible], "IsVisisble");
                Assert.AreEqual(3, ((string[])room[GameProperties.PropsListedInLobby]).Length, "PropertiesInLobby");
                Assert.AreEqual("L1", ((string[])room[GameProperties.PropsListedInLobby])[0], "PropertiesInLobby");
                Assert.AreEqual("L2", ((string[])room[GameProperties.PropsListedInLobby])[1], "PropertiesInLobby");
                Assert.AreEqual("L3", ((string[])room[GameProperties.PropsListedInLobby])[2], "PropertiesInLobby");
                Assert.AreEqual(null, room["P1"], "P1");
                Assert.AreEqual(20, room["P2"], "P2");
                Assert.AreEqual(3, room["P3"], "P3");


              
            }
            finally
            {
                if (masterClient1.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    masterClient1.Close();
                }

                if (masterClient2.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    masterClient2.Close();
                }

                if (gameClient1.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    gameClient1.Close();
                }

                if (gameClient2.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    gameClient2.Close();
                }
            }
        }

        [Test]
        public void SetPropertiesForLobby()
        {

            var masterClient1 = new TestClient(ConnectionProtocol.Tcp);
            var masterClient2 = new TestClient(ConnectionProtocol.Tcp);

            var gameClient1 = new TestClient(ConnectionProtocol.Tcp);
            var gameClient2 = new TestClient(ConnectionProtocol.Tcp);

            try
            {
                masterClient1.Connect(endPointMaster.Address.ToString(), endPointMaster.Port, AppId);
                masterClient1.WaitForConnect(WaitTime);

                Assert.IsTrue(masterClient1.PhotonClient.OpJoinLobby());
                var ev = masterClient1.WaitForEvent(EventCode.GameList, WaitTime);
                Assert.AreEqual(EventCode.GameList, ev.Code);
                var gameList = (Hashtable)ev.Parameters[ParameterCode.GameList];
                CheckGameListCount(0, gameList);

                masterClient2.Connect(endPointMaster.Address.ToString(), endPointMaster.Port, AppId);
                masterClient2.WaitForConnect(WaitTime);

                Assert.IsTrue(masterClient2.PhotonClient.OpJoinLobby());
                ev = masterClient2.WaitForEvent(EventCode.GameList, WaitTime);
                Assert.AreEqual(EventCode.GameList, ev.Code);
                gameList = (Hashtable)ev.Parameters[ParameterCode.GameList];
                CheckGameListCount(0, gameList);

                masterClient1.EventQueue.Clear();
                masterClient2.EventQueue.Clear();

                masterClient1.OperationResponseQueue.Clear();
                masterClient2.OperationResponseQueue.Clear();

                // open game
                string roomName = "SetPropertiesForLobby_" + Guid.NewGuid().ToString().Substring(0, 6);

                var player1Properties = new Hashtable();
                player1Properties.Add("Name", Player1);

                var gameProperties = new Hashtable();
                gameProperties["P1"] = 1;
                gameProperties["P2"] = 2;

                gameProperties["L1"] = 1;
                gameProperties["L2"] = 2;


                var lobbyProperties = new string[] { "L1", "L2", "L3" };

                Assert.IsTrue(
                    masterClient1.PhotonClient.OpCreateRoom(
                        roomName, true, true, 0, gameProperties, lobbyProperties, player1Properties));

                var operationResponse = masterClient1.WaitForOperationResponse(WaitTime);
                Assert.AreEqual(
                    Operations.OperationCode.CreateGame, (Operations.OperationCode)operationResponse.OperationCode);
                Assert.AreEqual(
                    Operations.ErrorCode.Ok,
                    (Operations.ErrorCode)operationResponse.ReturnCode,
                    operationResponse.DebugMessage);

                var gameServerAddress1 = (string)operationResponse.Parameters[(byte)Operations.ParameterCode.Address];
                Console.WriteLine("Created room " + roomName + " on GS: " + gameServerAddress1);

                // move 1st client to GS: 
                masterClient1.PhotonClient.Disconnect();

                string[] split = gameServerAddress1.Split(':');
                string gameServerIp = split[0];
                int gameServerPort = int.Parse(split[1]);

                gameClient1.Connect(gameServerIp, gameServerPort, AppId);
                gameClient1.WaitForConnect(WaitTime);


                gameClient1.PhotonClient.OpCreateRoom(
                    roomName, true, true, 0, gameProperties, lobbyProperties, player1Properties);
                operationResponse = gameClient1.WaitForOperationResponse(WaitTime);
                Assert.AreEqual(
                    Operations.OperationCode.CreateGame, (Operations.OperationCode)operationResponse.OperationCode);
                Assert.AreEqual(
                    Operations.ErrorCode.Ok,
                    (Operations.ErrorCode)operationResponse.ReturnCode,
                    operationResponse.DebugMessage);

                // get own join event: 
                ev = gameClient1.WaitForEvent(WaitTime);
                Assert.AreEqual(EventCode.Join, ev.Code);
                Assert.AreEqual(1, ev.Parameters[ParameterCode.ActorNr]);
                
                var actorList = (int[])ev.Parameters[ParameterCode.ActorList];
                Assert.AreEqual(1, actorList.Length); 
                Assert.AreEqual(1, actorList[0]);

                var playerProperties = ((Hashtable)ev.Parameters[ParameterCode.PlayerProperties]);
                Assert.AreEqual(Player1, playerProperties["Name"]);
                
                ev = masterClient2.WaitForEvent(EventCode.GameListUpdate, WaitTime);

                var roomList = (Hashtable)ev.Parameters[ParameterCode.GameList];
                Assert.GreaterOrEqual(roomList.Count, 1);

                var room = (Hashtable)roomList[roomName];
                Assert.IsNotNull(room);
                Assert.AreEqual(5, room.Count);

                Assert.IsNotNull(room[GameProperties.IsOpen], "IsOpen");
                Assert.IsNotNull(room[GameProperties.MaxPlayers], "MaxPlayers");
                Assert.IsNotNull(room[GameProperties.PlayerCount], "PlayerCount");
                Assert.IsNotNull(room["L1"], "L1");
                Assert.IsNotNull(room["L2"], "L2");
                

                Assert.AreEqual(true, room[GameProperties.IsOpen], "IsOpen");
                Assert.AreEqual(0, room[GameProperties.MaxPlayers], "MaxPlayers");
                Assert.AreEqual(1, room[GameProperties.PlayerCount], "PlayerCount");
                Assert.AreEqual(1, room["L1"], "L1");
                Assert.AreEqual(2, room["L2"], "L2");

                Assert.IsTrue(gameClient1.PhotonClient.OpSetCustomPropertiesOfRoom(new Hashtable { {"L3", 3 }, {"L1", null}, {"L2", 20 }}));
                operationResponse = gameClient1.WaitForOperationResponse(WaitTime); 
                Assert.AreEqual(OperationCode.SetProperties, operationResponse.OperationCode);
                Assert.AreEqual(ErrorCode.Ok, operationResponse.ReturnCode);


                ev = masterClient2.WaitForEvent(EventCode.GameListUpdate, WaitTime);

                roomList = (Hashtable)ev.Parameters[ParameterCode.GameList];
                Assert.AreEqual(1, roomList.Count);

                room = (Hashtable)roomList[roomName];
                Assert.IsNotNull(room);
                Assert.AreEqual(5, room.Count);

                Assert.IsNotNull(room[GameProperties.IsOpen], "IsOpen");
                Assert.IsNotNull(room[GameProperties.MaxPlayers], "MaxPlayers");
                Assert.IsNotNull(room[GameProperties.PlayerCount], "PlayerCount");
                Assert.IsNotNull(room["L2"], "L2");
                Assert.IsNotNull(room["L3"], "L3");

                Assert.AreEqual(true, room[GameProperties.IsOpen], "IsOpen");
                Assert.AreEqual(0, room[GameProperties.MaxPlayers], "MaxPlayers");
                Assert.AreEqual(1, room[GameProperties.PlayerCount], "PlayerCount");
                Assert.AreEqual(20, room["L2"], "L2");
                Assert.AreEqual(3, room["L3"], "L3");

                gameClient1.SendOperationRequest(new OperationRequest { OperationCode = OperationCode.Leave });
                gameClient2.SendOperationRequest(new OperationRequest { OperationCode = OperationCode.Leave }); 
            }
            finally
            {
                if (masterClient1.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    masterClient1.Close();
                }

                if (gameClient1.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    gameClient1.Close();
                }

                if (masterClient2.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    masterClient2.Close();
                }

                if (gameClient2.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    gameClient2.Close();
                }
            }
        }
       
        [Test]
        public void SuppressRoomEvents()
        {
            NunitClient client1 = null;
            NunitClient client2 = null;

            try
            {
                var roomName = GenerateRandomizedRoomName("SuppressRoomEvents_");
                client1 = CreateMasterClientAndAuthenticate(string.Empty);
                var createGameResponse = client1.CreateGame(roomName, true, true, 4, Operations.ErrorCode.Ok);
                client1.Dispose();

                client1 = new NunitClient(Protocol, AppId, WaitTime);
                client1.ConnectAndAuthenticate(createGameResponse.Address, string.Empty);

                var createRequest = new OperationRequest { OperationCode = OperationCode.CreateGame, Parameters = new Dictionary<byte, object>() };
                createRequest.Parameters.Add(ParameterCode.RoomName, createGameResponse.GameId);
                createRequest.Parameters.Add((byte)Operations.ParameterCode.SuppressRoomEvents, true);
                client1.SendRequestAndWaitForResponse(createRequest, ErrorCode.Ok);

                client2 = new NunitClient(Protocol, AppId, WaitTime);
                client2.ConnectAndAuthenticate(createGameResponse.Address, string.Empty);
                client2.JoinGame(roomName, Operations.ErrorCode.Ok);

                EventData eventData;
                Assert.IsFalse(client1.TryWaitForEvent(EventCode.Join, WaitTime, out eventData));

                client1.Dispose();
                Assert.IsFalse(client2.TryWaitForEvent(EventCode.Leave, WaitTime, out eventData));

            }
            finally
            {
                DisposeClients(client1, client2);
            }
        }

        [Test]
        public void MatchByProperties()
        {

            NunitClient masterClient = null;
            NunitClient gameClient = null;

            try
            {
                // create game on the game server
                string roomName = GenerateRandomizedRoomName("BroadcastProperties_");

                var gameProperties = new Hashtable();
                gameProperties["P1"] = 1;
                gameProperties["P2"] = 2;
                gameProperties["L1"] = 1;
                gameProperties["L2"] = 2;
                gameProperties["L3"] = 3;

                var lobbyProperties = new string[] { "L1", "L2", "L3" };

                gameClient = CreateGameOnGameServer(null, roomName, null, 0, true, true, 0, gameProperties, lobbyProperties);

                // test matchmaking
                masterClient = CreateMasterClientAndAuthenticate(null);
                masterClient.EventQueue.Clear();
                masterClient.OperationResponseQueue.Clear();

                var joinRequest = new JoinRandomGameRequest 
                { 
                    JoinRandomType = (byte)MatchmakingMode.FillRoom, 
                    GameProperties = new Hashtable()
                };
                
              
                joinRequest.GameProperties.Add("N", null);
                masterClient.OperationResponseQueue.Clear();
                masterClient.JoinRandomGame(joinRequest, (short)Operations.ErrorCode.NoMatchFound);

                joinRequest.GameProperties.Clear();
                joinRequest.GameProperties.Add("L1", 5);
                masterClient.OperationResponseQueue.Clear();
                masterClient.JoinRandomGame(joinRequest, (short)Operations.ErrorCode.NoMatchFound);

                joinRequest.GameProperties.Clear();
                joinRequest.GameProperties.Add("L1", 1);
                joinRequest.GameProperties.Add("L2", 1);
                masterClient.OperationResponseQueue.Clear();
                masterClient.JoinRandomGame(joinRequest, (short)Operations.ErrorCode.NoMatchFound);

                joinRequest.GameProperties.Clear();
                joinRequest.GameProperties.Add("L1", 1);
                joinRequest.GameProperties.Add("L2", 2);
                masterClient.OperationResponseQueue.Clear();
                masterClient.JoinRandomGame(joinRequest, (short)Operations.ErrorCode.Ok);

                gameClient.LeaveGame();
            }
            finally
            {
                DisposeClients(masterClient, gameClient);
            }
        }

        [Test]
        public void MatchmakingTypes()
        {
            NunitClient masterClient = null;
            var gameClients = new NunitClient[3];
            var roomNames = new string[3];
            try
            {
                // create games on game server
                for (int i = 0; i < gameClients.Length; i++)
                {
                    roomNames[i] = GenerateRandomizedRoomName("MatchmakingTypes_" + i + "_");
                    var createGameRequest = new CreateGameRequest { GameId = roomNames[i] };
                    gameClients[i] = CreateGameOnGameServer(null, createGameRequest);
                }

                // fill room - 3x: 
                masterClient = CreateMasterClientAndAuthenticate(null);
                var joinRandomRequest = new JoinRandomGameRequest { JoinRandomType = (byte)MatchmakingMode.FillRoom };
                
                masterClient.JoinRandomGame(joinRandomRequest, ErrorCode.Ok, roomNames[0]);
                masterClient.JoinRandomGame(joinRandomRequest, ErrorCode.Ok, roomNames[0]);
                masterClient.JoinRandomGame(joinRandomRequest, ErrorCode.Ok, roomNames[0]);


                // serial matching - 4x: 
                joinRandomRequest = new JoinRandomGameRequest { JoinRandomType = (byte)MatchmakingMode.SerialMatching };
                masterClient.JoinRandomGame(joinRandomRequest, ErrorCode.Ok, roomNames[1]);
                masterClient.JoinRandomGame(joinRandomRequest, ErrorCode.Ok, roomNames[2]);
                masterClient.JoinRandomGame(joinRandomRequest, ErrorCode.Ok, roomNames[0]);
                masterClient.JoinRandomGame(joinRandomRequest, ErrorCode.Ok, roomNames[1]);

                for (int i = 0; i < gameClients.Length; i++)
                {
                    gameClients[i].LeaveGame();
                }
            }
            finally
            {
                DisposeClients(masterClient);
                DisposeClients(gameClients);
            }
        }

        [Test]
        public void FiendFriends()
        {
            var userIds = new string[] { "User1", "User2", "User3" };

            NunitClient masterClient1 = null;
            NunitClient masterClient2 = null;
            NunitClient masterClient3 = null;
            NunitClient gameClient3 = null;

            try
            {

                bool[] onlineStates;
                string[] userStates;

                // connect first client 
                masterClient1 = CreateMasterClientAndAuthenticate(userIds[0]);
                masterClient1.FindFriends(userIds, out onlineStates, out userStates);
                Assert.AreEqual(true, onlineStates[0]);
                Assert.AreEqual(false, onlineStates[1]);
                Assert.AreEqual(false, onlineStates[2]);
                Assert.AreEqual(string.Empty, userStates[0]);
                Assert.AreEqual(string.Empty, userStates[1]);
                Assert.AreEqual(string.Empty, userStates[2]);

                // connect second client 
                masterClient2 = CreateMasterClientAndAuthenticate(userIds[1]);
                masterClient1.FindFriends(userIds, out onlineStates, out userStates);
                Assert.AreEqual(true, onlineStates[0]);
                Assert.AreEqual(true, onlineStates[1]);
                Assert.AreEqual(false, onlineStates[2]);
                Assert.AreEqual(string.Empty, userStates[0]);
                Assert.AreEqual(string.Empty, userStates[1]);
                Assert.AreEqual(string.Empty, userStates[2]);

                // connect third client and create game on game server
                masterClient3 = CreateMasterClientAndAuthenticate(userIds[2]);
                var response = masterClient3.CreateGame("FiendFriendsGame1", Operations.ErrorCode.Ok);
                masterClient3.Dispose();

                gameClient3 = new NunitClient(Protocol, AppId, WaitTime);
                gameClient3.ConnectAndAuthenticate(response.Address, userIds[2]);
                gameClient3.CreateGame("FiendFriendsGame1", Operations.ErrorCode.Ok);

                masterClient1.FindFriends(userIds, out onlineStates, out userStates);
                Assert.AreEqual(true, onlineStates[0]);
                Assert.AreEqual(true, onlineStates[1]);
                Assert.AreEqual(true, onlineStates[2]);
                Assert.AreEqual(string.Empty, userStates[0]);
                Assert.AreEqual(string.Empty, userStates[1]);
                Assert.AreEqual("FiendFriendsGame1", userStates[2]);
                masterClient1.EventQueue.Clear();

                // disconnect client2 and client3
                gameClient3.Dispose();
                masterClient2.Dispose();

                // wait some time until disconnect of client 3 was reported to game server
                Thread.Sleep(200);

                masterClient1.FindFriends(userIds, out onlineStates, out userStates);
                Assert.AreEqual(true, onlineStates[0]);
                Assert.AreEqual(false, onlineStates[1], "MasterClient2 disconencted, but not shown as offlien");
                Assert.AreEqual(false, onlineStates[2], "GameClient3 disconnected, but was not published to master in time");
                Assert.AreEqual(string.Empty, userStates[0]);
                Assert.AreEqual(string.Empty, userStates[1]);
                Assert.AreEqual(string.Empty, userStates[2]);

            }
            finally
            {
                DisposeClients(masterClient1, masterClient2, masterClient3, gameClient3);
            }
        }

        [Test]
        public void SqlLobbyMatchmaking()
        {
            NunitClient masterClient = null;
            NunitClient[] gameClients = null;

            try
            {
                const string lobbyName = "SqlLobby1";
                const byte lobbyType = 2;

                gameClients = new NunitClient[3];

                for (int i = 0; i < gameClients.Length; i++)
                {
                    var gameProperties = new Hashtable();
                    switch(i)
                    {
                        case 1:
                            gameProperties.Add("C0", 10);
                            break;

                        case 2:
                            gameProperties.Add("C0", "Map1");
                            break;
                    }

                    var roomName = "SqlLobbyMatchmaking" + i;         
                    gameClients[i] = CreateGameOnGameServer(null, roomName, lobbyName, lobbyType, true, true, 0, gameProperties, null);
                }


                masterClient = new NunitClient(Protocol, AppId, WaitTime);
                masterClient = CreateMasterClientAndAuthenticate("Tester");

                // client didn't joined lobby so all requests without 
                // a lobby specified should not return a match
                masterClient.JoinRandomGame(null, null, ErrorCode.NoRandomMatchFound);
                masterClient.JoinRandomGame(null, "C0=10", ErrorCode.NoRandomMatchFound);

                // specifing the lobbyname and type should give some matches
                masterClient.JoinLobby(lobbyName, lobbyType);
                masterClient.JoinRandomGame(null, null, ErrorCode.Ok, lobbyName, lobbyType);
                masterClient.JoinRandomGame(null, "C0=1", ErrorCode.NoRandomMatchFound, lobbyName, lobbyType);
                masterClient.JoinRandomGame(null, "C0<10", ErrorCode.NoRandomMatchFound, lobbyName, lobbyType);
                masterClient.JoinRandomGame(null, "C0>10", ErrorCode.Ok, lobbyName, lobbyType, "SqlLobbyMatchmaking2");
                masterClient.JoinRandomGame(null, "C0=10", ErrorCode.Ok, lobbyName, lobbyType, "SqlLobbyMatchmaking1");
                masterClient.JoinRandomGame(null, "C0>0", ErrorCode.Ok, lobbyName, lobbyType, "SqlLobbyMatchmaking1");
                masterClient.JoinRandomGame(null, "C0<20", ErrorCode.Ok, lobbyName, lobbyType, "SqlLobbyMatchmaking1");
                masterClient.JoinRandomGame(null, "C0='Map2'", ErrorCode.NoRandomMatchFound, lobbyName, lobbyType);
                masterClient.JoinRandomGame(null, "C0='Map1'", ErrorCode.Ok, lobbyName, lobbyType, "SqlLobbyMatchmaking2");

                // join client to lobby. Matches could be found without 
                // specifing the lobby
                masterClient.JoinLobby(lobbyName, lobbyType);
                masterClient.JoinRandomGame(null, null, ErrorCode.Ok);
                masterClient.JoinRandomGame(null, "C0=1", ErrorCode.NoRandomMatchFound);
                masterClient.JoinRandomGame(null, "C0<10", ErrorCode.NoRandomMatchFound);
                masterClient.JoinRandomGame(null, "C0>10", ErrorCode.Ok, null, null, "SqlLobbyMatchmaking2");
                masterClient.JoinRandomGame(null, "C0=10", ErrorCode.Ok);
                masterClient.JoinRandomGame(null, "C0>0", ErrorCode.Ok, null, null, "SqlLobbyMatchmaking1");
                masterClient.JoinRandomGame(null, "C0<20", ErrorCode.Ok, null, null, "SqlLobbyMatchmaking1");
                masterClient.JoinRandomGame(null, "C0='Map2'", ErrorCode.NoRandomMatchFound);
                masterClient.JoinRandomGame(null, "C0='Map1'", ErrorCode.Ok, null, null, "SqlLobbyMatchmaking2");

                // invalid sql should return error
                var joinResponse = masterClient.JoinRandomGame(null, "GRTF", ErrorCode.InvalidOperationCode);
                Assert.AreEqual(ErrorCode.InvalidOperationCode, joinResponse.ReturnCode);
            }
            finally
            {
                DisposeClients(masterClient);
                DisposeClients(gameClients);
            }
        }

        [Test]
        public void SqlLobbyMaxPlayersNoFilter()
        {
            NunitClient masterClient = null;
            NunitClient gameClient1 = null;
            NunitClient gameClient2 = null;

            const string lobbyName = "SqlLobbyMaxPlayers";
            const byte lobbyType = 2;

            try
            {
                string roomName = GenerateRandomizedRoomName("SqlLobbyMaxPlayers_");
                gameClient1 = CreateGameOnGameServer(null, roomName, lobbyName, lobbyType, true, true, 1, null, null);

                // join 2nd client on master - full: 
                masterClient = CreateMasterClientAndAuthenticate("Tester");

                masterClient.JoinRandomGame(null, null, ErrorCode.NoRandomMatchFound);
                masterClient.JoinRandomGame(null, "C0=10", ErrorCode.NoRandomMatchFound);

                // specifing the lobbyname and type should give some matches
                masterClient.JoinLobby(lobbyName, lobbyType);
                masterClient.JoinGame(roomName, Operations.ErrorCode.GameFull);

                // join random 2nd client on master - full: 
                var joinRequest = new JoinRandomGameRequest();
                masterClient.JoinRandomGame(joinRequest, (short)Operations.ErrorCode.NoMatchFound);
                joinRequest.JoinRandomType = (byte)MatchmakingMode.SerialMatching;
                masterClient.JoinRandomGame(joinRequest, (short)Operations.ErrorCode.NoMatchFound);
                joinRequest.JoinRandomType = (byte)MatchmakingMode.RandomMatching;
                masterClient.JoinRandomGame(joinRequest, (short)Operations.ErrorCode.NoMatchFound);
                masterClient.Dispose();

                // join directly on GS: 
                gameClient2 = new NunitClient(Protocol, AppId, WaitTime);
                gameClient2.ConnectAndAuthenticate(gameClient1.RemoteEndPoint, null);
                gameClient2.JoinGame(roomName, Operations.ErrorCode.GameFull);
            }
            finally
            {
                DisposeClients(masterClient, gameClient1, gameClient2);
            }
        }

        [Test]
        public void SqlLobbyMaxPlayersWithFilter()
        {
            NunitClient masterClient1 = null;
            NunitClient masterClient2 = null;
            
            NunitClient gameClient1 = null;
            NunitClient gameClient2 = null;
            NunitClient gameClient3 = null;

            const string lobbyName = "SqlLobbyMaxPlayers";
            const byte lobbyType = 2;

            try
            {
                string roomName = GenerateRandomizedRoomName("SqlLobbyMaxPlayers_");
                var gameProperties = new Hashtable();
                gameProperties["C0"] = 10;
                gameProperties["C5"] = "Name";

                gameClient1 = CreateGameOnGameServer(null, roomName, lobbyName, lobbyType, true, true, 2, gameProperties, null);

                masterClient1 = CreateMasterClientAndAuthenticate("Tester1");
                masterClient2 = CreateMasterClientAndAuthenticate("Tester2");

                // join 2nd client on master - no matches without lobby:
                masterClient1.JoinRandomGame(null, null, ErrorCode.NoRandomMatchFound);
                masterClient1.JoinRandomGame(null, "C0=10", ErrorCode.NoRandomMatchFound);

                // specifing the lobbyname and type should give some matches
                masterClient1.JoinLobby(lobbyName, lobbyType);
                masterClient2.JoinLobby(lobbyName, lobbyType);
                
                
                // join random - with filter:
                var joinRequest = new JoinRandomGameRequest();
                joinRequest.QueryData = "C0=10";
                masterClient1.JoinRandomGame(joinRequest, (short)Operations.ErrorCode.Ok);
                masterClient2.JoinRandomGame(joinRequest, (short)Operations.ErrorCode.NoMatchFound);
                
                
                // join directly on GS: 
                gameClient2 = new NunitClient(Protocol, AppId, WaitTime);
                gameClient2.ConnectAndAuthenticate(gameClient1.RemoteEndPoint, "Tester1");
                gameClient2.JoinGame(roomName, Operations.ErrorCode.Ok);

                gameClient3 = new NunitClient(Protocol, AppId, WaitTime);
                gameClient3.ConnectAndAuthenticate(gameClient1.RemoteEndPoint, null);
                gameClient3.JoinGame(roomName, Operations.ErrorCode.GameFull);

                // disconnect second client
                gameClient2.LeaveGame();
                gameClient2.Dispose();
                Thread.Sleep(500); // give the app lobby some time to update the game state
                masterClient2.JoinRandomGame(joinRequest, (short)Operations.ErrorCode.Ok);

            }
            finally
            {
                DisposeClients(masterClient1, masterClient2, gameClient1, gameClient2, gameClient3);
            }
        }

        [Test]
        [Explicit("Very long running test")]
        public void SqlLobbyMaxPlayersWithFilterJoinTimeout()
        {
            NunitClient masterClient1 = null;
            NunitClient masterClient2 = null;

            NunitClient gameClient1 = null;

            const string lobbyName = "SqlLobbyMaxPlayers";
            const byte lobbyType = 2;

            try
            {
                string roomName = GenerateRandomizedRoomName("SqlLobbyMaxPlayers_");
                var gameProperties = new Hashtable();
                gameProperties["C0"] = 10;
                gameProperties["C5"] = "Name";

                gameClient1 = CreateGameOnGameServer(null, roomName, lobbyName, lobbyType, true, true, 2, gameProperties, null);

                var joinRequest = new JoinRandomGameRequest();
                joinRequest.QueryData = "C0=10";

                // join first client
                masterClient1 = CreateMasterClientAndAuthenticate("Tester1");
                masterClient1.JoinLobby(lobbyName, lobbyType);
                masterClient1.JoinRandomGame(joinRequest, (short)Operations.ErrorCode.Ok);
                
                // join second client
                // should fail because first client is still connecting to the game server
                masterClient2 = CreateMasterClientAndAuthenticate("Tester2");
                masterClient2.JoinLobby(lobbyName, lobbyType);
                masterClient2.JoinRandomGame(joinRequest, (short)Operations.ErrorCode.NoMatchFound);
                masterClient2.Dispose();

                // wait for join timeout (default is currently 15 seconds)
                Thread.Sleep(30000);

                // join second client
                // should work because first client has timed out connecting to the game server
                masterClient2 = CreateMasterClientAndAuthenticate("Tester2");
                masterClient2.JoinLobby(lobbyName, lobbyType);
                masterClient2.JoinRandomGame(joinRequest, (short)Operations.ErrorCode.Ok);
                masterClient2.Dispose();
            }
            finally
            {
                DisposeClients(masterClient1, masterClient2, gameClient1);
            }
        }

        [Test]
        [Explicit("EmptyRoomLiveTime property of game server settings must be set to 1000 for this test to run")]
        public void EmptyRoomLiveTime()
        {
            NunitClient gameClient = null;

            try
            {
                string gameId = GenerateRandomizedRoomName("EmptyRoomLiveTime");

                var createGameRequest = new CreateGameRequest();
                createGameRequest.GameId = gameId;

                gameClient = CreateGameOnGameServer(null, createGameRequest);
                gameClient.LeaveGame();

                // Rejoin the game. The game should be still in the room cache
                gameClient.JoinGame(gameId, Operations.ErrorCode.Ok);
                gameClient.LeaveGame();

                Thread.Sleep(1200);
                // Rejoin the game. The game should not be in the room cache anymore
                gameClient.JoinGame(gameId, Operations.ErrorCode.GameIdNotExists);
            }
            finally
            {
                DisposeClients(gameClient);
            }
        }

        ////[Test]
        ////public void Test()
        ////{
        ////    NunitClient client1 = null;

        ////    try
        ////    {
        ////        var roomName = GenerateRandomizedRoomName("Test_");
        ////        client1 = CreateMasterClientAndAuthenticate(string.Empty);
        ////        var createGameResponse = client1.CreateGame(roomName, true, true, 4, Operations.ErrorCode.Ok);
        ////        client1.Dispose();

        ////        client1 = new NunitClient(Protocol, AppId, WaitTime);
        ////        client1.ConnectAndAuthenticate(createGameResponse.Address, string.Empty);

        ////        var createRequest = new OperationRequest { OperationCode = OperationCode.CreateGame, Parameters = new Dictionary<byte, object>() };
        ////        createRequest.Parameters.Add(ParameterCode.RoomName, createGameResponse.GameId);
        ////        client1.PhotonClient.OpCreateRoom(roomName, true, true, 4, null, null, null);
        ////        client1.PhotonClient.SendOutgoingCommands();
        ////        client1.Dispose();
        ////    }
        ////    finally
        ////    {
        ////        DisposeClients(client1);
        ////    }
        ////}
        #endregion

        #region Methods

        private static void CreateRoomOnGameServer(
            TestClient masterClient,
            string roomName,
            out string gameServerIp,
            out int gameServerPort,
            out TestClient gameClient)
        {
            CreateRoomOnGameServer(masterClient, true, true, 0, roomName, out gameServerIp, out gameServerPort, out gameClient);
        }

        private static void CreateRoomOnGameServer(
            TestClient masterClient,
            bool isVisible,
            bool isOpen,
            byte maxPlayers,
            string roomName,
            out string gameServerIp,
            out int gameServerPort,
            out TestClient gameClient)
        {

            gameClient = new TestClient(ConnectionProtocol.Tcp);

            // create
            Assert.IsTrue(
                masterClient.PhotonClient.OpCreateRoom(
                    roomName, isVisible, isOpen, maxPlayers, new Hashtable(), new string[0], new Hashtable()));
            var operationResponse = masterClient.WaitForOperationResponse(WaitTime);
            Assert.AreEqual(
                Operations.OperationCode.CreateGame, (Operations.OperationCode)operationResponse.OperationCode);
            Assert.AreEqual(
                Operations.ErrorCode.Ok,
                (Operations.ErrorCode)operationResponse.ReturnCode,
                operationResponse.DebugMessage);

            var gameServerAddress1 = (string)operationResponse.Parameters[(byte)Operations.ParameterCode.Address];
            Console.WriteLine("Created room " + roomName + " on GS: " + gameServerAddress1);

            // move 1st client to GS: 
            masterClient.PhotonClient.Disconnect();

            string[] split = gameServerAddress1.Split(':');
            gameServerIp = split[0];
            gameServerPort = int.Parse(split[1]);

            gameClient.Connect(gameServerIp, gameServerPort, AppId);
            gameClient.WaitForConnect(WaitTime);

            var player1Properties = new Hashtable();
            player1Properties.Add("Name", Player1);

            gameClient.PhotonClient.OpCreateRoom(
                roomName, isVisible, isOpen, maxPlayers, new Hashtable(), new string[0], player1Properties);
            operationResponse = gameClient.WaitForOperationResponse(WaitTime);
            Assert.AreEqual(
                Operations.OperationCode.CreateGame, (Operations.OperationCode)operationResponse.OperationCode);
            Assert.AreEqual(
                Operations.ErrorCode.Ok,
                (Operations.ErrorCode)operationResponse.ReturnCode,
                operationResponse.DebugMessage);

            // get own join event: 
            var ev = gameClient.WaitForEvent(WaitTime);
            Assert.AreEqual(EventCode.Join, ev.Code);
            Assert.AreEqual(1, ev.Parameters[ParameterCode.ActorNr]);
            var playerProperties = ((Hashtable)ev.Parameters[ParameterCode.PlayerProperties]);
            Assert.AreEqual(Player1, playerProperties["Name"]);
        }

        /// <summary>
        /// Creates a test client and connects to the master server.
        /// If a userId is specified an authentication requests will 
        /// be send after connection completed.
        /// </summary>
        private static NunitClient CreateMasterClientAndAuthenticate(string userId)
        {
            var client = new NunitClient(Protocol, AppId, WaitTime);
            client.ConnectAndAuthenticate(endPointMaster, userId);
            return client;
        }

        /// <summary>
        /// Helper function to dispose a list of test clients
        /// </summary>
        private static void DisposeClients(params NunitClient[] clients)
        {
            if (clients != null)
            {
                for (int i = 0; i < clients.Length; i++)
                {
                    if (clients[i] != null)
                    {
                        clients[i].Dispose();
                    }
                }
            }
        }

        private static NunitClient CreateGameOnGameServer(
            string userName, 
            string roomName, 
            string lobbyName, 
            byte lobbyType, 
            bool? isVisible, 
            bool? isOpen,
            byte? maxPlayer, 
            Hashtable gameProperties, 
            string[] lobbyProperties)
        {
                var createRequest = new CreateGameRequest 
                {
                    GameId = roomName,
                    GameProperties = gameProperties,   
                    LobbyName = lobbyName,
                    LobbyType = lobbyType
                };

                if (createRequest.GameProperties == null)
                {
                    createRequest.GameProperties = new Hashtable();
                }

                if (isVisible.HasValue)
                {
                    createRequest.GameProperties[(byte)GameParameter.IsVisible] = isVisible.Value;
                }

                if (isOpen.HasValue)
                {
                    createRequest.GameProperties[(byte)GameParameter.IsOpen] = isOpen.Value;
                }

                if (maxPlayer.HasValue)
                {
                    createRequest.GameProperties[(byte)GameParameter.MaxPlayer] = maxPlayer.Value;
                }

                if (lobbyProperties != null)
                {
                    createRequest.GameProperties[(byte)GameParameter.Properties] = lobbyProperties;
                }

                return CreateGameOnGameServer(userName, createRequest);
        }
       
        private static NunitClient CreateGameOnGameServer(string userName, CreateGameRequest createRequest)
        {
            NunitClient client = null;
            bool gameCreated = false;

            try
            {
                client = CreateMasterClientAndAuthenticate(userName);
                var response = client.CreateGame(createRequest, Operations.ErrorCode.Ok);
                client.Dispose();

                client = new NunitClient(Protocol, AppId, WaitTime);
                client.ConnectAndAuthenticate(response.Address, userName);
                client.CreateGame(createRequest, Operations.ErrorCode.Ok);
                gameCreated = true;
            }
            finally
            {
                if (!gameCreated)
                {
                    DisposeClients(client);
                }
            }

            return client;
        }

        private static string GenerateRandomizedRoomName(string roomName)
        {
            return roomName + Guid.NewGuid().ToString().Substring(0, 6);
        }

        [Test, Explicit]
        public void DebugHangingGame()
        {
            var masterClient1 = new TestClient(ConnectionProtocol.Tcp);
            var gameClient1 = new TestClient(ConnectionProtocol.Tcp);

            try
            {
                masterClient1.Connect(endPointMaster.Address.ToString(), endPointMaster.Port, AppId);
                masterClient1.WaitForConnect(WaitTime);

                Assert.IsTrue(masterClient1.PhotonClient.OpJoinLobby());
                var ev = masterClient1.WaitForEvent(EventCode.GameList, WaitTime);
                Assert.AreEqual(EventCode.GameList, ev.Code);
                var gameList = (Hashtable)ev.Parameters[ParameterCode.GameList];
                Assert.GreaterOrEqual(gameList.Count, 1);
                
                // Console.WriteLine("GameList event: " + ev.ToStringFull());

                foreach (var roomName in gameList.Keys)
                {
                    var gameInfo = (Hashtable)gameList[roomName];
                    string dbg = string.Format(
                        "IsOpen: {0}, MaxPlayers: {1}, PlayerCount: {2}",
                        gameInfo[GameProperties.IsOpen],
                        gameInfo[GameProperties.MaxPlayers],
                        gameInfo[GameProperties.PlayerCount]);

                    Console.WriteLine(dbg);

                    masterClient1.OperationResponseQueue.Clear();

                    Assert.IsTrue(masterClient1.PhotonClient.OpJoinRoom((string)roomName, new Hashtable()));
                    var response = masterClient1.WaitForOperationResponse(WaitTime);
                    Assert.AreEqual(ErrorCode.Ok, response.ReturnCode, response.DebugMessage);
                    Console.WriteLine("Joined hanging game " + roomName + ": " + response.ToStringFull());


                    //Assert.IsNotNull(response.Parameters[ParameterCode.Address], "Address");

                    //var address = (string)response.Parameters[ParameterCode.Address];
                    //string gameServerIp = address.Split(':')[0];
                    //short gameServerPort = short.Parse(address.Split(':')[1]);

                    //gameClient1.Connect(gameServerIp, gameServerPort, appId);
                    //gameClient1.WaitForConnect(waitTime);

                    //Assert.IsTrue(gameClient1.PhotonClient.OpJoinRoom((string)roomName, new Hashtable()));
                    //response = gameClient1.WaitForOperationResponse(waitTime);

                    //Assert.AreEqual(ErrorCode.Ok, response.ReturnCode, response.DebugMessage);

                }
            }
            finally
            {
                if (masterClient1.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    masterClient1.Close();
                }


                if (gameClient1.PhotonClient.PeerState == PeerStateValue.Connected)
                {
                    gameClient1.Close();
                }
            }
        }


        #endregion
    }
}