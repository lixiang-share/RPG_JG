// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationTests.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The operation tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Tests.Disconnected
{
    using System;
    using System.Collections;
    using System.Threading;

    using NUnit.Framework;

    using Photon.MmoDemo.Common;
    using Photon.MmoDemo.Server;
    using Photon.SocketServer;

    using Settings = Photon.MmoDemo.Tests.Settings;

    /// <summary>
    /// The operation tests.
    /// </summary>
    [TestFixture]
    public class OperationTests
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationTests"/> class.
        /// </summary>
        public OperationTests()
        {
            MmoWorld world;
            MmoWorldCache.Instance.Clear();
            MmoWorldCache.Instance.TryCreate(
                "TestWorld", 
                (new[] { 1f, 1f }).ToVector(), 
                (new[] { 10f, 10f }).ToVector(), 
                (new[] { 1f, 1f }).ToVector(), 
                out world);
        }

        /// <summary>
        /// The attach camera.
        /// </summary>
        [Test]
        public void AttachCamera()
        {
            using (var client = new Client("Test"))
            {
                SpawnItem(client);

                client.SendOperation(Operations.AttachCamera("MyItem", byte.MaxValue));
                client.BeginReceiveResponse();

                OperationResponse data;
                Assert.IsTrue(client.EndReceiveResponse(Settings.WaitTime, out data), "Event not received");
                Assert.AreEqual(data.ReturnCode, (int)ReturnCode.Ok);
                Func<OperationResponse, bool> checkAction =
                    e => (string)e.Parameters[(byte)ParameterCode.ItemId] == "MyItem" && (byte)e.Parameters[(byte)ParameterCode.ItemType] == byte.MaxValue;
                Assert.IsTrue(checkAction(data), "check action failed");
            }
        }

        /// <summary>
        /// The create world.
        /// </summary>
        [Test]
        public void CreateWorld()
        {
            using (var client = new Client("Test"))
            {
                client.SendOperation(Operations.CreateWorld("CreateWorld", new[] { 0f, 0f }, new[] { 10f, 10f }, new[] { 1f, 1f }));

                client.BeginReceiveResponse();

                OperationResponse data;
                Assert.IsTrue(client.EndReceiveResponse(Settings.WaitTime, out data), "Response not received");
                Assert.AreEqual(data.ReturnCode, (int)ReturnCode.Ok);

                // "Test" defined in setup
                client.SendOperation(Operations.CreateWorld("TestWorld", new[] { 0f, 0f }, new[] { 10f, 10f }, new[] { 1f, 1f }));
                client.BeginReceiveResponse();

                Assert.IsTrue(client.EndReceiveResponse(Settings.WaitTime, out data), "Response not received");
                Assert.AreEqual(data.ReturnCode, (byte)ReturnCode.WorldAlreadyExists);
            }
        }

        /// <summary>
        /// The destroy item.
        /// </summary>
        [Test]
        public void DestroyItem()
        {
            using (var client = new Client("Test"))
            {
                SpawnItem(client);
                client.SendOperation(Operations.DestroyItem("MyItem", byte.MaxValue));
                Func<EventData, bool> checkAction =
                    e => (string)e.Parameters[(byte)ParameterCode.ItemId] == "MyItem" && (byte)e.Parameters[(byte)ParameterCode.ItemType] == byte.MaxValue;
                client.BeginReceiveEvent(EventCode.ItemDestroyed, checkAction);

                EventData data;
                Assert.IsTrue(client.EndReceiveEvent(Settings.WaitTime, out data), "Event not received");
                Assert.AreEqual(data.Code, (byte)EventCode.ItemDestroyed);
                Assert.IsTrue(checkAction(data), "check action failed");
                client.EndReceiveEvent(Settings.WaitTime, out data);
            }
        }

        /// <summary>
        /// The detach camera.
        /// </summary>
        [Test]
        public void DetachCamera()
        {
            using (var client = new Client("Test"))
            {
                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);

                client.SendOperation(Operations.DetachCamera());
                client.BeginReceiveResponse();

                OperationResponse data;
                Assert.IsTrue(client.EndReceiveResponse(Settings.WaitTime, out data), "Response not received");
                Assert.AreEqual(data.ReturnCode, (int)ReturnCode.Ok);
            }
        }

        /// <summary>
        /// The enter world.
        /// </summary>
        [Test]
        public void EnterWorld()
        {
            using (var client = new Client("Test"))
            {
                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);

                client.SendOperation(Operations.EnterWorld("TestWorld", client.Username, null, new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }));
                ReceiveOperationResponse(client, ReturnCode.InvalidOperation);

                using (var client2 = new Client("Test"))
                {
                    EnterWorld(client2, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);

                    EventData @event;
                    client.BeginReceiveEvent(EventCode.WorldExited, d => true);
                    Assert.IsTrue(client.EndReceiveEvent(Settings.WaitTime, out @event), "Event not received");
                    Assert.AreEqual((byte)EventCode.WorldExited, @event.Code);

                    Assert.IsFalse(client.Peer.Connected);
                    //// client.SendOperation(Operations.Move(null, null, new[] { 2f, 2f }));
                    //// ReceiveOperationResponse(client, ReturnCode.InvalidOperation);
                }
            }
        }

        /// <summary>
        /// The exit world.
        /// </summary>
        [Test]
        public void ExitWorld()
        {
            using (var client = new Client("Test"))
            {
                client.SendOperation(Operations.ExitWorld());
                ReceiveOperationResponse(client, ReturnCode.InvalidOperation);

                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);
                ExitWorld(client);
            }
        }

        /// <summary>
        /// The move test.
        /// </summary>
        [Test]
        public void Move()
        {
            using (var client = new Client("Test"))
            {
                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);
                client.SendOperation(Operations.Move(null, null, new[] { 1f, 2f }));

                NotReceiveOperationResponse(client);
            }
        }

        /// <summary>
        /// The raise generic event.
        /// </summary>
        [Test]
        public void RaiseGenericEvent()
        {
            using (var client = new Client("Test"))
            {
                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);

                ////client.SendOperation(Operations.RaiseGenericEvent(null, null, byte.MaxValue, null, Reliability.Reliable, EventReceiver.WorldRegion));
                Func<EventData, bool> checkAction =
                    d =>
                    (string)d.Parameters[(byte)ParameterCode.ItemId] == client.Username && (byte)d.Parameters[(byte)ParameterCode.ItemType] == (byte)ItemType.Avatar &&
                    (byte)d.Parameters[(byte)ParameterCode.CustomEventCode] == byte.MaxValue;

                ////client.BeginReceiveEvent(EventCode.ItemGeneric, checkAction);
                EventData data;

                ////Assert.IsTrue(client.EndReceiveEvent(Settings.WaitTime, out data), "Event not received - target avatar world region");
                ////Assert.AreEqual((byte)data.Code, (byte)EventCode.ItemGeneric);
                ////Assert.IsTrue(checkAction(data), "check action failed - target avatar world region");
                client.SendOperation(Operations.RaiseGenericEvent(null, null, byte.MaxValue, null, Reliability.Reliable, EventReceiver.ItemOwner));
                client.BeginReceiveEvent(EventCode.ItemGeneric, checkAction);
                Assert.IsTrue(client.EndReceiveEvent(Settings.WaitTime, out data), "Event not received - target avatar");
                Assert.AreEqual(data.Code, (byte)EventCode.ItemGeneric);
                Assert.IsTrue(checkAction(data), "check action failed - target avatar");

                client.SendOperation(Operations.RaiseGenericEvent(null, null, byte.MaxValue, null, Reliability.Reliable, EventReceiver.ItemSubscriber));
                client.BeginReceiveEvent(EventCode.ItemGeneric, checkAction);
                Assert.IsFalse(client.EndReceiveEvent(Settings.WaitTime, out data), "Event received - but avatar not subscribed");

                // subscribe own avatar
                client.SendOperation(Operations.SubscribeItem(client.Username, (byte)ItemType.Avatar, null));
                client.SendOperation(Operations.RaiseGenericEvent(null, null, byte.MaxValue, null, Reliability.Reliable, EventReceiver.ItemSubscriber));
                client.BeginReceiveEvent(EventCode.ItemGeneric, checkAction);
                Assert.IsTrue(client.EndReceiveEvent(Settings.WaitTime, out data), "Event not received - target avatar subscriber");
                Assert.AreEqual(data.Code, (byte)EventCode.ItemGeneric);
                Assert.IsTrue(checkAction(data), "check action failed - target avatar subscriber");
            }
        }

        /// <summary>
        /// The set properties.
        /// </summary>
        [Test]
        public void SetProperties()
        {
            using (var client = new Client("Test"))
            {
                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);
                client.SendOperation(Operations.SetProperties(null, null, new Hashtable { { "Key", "Value" } }, null));

                NotReceiveOperationResponse(client);
            }
        }

        /// <summary>
        /// The set view distance.
        /// </summary>
        [Test]
        public void SetViewDistance()
        {
            using (var client = new Client("Test"))
            {
                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);
                client.SendOperation(Operations.SetViewDistance(new[] { 2f, 2f }, new[] { 3f, 3f }));

                NotReceiveOperationResponse(client);
            }
        }

        /// <summary>
        /// The spawn item.
        /// </summary>
        [Test]
        public void SpawnItem()
        {
            using (var client = new Client("Test"))
            {
                SpawnItem(client);
            }
        }

        /// <summary>
        /// The subscribe item.
        /// </summary>
        [Test]
        public void SubscribeItem()
        {
            using (var client = new Client("Test"))
            {
                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);
                client.SendOperation(Operations.SubscribeItem(client.Username, (byte)ItemType.Avatar, null));
                client.BeginReceiveEvent(EventCode.ItemSubscribed, d => (string)d.Parameters[(byte)ParameterCode.ItemId] == client.Username);
                EventData data;
                client.EndReceiveEvent(Settings.WaitTime, out data);
                Assert.AreEqual(data.Code, (byte)EventCode.ItemSubscribed);

                // check if subscription worked
                client.SendOperation(Operations.Move(null, null, new[] { 1f, 2f }));

                Func<EventData, bool> checkAction =
                    d =>
                    (string)d.Parameters[(byte)ParameterCode.ItemId] == client.Username && (byte)d.Parameters[(byte)ParameterCode.ItemType] == (byte)ItemType.Avatar;
                client.BeginReceiveEvent(EventCode.ItemMoved, checkAction);
                Assert.IsTrue(client.EndReceiveEvent(Settings.WaitTime, out data), "Event not received");
                Assert.AreEqual(data.Code, (byte)EventCode.ItemMoved);
                Assert.IsTrue(checkAction(data), "check action failed");
            }
        }

        /// <summary>
        /// The tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            // wait for client disconnect
            Thread.Sleep(100);
        }

        /// <summary>
        /// The unsubscribe item.
        /// </summary>
        [Test]
        public void UnsubscribeItem()
        {
            using (var client = new Client("Test"))
            {
                EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);
                client.SendOperation(Operations.SubscribeItem(client.Username, (byte)ItemType.Avatar, null));
                client.BeginReceiveEvent(EventCode.ItemSubscribed, d => (string)d.Parameters[(byte)ParameterCode.ItemId] == client.Username);
                EventData data;
                client.EndReceiveEvent(Settings.WaitTime, out data);
                Assert.AreEqual(data.Code, (byte)EventCode.ItemSubscribed);

                client.SendOperation(Operations.UnsubscribeItem(client.Username, (byte)ItemType.Avatar));
                client.BeginReceiveEvent(EventCode.ItemUnsubscribed, d => (string)d.Parameters[(byte)ParameterCode.ItemId] == client.Username);
                client.EndReceiveEvent(Settings.WaitTime, out data);
                Assert.AreEqual(data.Code, (byte)EventCode.ItemUnsubscribed);

                // check if subscription worked
                client.SendOperation(Operations.Move(null, null, new[] { 1f, 2f }));

                Func<EventData, bool> checkAction = d => true;
                client.BeginReceiveEvent(EventCode.ItemMoved, checkAction);
                Assert.IsFalse(client.EndReceiveEvent(Settings.WaitTime, out data), "Event received");
            }
        }

        /// <summary>
        /// The enter world.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="worldName">
        /// The world name.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="viewDistanceEnter">
        /// The view Distance Enter.
        /// </param>
        /// <param name="viewDistanceExit">
        /// The view Distance Exit.
        /// </param>
        /// <param name="properties">
        /// The properties.
        /// </param>
        private static void EnterWorld(
            Client client, string worldName, float[] position, float[] viewDistanceEnter, float[] viewDistanceExit, Hashtable properties)
        {
            client.SendOperation(Operations.EnterWorld(worldName, client.Username, properties, position, viewDistanceEnter, viewDistanceExit));

            ReceiveOperationResponse(client, ReturnCode.Ok);
        }

        /// <summary>
        /// The exit world.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        private static void ExitWorld(Client client)
        {
            EventData data;
            client.SendOperation(Operations.ExitWorld());
            client.BeginReceiveEvent(EventCode.WorldExited, d => true);
            Assert.IsTrue(client.EndReceiveEvent(Settings.WaitTime, out data), "Event not received");
            Assert.AreEqual(data.Code, (byte)EventCode.WorldExited);
        }

        /// <summary>
        /// The not receive operation response.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        private static void NotReceiveOperationResponse(Client client)
        {
            client.BeginReceiveResponse();

            OperationResponse data;
            Assert.IsFalse(client.EndReceiveResponse(Settings.WaitTime, out data), "Response not received");
        }

        /// <summary>
        /// The receive operation response.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="expectedReturn">
        /// The expected Return.
        /// </param>
        private static void ReceiveOperationResponse(Client client, ReturnCode expectedReturn)
        {
            client.BeginReceiveResponse();

            OperationResponse data;
            Assert.IsTrue(client.EndReceiveResponse(Settings.WaitTime, out data), "Response not received");
            var errorCode = (ReturnCode)data.ReturnCode;
            Assert.AreEqual(errorCode, expectedReturn);
        }

        /// <summary>
        /// The receive operation response.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <returns>
        /// the operation response
        /// </returns>
        private static OperationResponse ReceiveOperationResponse(Client client)
        {
            client.BeginReceiveResponse();

            OperationResponse data;
            Assert.IsTrue(client.EndReceiveResponse(Settings.WaitTime, out data), "Response not received");
            return data;
        }

        /// <summary>
        /// The spawn item.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        private static void SpawnItem(Client client)
        {
            EnterWorld(client, "TestWorld", new[] { 1f, 1f }, new[] { 1f, 1f }, new[] { 2f, 2f }, null);
            client.SendOperation(Operations.SpawnItem("MyItem", byte.MaxValue, new[] { 1f, 1f }, null, true));

            OperationResponse data = ReceiveOperationResponse(client);

            Assert.AreEqual("MyItem", data.Parameters[(byte)ParameterCode.ItemId]);
            Assert.AreEqual(byte.MaxValue, data.Parameters[(byte)ParameterCode.ItemType]);

            if (data.ReturnCode != (int)ReturnCode.Ok)
            {
                // move item to view area
                client.SendOperation(Operations.Move("MyItem", byte.MaxValue, new[] { 1f, 1f }));

                ReceiveOperationResponse(client, ReturnCode.Ok);
            }
        }
    }
}