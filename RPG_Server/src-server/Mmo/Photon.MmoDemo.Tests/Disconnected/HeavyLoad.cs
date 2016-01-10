// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeavyLoad.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The heavy load.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Tests.Disconnected
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    using ExitGames.Concurrency.Fibers;
    using ExitGames.Logging;
    using ExitGames.Threading;

    using NUnit.Framework;

    using Photon.MmoDemo.Common;
    using Photon.MmoDemo.Server;

    using SocketServer;
    using SocketServer.Diagnostics;
    using SocketServer.Mmo.Messages;

    using Settings = Tests.Settings;

    /// <summary>
    /// The heavy load.
    /// </summary>
    [TestFixture]
    [Explicit]
    public class HeavyLoad
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The clients.
        /// </summary>
        private static int clientCount;

        /// <summary>
        /// Tests the client setup.
        /// </summary>
        [Test]
        public void SetupClients()
        {
            MmoWorld world;
            MmoWorldCache.Instance.Clear();
            MmoWorldCache.Instance.TryCreate("TestWorld", (new[] { 0f, 0f }).ToVector(), (new[] { 399f, 399f }).ToVector(), (new[] { 10f, 10f }).ToVector(), out world);

            for (int i = 0; i < 10; i++)
            {
                List<Client> clients = SetupClients(world);
                DisconnectClients(clients);
            }
        }

        /// <summary>
        /// The run test.
        /// </summary>
        [Test]
        public void Run()
        {
            MmoWorld world;
            MmoWorldCache.Instance.Clear();
            MmoWorldCache.Instance.TryCreate("TestWorld", (new[] { 0f, 0f }).ToVector(), (new[] { 299f, 299f }).ToVector(), (new[] { 10f, 10f }).ToVector(), out world);

            List<Client> clients = SetupClients(world);

            Stopwatch t = Stopwatch.StartNew();
            using (var fiber = new PoolFiber(new FailSafeBatchExecutor()))
            {
                fiber.Start();
                fiber.ScheduleOnInterval(() => PrintStatsPerSecond(t), 1000, 1000);

                MoveClients(clients);
                PrintStats(t);
                MoveClients(clients);
                PrintStats(t);
                MoveClients(clients);
                PrintStats(t);
                MoveClients(clients);
                PrintStats(t);
                MoveClients(clients);
                PrintStats(t);
                log.Info("move completed");
                Assert.AreEqual(0, Client.Exceptions, "Exceptions occured at exit");

                DisconnectClients(clients);
            }
        }

        /// <summary>
        /// The run for time.
        /// </summary>
        [Test]
        public void RunForTime()
        {
            MmoWorldCache.Instance.Clear();
            MmoWorld world;
            MmoWorldCache.Instance.TryCreate("TestWorld", (new[] { 0f, 0f }).ToVector(), (new[] { 299f, 299f }).ToVector(), (new[] { 10f, 10f }).ToVector(), out world);

            List<Client> clients = SetupClients(world);

            Stopwatch t = Stopwatch.StartNew();
            using (var fiber = new PoolFiber(new FailSafeBatchExecutor()))
            {
                fiber.Start();
                fiber.ScheduleOnInterval(() => PrintStatsPerSecond(t), 1000, 1000);

                while (t.ElapsedMilliseconds < 10000)
                {
                    MoveClients(clients);
                    PrintStats(t);
                }
            }

            DisconnectClients(clients);
        }

        /// <summary>
        /// The begin receive event.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="eventCode">
        /// The event code.
        /// </param>
        /// <param name="checkAction">
        /// The check action.
        /// </param>
        private static void BeginReceiveEvent(Client client, EventCode eventCode, Func<EventData, bool> checkAction)
        {
            client.BeginReceiveEvent(eventCode, checkAction);
        }

        /// <summary>
        /// The begin receive event.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="eventCode">
        /// The event code.
        /// </param>
        private static void BeginReceiveEvent(Client client, EventCode eventCode)
        {
            client.BeginReceiveEvent(eventCode, d => true);
        }

        /// <summary>
        /// The disconnect clients.
        /// </summary>
        /// <param name="clients">
        /// The clients.
        /// </param>
        private static void DisconnectClients(List<Client> clients)
        {
            Stopwatch t = Stopwatch.StartNew();
            clients.ForEach(ExitWorldBegin);
            clients.ForEach(ExitWorldEnd);
            log.Info("exit completed");
            PrintStats(t);

            Assert.AreEqual(0, Client.Exceptions, "Exceptions occured at exit");

            t = Stopwatch.StartNew();
            clients.ForEach(c => c.Disconnect());
            Thread.Sleep(100);
            log.Info("disconnect completed");
            PrintStats(t);

            Assert.AreEqual(0, Client.Exceptions, "Exceptions occured at exit");
        }

        /// <summary>
        /// The end receive event.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="eventCode">
        /// The event EventCode.
        /// </param>
        /// <returns>
        /// the received event
        /// </returns>
        private static EventData EndReceiveEvent(Client client, EventCode eventCode)
        {
            EventData data;
            Assert.IsTrue(client.EndReceiveEvent(Settings.WaitTime, out data), "Event not received");
            Assert.AreEqual(eventCode, (EventCode)data.Code);
            return data;
        }

        /// <summary>
        /// The enter world.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="world">
        /// The world.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        private static void EnterWorldBegin(Client client, MmoWorld world, float[] position)
        {
            var viewDistanceEnter = new[] { 1f, 1f };
            var viewDistanceExit = new[] { 2f, 2f };

            client.Position = position;

            ThreadPoolEnqueue(
                client,
                () =>
                {
                    client.SendOperation(Operations.EnterWorld(world.Name, client.Username, null, position, viewDistanceEnter, viewDistanceExit));
                    client.BeginReceiveResponse();
                });
        }

        /// <summary>
        /// The enter world end.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        private static void EnterWorldEnd(Client client)
        {
            OperationResponse data;
            Assert.IsTrue(client.EndReceiveResponse(Settings.WaitTime, out data));
            Assert.AreEqual(0, data.ReturnCode);
        }

        /// <summary>
        /// The exit world begin.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        private static void ExitWorldBegin(Client client)
        {
            ThreadPoolEnqueue(
                client,
                () =>
                {
                    client.SendOperation(Operations.ExitWorld());
                    BeginReceiveEvent(client, EventCode.WorldExited);
                });
        }

        /// <summary>
        /// The exit world end.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        private static void ExitWorldEnd(Client client)
        {
            EndReceiveEvent(client, EventCode.WorldExited);
        }

        /// <summary>
        /// The client movement.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        private static void Move(Client client)
        {
            client.Peer.RequestFiber.Enqueue(() => MoveAction(client, 1));
        }

        /// <summary>
        /// The move action.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="number">
        /// The number.
        /// </param>
        private static void MoveAction(Client client, int number)
        {
            float[] pos = client.Position;
            client.SendOperation(Operations.Move(null, null, pos));
            number++;
            if (number < 6)
            {
                client.Peer.RequestFiber.Schedule(() => MoveAction(client, number), 100);
            }
        }

        /// <summary>
        /// The move clients.
        /// </summary>
        /// <param name="clients">
        /// The clients.
        /// </param>
        private static void MoveClients(List<Client> clients)
        {
            clients.ForEach(
                c =>
                {
                    Move(c);
                    BeginReceiveEvent(c, EventCode.ItemMoved);
                });

            clients.ForEach(c => EndReceiveEvent(c, EventCode.ItemMoved));
            Thread.Sleep(100);
            clients.ForEach(c => BeginReceiveEvent(c, EventCode.ItemMoved));
            clients.ForEach(c => EndReceiveEvent(c, EventCode.ItemMoved));
            Thread.Sleep(100);
            clients.ForEach(c => BeginReceiveEvent(c, EventCode.ItemMoved));
            clients.ForEach(c => EndReceiveEvent(c, EventCode.ItemMoved));
            Thread.Sleep(100);
            clients.ForEach(c => BeginReceiveEvent(c, EventCode.ItemMoved));
            clients.ForEach(c => EndReceiveEvent(c, EventCode.ItemMoved));
            Thread.Sleep(100);
            clients.ForEach(c => BeginReceiveEvent(c, EventCode.ItemMoved));
            clients.ForEach(c => EndReceiveEvent(c, EventCode.ItemMoved));
        }

        /// <summary>
        /// The print stats.
        /// </summary>
        /// <param name="t">
        /// The stopwatch.
        /// </param>
        private static void PrintStats(Stopwatch t)
        {
            log.InfoFormat(
                "{7:00000} Client({8}): {9} operations, {0} fast events in {1:0.00}ms avg, {2} middle events in {3:0.00}ms avg, {4} slow events in {5:0.00}ms avg, {6:0.00}ms max - {10} exceptions",
                Client.EventsReceivedFast,
                Client.EventsReceivedTimeTotalFast / (double)Client.EventsReceivedFast,
                Client.EventsReceivedMiddle,
                Client.EventsReceivedTimeTotalMiddle / (double)Client.EventsReceivedMiddle,
                Client.EventsReceivedSlow,
                Client.EventsReceivedTimeTotalSlow / (double)Client.EventsReceivedSlow,
                Client.EventsReceivedTimeMax,
                t.ElapsedMilliseconds,
                clientCount,
                Client.OperationsSent,
                Client.Exceptions);

            Client.ResetStats();
        }

        /// <summary>
        /// The print stats per second.
        /// </summary>
        /// <param name="t">
        /// The stopwatch.
        /// </param>
        private static void PrintStatsPerSecond(Stopwatch t)
        {
            log.InfoFormat(
                "{4:00000} Operations: {0:0.00} fast, {1:0.00} middle, {2:0.00} slow ({3}ms max)",
                PhotonCounter.OperationsFast.GetNextValue(),
                PhotonCounter.OperationsMiddle.GetNextValue(),
                PhotonCounter.OperationsSlow.GetNextValue(),
                PhotonCounter.OperationsMaxTime.RawValue,
                t.ElapsedMilliseconds);

            PhotonCounter.OperationsMaxTime.RawValue = 0;

            log.InfoFormat(
                "{2:00000} Itemessages: {0:0.00} sent, {1:0.00} received",
                MessageCounters.CounterSend.GetNextValue(),
                MessageCounters.CounterReceive.GetNextValue(),
                t.ElapsedMilliseconds);
        }

        /// <summary>
        /// The setup clients. creates 2 clients per region.
        /// </summary>
        /// <param name="world">
        /// The world.
        /// </param>
        /// <returns>
        /// the list of created clients
        /// </returns>
        private static List<Client> SetupClients(MmoWorld world)
        {
            Stopwatch t = Stopwatch.StartNew();

            var clients = new List<Client>();
            for (int x = world.Area.Min.X + (world.TileDimensions.X / 2); x < world.Area.Max.X; x += world.TileDimensions.X)
            {
                for (int y = world.Area.Min.Y + (world.TileDimensions.Y / 2);
                     y < world.Area.Max.Y;
                     y += world.TileDimensions.Y)
                {
                    string name = string.Format("MyUsername{0}/{1}", x, y);
                    var client = new Client(name);
                    EnterWorldBegin(client, world, new[] { x / 100f, y / 100f });

                    clients.Add(client);
                    clientCount++;

                    Sleep();
                }

                for (int y = world.Area.Min.Y + (world.TileDimensions.Y / 2) + 1;
                     y < world.Area.Max.Y;
                     y += world.TileDimensions.Y)
                {
                    string name = string.Format("MyUsername{0}/{1}", x + 1, y);
                    var client = new Client(name);
                    EnterWorldBegin(client, world, new[] { (x + 1) / 100f, y / 100f });

                    clients.Add(client);
                    clientCount++;

                    Sleep();
                }
            }

            clients.ForEach(EnterWorldEnd);
            clients.ForEach(c => BeginReceiveEvent(c, EventCode.ItemSubscribed, d => true));
            clients.ForEach(c => EndReceiveEvent(c, EventCode.ItemSubscribed));
            PrintStats(t);

            Assert.AreEqual(0, Client.Exceptions, "Exceptions occured at start");
            log.Info("enter completed");

            Assert.AreEqual(0, Client.Exceptions, "Exceptions occured at exit");

            return clients;
        }

        /// <summary>
        /// The sleep.
        /// </summary>
        private static void Sleep()
        {
            ////Thread.Sleep(1);
        }

        /////// <summary>
        /////// The receive event.
        /////// </summary>
        /////// <param name="client">
        /////// The client.
        /////// </param>
        /////// <param name="eventCode">
        /////// The event code.
        /////// </param>
        /////// <param name="checkAction">
        /////// The check action.
        /////// </param>
        /////// <returns>
        /////// </returns>
        ////private static EventData ReceiveEvent(Client client, Code eventCode, Func<EventData, bool> checkAction)
        ////{
        ////    BeginReceiveEvent(client, eventCode, checkAction);
        ////    return EndReceiveEvent(client);
        ////}

        /////// <summary>
        /////// The receive event.
        /////// </summary>
        /////// <param name="client">
        /////// The client.
        /////// </param>
        /////// <param name="eventCode">
        /////// The event code.
        /////// </param>
        /////// <returns>
        /////// </returns>
        ////private static EventData ReceiveEvent(Client client, Code eventCode)
        ////{
        ////    return ReceiveEvent(client, eventCode, d => true);
        ////}

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
        /////// </returns>
        ////private static EventData ReceiveOperationResponse(Client client, OperationCode operationCode)
        ////{
        ////    Func<EventData, bool> checkAction =
        ////        d => (byte)d.Parameters[(short)ParameterCode.OperationCode] == (byte)operationCode;
        ////    EventData data = ReceiveEvent(client, EventCode.OperationSuccess, checkAction);
        ////    var eventCode = (EventCode)(byte)data.Code;
        ////    Assert.IsTrue(eventCode == EventCode.OperationSuccess || eventCode == EventCode.OperationError);
        ////    Assert.AreEqual((OperationCode)(byte)data.Parameters[(short)ParameterCode.OperationCode], operationCode);
        ////    return data;
        ////}

        /// <summary>
        /// The thread pool enqueue.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        private static void ThreadPoolEnqueue(Client client, Action action)
        {
            ThreadPool.QueueUserWorkItem(
                o =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception e)
                    {
                        client.HandleException(e);
                    }
                });
        }
    }
}