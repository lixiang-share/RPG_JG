// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorldForm.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The world form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Client.WinGrid
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    using ExitGames.Client.Photon;
    using ExitGames.Concurrency.Core;
    using ExitGames.Concurrency.Fibers;

    using ExitGames.Logging;

    using Photon.MmoDemo.Common;

    using ZedGraph;

    using PhotonPeer = Photon.MmoDemo.Client.PhotonPeer;

    /// <summary>
    /// The world form.
    /// </summary>
    public partial class WorldForm : Form, IPhotonPeerListener
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The colors.
        /// </summary>
        private readonly List<Color> colors = new List<Color> { Color.HotPink, Color.Red, Color.Blue, Color.Green, Color.DarkGray, Color.Cyan, Color.Orange, Color.Purple, Color.Navy, Color.Olive };

        /// <summary>
        /// The diagnostics peer.
        /// </summary>
        private readonly PhotonPeer diagnosticsPeer;

        /// <summary>
        /// The fiber.
        /// </summary>
        private readonly FormFiber fiber;

        /// <summary>
        /// The game counter.
        /// </summary>
        private int gameCounter;

        /// <summary>
        /// The start time.
        /// </summary>
        private DateTime? startTime;

        /// <summary>
        /// The update timer.
        /// </summary>
        private IDisposable updateTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldForm"/> class.
        /// </summary>
        public WorldForm()
        {
            this.gameCounter = 0;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_OnUnhandledException;
            Application.ThreadException += Application_OnThreadException;

            this.InitializeComponent();

            this.MouseWheel += this.OnMouseWheel;

            // diagnostics
            this.fiber = new FormFiber(this, new DefaultExecutor());
            this.fiber.Start();
            Settings settings = Program.GetDefaultSettings();
            this.diagnosticsPeer = new PhotonPeer(this, settings.UseTcp);
            this.counterGraph.GraphPane.Title.Text = "Server Performance";
            this.counterGraph.GraphPane.YAxis.Title.Text = "Value";
            this.counterGraph.GraphPane.YAxis.Scale.Min = 0;
            this.counterGraph.GraphPane.XAxis.Title.Text = "Time";
            this.counterGraph.GraphPane.XAxis.Scale.MajorUnit = DateUnit.Second;
        }

        #region Implemented Interfaces

        #region IPhotonPeerListener

        /// <summary>
        /// The debug return.
        /// </summary>
        /// <param name="debugLevel">
        /// The debug Level.
        /// </param>
        /// <param name="debug">
        /// The debug.
        /// </param>
        void IPhotonPeerListener.DebugReturn(DebugLevel debugLevel, string debug)
        {
            if (log.IsDebugEnabled)
            {
                // we do not use debugLevel here - just log whatever debug we have
                log.DebugFormat(debug);
            }
        }
       
        void IPhotonPeerListener.OnEvent(EventData @event)
        {
            switch (@event.Code)
            {
                case (byte)EventCode.CounterData:
                    {
                        var name = (string)@event.Parameters[(byte)ParameterCode.CounterName];
                        var values = (float[])@event.Parameters[(byte)ParameterCode.CounterValues];
                        var timestamps = (long[])@event.Parameters[(byte)ParameterCode.CounterTimeStamps];
                        var curve = (LineItem)this.counterGraph.GraphPane.CurveList[name];
                        if (curve == null)
                        {
                            if (!this.startTime.HasValue)
                            {
                                this.startTime = DateTime.FromBinary(timestamps[0]);
                            }

                            Color color = this.colors[0];
                            this.colors.RemoveAt(0);
                            curve = this.counterGraph.GraphPane.AddCurve(name, new RollingPointPairList(90), color, SymbolType.Circle);
                            curve.Symbol.Fill = new Fill(curve.Color);
                            curve.Symbol.Size = 5;
                            curve.Line.IsSmooth = true;

                            //// curve.Line.SmoothTension = 0.5f;
                        }

                        var list = (IPointListEdit)curve.Points;
                        for (int index = 0; index < values.Length; index++)
                        {
                            DateTime time = DateTime.FromBinary(timestamps[index]);
                            TimeSpan diff = time.Subtract(this.startTime.GetValueOrDefault());
                            list.Add(diff.TotalSeconds, values[index]);
                        }

                        this.counterGraph.GraphPane.XAxis.Scale.Min = list[0].X;
                        this.counterGraph.GraphPane.XAxis.Scale.Max = list[list.Count - 1].X;

                        this.counterGraph.AxisChange();
                        this.counterGraph.Invalidate();
                        break;
                    }

                case (byte)EventCode.RadarUpdate:
                    {
                        var itemId = (string)@event.Parameters[(byte)ParameterCode.ItemId];
                        var itemType = (byte)@event.Parameters[(byte)ParameterCode.ItemType];
                        var position = (float[])@event.Parameters[(byte)ParameterCode.Position];
                        this.tabPageRadar.OnRadarUpdate(itemId, itemType, position);
                        break;
                    }

                default:
                    {
                        log.ErrorFormat("diagnostics: unexpected event {0}", (EventCode)@event.Code);
                        break;
                    }
            }
        }

        void IPhotonPeerListener.OnOperationResponse(OperationResponse operationResponse)
        {
            switch (operationResponse.OperationCode)
            {
                case (byte)OperationCode.SubscribeCounter:
                    {
                        // init first engine
                        var newPage = new GameTabPage { Padding = new Padding(3), UseVisualStyleBackColor = true };
                        this.tabControlTabs.TabPages.Add(newPage);
                        this.StartEngine(newPage, Program.GetDefaultSettings());
                        newPage.Run();
                        break;
                    }

                case (byte)OperationCode.RadarSubscribe:
                    {
                        break;
                    }

                default:
                    {
                        log.ErrorFormat("diagnostics: unexpected return {0}", operationResponse.ReturnCode);
                        break;
                    }
            }
        }

        /// <summary>
        /// The i photon peer listener. peer status callback.
        /// </summary>
        /// <param name="returnCode">
        /// The return code.
        /// </param>
        void IPhotonPeerListener.OnStatusChanged(StatusCode returnCode)
        {
            try
            {
                switch (returnCode)
                {
                    case StatusCode.Connect:
                        {
                            log.InfoFormat("diagnostics: connected");
                            Operations.CounterSubscribe(this.diagnosticsPeer, 5000);
                            break;
                        }

                    case StatusCode.Disconnect:
                    case StatusCode.DisconnectByServer:
                    case StatusCode.DisconnectByServerLogic:
                    case StatusCode.DisconnectByServerUserLimit:
                    case StatusCode.TimeoutDisconnect:
                        {
                            log.InfoFormat("diagnostics: {0}", returnCode);
                            break;
                        }

                    default:
                        {
                            log.ErrorFormat("diagnostics: unexpected return {0}", returnCode);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                // exceptions in this method vanish if not caught here
                log.Error(e);
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// The add velocity.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="velocity">
        /// The velocity.
        /// </param>
        /// <returns>
        /// The new movement vector.
        /// </returns>
        private static float[] AddVelocity(float[] input, int velocity)
        {
            if (velocity == 1)
            {
                return input;
            }

            var result = new float[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = input[i] * velocity;
            }

            return result;
        }

        /// <summary>
        /// The application_ thread exception.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private static void Application_OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            log.Error(e.Exception);
        }

        /// <summary>
        /// The current domain_ unhandled exception.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private static void CurrentDomain_OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            log.Error(e.ExceptionObject);
        }

        /// <summary>
        /// The on key press.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private static void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// The on numeric text box validating.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private static void OnNumericTextBoxValidating(object sender, CancelEventArgs e)
        {
            int number;
            if (int.TryParse(((TextBox)sender).Text, out number) == false || number < 0)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// returns the current game tab engine - either visible or if tagged to settings tab
        /// </summary>
        /// <returns>
        /// The current game page.
        /// </returns>
        private GameTabPage GetCurrentEnginePage()
        {
            var page = this.tabControlTabs.SelectedTab as GameTabPage;
            if (page == null)
            {
                return this.tabPageSettings.Tag as GameTabPage;
            }

            return page;
        }

        /// <summary>
        /// The handle game world entered.
        /// </summary>
        /// <param name="page">
        /// The game page.
        /// </param>
        private void HandleGameWorldEntered(GameTabPage page)
        {
            page.WorldEntered -= this.HandleGameWorldEntered;

            if (!this.tabPageRadar.Initialized)
            {
                this.tabPageRadar.Initialize(page.Game.WorldData);
                Operations.RadarSubscribe(this.diagnosticsPeer, page.Game.WorldData.Name);
            }
        }

        /////// <summary>
        /////// The initialize.
        /////// </summary>
        ////private void Initialize()
        ////{
        ////    LogAppender.OnLog += this.LogText;
        ////    this.tabPageArea.Run();
        ////}

        /// <summary>
        /// tag page to settings tab and load values from game engine settings
        /// </summary>
        /// <param name="page">
        /// The game tab page.
        /// </param>
        private void InitTabSettings(GameTabPage page)
        {
            this.tabPageSettings.Tag = page;
            if (page != null)
            {
                Game engine = page.Game;
                var settings = (Settings)engine.Settings;

                this.tabPageSettings.Text = engine.Avatar.Text;
                this.textBoxSendMovementInterval.Text = settings.SendInterval.ToString();
                this.textBoxAutoMoveInterval.Text = settings.AutoMoveInterval.ToString();
                this.textBoxPlayerText.Text = engine.Avatar.Text;
                this.buttonPlayerColor.BackColor = Color.FromArgb(engine.Avatar.Color);
                this.checkBoxSendReliable.Checked = settings.SendReliable;
                this.textBoxAutoMoveVelocity.Text = settings.AutoMoveVelocity.ToString();
                this.checkBoxAutoMoveEnabled.Checked = settings.AutoMove;
            }
            else
            {
                this.tabPageSettings.Text = "Settings";
            }
        }

        /// <summary>
        /// The log text.
        /// </summary>
        /// <param name="text">
        /// The output text.
        /// </param>
        private void LogText(string text)
        {
            if (this.textBoxLog.InvokeRequired)
            {
                Action a = () => this.LogText(text);
                this.textBoxLog.BeginInvoke(a);
            }
            else
            {
                this.textBoxLog.AppendText(text);
                this.textBoxLog.AppendText("\r\n");
            }
        }

        /// <summary>
        /// change color on buttonColor click
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void OnButtonColorClick(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() != DialogResult.Cancel)
            {
                this.buttonPlayerColor.BackColor = colorDialog.Color;
                GameTabPage page = this.GetCurrentEnginePage();
                if (page != null)
                {
                    page.Game.Avatar.SetColor(colorDialog.Color.ToArgb());
                }
            }
        }

        /// <summary>
        /// start/stop auto moving when checkBoxAutoMove changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void OnCheckBoxAutoMoveCheckedChanged(object sender, EventArgs e)
        {
            GameTabPage page = this.GetCurrentEnginePage();
            if (page == null)
            {
                return;
            }

            ((Settings)page.Game.Settings).AutoMove = this.checkBoxAutoMoveEnabled.Checked;

            if (this.checkBoxAutoMoveEnabled.Checked)
            {
                page.AutoMove();
            }
        }

        /// <summary>
        /// The on check box send reliable checked changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void OnCheckBoxSendReliableCheckedChanged(object sender, EventArgs e)
        {
            GameTabPage page = this.GetCurrentEnginePage();
            if (page == null)
            {
                return;
            }

            page.Game.Settings.SendReliable = this.checkBoxSendReliable.Checked;
        }

        /// <summary>
        /// stop all engines on form closing
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.updateTimer != null)
            {
                this.updateTimer.Dispose();
                this.updateTimer = null;
            }

            this.diagnosticsPeer.Disconnect();
            this.fiber.Dispose();

            ////if (this.tabControlTabs.TabCount > 3)
            ////{
            // e.Cancel = true;
            foreach (object control in this.tabControlTabs.TabPages)
            {
                var page = control as GameTabPage;
                if (page != null)
                {
                    this.StopEngine(page);
                }
            }

            // e.Cancel = false;
            ////}
        }

        /// <summary>
        /// The on key down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // ignore keyboard when entering text
            if (this.textBoxPlayerText.Focused)
            {
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.Space:
                    {
                        this.tabControlTabs.SuspendLayout();
                        if (this.tabControlTabs.SelectedTab == this.tabPageSettings)
                        {
                            var t = this.tabPageSettings.Tag as TabPage;
                            this.tabControlTabs.SelectedTab = t;
                        }
                        else
                        {
                            this.tabControlTabs.SelectedTab = this.tabPageSettings;
                        }

                        this.tabControlTabs.ResumeLayout(true);

                        return;
                    }

                case Keys.Oemplus:
                case Keys.Add:
                    {
                        var newPage = new GameTabPage { Padding = new Padding(3), UseVisualStyleBackColor = true };
                        this.tabControlTabs.TabPages.Add(newPage);
                        this.StartEngine(newPage, Program.GetDefaultSettings());
                        newPage.Run();
                        return;
                    }
            }

            var page = this.tabControlTabs.SelectedTab as GameTabPage;
            if (page == null)
            {
                return;
            }

            Game engine = page.Game;
            var settings = (Settings)engine.Settings;

            switch (e.KeyCode)
            {
                case Keys.M:
                    {
                        if (settings.AutoMove)
                        {
                            settings.AutoMove = false;
                        }
                        else
                        {
                            settings.AutoMove = true;
                            page.AutoMove();
                        }

                        break;
                    }

                case Keys.W:
                case Keys.NumPad8:
                    {
                        engine.Avatar.MoveRelative(AddVelocity(Game.MoveUp, settings.AutoMoveVelocity), null);
                        break;
                    }

                case Keys.A:
                case Keys.NumPad4:
                    {
                        engine.Avatar.MoveRelative(AddVelocity(Game.MoveLeft, settings.AutoMoveVelocity), null);
                        break;
                    }

                case Keys.S:
                case Keys.NumPad2:
                    {
                        engine.Avatar.MoveRelative(AddVelocity(Game.MoveDown, settings.AutoMoveVelocity), null);
                        break;
                    }

                case Keys.D:
                case Keys.NumPad6:
                    {
                        engine.Avatar.MoveRelative(AddVelocity(Game.MoveRight, settings.AutoMoveVelocity), null);
                        break;
                    }

                case Keys.NumPad5:
                    {
                        engine.Avatar.MoveAbsolute(new[] { engine.WorldData.BottomRightCorner[0] / 2, engine.WorldData.BottomRightCorner[1] / 2 }, null);
                        break;
                    }

                case Keys.NumPad7:
                    {
                        engine.Avatar.MoveRelative(AddVelocity(Game.MoveUpLeft, settings.AutoMoveVelocity), null);
                        break;
                    }

                case Keys.NumPad9:
                    {
                        engine.Avatar.MoveRelative(AddVelocity(Game.MoveUpRight, settings.AutoMoveVelocity), null);
                        break;
                    }

                case Keys.NumPad1:
                    {
                        engine.Avatar.MoveRelative(AddVelocity(Game.MoveDownLeft, settings.AutoMoveVelocity), null);
                        break;
                    }

                case Keys.NumPad3:
                    {
                        engine.Avatar.MoveRelative(AddVelocity(Game.MoveDownRight, settings.AutoMoveVelocity), null);
                        break;
                    }

                case Keys.OemMinus:
                case Keys.Subtract:
                    {
                        this.StopEngine(page);
                        break;
                    }

                case Keys.Insert:
                    {
                        page.SpawnBot();
                        break;
                    }

                case Keys.Delete:
                    {
                        page.DestroyBot();
                        break;
                    }
            }
        }

        /// <summary>
        /// The on mouse wheel.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            var page = this.tabControlTabs.SelectedTab as GameTabPage;
            if (page == null)
            {
                return;
            }

            Game engine = page.Game;
            if (engine.State == GameState.WorldEntered)
            {
                var offset = new[] { engine.WorldData.TileDimensions[0] / 2, engine.WorldData.TileDimensions[1] / 2 };
                int factor = e.Delta > 0 ? 1 : -1;
                float[] currentViewDistance = page.MainCamera.ViewDistanceEnter;
                var newViewDistance = new[] { currentViewDistance[0] + (offset[0] * factor), currentViewDistance[1] + (offset[1] * factor) };
                page.MainCamera.SetViewDistance(newViewDistance);
            }
        }

        /// <summary>
        /// tag current game engine to settings tab or apply settings when switching back from settings tab
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void OnTabControlTabsSelectedIndexChanged(object sender, EventArgs e)
        {
            GameTabPage page = this.GetCurrentEnginePage();
            if (page != null)
            {
                this.InitTabSettings(page);
            }
            else if (this.tabControlTabs.SelectedTab == this.tabPageLog)
            {
                this.textBoxLog.ScrollToCaret();
            }
        }

        /// <summary>
        /// The on text box auto move interval leave.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void OnTextBoxAutoMoveIntervalLeave(object sender, EventArgs e)
        {
            GameTabPage page = this.GetCurrentEnginePage();
            if (page == null)
            {
                return;
            }

            ((Settings)page.Game.Settings).AutoMoveInterval = int.Parse(this.textBoxAutoMoveInterval.Text);
        }

        /// <summary>
        /// The on text box auto move velocity leave.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void OnTextBoxAutoMoveVelocityLeave(object sender, EventArgs e)
        {
            GameTabPage page = this.GetCurrentEnginePage();
            if (page == null)
            {
                return;
            }

            ((Settings)page.Game.Settings).AutoMoveVelocity = int.Parse(this.textBoxAutoMoveVelocity.Text);
        }

        /// <summary>
        /// change game engine's actor name on TextboxName changed
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void OnTextBoxNameTextChanged(object sender, EventArgs e)
        {
            GameTabPage page = this.GetCurrentEnginePage();
            if (page == null)
            {
                return;
            }

            Game engine = page.Game;
            engine.Avatar.SetText(this.textBoxPlayerText.Text);
        }

        /// <summary>
        /// The on text box send movement leave.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event args.
        /// </param>
        private void OnTextBoxSendMovementLeave(object sender, EventArgs e)
        {
            GameTabPage page = this.GetCurrentEnginePage();
            if (page == null)
            {
                return;
            }

            ((Settings)page.Game.Settings).SendInterval = int.Parse(this.textBoxSendMovementInterval.Text);
        }

        /// <summary>
        /// The start engine.
        /// </summary>
        /// <param name="page">
        /// The game page.
        /// </param>
        /// <param name="settings">
        /// The settings.
        /// </param>
        private void StartEngine(GameTabPage page, Settings settings)
        {
            var engine = new Game(page, settings, Environment.UserName + ++this.gameCounter);
            var peer = new PhotonPeer(engine, settings.UseTcp);

            ////{
            ////    DebugOut = log.IsDebugEnabled ? DE.Exitgames.Neutron.Client.NPeer.DebugLevel.ALL : DE.Exitgames.Neutron.Client.NPeer.DebugLevel.INFO 
            ////};
            if (!this.tabPageRadar.Initialized)
            {
                page.WorldEntered += this.HandleGameWorldEntered;
            }

            engine.Initialize(peer);
            page.Initialize(engine);

            // set focus on game tab
            this.tabControlTabs.SelectedTab = page;
        }

        /// <summary>
        /// The stop engine.
        /// </summary>
        /// <param name="page">
        /// The game page.
        /// </param>
        private void StopEngine(GameTabPage page)
        {
            if (this.tabControlTabs.SelectedTab == page)
            {
                // set focus on tab before closing tab
                this.tabControlTabs.SelectedIndex = Math.Max(0, this.tabControlTabs.SelectedIndex - 1);
            }

            // shut down engine (event handlers are removed in OnGameStateChanged handler)
            page.Game.Disconnect();

            // remove tab 
            this.tabControlTabs.Controls.Remove(page);
            page.Dispose();
        }

        /// <summary>
        /// The update diagnostics.
        /// </summary>
        private void UpdateDiagnostics()
        {
            try
            {
                this.diagnosticsPeer.Service();
                this.tabPageRadar.Invalidate();
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        /// <summary>
        /// The world form onload.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="eventArgs">
        /// The eventArgs.
        /// </param>
        private void WorldForm_OnLoad(object sender, EventArgs eventArgs)
        {
            LogAppender.OnLog += this.LogText;

            Settings settings = Program.GetDefaultSettings();
            this.diagnosticsPeer.Connect(settings.ServerAddress, settings.ApplicationName);

            this.updateTimer = this.fiber.ScheduleOnInterval(this.UpdateDiagnostics, settings.DrawInterval, settings.DrawInterval);
        }
    }
}