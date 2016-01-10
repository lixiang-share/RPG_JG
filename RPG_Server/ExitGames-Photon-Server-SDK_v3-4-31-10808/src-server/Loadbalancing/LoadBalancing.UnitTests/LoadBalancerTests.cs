// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadBalancerTests.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the LoadBalancerTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    using ExitGames.Logging;
    using ExitGames.Logging.Log4Net;

    using log4net.Config;

    using NUnit.Framework;

    using Photon.LoadBalancing.LoadBalancer;
    using Photon.LoadBalancing.LoadShedding;

    [TestFixture]
    public class LoadBalancerTests
    {
        private LoadBalancer<Server> balancer;
        private List<Server> servers;

        [TestFixtureSetUp]
        public void Setup()
        {
            LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
            XmlConfigurator.ConfigureAndWatch(new FileInfo("log4net.config")); 

            this.balancer = new LoadBalancer<Server>();
            
            this.servers = new List<Server>();
            for (int i = 0; i < 5; i++)
            {
                this.servers.Add(new Server { Name = "Server" + i });
            }
        }

        [Test]
        public void Basics()
        {
            Server server;

            this.balancer = new LoadBalancer<Server>();
            bool result = this.balancer.TryGetServer(out server);
            Assert.IsFalse(result);

            result = this.balancer.TryAddServer(this.servers[0], FeedbackLevel.Highest);
            Assert.IsTrue(result);

            result = this.balancer.TryAddServer(this.servers[0], FeedbackLevel.Highest);
            Assert.IsFalse(result);

            result = this.balancer.TryGetServer(out server);
            Assert.IsFalse(result);

            result = this.balancer.TryUpdateServer(this.servers[0], FeedbackLevel.Normal);
            Assert.IsTrue(result);

            result = this.balancer.TryGetServer(out server);
            Assert.IsTrue(result);
            Assert.AreSame(this.servers[0], server);

            result = this.balancer.TryRemoveServer(this.servers[0]);
            Assert.IsTrue(result);

            result = this.balancer.TryRemoveServer(this.servers[0]);
            Assert.IsFalse(result);
        }

        [Test]
        public void LoadSpreadByDefault()
        {
            const int Count = 100000;

            // default, as per DefaultConfiguration.GetDefaultWeights: 
            /*
            var loadLevelWeights = new int[]
            {
                40, // FeedbackLevel.Lowest
                30, // FeedbackLevel.Low
                20, // FeedbackLevel.Normal
                10, // FeedbackLevel.High
                0 // FeedbackLevel.Highest
            };
             */

            this.balancer = new LoadBalancer<Server>();

            for (int i = 0; i < this.servers.Count; i++)
            {
                bool result = this.balancer.TryAddServer(this.servers[i], FeedbackLevel.Lowest);
                Assert.IsTrue(result);
            }

            // 5 servers with a load level of lowest
            // every server should get about 20 percent of the assigments
            this.AssignServerLoop(Count);
            for (int i = 0; i < this.servers.Count; i++)
            {
                this.CheckServer(this.servers[i], Count, 20, 5);
            }

            this.UpdateServer(this.servers[0], FeedbackLevel.Lowest);
            this.UpdateServer(this.servers[1], FeedbackLevel.Low);
            this.UpdateServer(this.servers[2], FeedbackLevel.Normal);
            this.UpdateServer(this.servers[3], FeedbackLevel.High);
            this.UpdateServer(this.servers[4], FeedbackLevel.Highest);
            
            this.AssignServerLoop(Count);
            this.CheckServer(this.servers[0], Count, 40, 5);
            this.CheckServer(this.servers[1], Count, 30, 5);
            this.CheckServer(this.servers[2], Count, 20, 5);
            this.CheckServer(this.servers[3], Count, 10, 5);
            this.CheckServer(this.servers[4], Count, 0, 0);

            this.UpdateServer(this.servers[0], FeedbackLevel.Lowest);
            this.UpdateServer(this.servers[1], FeedbackLevel.Low);
            this.UpdateServer(this.servers[2], FeedbackLevel.Low);
            this.UpdateServer(this.servers[3], FeedbackLevel.Normal);
            this.UpdateServer(this.servers[4], FeedbackLevel.Normal);
            this.AssignServerLoop(Count);
            this.CheckServer(this.servers[0], Count, 28, 5);
            this.CheckServer(this.servers[1], Count, 21, 5);
            this.CheckServer(this.servers[2], Count, 21, 5);
            this.CheckServer(this.servers[3], Count, 14, 5);
            this.CheckServer(this.servers[4], Count, 14, 5);

            this.UpdateServer(this.servers[0], FeedbackLevel.Low);
            this.UpdateServer(this.servers[1], FeedbackLevel.Normal);
            this.UpdateServer(this.servers[2], FeedbackLevel.Normal);
            this.UpdateServer(this.servers[3], FeedbackLevel.Normal);
            this.UpdateServer(this.servers[4], FeedbackLevel.Normal);
            this.AssignServerLoop(Count);
            this.CheckServer(this.servers[0], Count, 27, 5);
            this.CheckServer(this.servers[1], Count, 18, 5);
            this.CheckServer(this.servers[2], Count, 18, 5);
            this.CheckServer(this.servers[3], Count, 18, 5);
            this.CheckServer(this.servers[4], Count, 18, 5);

            this.UpdateServer(this.servers[0], FeedbackLevel.Low);
            this.UpdateServer(this.servers[1], FeedbackLevel.Highest);
            this.UpdateServer(this.servers[2], FeedbackLevel.Highest);
            this.UpdateServer(this.servers[3], FeedbackLevel.Highest);
            this.UpdateServer(this.servers[4], FeedbackLevel.Highest);
            this.AssignServerLoop(Count);
            this.CheckServer(this.servers[0], Count, 100, 0);
            this.CheckServer(this.servers[1], Count, 0, 0);
            this.CheckServer(this.servers[2], Count, 0, 0);
            this.CheckServer(this.servers[3], Count, 0, 0);
            this.CheckServer(this.servers[4], Count, 0, 0);

            this.UpdateServer(this.servers[0], FeedbackLevel.Highest);
            Server server;
            Assert.IsFalse(this.balancer.TryGetServer(out server));
        }

        [Test]
        public void LoadSpread()
        {
            const int Count = 100000;

            var loadLevelWeights = new int[] 
            { 
                50, // FeedbackLevel.Lowest
                30, // FeedbackLevel.Low
                15, // FeedbackLevel.Normal
                5, // FeedbackLevel.High
                0 // FeedbackLevel.Highest
            };

            this.balancer = new LoadBalancer<Server>(loadLevelWeights, 42);

            for (int i = 0; i < this.servers.Count; i++)
            {
                bool result = this.balancer.TryAddServer(this.servers[i], FeedbackLevel.Lowest);
                Assert.IsTrue(result);
            }

            // 5 servers with a load level of lowest
            // every server should get about 20 percent of the assigments
            this.AssignServerLoop(Count);
            for (int i = 0; i < this.servers.Count; i++)
            {
                this.CheckServer(this.servers[i], Count, 20, 5);
            }

            this.UpdateServer(this.servers[0], FeedbackLevel.Lowest);
            this.UpdateServer(this.servers[1], FeedbackLevel.Low);
            this.UpdateServer(this.servers[2], FeedbackLevel.Normal);
            this.UpdateServer(this.servers[3], FeedbackLevel.High);
            this.UpdateServer(this.servers[4], FeedbackLevel.Highest);
            this.AssignServerLoop(Count);
            this.CheckServer(this.servers[0], Count, 50, 5);
            this.CheckServer(this.servers[1], Count, 30, 5);
            this.CheckServer(this.servers[2], Count, 15, 5);
            this.CheckServer(this.servers[3], Count, 5, 5);
            this.CheckServer(this.servers[4], Count, 0, 0);

            this.UpdateServer(this.servers[0], FeedbackLevel.Lowest);
            this.UpdateServer(this.servers[1], FeedbackLevel.Low);
            this.UpdateServer(this.servers[2], FeedbackLevel.Low);
            this.UpdateServer(this.servers[3], FeedbackLevel.Normal);
            this.UpdateServer(this.servers[4], FeedbackLevel.Normal);
            this.AssignServerLoop(Count);
            this.CheckServer(this.servers[0], Count, 36, 5);
            this.CheckServer(this.servers[1], Count, 21, 5);
            this.CheckServer(this.servers[2], Count, 21, 5);
            this.CheckServer(this.servers[3], Count, 11, 5);
            this.CheckServer(this.servers[4], Count, 11, 5);

            this.UpdateServer(this.servers[0], FeedbackLevel.Low);
            this.UpdateServer(this.servers[1], FeedbackLevel.Normal);
            this.UpdateServer(this.servers[2], FeedbackLevel.Normal);
            this.UpdateServer(this.servers[3], FeedbackLevel.Normal);
            this.UpdateServer(this.servers[4], FeedbackLevel.Normal);
            this.AssignServerLoop(Count);
            this.CheckServer(this.servers[0], Count, 33, 5);
            this.CheckServer(this.servers[1], Count, 17, 5);
            this.CheckServer(this.servers[2], Count, 17, 5);
            this.CheckServer(this.servers[3], Count, 17, 5);
            this.CheckServer(this.servers[4], Count, 17, 5);

            this.UpdateServer(this.servers[0], FeedbackLevel.Low);
            this.UpdateServer(this.servers[1], FeedbackLevel.Highest);
            this.UpdateServer(this.servers[2], FeedbackLevel.Highest);
            this.UpdateServer(this.servers[3], FeedbackLevel.Highest);
            this.UpdateServer(this.servers[4], FeedbackLevel.Highest);
            this.AssignServerLoop(Count);
            this.CheckServer(this.servers[0], Count, 100, 0);
            this.CheckServer(this.servers[1], Count, 0, 0);
            this.CheckServer(this.servers[2], Count, 0, 0);
            this.CheckServer(this.servers[3], Count, 0, 0);
            this.CheckServer(this.servers[4], Count, 0, 0);

            this.UpdateServer(this.servers[0], FeedbackLevel.Highest);
            Server server;
            Assert.IsFalse(this.balancer.TryGetServer(out server));
        }

        [Test]
        public void LoadSpreadFromConfig()
        {
            const int Count = 100000;

            var configPath = "LoadBalancer.config";
            this.balancer = new LoadBalancer<Server>(configPath); 

            for (int i = 0; i < this.servers.Count; i++)
            {
                bool result = this.balancer.TryAddServer(this.servers[i], FeedbackLevel.Lowest);
                Assert.IsTrue(result);
            }

            // 5 servers with a load level of lowest
            // every server should get about 20 percent of the assigments
            this.AssignServerLoop(Count);
            for (int i = 0; i < this.servers.Count; i++)
            {
                this.CheckServer(this.servers[i], Count, 20, 5);
            }

            this.UpdateServer(this.servers[0], FeedbackLevel.Lowest);
            this.UpdateServer(this.servers[1], FeedbackLevel.Low);
            this.UpdateServer(this.servers[2], FeedbackLevel.Normal);
            this.UpdateServer(this.servers[3], FeedbackLevel.High);
            this.UpdateServer(this.servers[4], FeedbackLevel.Highest);
            this.AssignServerLoop(Count);
            this.CheckServer(this.servers[0], Count, 25, 5);
            this.CheckServer(this.servers[1], Count, 25, 5);
            this.CheckServer(this.servers[2], Count, 25, 5);
            this.CheckServer(this.servers[3], Count, 25, 5);
            this.CheckServer(this.servers[4], Count, 0, 0);

            this.UpdateServer(this.servers[0], FeedbackLevel.Lowest);
            this.UpdateServer(this.servers[1], FeedbackLevel.Low);
            this.UpdateServer(this.servers[2], FeedbackLevel.Low);
            this.UpdateServer(this.servers[3], FeedbackLevel.Normal);
            this.UpdateServer(this.servers[4], FeedbackLevel.Normal);
            this.AssignServerLoop(Count);
            this.CheckServer(this.servers[0], Count, 20, 5);
            this.CheckServer(this.servers[1], Count, 20, 5);
            this.CheckServer(this.servers[2], Count, 20, 5);
            this.CheckServer(this.servers[3], Count, 20, 5);
            this.CheckServer(this.servers[4], Count, 20, 5);

            this.UpdateServer(this.servers[0], FeedbackLevel.Low);
            this.UpdateServer(this.servers[1], FeedbackLevel.Normal);
            this.UpdateServer(this.servers[2], FeedbackLevel.Normal);
            this.UpdateServer(this.servers[3], FeedbackLevel.Normal);
            this.UpdateServer(this.servers[4], FeedbackLevel.Normal);
            this.AssignServerLoop(Count);
            this.CheckServer(this.servers[0], Count, 20, 5);
            this.CheckServer(this.servers[1], Count, 20, 5);
            this.CheckServer(this.servers[2], Count, 20, 5);
            this.CheckServer(this.servers[3], Count, 20, 5);
            this.CheckServer(this.servers[4], Count, 20, 5);

            this.UpdateServer(this.servers[0], FeedbackLevel.Low);
            this.UpdateServer(this.servers[1], FeedbackLevel.Highest);
            this.UpdateServer(this.servers[2], FeedbackLevel.Highest);
            this.UpdateServer(this.servers[3], FeedbackLevel.Highest);
            this.UpdateServer(this.servers[4], FeedbackLevel.Highest);
            this.AssignServerLoop(Count);
            this.CheckServer(this.servers[0], Count, 100, 0);
            this.CheckServer(this.servers[1], Count, 0, 0);
            this.CheckServer(this.servers[2], Count, 0, 0);
            this.CheckServer(this.servers[3], Count, 0, 0);
            this.CheckServer(this.servers[4], Count, 0, 0);

            this.UpdateServer(this.servers[0], FeedbackLevel.Highest);
            Server server;
            Assert.IsFalse(this.balancer.TryGetServer(out server));
        }

        [Test]
        public void LoadSpreadAfterConfigChange()
        {
            const int Count = 100000;

            var configPath = "LoadBalancer.config";
            this.balancer = new LoadBalancer<Server>(configPath);

            for (int i = 0; i < this.servers.Count; i++)
            {
                bool result = this.balancer.TryAddServer(this.servers[i], FeedbackLevel.Lowest);
                Assert.IsTrue(result);
            }

            // 5 servers with a load level of lowest
            // every server should get about 20 percent of the assigments
            this.AssignServerLoop(Count);
            for (int i = 0; i < this.servers.Count; i++)
            {
                this.CheckServer(this.servers[i], Count, 20, 5);
            }

            this.UpdateServer(this.servers[0], FeedbackLevel.Lowest);
            this.UpdateServer(this.servers[1], FeedbackLevel.Low);
            this.UpdateServer(this.servers[2], FeedbackLevel.Normal);
            this.UpdateServer(this.servers[3], FeedbackLevel.High);
            this.UpdateServer(this.servers[4], FeedbackLevel.Highest);
           
            this.AssignServerLoop(Count);
            this.CheckServer(this.servers[0], Count, 25, 5);
            this.CheckServer(this.servers[1], Count, 25, 5);
            this.CheckServer(this.servers[2], Count, 25, 5);
            this.CheckServer(this.servers[3], Count, 25, 5);
            this.CheckServer(this.servers[4], Count, 0, 0);

            File.Copy("LoadBalancer.config", "LoadBalancer.config.bak", true);
            File.Delete("LoadBalancer.config");
            
            // wait a bit until the update is done: 
            Thread.Sleep(1000);

            this.AssignServerLoop(Count);

            this.CheckServer(this.servers[0], Count, 40, 5);
            this.CheckServer(this.servers[1], Count, 30, 5);
            this.CheckServer(this.servers[2], Count, 20, 5);
            this.CheckServer(this.servers[3], Count, 10, 5);
            this.CheckServer(this.servers[4], Count, 0, 0);

            File.Copy("LoadBalancer.config.bak", "LoadBalancer.config", true);

            // wait a bit until the update is done: 
            Thread.Sleep(1000);

            this.AssignServerLoop(Count);
            this.CheckServer(this.servers[0], Count, 25, 5);
            this.CheckServer(this.servers[1], Count, 25, 5);
            this.CheckServer(this.servers[2], Count, 25, 5);
            this.CheckServer(this.servers[3], Count, 25, 5);
            this.CheckServer(this.servers[4], Count, 0, 0);
        }

        private void AssignServerLoop(int count)
        {
            this.ResetServerCount();

            for (int i = 0; i < count; i++)
            {
                Server server;
                bool result = this.balancer.TryGetServer(out server);
                Assert.IsTrue(result);
                server.Count++;
            }
        }

        private void ResetServerCount()
        {
            for (int i = 0; i < this.servers.Count; i++)
            {
                this.servers[i].Count = 0;
            }
        }

        private void CheckServer(Server server, int count, int expectedPercent, int toleranceInPercent)
        {
            int expectedCount = count * expectedPercent / 100;
            int tolerance = Math.Abs(expectedCount * toleranceInPercent / 100);

            int difference = Math.Abs(expectedCount - server.Count);
            if (difference > tolerance)
            {
                Assert.Fail(
                    "{0} has an unexpected count of assigments. Expected a value between {1} and {2} but is {3}", 
                    server.Name, 
                    expectedCount - tolerance, 
                    expectedCount + tolerance, 
                    server.Count);
            }
        }

        private void UpdateServer(Server server, FeedbackLevel newLoadLevel)
        {
            bool result = this.balancer.TryUpdateServer(server, newLoadLevel);
            Assert.IsTrue(result, "Failed to update {0} to the load level '{1}'", server.Name, newLoadLevel);
        }

        private class Server
        {
            public string Name { get; set; }

            public int Count { get; set; }

            public override string ToString()
            {
                return string.Format("{0}: Count={1}", this.Name, this.Count);
            }
        }
    }
}
