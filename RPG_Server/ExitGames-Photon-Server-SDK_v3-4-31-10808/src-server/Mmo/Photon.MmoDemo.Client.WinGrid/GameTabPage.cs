// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameTabPage.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The double buffered game tab page.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Client.WinGrid
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    using ExitGames.Client.Photon;
    using ExitGames.Concurrency.Channels;
    using ExitGames.Concurrency.Core;
    using ExitGames.Concurrency.Fibers;
    using ExitGames.Diagnostics.Counter;

    using ExitGames.Logging;

    using Photon.MmoDemo.Common;

    /// <summary>
    /// The double buffered game tab page.
    /// </summary>
    [CLSCompliant(false)]
    public class GameTabPage : TabPage, IGameListener
    {
        /// <summary>
        /// The background color.
        /// </summary>
        internal static readonly Pen LinePen = new Pen(Color.Gray, 1);

        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The fiber.
        /// </summary>
        private readonly FormFiber fiber;

        /// <summary>
        /// The frame counter.
        /// </summary>
        private readonly CountsPerSecondCounter frameCounter = new CountsPerSecondCounter();

        /// <summary>
        /// The movement channel.
        /// </summary>
        private readonly Channel<MouseEventArgs> movementChannel = new Channel<MouseEventArgs>();

        /// <summary>
        /// The owned items.
        /// </summary>
        private readonly List<MyItem> ownedItems = new List<MyItem>();

        /// <summary>
        /// The random.
        /// </summary>
        private readonly Random random = new Random();

        /// <summary>
        /// The bot counter.
        /// </summary>
        private int botCounter;

        /// <summary>
        /// The frames per second.
        /// </summary>
        private float fps;

        /// <summary>
        /// The main camera.
        /// </summary>
        private InterestArea mainCamera;

        /// <summary>
        /// The running.
        /// </summary>
        private bool running;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameTabPage"/> class.
        /// </summary>
        public GameTabPage()
        {
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

            this.MouseMove += this.OnMouseMove;
            this.MouseClick += this.OnMouseClick;

            this.fiber = new FormFiber(this, new DefaultExecutor());
            this.fiber.Start();
        }

        /// <summary>
        /// The world entered.
        /// </summary>
        public event Action<GameTabPage> WorldEntered;

        /// <summary>
        /// Gets or sets BackgroundImage.
        /// </summary>
        public override Image BackgroundImage
        {
            get
            {
                return Settings.IslandImage;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets BackgroundImageLayout.
        /// </summary>
        public override ImageLayout BackgroundImageLayout
        {
            get
            {
                return ImageLayout.Stretch;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets Game.
        /// </summary>
        public Game Game { get; private set; }

        /// <summary>
        /// Gets a value indicating whether IsDebugLogEnabled.
        /// </summary>
        public bool IsDebugLogEnabled
        {
            get
            {
                return log.IsDebugEnabled;
            }
        }

        /// <summary>
        /// Gets MainCamera.
        /// </summary>
        public InterestArea MainCamera
        {
            get
            {
                return this.mainCamera;
            }
        }

        /// <summary>
        /// The attach interest area to next item.
        /// </summary>
        public void AttachInterestAreaToNextItem()
        {
            int nextIndex;
            if (this.mainCamera.AttachedItem == null)
            {
                nextIndex = 0;
            }
            else
            {
                int lastIndex = this.ownedItems.IndexOf(this.mainCamera.AttachedItem);
                nextIndex = lastIndex + 1;
                if (nextIndex >= this.ownedItems.Count)
                {
                    nextIndex = 0;
                }
            }

            this.mainCamera.AttachItem(this.ownedItems[nextIndex]);
        }

        /// <summary>
        /// The auto move.
        /// </summary>
        public void AutoMove()
        {
            if (this.Game.State == GameState.WorldEntered)
            {
                this.ownedItems.ForEach(this.AutoMoveStart);
            }
        }

        /// <summary>
        /// The destroy bot.
        /// </summary>
        public void DestroyBot()
        {
            // do not destroy avatar
            if (this.ownedItems.Count == 1)
            {
                return;
            }

            MyItem item = this.ownedItems[this.ownedItems.Count - 1];
            this.ownedItems.RemoveAt(this.ownedItems.Count - 1);
            item.Destroy();

            ////this.LogInfo(this.Game, (this.ownedItems.Count - 1) + " Bots running");
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        public void Initialize(Game game)
        {
            this.Text = game.Avatar.Text;
            this.Game = game;
            game.TryGetCamera(0, out this.mainCamera);
            this.ownedItems.Add(game.Avatar);
        }

        /////// <summary>
        /////// The log error.
        /////// </summary>
        /////// <param name="publisher">
        /////// The mmo game.
        /////// </param>
        /////// <param name="errorCode">
        /////// The error code.
        /////// </param>
        /////// <param name="debugMessage">
        /////// The debug message.
        /////// </param>
        /////// <param name="operationCode">
        /////// The operation code.
        /////// </param>
        ////public void LogError(Game publisher, ReturnCode errorCode, string debugMessage, OperationCode operationCode)
        ////{
        ////    this.LogError(publisher, debugMessage);
        ////}

        /// <summary>
        /// The run method.
        /// </summary>
        public void Run()
        {
            this.running = true;
            var settings = (Settings)this.Game.Settings;
            this.fiber.ScheduleOnInterval(this.GameLoop, settings.DrawInterval, settings.DrawInterval);
            this.fiber.ScheduleOnInterval(this.ReadFps, 1000, 10000);
            this.movementChannel.SubscribeToLast(this.fiber, this.OnMove, settings.SendInterval);
        }

        /// <summary>
        /// The spawn bot.
        /// </summary>
        public void SpawnBot()
        {
            string name = Environment.UserName + " Bot" + this.botCounter++;
            var item = new MyItem(Guid.NewGuid().ToString(), (byte)ItemType.Bot, this.Game, name);
            this.Game.AddItem(item);
            item.Spawn(this.GetRandomPosition(), null, this.Game.Avatar.Color, false);
            this.ownedItems.Add(item);

            ////this.LogInfo(this.Game, (this.ownedItems.Count - 1) + " Bots running");
        }

        #region Implemented Interfaces

        #region IGameListener

        /// <summary>
        /// The log debug.
        /// </summary>
        /// <param name="publisher">
        /// The mmo game.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public void LogDebug(Game publisher, string message)
        {
            log.Debug(message);
        }

        /// <summary>
        /// The log error.
        /// </summary>
        /// <param name="publisher">
        /// The mmo game.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public void LogError(Game publisher, string message)
        {
            log.Error(message);
        }

        /////// <summary>
        /////// The log error.
        /////// </summary>
        /////// <param name="publisher">
        /////// The mmo game.
        /////// </param>
        /////// <param name="errorCode">
        /////// The error code.
        /////// </param>
        /////// <param name="debugMessage">
        /////// The debug message.
        /////// </param>
        /////// <param name="operationCode">
        /////// The operation code.
        /////// </param>
        ////public void LogError(Game publisher, Photon.Mmo.Common.ReturnCode errorCode, string debugMessage, OperationCode operationCode)
        ////{
        ////    log.Error(debugMessage);
        ////}

        /// <summary>
        /// The log error.
        /// </summary>
        /// <param name="publisher">
        /// The mmo game.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public void LogError(Game publisher, Exception exception)
        {
            log.Error(exception);
        }

        /// <summary>
        /// The log info.
        /// </summary>
        /// <param name="publisher">
        /// The mmo game.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public void LogInfo(Game publisher, string message)
        {
            log.Info(message);
        }

        /// <summary>
        /// The on camera attached.
        /// </summary>
        /// <param name="itemId">
        /// The item Id.
        /// </param>
        /// <param name="itemType">
        /// The item Type.
        /// </param>
        public void OnCameraAttached(string itemId, byte itemType)
        {
        }

        /// <summary>
        /// The on camera detached.
        /// </summary>
        public void OnCameraDetached()
        {
        }

        /// <summary>
        /// The on connect.
        /// </summary>
        /// <param name="publisher">
        /// The mmo game.
        /// </param>
        public void OnConnect(Game publisher)
        {
            this.LogInfo(publisher, string.Format("{0}: connected", publisher.Avatar.Id));
        }

        /// <summary>
        /// The on disconnect.
        /// </summary>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        /// <param name="returnCode">
        /// The return code.
        /// </param>
        public void OnDisconnect(Game game, StatusCode returnCode)
        {
            this.LogInfo(game, string.Format("{0}: {1}", game.Avatar.Id, returnCode));
            this.running = false;
        }

        /// <summary>
        /// The on item added.
        /// </summary>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        /// <param name="item">
        /// The mmo item.
        /// </param>
        public void OnItemAdded(Game game, Item item)
        {
        }

        /// <summary>
        /// The on item removed.
        /// </summary>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        /// <param name="item">
        /// The mmo item.
        /// </param>
        public void OnItemRemoved(Game game, Item item)
        {
        }

        /// <summary>
        /// The on item spawned.
        /// </summary>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        public void OnItemSpawned(byte itemType, string itemId)
        {
            Item item;
            if (this.Game.TryGetItem(itemType, itemId, out item))
            {
                if (item.IsMine)
                {
                    this.AutoMoveStart((MyItem)item);
                }
            }
        }

        /// <summary>
        /// The on radar update.
        /// </summary>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        public void OnRadarUpdate(string itemId, byte itemType, float[] position)
        {
        }

        /// <summary>
        /// The on world entered.
        /// </summary>
        /// <param name="publisher">
        /// The mmo game.
        /// </param>
        public void OnWorldEntered(Game publisher)
        {
            this.LogInfo(publisher, string.Format("{0}: entered world {1}", publisher.Avatar.Id, publisher.WorldData.Name));
            publisher.Avatar.MoveAbsolute(this.GetRandomPosition(), null);
            if (((Settings)publisher.Settings).AutoMove)
            {
                this.AutoMove();
            }

            this.OnWorldEntered();
        }

        #endregion

        #endregion

        /// <summary>
        /// The paint world.
        /// </summary>
        /// <param name="grfx">
        /// The grafix.
        /// </param>
        /// <param name="boardSize">
        /// The board size.
        /// </param>
        /// <param name="world">
        /// The world.
        /// </param>
        internal static void PaintGrid(Graphics grfx, int[] boardSize, WorldData world)
        {
            float width = world.Width;
            float height = world.Height;
            float w = boardSize[0] / width;
            float h = boardSize[1] / height;
            float boxSizeX = w * world.TileDimensions[0];
            float boxSizeY = h * world.TileDimensions[1];

            for (float y = boxSizeY; y < boardSize[1] - 1; y += boxSizeY)
            {
                grfx.DrawLine(LinePen, new Point(0, (int)y), new Point(boardSize[0], (int)y));
            }

            for (float x = boxSizeX; x < boardSize[0] - 1; x += boxSizeX)
            {
                grfx.DrawLine(LinePen, new Point((int)x, 0), new Point((int)x, boardSize[1]));
            }
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.movementChannel.ClearSubscribers();
                this.fiber.Stop();
                this.fiber.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// The on paint.
        /// </summary>
        /// <param name="e">
        /// The PaintEventArgs.
        /// </param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.TopLevelControl == null)
            {
                return;
            }

            Game engine = this.Game;

            switch (engine.State)
            {
                case GameState.WorldEntered:
                    {
                        var boardSize = new[] { e.ClipRectangle.Width, e.ClipRectangle.Height };
                        PaintGrid(e.Graphics, boardSize, engine.WorldData);

                        Dictionary<string, Item> bots;
                        if (engine.Items.TryGetValue((byte)ItemType.Bot, out bots))
                        {
                            foreach (Item bot in bots.Values)
                            {
                                if (bot.IsVisible)
                                {
                                    this.PaintActor(engine, e.Graphics, boardSize, bot);
                                }
                            }
                        }

                        Dictionary<string, Item> actors = engine.Items[(byte)ItemType.Avatar];
                        foreach (Item actor in actors.Values)
                        {
                            if (actor.IsVisible)
                            {
                                this.PaintActor(engine, e.Graphics, boardSize, actor);
                            }
                        }

                        PaintCamera(engine, e.Graphics, boardSize, this.mainCamera);
                        string rttString = string.Format(
                            "RTT/Var: {0}/{1}; FPS {2:0.00}; Bots: {3}", 
                            engine.Peer.RoundTripTime, 
                            engine.Peer.RoundTripTimeVariance, 
                            this.fps, 
                            this.ownedItems.Count - 1);
                        e.Graphics.DrawString(rttString, this.Font, Settings.IslandTextBrush, 0, 0);
                        break;
                    }

                case GameState.Disconnected:
                    {
                        e.Graphics.DrawString(engine.State.ToString(), this.Font, Settings.IslandTextBrush, 0, 0);
                        e.Graphics.DrawString("Press + to open new tab", this.Font, Settings.IslandTextBrush, 0, this.Font.SizeInPoints + 4);
                        e.Graphics.DrawString("Press - to close tab", this.Font, Settings.IslandTextBrush, 0, (this.Font.SizeInPoints + 4) * 2);
                        break;
                    }

                default:
                    {
                        e.Graphics.DrawString(engine.State.ToString(), this.Font, Settings.IslandTextBrush, 0, 0);
                        break;
                    }
            }
        }

        /// <summary>
        /// The paint camera.
        /// </summary>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        /// <param name="graphics">
        /// The graphics.
        /// </param>
        /// <param name="boardSize">
        /// The board size.
        /// </param>
        /// <param name="camera">
        /// The camera.
        /// </param>
        private static void PaintCamera(Game game, Graphics graphics, int[] boardSize, InterestArea camera)
        {
            Color color = Color.FromArgb(game.Avatar.Color);
            var pen = new Pen(color);

            WorldData world = game.WorldData;

            float width = world.Width;
            float height = world.Height;
            float w = boardSize[0] / width;
            float h = boardSize[1] / height;

            float x = ((world.BottomRightCorner[0] - camera.Position[0]) * w) - 1;
            float y = (camera.Position[1] * h) - 1;

            // draw view distance
            graphics.DrawRectangle(
                pen, 
                x - (camera.ViewDistanceEnter[0] * w), 
                y - (camera.ViewDistanceEnter[1] * h), 
                w * 2 * camera.ViewDistanceEnter[0], 
                h * 2 * camera.ViewDistanceEnter[1]);

            // draw view distance exit
            graphics.DrawRectangle(
                pen, 
                x - (camera.ViewDistanceExit[0] * w), 
                y - (camera.ViewDistanceExit[1] * h), 
                w * 2 * camera.ViewDistanceExit[0], 
                h * 2 * camera.ViewDistanceExit[1]);

            // make view distance distance thicker 
            graphics.DrawRectangle(
                pen, 
                x - (camera.ViewDistanceEnter[0] * w) + 1, 
                y - (camera.ViewDistanceEnter[1] * h) + 1, 
                (w * camera.ViewDistanceEnter[0] * 2) - 2, 
                (h * camera.ViewDistanceEnter[1] * 2) - 2);

            // make view distance distance thicker 
            graphics.DrawRectangle(
                pen, 
                x - (camera.ViewDistanceExit[0] * w) + 1, 
                y - (camera.ViewDistanceExit[1] * h) + 1, 
                (w * camera.ViewDistanceExit[0] * 2) - 2, 
                (h * camera.ViewDistanceExit[1] * 2) - 2);
        }

        /// <summary>
        /// The auto move.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="horizontal">
        /// The horizontal.
        /// </param>
        /// <param name="vertical">
        /// The vertical.
        /// </param>
        /// <param name="t">
        /// The stop watch.
        /// </param>
        /// <param name="item">
        /// The mmo item.
        /// </param>
        private void AutoMove(Settings settings, int horizontal, int vertical, Stopwatch t, MyItem item)
        {
            try
            {
                if (item.IsDestroyed)
                {
                    item.IsMoving = false;
                    return;
                }

                if (settings.AutoMove && this.Game.State == GameState.WorldEntered)
                {
                    if (t.ElapsedMilliseconds < settings.AutoMoveInterval)
                    {
                        if (false == item.MoveRelative(new float[] { horizontal, vertical }, null))
                        {
                            float[] newPos = this.GetRandomPosition();
                            item.MoveAbsolute(newPos, null);
                        }

                        this.fiber.Schedule(() => this.AutoMove(settings, horizontal, vertical, t, item), settings.SendInterval);
                    }
                    else
                    {
                        item.IsMoving = false;
                        this.fiber.Schedule(() => this.AutoMoveStart(item), settings.SendInterval);
                    }
                }
                else
                {
                    item.IsMoving = false;
                }
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        /// <summary>
        /// The auto move start.
        /// </summary>
        /// <param name="item">
        /// The mmo item.
        /// </param>
        private void AutoMoveStart(MyItem item)
        {
            if (item.IsMoving)
            {
                return;
            }

            if (item.IsDestroyed)
            {
                return;
            }

            item.IsMoving = true;

            Stopwatch t = Stopwatch.StartNew();

            int horizontal;
            int vertical;
            do
            {
                horizontal = this.random.Next(-1, 2);
                vertical = this.random.Next(-1, 2);
            }
            while (horizontal == 0 & vertical == 0);

            var settings = (Settings)this.Game.Settings;
            horizontal *= settings.AutoMoveVelocity;
            vertical *= settings.AutoMoveVelocity;

            this.AutoMove(settings, horizontal, vertical, t, item);
        }

        /// <summary>
        /// The game loop.
        /// </summary>
        private void GameLoop()
        {
            if (this.running)
            {
                try
                {
                    this.Game.Update();
                    this.Invalidate();
                    this.frameCounter.Increment();
                }
                catch (Exception e)
                {
                    log.Error(e);
                }
            }
        }

        /// <summary>
        /// The get random position.
        /// </summary>
        /// <returns>
        /// a random position
        /// </returns>
        private float[] GetRandomPosition()
        {
            return new float[]
                {
                    this.random.Next((int)this.Game.WorldData.TopLeftCorner[0], (int)this.Game.WorldData.BottomRightCorner[0]), 
                    this.random.Next((int)this.Game.WorldData.TopLeftCorner[1], (int)this.Game.WorldData.BottomRightCorner[1])
                };
        }

        /// <summary>
        /// The on mouse click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                this.mainCamera.ResetViewDistance();
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (this.mainCamera.AttachedItem != null)
                {
                    this.mainCamera.Detach();
                }
                else
                {
                    this.mainCamera.AttachItem(this.Game.Avatar);
                }
            }
        }

        /// <summary>
        /// The on mouse down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event artgs.
        /// </param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            this.movementChannel.Publish(e);
        }

        /// <summary>
        /// The on move.
        /// </summary>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void OnMove(MouseEventArgs e)
        {
            try
            {
                Game engine = this.Game;
                if (engine.State == GameState.WorldEntered)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        float x = e.X * engine.WorldData.Width / this.Width;
                        float y = e.Y * engine.WorldData.Height / this.Height;

                        if (this.mainCamera.AttachedItem == null)
                        {
                            this.mainCamera.Move(new[] { engine.WorldData.BottomRightCorner[0] - x, y });
                        }
                        else
                        {
                            engine.Avatar.MoveAbsolute(new[] { engine.WorldData.BottomRightCorner[0] - x, y }, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        /// <summary>
        /// The on world entered.
        /// </summary>
        private void OnWorldEntered()
        {
            if (this.WorldEntered != null)
            {
                this.WorldEntered(this);
            }
        }

        /// <summary>
        /// The paint actor.
        /// </summary>
        /// <param name="engine">
        /// The engine.
        /// </param>
        /// <param name="grfx">
        /// The grafix.
        /// </param>
        /// <param name="boardSize">
        /// The board size.
        /// </param>
        /// <param name="actor">
        /// The mmo actor.
        /// </param>
        private void PaintActor(Game engine, Graphics grfx, int[] boardSize, Item actor)
        {
            WorldData world = engine.WorldData;

            float width = world.Width;
            float height = world.Height;
            float w = boardSize[0] / width;
            float h = boardSize[1] / height;
            float minW = Math.Max(4, w);
            float minH = Math.Max(4, h);

            Color color = Color.FromArgb(actor.Color);
            var pen = new Pen(color);

            float x = ((world.BottomRightCorner[0] - actor.Position[0]) * w) - 1;
            float y = (actor.Position[1] * h) - 1;

            if (actor.InterestAreaAttached)
            {
                if (!actor.IsMine)
                {
                    // draw view distance
                    grfx.DrawRectangle(
                        pen, 
                        x - (actor.ViewDistanceEnter[0] * w), 
                        y - (actor.ViewDistanceEnter[1] * h), 
                        w * 2 * actor.ViewDistanceEnter[0], 
                        h * 2 * actor.ViewDistanceEnter[1]);

                    // draw view distance exit
                    grfx.DrawRectangle(
                        pen, 
                        x - (actor.ViewDistanceExit[0] * w), 
                        y - (actor.ViewDistanceExit[1] * h), 
                        w * 2 * actor.ViewDistanceExit[0], 
                        h * 2 * actor.ViewDistanceExit[1]);
                }

                ////if (actor == engine.Avatar)
                ////{
                ////    // make view distance distance thicker 
                ////    grfx.DrawRectangle(
                ////        pen,
                ////        x - (actor.ViewDistanceEnter[0] * w) + 1,
                ////        y - (actor.ViewDistanceEnter[1] * h) + 1,
                ////        (w * actor.ViewDistanceEnter[0] * 2) - 2,
                ////        (h * actor.ViewDistanceEnter[1] * 2) - 2);

                ////    grfx.DrawRectangle(
                ////        pen,
                ////        x - (actor.ViewDistanceExit[0] * w) + 1,
                ////        y - (actor.ViewDistanceExit[1] * h) + 1,
                ////        (w * actor.ViewDistanceExit[0] * 2) - 2,
                ////        (h * actor.ViewDistanceExit[1] * 2) - 2);
                ////}
            }

            var brush = new SolidBrush(color);
            if (actor.PreviousPosition != null && actor.PreviousPosition != actor.Position)
            {
                float lastX = ((world.BottomRightCorner[0] - actor.PreviousPosition[0]) * w) - 1;
                float lastY = (actor.PreviousPosition[1] * h) - 1;

                float predictedX = x + (x - lastX);
                float predictedY = y + (y - lastY);

                // draw movement line
                grfx.DrawLine(pen, lastX, lastY, predictedX, predictedY);

                // draw last Pos
                grfx.FillRectangle(brush, lastX - (minW / 2) + 1, lastY - (minH / 2) + 1, minW - 1, minH - 1);

                actor.ResetPreviousPosition();
            }

            var txtBrush = new SolidBrush(color);
            if (actor == engine.Avatar)
            {
                float localX = ((world.BottomRightCorner[0] - engine.Avatar.Position[0]) * w) - 1;
                float localY = (engine.Avatar.Position[1] * h) + 1;

                // draw local dot thicker
                grfx.FillRectangle(brush, localX - (minW / 2) + 1, localY - (minH / 2) - 1, minW, minH);

                grfx.DrawString(
                    string.Format("{0} ({1:0},{2:0})", actor.Text, world.BottomRightCorner[0] - actor.Position[0], actor.Position[1]), 
                    this.Font, 
                    txtBrush, 
                    x + (minW / 2) + 1, 
                    y);
            }
            else
            {
                ////grfx.DrawString(string.Format("{0} (#{1:x})", actor.Name, actor.Color), this.Font, txtBrush, x + (minW / 2) + 1, y);
                grfx.DrawString(actor.Text, this.Font, txtBrush, x + (minW / 2) + 1, y);
            }

            grfx.FillRectangle(brush, x - (minW / 2) + 1, y - (minH / 2) + 1, minW - 1, minH - 1);

            // grfx.DrawString(
            // string.Format("#{3}: {0} ({1},{2})", actor.Name, actor.Position[0], actor.Position[1], actor.Number), 
            // this.Font, 
            // txtBrush, 
            // actor.Position[0] * w, 
            // actor.Position[1] * h);

            // grfx.FillRectangle(txtBrush, x, y, 1, 1);
        }

        /// <summary>
        /// The read fps.
        /// </summary>
        private void ReadFps()
        {
            this.fps = this.frameCounter.GetNextValue();
        }
    }
}