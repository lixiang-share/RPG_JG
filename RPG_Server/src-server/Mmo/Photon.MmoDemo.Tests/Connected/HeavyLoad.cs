// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeavyLoad.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The heavy load.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Tests.Connected
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    using ExitGames.Client.Photon;
    using ExitGames.Logging;

    using NUnit.Framework;

    using Photon.MmoDemo.Common;
    using Photon.MmoDemo.Server;

    using Settings = Photon.MmoDemo.Tests.Settings;

    /// <summary>
    ///   The heavy load.
    /// </summary>
    [TestFixture]
    [Explicit]
    public class HeavyLoad
    {
        #region Constants and Fields

        /// <summary>
        ///   The logger.
        /// </summary>
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///   The clients.
        /// </summary>
        private static int clientCount;

        #endregion

        #region Public Methods

        /// <summary>
        ///   The thread run.
        /// </summary>
        [Test]
        public void Run()
        {
            MmoWorldCache.Instance.Clear();
            MmoWorld world;
            MmoWorldCache.Instance.TryCreate(
                "HeavyLoad2", (new[] { 0f, 0f }).ToVector(), (new[] { 99f, 99f }).ToVector(), (new[] { 10f, 10f }).ToVector(), out world);

            using (var client = new Client(string.Empty))
            {
                Assert.IsTrue(client.Connect());
                CreateWorld(world, client);
            }

            Stopwatch t = Stopwatch.StartNew();

            var clients = new List<Client>();
            try
            {
                SetupClients(world, clients, t);

                Client.ResetStats();
                log.Info("ItemPositionUpdate wait completed");
                Assert.AreEqual(0, Client.Exceptions, "Exceptions occured at exit");

                t = Stopwatch.StartNew();
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
                Client.ResetStats();
                log.Info("move completed");
                Assert.AreEqual(0, Client.Exceptions, "Exceptions occured at exit");

                DisconnectClients(clients);
            }
            finally
            {
                clients.ForEach(c => c.Dispose());
            }
        }

        /// <summary>
        ///   The run for time.
        /// </summary>
        [Test]
        public void RunForTime()
        {
            MmoWorldCache.Instance.Clear();
            MmoWorld world;
            MmoWorldCache.Instance.TryCreate(
                "HeavyLoad3", (new[] { 0f, 0f }).ToVector(), (new[] { 299f, 299f }).ToVector(), (new[] { 10f, 10f }).ToVector(), out world);

            using (var client = new Client(string.Empty))
            {
                Assert.IsTrue(client.Connect());
                CreateWorld(world, client);
            }

            Stopwatch t = Stopwatch.StartNew();

            var clients = new List<Client>();
            try
            {
                SetupClients(world, clients, t);

                Client.ResetStats();
                t = Stopwatch.StartNew();
                while (t.ElapsedMilliseconds < 10000)
                {
                    MoveClients(clients);
                    PrintStats(t);
                    Client.ResetStats();
                }

                log.Info("move completed");
                Assert.AreEqual(0, Client.Exceptions, "Exceptions occured at exit");

                DisconnectClients(clients);
            }
            finally
            {
                clients.ForEach(c => c.Dispose());
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The begin receive event.
        /// </summary>
        /// <param name = "client">
        ///   The client.
        /// </param>
        /// <param name = "eventCode">
        ///   The event code.
        /// </param>
        /// <param name = "checkAction">
        ///   The check action.
        /// </param>
        /// <param name = "delay">
        ///   The delay.
        /// </param>
        private static void BeginReceiveEvent(Client client, EventCode eventCode, Func<EventData, bool> checkAction, int delay)
        {
            client.BeginReceiveEvent(eventCode, checkAction, delay);
        }

        /// <summary>
        ///   The begin receive event.
        /// </summary>
        /// <param name = "client">
        ///   The client.
        /// </param>
        /// <param name = "eventCode">
        ///   The event code.
        /// </param>
        /// <param name = "delay">
        ///   The delay.
        /// </param>
        private static void BeginReceiveEvent(Client client, EventCode eventCode, int delay)
        {
            client.BeginReceiveEvent(eventCode, d => true, delay);
        }

        /// <summary>
        ///   The create world.
        /// </summary>
        /// <param name = "world">
        ///   The world.
        /// </param>
        /// <param name = "client">
        ///   The client.
        /// </param>
        private static void CreateWorld(MmoWorld world, Client client)
        {
            Operations.CreateWorld(client, world.Name, world.Area.Min.ToFloatArray(2), world.Area.Max.ToFloatArray(2), world.TileDimensions.ToFloatArray(2));

            client.BeginReceiveResponse(0);

            OperationResponse data;
            Assert.IsTrue(client.EndReceiveResponse(Settings.WaitTime, out data), "Response not received");
            Assert.IsTrue(data.ReturnCode == (int)ReturnCode.Ok || data.ReturnCode == (int)ReturnCode.WorldAlreadyExists);
        }

        /// <summary>
        ///   The disconnect clients.
        /// </summary>
        /// <param name = "clients">
        ///   The clients.
        /// </param>
        private static void DisconnectClients(List<Client> clients)
        {
            Stopwatch t = Stopwatch.StartNew();
            clients.ForEach(ExitWorldBegin);
            clients.ForEach(ExitWorldEnd);
            log.Info("exit completed");
            PrintStats(t);
            Client.ResetStats();
            Assert.AreEqual(0, Client.Exceptions, "Exceptions occured at exit");

            t = Stopwatch.StartNew();
            clients.ForEach(c => c.BeginDisconnect());
            clients.ForEach(c => c.EndDisconnect());
            log.Info("disconnect completed");
            PrintStats(t);
            Client.ResetStats();
            Assert.AreEqual(0, Client.Exceptions, "Exceptions occured at exit");
        }

        /// <summary>
        ///   The end receive event.
        /// </summary>
        /// <param name = "client">
        ///   The client.
        /// </param>
        /// <param name = "eventCode">
        ///   The event EventCode.
        /// </param>
        /// <returns>
        ///   the received event
        /// </returns>
        private static EventData EndReceiveEvent(Client client, EventCode eventCode)
        {
            EventData data;
            Assert.IsTrue(client.EndReceiveEvent(Settings.WaitTime, out data), "Event not received");
            Assert.AreEqual(eventCode, (EventCode)data.Code);
            return data;
        }

        /// <summary>
        ///   The enter world.
        /// </summary>
        /// <param name = "client">
        ///   The client.
        /// </param>
        /// <param name = "world">
        ///   The world.
        /// </param>
        private static void EnterWorldBegin(Client client, MmoWorld world)
        {
            var viewDistanceEnter = new[] { 1f, 1f };
            var viewDistanceExit = new[] { 2f, 2f };

            ThreadPoolEnqueue(
                client, 
                () =>
                    {
                        Operations.EnterWorld(client, world.Name, client.Username, null, client.Position, viewDistanceEnter, viewDistanceExit);
                        client.BeginReceiveResponse(10);
                    });
        }

        /// <summary>
        ///   The enter world end.
        /// </summary>
        /// <param name = "client">
        ///   The client.
        /// </param>
        private static void EnterWorldEnd(Client client)
        {
            OperationResponse data;
            Assert.IsTrue(client.EndReceiveResponse(Settings.WaitTime, out data));
        }

        /// <summary>
        ///   The exit world begin.
        /// </summary>
        /// <param name = "client">
        ///   The client.
        /// </param>
        private static void ExitWorldBegin(Client client)
        {
            ThreadPoolEnqueue(client, () => Operations.ExitWorld(client));
        }

        /// <summary>
        ///   The exit world end.
        /// </summary>
        /// <param name = "client">
        ///   The client.
        /// </param>
        private static void ExitWorldEnd(Client client)
        {
            BeginReceiveEvent(client, EventCode.WorldExited, 0);
            EndReceiveEvent(client, EventCode.WorldExited);
        }

        /// <summary>
        ///   The move operation.
        /// </summary>
        /// <param name = "client">
        ///   The client.
        /// </param>
        private static void Move(Client client)
        {
            client.OperationFiber.Enqueue(() => MoveAction(client, 1));
        }

        /// <summary>
        ///   The move action.
        /// </summary>
        /// <param name = "client">
        ///   The client.
        /// </param>
        /// <param name = "number">
        ///   The number.
        /// </param>
        private static void MoveAction(Client client, int number)
        {
            float[] pos = client.Position;
            Operations.Move(client, null, null, pos);
            number++;
            if (number < 6)
            {
                client.OperationFiber.Schedule(() => MoveAction(client, number), 100);
            }
        }

        /// <summary>
        ///   The move clients.
        /// </summary>
        /// <param name = "clients">
        ///   The clients.
        /// </param>
        private static void MoveClients(List<Client> clients)
        {
            clients.ForEach(
                c =>
                    {
                        Move(c);
                        BeginReceiveEvent(c, EventCode.ItemMoved, 10);
                    });

            clients.ForEach(c => EndReceiveEvent(c, EventCode.ItemMoved));
            Thread.Sleep(100);
            clients.ForEach(c => BeginReceiveEvent(c, EventCode.ItemMoved, 0));
            clients.ForEach(c => EndReceiveEvent(c, EventCode.ItemMoved));
            Thread.Sleep(100);
            clients.ForEach(c => BeginReceiveEvent(c, EventCode.ItemMoved, 0));
            clients.ForEach(c => EndReceiveEvent(c, EventCode.ItemMoved));
            Thread.Sleep(100);
            clients.ForEach(c => BeginReceiveEvent(c, EventCode.ItemMoved, 0));
            clients.ForEach(c => EndReceiveEvent(c, EventCode.ItemMoved));
            Thread.Sleep(100);
            clients.ForEach(c => BeginReceiveEvent(c, EventCode.ItemMoved, 0));
            clients.ForEach(c => EndReceiveEvent(c, EventCode.ItemMoved));
        }

        /// <summary>
        ///   The print stats.
        /// </summary>
        /// <param name = "t">
        ///   The stopwatch.
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
        }

        /// <summary>
        ///   The setup clients.
        /// </summary>
        /// <param name = "world">
        ///   The world.
        /// </param>
        /// <param name = "clients">
        ///   The clients.
        /// </param>
        /// <param name = "t">
        ///   The stopwatch.
        /// </param>
        private static void SetupClients(MmoWorld world, List<Client> clients, Stopwatch t)
        {
            for (int x = world.Area.Min.X + (world.TileDimensions.X / 2); x < world.Area.Max.X; x += world.TileDimensions.X)
            {
                for (int y = world.Area.Min.Y + (world.TileDimensions.Y / 2); y < world.Area.Max.Y; y += world.TileDimensions.Y)
                {
                    string name = string.Format("MyUsername{0}/{1}", x, y);
                    var client = new Client(name, new[] { x / 100f, y / 100f });
                    client.BeginConnect();
                    clients.Add(client);
                    clientCount++;
                }

                for (int y = world.Area.Min.Y + (world.TileDimensions.Y / 2) + 1; y < world.Area.Max.Y; y += world.TileDimensions.Y)
                {
                    string name = string.Format("MyUsername{0}/{1}", x + 1, y);
                    var client = new Client(name, new[] { (x + 1) / 100f, y / 100f });
                    client.BeginConnect();
                    clients.Add(client);
                    clientCount++;
                }
            }

            clients.ForEach(c => Assert.IsTrue(c.EndConnect()));
            log.InfoFormat("connect completed, {0} clients", clientCount);

            clients.ForEach(c => EnterWorldBegin(c, world));
            clients.ForEach(EnterWorldEnd);
            clients.ForEach(c => BeginReceiveEvent(c, EventCode.ItemSubscribed, d => (string)d[(byte)ParameterCode.ItemId] != c.Username, 500));
            clients.ForEach(c => EndReceiveEvent(c, EventCode.ItemSubscribed));
            PrintStats(t);
            Client.ResetStats();
            Assert.AreEqual(0, Client.Exceptions, "Exceptions occured at start");
            log.Info("enter completed");
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
        ////private static Hashtable ReceiveEvent(Client client, Code eventCode, Func<Hashtable, bool> checkAction)
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
        ////private static Hashtable ReceiveEvent(Client client, Code eventCode)
        ////{
        ////    return ReceiveEvent(client, eventCode, d => true);
        ////}

        /// <summary>
        ///   The thread pool enqueue.
        /// </summary>
        /// <param name = "client">
        ///   The client.
        /// </param>
        /// <param name = "action">
        ///   The action.
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

        #endregion
    }
}