// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BasicUseCases.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The basic use cases.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteLobby.Tests.Disconnected
{
    using System.Collections;
    using System.Collections.Generic;

    using ExitGames.Client.Photon.Lite;

    using Lite.Operations;
    using Lite.Tests.Disconnected;

    using LiteLobby.Operations;

    using NUnit.Framework;

    using Photon.SocketServer;

    using EventData = Photon.SocketServer.EventData;
    using OperationRequest = Photon.SocketServer.OperationRequest;
    using OperationResponse = Photon.SocketServer.OperationResponse;

    /// <summary>
    /// The basic use cases.
    /// </summary>
    [TestFixture]
    public class BasicUseCases
    {
        /// <summary>
        /// The wait timeout.
        /// </summary>
        private const int WaitTimeout = 1000;

        private LiteLobbyApplication application; 

        [SetUp]
        public void Setup()
        {
            this.application = new LiteLobbyApplication();
            this.application.OnStart("NUnit", "LiteLobby", new DummyApplicationSink(), null, null, string.Empty);
        }

        /// <summary>
        /// The join and leave.
        /// </summary>
        [Test]
        public void JoinAndJoin()
        {
            var peerOne = new DummyPeer { SessionId = "#1" };
            var peerTwo = new DummyPeer { SessionId = "#2" };
            var peerLobby = new DummyPeer { SessionId = "#L" };
            var litePeerOne = new LiteLobbyPeer(peerOne.Protocol, peerOne);
            var litePeerTwo = new LiteLobbyPeer(peerTwo.Protocol, peerTwo);
            var litePeerLobby = new LiteLobbyPeer(peerLobby.Protocol, peerLobby);

            try
            {
                // peer lobby: join
                OperationRequest request = GetJoinLobbyRequest();
                PeerHelper.InvokeOnOperationRequest(litePeerLobby, request, new SendParameters());
                Assert.IsTrue(peerLobby.WaitForNextResponse(WaitTimeout));
                List<OperationResponse> responseList = peerLobby.GetResponseList();
                Assert.AreEqual(1, responseList.Count);

                // peer lobby: receive lobby games
                Assert.IsTrue(peerLobby.WaitForNextEvent(WaitTimeout));
                List<EventData> eventList = peerLobby.GetEventList();
                Assert.AreEqual(1, eventList.Count);
                EventData eventData = eventList[0];
                Assert.AreEqual((byte)LiteLobbyEventCode.GameList, eventData.Code);

                // peer 1: join
                request = GetJoinRequest();
                PeerHelper.InvokeOnOperationRequest(litePeerOne, request, new SendParameters());
                Assert.IsTrue(peerOne.WaitForNextResponse(WaitTimeout));

                responseList = peerOne.GetResponseList();
                Assert.AreEqual(1, responseList.Count);
                OperationResponse response = responseList[0];
                Assert.AreEqual(1, response.Parameters[(byte)ParameterKey.ActorNr]);

                // peer 1: receive own join event
                Assert.IsTrue(peerOne.WaitForNextEvent(WaitTimeout));
                eventList = peerOne.GetEventList();
                Assert.AreEqual(1, eventList.Count);
                eventData = eventList[0];
                Assert.AreEqual(LiteOpCode.Join, eventData.Code);
                Assert.AreEqual(1, eventData.Parameters[(byte)ParameterKey.ActorNr]);

                // peer lobby: wait for lobby update (all 2 sec)
                Assert.IsTrue(peerLobby.WaitForNextEvent(2000 + WaitTimeout));
                eventList = peerLobby.GetEventList();
                Assert.AreEqual(1, eventList.Count);
                eventData = eventList[0];
                var games = (Hashtable)eventData.Parameters[(byte)ParameterKey.Data];
                Assert.AreEqual(1, games.Count);
                Assert.AreEqual("1", games["testGame"]);

                // peer 2: join 
                request = GetJoinRequest();
                PeerHelper.InvokeOnOperationRequest(litePeerTwo, request, new SendParameters());
                Assert.IsTrue(peerTwo.WaitForNextResponse(WaitTimeout));

                responseList = peerTwo.GetResponseList();
                Assert.AreEqual(1, responseList.Count);
                response = responseList[0];
                Assert.AreEqual(2, response.Parameters[(byte)ParameterKey.ActorNr]);

                // peer 2: receive own join event
                Assert.IsTrue(peerTwo.WaitForNextEvent(WaitTimeout));
                eventList = peerTwo.GetEventList();
                Assert.AreEqual(1, eventList.Count);
                eventData = eventList[0];
                Assert.AreEqual(LiteOpCode.Join, eventData.Code);
                Assert.AreEqual(2, eventData.Parameters[(byte)ParameterKey.ActorNr]);

                // peer 1: receive join event of peer 2
                Assert.IsTrue(peerOne.WaitForNextEvent(WaitTimeout));
                eventList = peerOne.GetEventList();
                Assert.AreEqual(1, eventList.Count);
                eventData = eventList[0];
                Assert.AreEqual(LiteOpCode.Join, eventData.Code);
                Assert.AreEqual(2, eventData.Parameters[(byte)ParameterKey.ActorNr]);

                // peer lobby: wait for lobby update (all 2 sec)
                Assert.IsTrue(peerLobby.WaitForNextEvent(2000 + WaitTimeout));
                eventList = peerLobby.GetEventList();
                Assert.AreEqual(1, eventList.Count);
                eventData = eventList[0];
                games = (Hashtable)eventData.Parameters[(byte)ParameterKey.Data];
                Assert.AreEqual(1, games.Count);
                Assert.AreEqual("2", games["testGame"]);

                // peer 2: join again
                request = GetJoinRequest();
                PeerHelper.InvokeOnOperationRequest(litePeerTwo, request, new SendParameters());
                Assert.IsTrue(peerTwo.WaitForNextResponse(WaitTimeout));
                responseList = peerTwo.GetResponseList();
                Assert.AreEqual(1, responseList.Count);

                // peer 2: do not receive own leave event, receive own join event
                Assert.IsTrue(peerTwo.WaitForNextEvent(WaitTimeout));
                eventList = peerTwo.GetEventList();
                Assert.AreEqual(1, eventList.Count);
                eventData = eventList[0];
                Assert.AreEqual(LiteOpCode.Join, eventData.Code);
                Assert.AreEqual(3, eventData.Parameters[(byte)ParameterKey.ActorNr]);

                // peer 1: receive leave and event of peer 2
                Assert.IsTrue(peerOne.WaitForNextEvent(WaitTimeout));
                eventList = peerOne.GetEventList();
                Assert.GreaterOrEqual(eventList.Count, 1);
                eventData = eventList[0];
                Assert.AreEqual(LiteOpCode.Leave, eventData.Code);
                Assert.AreEqual(2, eventData.Parameters[(byte)ParameterKey.ActorNr]);

                if (eventList.Count == 1)
                {
                    // waiting for join event
                    Assert.IsTrue(peerOne.WaitForNextEvent(WaitTimeout));
                    eventList = peerOne.GetEventList();
                    Assert.AreEqual(1, eventList.Count);
                    eventData = eventList[0];
                }
                else
                {
                    eventData = eventList[1];
                }

                Assert.AreEqual(LiteOpCode.Join, eventData.Code);
                Assert.AreEqual(3, eventData.Parameters[(byte)ParameterKey.ActorNr]);

                // peer lobby: wait for lobby update (all 2 sec)
                Assert.IsTrue(peerLobby.WaitForNextEvent(2000 + WaitTimeout));
                eventList = peerLobby.GetEventList();
                Assert.AreEqual(1, eventList.Count);
                eventData = eventList[0];
                games = (Hashtable)eventData.Parameters[(byte)ParameterKey.Data];
                Assert.AreEqual(1, games.Count);
                Assert.AreEqual("2", games["testGame"]);
            }
            finally
            {
                PeerHelper.SimulateDisconnect(litePeerOne);
                PeerHelper.SimulateDisconnect(litePeerTwo);
                PeerHelper.SimulateDisconnect(litePeerLobby);
            }
        }

        /// <summary>
        /// The join and leave.
        /// </summary>
        [Test]
        public void JoinAndLeave()
        {
            var peerOne = new DummyPeer { SessionId = "#1" };
            var peerTwo = new DummyPeer { SessionId = "#2" };
            var peerLobby = new DummyPeer { SessionId = "#L" };
            var litePeerOne = new LiteLobbyPeer(peerOne.Protocol, peerOne);
            var litePeerTwo = new LiteLobbyPeer(peerTwo.Protocol, peerTwo);
            var litePeerLobby = new LiteLobbyPeer(peerLobby.Protocol, peerLobby);

            try
            {
                // peer lobby: join
                OperationRequest request = GetJoinLobbyRequest();
                PeerHelper.InvokeOnOperationRequest(litePeerLobby, request, new SendParameters());
                Assert.IsTrue(peerLobby.WaitForNextResponse(WaitTimeout));
                List<OperationResponse> responseList = peerLobby.GetResponseList();
                Assert.AreEqual(1, responseList.Count);

                // peer lobby: receive lobby games
                Assert.IsTrue(peerLobby.WaitForNextEvent(WaitTimeout));
                List<EventData> eventList = peerLobby.GetEventList();
                Assert.AreEqual(1, eventList.Count);
                EventData eventData = eventList[0];
                Assert.AreEqual((byte)LiteLobbyEventCode.GameList, eventData.Code);

                // peer 1: join
                request = GetJoinRequest();
                PeerHelper.InvokeOnOperationRequest(litePeerOne, request, new SendParameters());
                Assert.IsTrue(peerOne.WaitForNextResponse(WaitTimeout));

                responseList = peerOne.GetResponseList();
                Assert.AreEqual(1, responseList.Count);
                OperationResponse response = responseList[0];
                Assert.AreEqual(1, response.Parameters[(byte)ParameterKey.ActorNr]);

                // peer 1: receive own join event
                Assert.IsTrue(peerOne.WaitForNextEvent(WaitTimeout));
                eventList = peerOne.GetEventList();
                Assert.AreEqual(1, eventList.Count);
                eventData = eventList[0];
                Assert.AreEqual(LiteOpCode.Join, eventData.Code);
                Assert.AreEqual(1, eventData.Parameters[(byte)ParameterKey.ActorNr]);

                // peer lobby: wait for lobby update (all 2 sec)
                Assert.IsTrue(peerLobby.WaitForNextEvent(2000 + WaitTimeout));
                eventList = peerLobby.GetEventList();
                Assert.AreEqual(1, eventList.Count);
                eventData = eventList[0];
                var games = (Hashtable)eventData.Parameters[(byte)ParameterKey.Data];
                Assert.AreEqual(1, games.Count);
                Assert.AreEqual("1", games["testGame"]);

                // peer 2: join 
                request = GetJoinRequest();
                PeerHelper.InvokeOnOperationRequest(litePeerTwo, request, new SendParameters());
                Assert.IsTrue(peerTwo.WaitForNextResponse(WaitTimeout));

                responseList = peerTwo.GetResponseList();
                Assert.AreEqual(1, responseList.Count);
                response = responseList[0];
                Assert.AreEqual(2, response.Parameters[(byte)ParameterKey.ActorNr]);

                // peer 2: receive own join event
                Assert.IsTrue(peerTwo.WaitForNextEvent(WaitTimeout));
                eventList = peerTwo.GetEventList();
                Assert.AreEqual(1, eventList.Count);
                eventData = eventList[0];
                Assert.AreEqual(LiteOpCode.Join, eventData.Code);
                Assert.AreEqual(2, eventData.Parameters[(byte)ParameterKey.ActorNr]);

                // peer 1: receive join event of peer 2
                Assert.IsTrue(peerOne.WaitForNextEvent(WaitTimeout));
                eventList = peerOne.GetEventList();
                Assert.AreEqual(1, eventList.Count);
                eventData = eventList[0];
                Assert.AreEqual(LiteOpCode.Join, eventData.Code);
                Assert.AreEqual(2, eventData.Parameters[(byte)ParameterKey.ActorNr]);

                // peer lobby: wait for lobby update (all 2 sec)
                Assert.IsTrue(peerLobby.WaitForNextEvent(2000 + WaitTimeout));
                eventList = peerLobby.GetEventList();
                Assert.AreEqual(1, eventList.Count);
                eventData = eventList[0];
                games = (Hashtable)eventData.Parameters[(byte)ParameterKey.Data];
                Assert.AreEqual(1, games.Count);
                Assert.AreEqual("2", games["testGame"]);

                // peer 2: leave
                request = GetLeaveRequest();
                PeerHelper.InvokeOnOperationRequest(litePeerTwo, request, new SendParameters());
                Assert.IsTrue(peerTwo.WaitForNextResponse(WaitTimeout));
                responseList = peerTwo.GetResponseList();
                Assert.AreEqual(1, responseList.Count);

                // peer 2: do not receive own leave event
                eventList = peerTwo.GetEventList();
                Assert.AreEqual(0, eventList.Count);

                // peer 1: receive leave event of peer 2
                Assert.IsTrue(peerOne.WaitForNextEvent(WaitTimeout));
                eventList = peerOne.GetEventList();
                Assert.AreEqual(1, eventList.Count);
                eventData = eventList[0];
                Assert.AreEqual(LiteOpCode.Leave, eventData.Code);
                Assert.AreEqual(2, eventData.Parameters[(byte)ParameterKey.ActorNr]);

                // peer lobby: wait for lobby update (all 2 sec)
                Assert.IsTrue(peerLobby.WaitForNextEvent(2000 + WaitTimeout));
                eventList = peerLobby.GetEventList();
                Assert.AreEqual(1, eventList.Count);
                eventData = eventList[0];
                games = (Hashtable)eventData.Parameters[(byte)ParameterKey.Data];
                Assert.AreEqual(1, games.Count);
                Assert.AreEqual("1", games["testGame"]);
            }
            finally
            {
                PeerHelper.SimulateDisconnect(litePeerOne);
                PeerHelper.SimulateDisconnect(litePeerTwo);
                PeerHelper.SimulateDisconnect(litePeerLobby);
            }
        }

        /// <summary>
        /// The get join lobby request.
        /// </summary>
        /// <returns>
        /// a join request
        /// </returns>
        private static OperationRequest GetJoinLobbyRequest()
        {
            var request = new OperationRequest { OperationCode = (byte)OperationCode.Join, Parameters = new Dictionary<byte, object>() };
            request.Parameters.Add((byte)ParameterKey.GameId, "testlobby");
            return request;
        }

        /// <summary>
        /// The get join request.
        /// </summary>
        /// <returns>
        /// a join request
        /// </returns>
        private static OperationRequest GetJoinRequest()
        {
            var request = new OperationRequest { OperationCode = (byte)OperationCode.Join, Parameters = new Dictionary<byte, object>() };
            request.Parameters.Add((byte)ParameterKey.GameId, "testGame");
            request.Parameters.Add((byte)LobbyParameterKeys.LobbyId, "testlobby");
            return request;
        }

        /// <summary>
        /// The get leave request.
        /// </summary>
        /// <returns>
        /// a leave request
        /// </returns>
        private static OperationRequest GetLeaveRequest()
        {
            var request = new OperationRequest { OperationCode = (byte)OperationCode.Leave };
            return request;
        }
    }
}