// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LobbyTests.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The lobby tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteLobby.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Lite.Operations;
    using Lite.Tests.Client;

    using LiteLobby.Operations;

    using NUnit.Framework;

    using Photon.SocketServer;
    
    /// <summary>
    /// The lobby tests.
    /// </summary>
    [TestFixture]
    public class LobbyTests
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LobbyTests"/> class.
        /// </summary>
        public LobbyTests()
        {
            this.WaitTime = 5000;
        }

        /// <summary>
        /// Gets or sets WaitTime.
        /// </summary>
        protected int WaitTime { get; set; }

        /// <summary>
        /// The join game with lobby.
        /// </summary>
        [Test]
        public void JoinGameWithLobby()
        {
            string roomName = CreateRandomRoomName();

            TestClient client = this.InitClient();

            var request = new OperationRequest { OperationCode = (byte)OperationCode.Join, Parameters = new Dictionary<byte, object>() };
            request.Parameters.Add((byte)ParameterKey.GameId, roomName);
            request.Parameters.Add((byte)LobbyParameterKeys.LobbyId, "Mainlobby");
            client.SendOperationRequest(request);
            OperationResponse response = client.WaitForOperationResponse(this.WaitTime);
            EventData eventArgs = client.WaitForEvent(this.WaitTime);

            // check operation params
            CheckDefaultOperationParameters(response, OperationCode.Join);
            CheckParam(response, ParameterKey.ActorNr, 1);

            // check event params
            CheckDefaultEventParameters(eventArgs, OperationCode.Join, 1);
            CheckEventParamExists(eventArgs, ParameterKey.Actors);

            // cleanup
            client.Close();
            client.Dispose();
        }

        /// <summary>
        /// The join lobby.
        /// </summary>
        [Test]
        public void JoinLobby()
        {
            TestClient client = this.InitClient();

            var request = new OperationRequest { OperationCode = (byte)OperationCode.Join, Parameters = new Dictionary<byte, object>() };
            request.Parameters.Add((byte)ParameterKey.GameId, "Mainlobby");
            client.SendOperationRequest(request);
            OperationResponse response = client.WaitForOperationResponse(this.WaitTime);
            client.WaitForEvent(this.WaitTime);

            // check operation params
            CheckDefaultOperationParameters(response, OperationCode.Join);
            CheckParam(response, ParameterKey.ActorNr, 1);

            ////// check event params
            ////CheckDefaultEventParameters(eventArgs, OperationCodes.Join, 1);
            ////CheckEventParamExists(eventArgs, ParameterKeys.Actors);
            TestClient client2 = this.InitClient();
            string roomName = CreateRandomRoomName();

            request = new OperationRequest { OperationCode = (byte)OperationCode.Join, Parameters = new Dictionary<byte, object>() };
            request.Parameters.Add((byte)ParameterKey.GameId, roomName);
            request.Parameters.Add((byte)LobbyParameterKeys.LobbyId, "Mainlobby");
            client2.SendOperationRequest(request);
            response = client2.WaitForOperationResponse(this.WaitTime);
            EventData eventArgs = client2.WaitForEvent(this.WaitTime);

            // check operation params
            CheckDefaultOperationParameters(response, OperationCode.Join);
            CheckParam(response, ParameterKey.ActorNr, 1);

            // check event params
            CheckDefaultEventParameters(eventArgs, OperationCode.Join, 1);
            CheckEventParamExists(eventArgs, ParameterKey.Actors);

            // cleanup
            client2.Close();
            client2.Dispose();

            client.Close();
            client.Dispose();
        }

        /// <summary>
        /// The check default event params.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        /// <param name="operationCode">
        /// The operation code.
        /// </param>
        /// <param name="actorNumber">
        /// The actor number.
        /// </param>
        protected static void CheckDefaultEventParameters(EventData eventArgs, OperationCode operationCode, int actorNumber)
        {
            CheckEventParam(eventArgs, ParameterKey.ActorNr, actorNumber);
        }

        /// <summary>
        /// The check default event params.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        /// <param name="eventCode">
        /// The event code.
        /// </param>
        /// <param name="actorNumber">
        /// The actor number.
        /// </param>
        protected static void CheckDefaultEventParameters(EventData eventArgs, LiteLobbyEventCode eventCode, int actorNumber)
        {
            CheckEventParam(eventArgs, ParameterKey.ActorNr, actorNumber);
        }

        /// <summary>
        /// The check default operation params.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="operationCode">
        /// The operation code.
        /// </param>
        protected static void CheckDefaultOperationParameters(OperationResponse response, OperationCode operationCode)
        {
            Assert.AreEqual((byte)operationCode, response.OperationCode, "Unexpected operation code received.");
            Assert.AreEqual(0, response.ReturnCode, string.Format("Response has Error. ERR={0}, DBG={1}", response.ReturnCode, response.DebugMessage));
        }

        /// <summary>
        /// The check event param.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        /// <param name="paramKey">
        /// The param key.
        /// </param>
        /// <param name="expectedValue">
        /// The expected value.
        /// </param>
        protected static void CheckEventParam(EventData eventArgs, ParameterKey paramKey, object expectedValue)
        {
            CheckEventParamExists(eventArgs, paramKey);
            Assert.AreEqual(expectedValue, eventArgs.Parameters[(byte)paramKey], "Event param '{0}' has unexpected value", paramKey);
        }

        /// <summary>
        /// The check event param.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="expectedValue">
        /// The expected value.
        /// </param>
        protected static void CheckEventParam(EventData eventArgs, object key, object expectedValue)
        {
            CheckEventParamExists(eventArgs, key);
            Assert.AreEqual(expectedValue, eventArgs.Parameters[(byte)key], "Event param '{0}' has unexpected value", key);
        }

        /// <summary>
        /// The check event param exists.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        /// <param name="paramKey">
        /// The param key.
        /// </param>
        protected static void CheckEventParamExists(EventData eventArgs, ParameterKey paramKey)
        {
            Assert.Contains((short)paramKey, eventArgs.Parameters.Keys, "Parameter '{0}' is missing in event.", paramKey);
        }

        /// <summary>
        /// The check event param exists.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        protected static void CheckEventParamExists(EventData eventArgs, object key)
        {
            Assert.Contains(key, eventArgs.Parameters.Keys, "Parameter '{0}' is missing in event.", key);
        }

        /// <summary>
        /// The check join event.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        /// <param name="actorNumber">
        /// The actor number.
        /// </param>
        protected static void CheckJoinEvent(EventData eventArgs, int actorNumber)
        {
            CheckJoinEvent(eventArgs, actorNumber, null);
        }

        /// <summary>
        /// The check join event.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        /// <param name="actorNumber">
        /// The actor number.
        /// </param>
        /// <param name="expectedActorProperties">
        /// The expected actor properties.
        /// </param>
        protected static void CheckJoinEvent(EventData eventArgs, int actorNumber, Hashtable expectedActorProperties)
        {
            CheckDefaultEventParameters(eventArgs, OperationCode.Join, actorNumber);
            CheckEventParamExists(eventArgs, ParameterKey.Actors);

            if (expectedActorProperties != null)
            {
                CheckEventParamExists(eventArgs, ParameterKey.ActorProperties);
                CheckEventParam(eventArgs, ParameterKey.ActorProperties, expectedActorProperties);
            }
        }

        /// <summary>
        /// The check join response.
        /// </summary>
        /// <param name="operationResponse">
        /// The operation response.
        /// </param>
        /// <param name="expectedActorNumber">
        /// The expected actor number.
        /// </param>
        protected static void CheckJoinResponse(OperationResponse operationResponse, int expectedActorNumber)
        {
            CheckDefaultOperationParameters(operationResponse, OperationCode.Join);
            CheckParam(operationResponse, ParameterKey.ActorNr, expectedActorNumber);
        }

        /// <summary>
        /// The check param.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="paramKey">
        /// The param key.
        /// </param>
        /// <param name="expectedValue">
        /// The expected value.
        /// </param>
        protected static void CheckParam(OperationResponse response, ParameterKey paramKey, object expectedValue)
        {
            CheckParamExists(response, paramKey);
            object value = response.Parameters[(byte)paramKey];
            Assert.AreEqual(expectedValue, value, "Parameter '{0} has an unexpected value", paramKey);
        }

        /// <summary>
        /// The check param exists.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="paramKey">
        /// The param key.
        /// </param>
        protected static void CheckParamExists(OperationResponse response, ParameterKey paramKey)
        {
            Assert.Contains((short)paramKey, response.Parameters.Keys, "Parameter '{0}' is missing in operation response.", paramKey);
        }

        /// <summary>
        /// The create random room name.
        /// </summary>
        /// <returns>
        /// The random room name.
        /// </returns>
        protected static string CreateRandomRoomName()
        {
            return Guid.NewGuid().ToString();
        }
       
        /// <summary>
        /// The init client.
        /// </summary>
        /// <returns>
        /// a test client
        /// </returns>
        protected virtual TestClient InitClient()
        {
            var client = new TestClient(true);

            client.Connect("localhost", 4530, "LiteLobby");

            if (client.WaitForConnect(this.WaitTime) == false)
            {
                Assert.Fail("Didn't received init response in expected time.");
            }

            return client;
        }
    }
}