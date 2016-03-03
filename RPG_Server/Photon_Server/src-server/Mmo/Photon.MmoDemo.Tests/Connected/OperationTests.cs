// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationTests.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The operation tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Tests.Connected
{
    using System;
    using System.Collections;
    using System.Threading;

    using NUnit.Framework;

    using Photon.MmoDemo.Common;
    using Photon.SocketServer;

    using EventData = ExitGames.Client.Photon.EventData;
    using OperationResponse = ExitGames.Client.Photon.OperationResponse;

    /// <summary>
    ///   The operation tests.
    /// </summary>
    [TestFixture]
    public class OperationTests
    {
        #region Public Methods

        /// <summary>
        ///   The attach camera.
        /// </summary>
        [Test]
        public void AttachCamera()
        {
            using (var client = new Client("Test"))
            {
                Assert.IsTrue(client.Connect());
                CreateWorld("TestWorld", client);
                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);

                Operations.SpawnItem(client, "MyItem", byte.MaxValue, new[] { 10f, 10f }, null, true);
                Func<OperationResponse, bool> checkAction =
                    e => (string)e[(byte)ParameterCode.ItemId] == "MyItem" && (byte)e[(byte)ParameterCode.ItemType] == byte.MaxValue;
                client.BeginReceiveResponse(0);

                OperationResponse data;
                Assert.IsTrue(client.EndReceiveResponse(Settings.WaitTime, out data), "Response not received");
                if (data.ReturnCode != (int)ReturnCode.Ok)
                {
                    Assert.AreEqual(ReturnCode.ItemAlreadyExists, (ReturnCode)data.ReturnCode);
                    Operations.Move(client, "MyItem", byte.MaxValue, new[] { 10f, 10f });
                }
                else
                {
                    Assert.IsTrue(checkAction(data), "check action failed");
                }

                Operations.AttachCamera(client, "MyItem", byte.MaxValue);
                client.BeginReceiveResponse(0);

                Assert.IsTrue(client.EndReceiveResponse(Settings.WaitTime, out data), "Response not received");
                Assert.AreEqual(data.ReturnCode, (int)ReturnCode.Ok);
                Assert.IsTrue(checkAction(data), "check action failed");
            }
        }

        /// <summary>
        ///   The connect.
        /// </summary>
        [Test]
        public void Connect()
        {
            using (var client = new Client("Test"))
            {
                Assert.IsTrue(client.Connect());
                Thread.Sleep(1000);
                Assert.IsTrue(client.Disconnect());
            }
        }

        /// <summary>
        ///   The create world.
        /// </summary>
        [Test]
        public void CreateWorld()
        {
            using (var client = new Client("Test"))
            {
                Assert.IsTrue(client.Connect());
                CreateWorld("CreateWorld", client);

                // "Test" defined in setup
                Operations.CreateWorld(client, "CreateWorld", new[] { 0f, 0f }, new[] { 10f, 10f }, new[] { 1f, 1f });

                Func<OperationResponse, bool> checkAction = d => d.OperationCode == (byte)OperationCode.CreateWorld;
                client.BeginReceiveResponse(10);

                OperationResponse data;
                Assert.IsTrue(client.EndReceiveResponse(Settings.WaitTime, out data), "Response not received");
                Assert.AreEqual(data.ReturnCode, (int)ReturnCode.WorldAlreadyExists);
                Assert.IsTrue(checkAction(data), "check action failed");
            }
        }

        /// <summary>
        ///   The destroy item.
        /// </summary>
        [Test]
        public void DestroyItem()
        {
            using (var client = new Client("Test"))
            {
                Assert.IsTrue(client.Connect());
                CreateWorld("TestWorld", client);
                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);
                SpawnItem(client);
                Operations.DestroyItem(client, "MyItem", byte.MaxValue);

                Func<EventData, bool> checkAction =
                    d => (string)d.Parameters[(byte)ParameterCode.ItemId] == "MyItem" && (byte)d[(byte)ParameterCode.ItemType] == byte.MaxValue;
                client.BeginReceiveEvent(EventCode.ItemDestroyed, checkAction, 10);
                EventData data;
                Assert.IsTrue(client.EndReceiveEvent(Settings.WaitTime, out data), "Event not received");
                Assert.AreEqual(data.Code, (byte)EventCode.ItemDestroyed);
                Assert.IsTrue(checkAction(data), "check action failed");
            }
        }

        /// <summary>
        ///   The detach camera.
        /// </summary>
        [Test]
        public void DetachCamera()
        {
            using (var client = new Client("Test"))
            {
                Assert.IsTrue(client.Connect());
                CreateWorld("TestWorld", client);
                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);

                Operations.DetachCamera(client);
                client.BeginReceiveResponse(0);

                OperationResponse data;
                Assert.IsTrue(client.EndReceiveResponse(Settings.WaitTime, out data), "Response not received");
                Assert.AreEqual(data.ReturnCode, (byte)ReturnCode.Ok);
            }
        }

        /// <summary>
        ///   The enter world.
        /// </summary>
        [Test]
        public void EnterWorld()
        {
            using (var client = new Client("Test"))
            {
                Assert.IsTrue(client.Connect());
                CreateWorld("TestWorld", client);
                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);
            }
        }

        /// <summary>
        ///   The exit world.
        /// </summary>
        [Test]
        public void ExitWorld()
        {
            using (var client = new Client("Test"))
            {
                Assert.IsTrue(client.Connect());
                CreateWorld("TestWorld", client);
                Operations.ExitWorld(client);

                client.BeginReceiveResponse(0);

                OperationResponse data;
                Assert.IsTrue(client.EndReceiveResponse(Settings.WaitTime, out data), "Response not received");
                Assert.AreEqual(data.ReturnCode, (byte)ReturnCode.InvalidOperation);
                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);
                ExitWorld(client);
            }
        }

        /// <summary>
        ///   The move test.
        /// </summary>
        [Test]
        public void Move()
        {
            using (var client = new Client("Test"))
            {
                Assert.IsTrue(client.Connect());
                CreateWorld("TestWorld", client);
                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);
                Operations.Move(client, null, null, new[] { 1f, 2f });

                client.BeginReceiveResponse(0);
                OperationResponse data;
                Assert.IsFalse(client.EndReceiveResponse(Settings.WaitTime, out data), "Response received");
            }
        }

        /// <summary>
        ///   The raise generic event.
        /// </summary>
        [Test]
        public void RaiseGenericEvent()
        {
            using (var client = new Client("Test"))
            {
                Assert.IsTrue(client.Connect());
                CreateWorld("TestWorld", client);
                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);

                ////Operations.RaiseGenericEvent(client, null, null, byte.MaxValue, null, Reliability.Reliable, EventReceiver.WorldRegion);
                Func<EventData, bool> checkAction =
                    d =>
                    (string)d[(byte)ParameterCode.ItemId] == client.Username && (byte)d[(byte)ParameterCode.ItemType] == (byte)ItemType.Avatar &&
                    (byte)d[(byte)ParameterCode.CustomEventCode] == byte.MaxValue;

                ////client.BeginReceiveEvent(EventCode.ItemGeneric, checkAction, 0);
                EventData data;

                ////Assert.IsTrue(client.EndReceiveEvent(Settings.WaitTime, out data), "Event not received - target avatar world region");
                ////Assert.AreEqual((byte)data.Code, (byte)EventCode.ItemGeneric);
                ////Assert.IsTrue(checkAction(data), "check action failed");
                Operations.RaiseGenericEvent(client, null, null, byte.MaxValue, null, Reliability.Reliable, EventReceiver.ItemOwner);
                client.BeginReceiveEvent(EventCode.ItemGeneric, checkAction, 0);
                Assert.IsTrue(client.EndReceiveEvent(Settings.WaitTime, out data), "Event not received - target avatar owner");
                Assert.AreEqual(data.Code, (byte)EventCode.ItemGeneric);
                Assert.IsTrue(checkAction(data), "check action failed");

                Operations.RaiseGenericEvent(client, null, null, byte.MaxValue, null, Reliability.Reliable, EventReceiver.ItemSubscriber);
                client.BeginReceiveEvent(EventCode.ItemGeneric, checkAction, 0);
                Assert.IsFalse(client.EndReceiveEvent(Settings.WaitTime, out data), "Event received - avator should not be subscribed");

                // subscribe avatar 
                Operations.SubscribeItem(client, client.Username, (byte)ItemType.Avatar, null);
                Operations.RaiseGenericEvent(client, null, null, byte.MaxValue, null, Reliability.Reliable, EventReceiver.ItemSubscriber);
                client.BeginReceiveEvent(EventCode.ItemGeneric, checkAction, 0);
                Assert.IsTrue(client.EndReceiveEvent(Settings.WaitTime, out data), "Event not received - target avatar subscriber");
                Assert.AreEqual(data.Code, (byte)EventCode.ItemGeneric);
                Assert.IsTrue(checkAction(data), "check action failed");
            }
        }

        /// <summary>
        ///   The set properties.
        /// </summary>
        [Test]
        public void SetProperties()
        {
            using (var client = new Client("Test"))
            {
                Assert.IsTrue(client.Connect());
                CreateWorld("TestWorld", client);
                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);

                Operations.SetProperties(client, null, null, new Hashtable { { "Key", "Value" } }, null);

                client.BeginReceiveResponse(0);
                EventData data;
                Assert.IsFalse(client.EndReceiveEvent(Settings.WaitTime, out data), "Response received");
            }
        }

        /// <summary>
        ///   The set view distance.
        /// </summary>
        [Test]
        public void SetViewDistance()
        {
            using (var client = new Client("Test"))
            {
                Assert.IsTrue(client.Connect());
                CreateWorld("TestWorld", client);
                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);

                Operations.SetViewDistance(client, new[] { 2f, 2f }, new[] { 3f, 3f });

                client.BeginReceiveResponse(0);
                OperationResponse data;
                Assert.IsFalse(client.EndReceiveResponse(Settings.WaitTime, out data), "Response received");
            }
        }

        /// <summary>
        ///   The spawn item.
        /// </summary>
        [Test]
        public void SpawnItem()
        {
            using (var client = new Client("Test"))
            {
                Assert.IsTrue(client.Connect());
                CreateWorld("TestWorld", client);
                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);
                SpawnItem(client);
            }
        }

        /// <summary>
        ///   The subscribe item.
        /// </summary>
        [Test]
        public void SubscribeItem()
        {
            using (var client = new Client("Test"))
            {
                Assert.IsTrue(client.Connect());
                CreateWorld("TestWorld", client);
                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);

                Operations.SubscribeItem(client, client.Username, (byte)ItemType.Avatar, null);
                client.BeginReceiveEvent(EventCode.ItemSubscribed, d => (string)d[(byte)ParameterCode.ItemId] == client.Username, 0);
                EventData data;
                client.EndReceiveEvent(Settings.WaitTime, out data);
                Assert.AreEqual(data.Code, (byte)EventCode.ItemSubscribed);

                // check if subscription worked
                Operations.Move(client, null, null, new[] { 1f, 2f });

                Func<EventData, bool> checkAction =
                    d => (string)d[(byte)ParameterCode.ItemId] == client.Username && (byte)d[(byte)ParameterCode.ItemType] == (byte)ItemType.Avatar;
                client.BeginReceiveEvent(EventCode.ItemMoved, checkAction, 0);
                Assert.IsTrue(client.EndReceiveEvent(Settings.WaitTime, out data), "Event not received");
                Assert.AreEqual(data.Code, (byte)EventCode.ItemMoved);
                Assert.IsTrue(checkAction(data), "check action failed");
            }
        }

        /////// <summary>
        /////// The tear down.
        /////// </summary>
        ////[TearDown]
        ////public void TearDown()
        ////{
        ////    // wait for client disconnect
        ////    Thread.Sleep(100);
        ////}

        /// <summary>
        ///   The unsubscribe item.
        /// </summary>
        [Test]
        public void UnsubscribeItem()
        {
            using (var client = new Client("Test"))
            {
                Assert.IsTrue(client.Connect());
                CreateWorld("TestWorld", client);
                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);

                Operations.SubscribeItem(client, client.Username, (byte)ItemType.Avatar, null);
                client.BeginReceiveEvent(EventCode.ItemSubscribed, d => (string)d[(byte)ParameterCode.ItemId] == client.Username, 0);
                EventData data;
                client.EndReceiveEvent(Settings.WaitTime, out data);
                Assert.AreEqual(data.Code, (byte)EventCode.ItemSubscribed);

                Operations.UnsubscribeItem(client, client.Username, (byte)ItemType.Avatar);
                client.BeginReceiveEvent(EventCode.ItemUnsubscribed, d => (string)d[(byte)ParameterCode.ItemId] == client.Username, 0);
                client.EndReceiveEvent(Settings.WaitTime, out data);
                Assert.AreEqual(data.Code, (byte)EventCode.ItemUnsubscribed);

                // check if subscription worked
                Operations.Move(client, null, null, new[] { 1f, 2f });

                Func<EventData, bool> checkAction = d => true;
                client.BeginReceiveEvent(EventCode.ItemMoved, checkAction, 0);
                Assert.IsFalse(client.EndReceiveEvent(Settings.WaitTime, out data), "Event received");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The create world.
        /// </summary>
        /// <param name = "world">
        ///   The world.
        /// </param>
        /// <param name = "client">
        ///   The client.
        /// </param>
        private static void CreateWorld(string world, Client client)
        {
            Operations.CreateWorld(client, world, new[] { 0f, 0f }, new[] { 10f, 10f }, new[] { 1f, 1f });

            client.BeginReceiveResponse(0);

            OperationResponse data;
            Assert.IsTrue(client.EndReceiveResponse(Settings.WaitTime, out data), "Response not received");
            Assert.IsTrue(data.ReturnCode == (int)ReturnCode.Ok || data.ReturnCode == (int)ReturnCode.WorldAlreadyExists);
        }

        /// <summary>
        ///   The enter world.
        /// </summary>
        /// <param name = "client">
        ///   The client.
        /// </param>
        /// <param name = "worldName">
        ///   The world name.
        /// </param>
        /// <param name = "position">
        ///   The position.
        /// </param>
        /// <param name = "viewDistanceEnter">
        ///   The view Distance Enter.
        /// </param>
        /// <param name = "viewDistanceExit">
        ///   The view Distance Exit.
        /// </param>
        /// <param name = "properties">
        ///   The properties.
        /// </param>
        private static void EnterWorld(
            Client client, string worldName, float[] position, float[] viewDistanceEnter, float[] viewDistanceExit, Hashtable properties)
        {
            Operations.EnterWorld(client, worldName, client.Username, properties, position, viewDistanceEnter, viewDistanceExit);

            client.BeginReceiveResponse(0);

            OperationResponse data;
            Assert.IsTrue(client.EndReceiveResponse(Settings.WaitTime, out data), "Event not received");
            Assert.AreEqual(data.ReturnCode, (int)ReturnCode.Ok);
        }

        /// <summary>
        ///   The exit world.
        /// </summary>
        /// <param name = "client">
        ///   The client.
        /// </param>
        private static void ExitWorld(Client client)
        {
            EventData data;
            Operations.ExitWorld(client);
            client.BeginReceiveEvent(EventCode.WorldExited, d => true, 0);
            Assert.IsTrue(client.EndReceiveEvent(Settings.WaitTime, out data), "Event not received");
            Assert.AreEqual(data.Code, (byte)EventCode.WorldExited);
        }

        /////// <summary>
        /////// The receive operation response.
        /////// </summary>
        /////// <param name="client">
        /////// The client.
        /////// </param>
        /////// <param name="operationCode">
        /////// The operation code.
        /////// </param>
        /////// <returns>
        /////// the received response event
        /////// </returns>
        ////private static Hashtable ReceiveOperationResponse(Client client, OperationCode operationCode)
        ////{
        ////    Func<Hashtable, bool> checkAction = d => (byte)d.OperationCode == (byte)operationCode;
        ////    client.BeginReceiveEvent(EventCode.OperationSuccess, checkAction, 10);

        ////    Hashtable data;
        ////    Assert.IsTrue(client.EndReceiveEvent(Settings.WaitTime, out data), "Event not received");
        ////    var eventCode = (EventCode)(byte)data.Code;
        ////    Assert.IsTrue(eventCode == EventCode.OperationSuccess || eventCode == EventCode.OperationError);
        ////    Assert.AreEqual((OperationCode)(byte)data.OperationCode, operationCode);
        ////    return data;
        ////}

        /////// <summary>
        /////// The receive operation success.
        /////// </summary>
        /////// <param name="client">
        /////// The client.
        /////// </param>
        /////// <param name="operationCode">
        /////// The operation code.
        /////// </param>
        ////private static void ReceiveOperationSuccess(Client client, OperationCode operationCode)
        ////{
        ////    Hashtable data = ReceiveOperationResponse(client, operationCode);
        ////    Assert.AreEqual((EventCode)(byte)data.Code, EventCode.OperationSuccess);
        ////}

        /// <summary>
        ///   The spawn item.
        /// </summary>
        /// <param name = "client">
        ///   The client.
        /// </param>
        private static void SpawnItem(Client client)
        {
            Operations.SpawnItem(client, "MyItem", byte.MaxValue, new[] { 1f, 1f }, null, true);

            Func<OperationResponse, bool> checkAction =
                e => (string)e[(byte)ParameterCode.ItemId] == "MyItem" && (byte)e[(byte)ParameterCode.ItemType] == byte.MaxValue;
            client.BeginReceiveResponse(0);

            OperationResponse data;
            Assert.IsTrue(client.EndReceiveResponse(Settings.WaitTime, out data), "Response not received");
            if (data.ReturnCode != (int)ReturnCode.Ok)
            {
                Assert.AreEqual(ReturnCode.ItemAlreadyExists, (ReturnCode)data.ReturnCode);
                Operations.Move(client, "MyItem", byte.MaxValue, new[] { 1f, 1f });
            }
            else
            {
                Assert.IsTrue(checkAction(data), "check action failed");
            }
        }

        #endregion
    }
}