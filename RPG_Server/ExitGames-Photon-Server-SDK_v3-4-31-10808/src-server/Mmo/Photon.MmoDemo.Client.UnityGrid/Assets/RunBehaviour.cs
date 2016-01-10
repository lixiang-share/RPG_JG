// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RunBehaviour.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The run behaviour.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



using System;

using ExitGames.Client.Photon;

using Photon.MmoDemo.Client;
using Photon.MmoDemo.Common;

using UnityEngine;

/// <summary>
/// The run behaviour.
/// </summary>
public class RunBehaviour : Radar, IGameListener
{
    /// <summary>
    /// The change text.
    /// </summary>
    private bool changeText;

    /// <summary>
    /// The game.
    /// </summary>
    private Game game;

    /// <summary>
    /// The is camera attached.
    /// </summary>
    private bool isCameraAttached;

    /// <summary>
    /// The last key press.
    /// </summary>
    private float lastKeyPress;

    /// <summary>
    /// The last move position.
    /// </summary>
    private float[] lastMovePosition;

    /// <summary>
    /// The next move time.
    /// </summary>
    private float nextMoveTime;

    /// <summary>
    /// Gets or sets Game.
    /// </summary>
    public Game Game
    {
        get
        {
            return this.game;
        }

        set
        {
            this.game = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether IsCameraAttached.
    /// </summary>
    public bool IsCameraAttached
    {
        get
        {
            return this.isCameraAttached;
        }

        set
        {
            this.isCameraAttached = value;
        }
    }

    /// <summary>
    /// Gets a value indicating whether IsDebugLogEnabled.
    /// </summary>
    public bool IsDebugLogEnabled
    {
        get
        {
            return false;
        }
    }

    /// <summary>
    /// The on application quit.
    /// </summary>
    public void OnApplicationQuit()
    {
        try
        {
            this.Game.Disconnect();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    /// <summary>
    /// The on gui.
    /// </summary>
    public override void OnGUI()
    {
        base.OnGUI();

        if (this.game != null && this.game.State == GameState.WorldEntered)
        {
            if (Event.current.type == EventType.ScrollWheel)
            {
                if (Event.current.delta.y < 0)
                {
                    this.IncreaseViewDistance();
                }
                else if (Event.current.delta.y > 0)
                {
                    this.DecreaseViewDistance();
                }
            }
            else if (Event.current.type == EventType.MouseDown)
            {
                if (Event.current.button == 2)
                {
                    InterestArea cam;
                    this.Game.TryGetCamera(0, out cam);
                    cam.ResetViewDistance();
                }
                else if (Event.current.button == 0)
                {
                    this.MoveActorToMousePosition();
                }
            }
            else if (Event.current.type == EventType.MouseDrag)
            {
                if (Event.current.button == 0)
                {
                    this.MoveActorToMousePosition();
                }
            }
        }
    }

    /// <summary>
    /// The start.
    /// </summary>
    public void Start()
    {
        this.mapPosition = new Vector2(50, 250);
        
        // Make the game run even when in background
        Application.runInBackground = true;
#if UNITY_IPHONE
            iPhoneSettings.verticalOrientation = false;
            iPhoneSettings.screenCanDarken = false;
#endif

        try
        {
            if (this.IsDebugLogEnabled)
            {
                Debug.Log("Start");
            }

            Settings settings = Settings.GetDefaultSettings();
            this.Game = new Game(this, settings, "Unity");
            this.game.Avatar.SetText("Unity");
            Photon.MmoDemo.Client.PhotonPeer peer = new Photon.MmoDemo.Client.PhotonPeer(this.Game, settings.UseTcp);
            this.Game.Initialize(peer);

            RoundTripTime.Game = this.Game;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    /// <summary>
    /// The update.
    /// </summary>
    public void Update()
    {
        try
        {
            this.Game.Update();

            // send queued movement
            this.Move();

            if (this.Game.State == GameState.WorldEntered)
            {
                this.ReadKeyboardInput();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    #region Implemented Interfaces

    #region IGameListener

    /// <summary>
    /// The log debug.
    /// </summary>
    /// <param name="game">
    /// The game.
    /// </param>
    /// <param name="message">
    /// The message.
    /// </param>
    public void LogDebug(Game game, string message)
    {
        Debug.Log(message);
    }

    /// <summary>
    /// The log error.
    /// </summary>
    /// <param name="game">
    /// The game.
    /// </param>
    /// <param name="message">
    /// The message.
    /// </param>
    public void LogError(Game game, string message)
    {
        Debug.Log(message);
    }

    /// <summary>
    /// The log error.
    /// </summary>
    /// <param name="game">
    /// The game.
    /// </param>
    /// <param name="errorCode">
    /// The error code.
    /// </param>
    /// <param name="debugMessage">
    /// The debug message.
    /// </param>
    /// <param name="operationCode">
    /// The operation code.
    /// </param>
    public void LogError(Game game, ReturnCode errorCode, string debugMessage, OperationCode operationCode)
    {
        Debug.Log(debugMessage);
    }

    /// <summary>
    /// The log error.
    /// </summary>
    /// <param name="game">
    /// The game.
    /// </param>
    /// <param name="exception">
    /// The exception.
    /// </param>
    public void LogError(Game game, Exception exception)
    {
        Debug.Log(exception.ToString());
    }

    /// <summary>
    /// The log info.
    /// </summary>
    /// <param name="game">
    /// The game.
    /// </param>
    /// <param name="message">
    /// The message.
    /// </param>
    public void LogInfo(Game game, string message)
    {
        Debug.Log(message);
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
    /// <exception cref="NotImplementedException">
    /// </exception>
    public void OnCameraAttached(string itemId, byte itemType)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// The on camera detached.
    /// </summary>
    /// <exception cref="NotImplementedException">
    /// </exception>
    public void OnCameraDetached()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// The on connect.
    /// </summary>
    /// <param name="game">
    /// The game.
    /// </param>
    public void OnConnect(Game game)
    {
        Debug.Log("connected");
    }

    /// <summary>
    /// The on disconnect.
    /// </summary>
    /// <param name="game">
    /// The game.
    /// </param>
    /// <param name="returnCode">
    /// The return code.
    /// </param>
    public void OnDisconnect(Game game, StatusCode returnCode)
    {
        Debug.Log("disconnected");
    }

    /// <summary>
    /// on item added
    /// </summary>
    /// <param name="game">
    /// The game.
    /// </param>
    /// <param name="item">
    /// The item.
    /// </param>
    public void OnItemAdded(Game game, Item item)
    {
        if (this.Game != null)
        {
            if (this.IsDebugLogEnabled)
            {
                Debug.Log("add item " + item.Id);
            }

            this.CreateActor(game, item);
        }
    }

    /// <summary>
    /// on item removed
    /// </summary>
    /// <param name="game">
    /// The game.
    /// </param>
    /// <param name="item">
    /// The item.
    /// </param>
    public void OnItemRemoved(Game game, Item item)
    {
        GameObject actorCube = GameObject.Find("Item" + item.Id);
        if (actorCube != null)
        {
            ItemBehaviour behaviour = (ItemBehaviour)actorCube.GetComponent(typeof(ItemBehaviour));
            behaviour.Destroy();
            if (this.IsDebugLogEnabled)
            {
                Debug.Log("destroy item " + item.Id);
            }
        }
        else
        {
            if (this.IsDebugLogEnabled)
            {
                Debug.Log("destroy item NOT FOUND " + item.Id);
            }
        }
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
    /// <exception cref="NotImplementedException">
    /// </exception>
    public void OnItemSpawned(byte itemType, string itemId)
    {
        
    }
    
    /// <summary>
    /// The on world entered.
    /// </summary>
    /// <param name="game">
    /// The game.
    /// </param>
    public void OnWorldEntered(Game game)
    {
        WorldData world = game.WorldData;
        float tilesX = world.Width / world.TileDimensions[0];
        float tilesY = world.Height / world.TileDimensions[1];

        // Material mat = (Material)Resources.Load("NonReflectiveMaterial");
        Material mat = (Material)Resources.Load("TileMaterial");

        for (int y = 0; y < tilesY; y++)
        {
            for (int x = 0; x < tilesX; x++)
            {
                float posX = x * world.TileDimensions[0];
                float posY = y * world.TileDimensions[1];

                const float ColorDark = 0x80 / 255f;
                const float ColorLight = 0xAF / 255f;

                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Plane);
                tile.transform.renderer.material = mat;
                tile.transform.renderer.material.color = (x + y) % 2 == 0
                                                             ? new Color(ColorDark, ColorDark, ColorDark)
                                                             : new Color(ColorLight, ColorLight, ColorLight);
                tile.transform.position = new Vector3(posX + (world.TileDimensions[0] / 2f), -0.1f, posY + (world.TileDimensions[1] / 2f));
                tile.transform.localScale = new Vector3(world.TileDimensions[0] / 10f, 0.1f, world.TileDimensions[1] / 10f);
            }
        }

        // init camera
        float maxDimension = Mathf.Max(world.Width, world.Height);

        // set camera in the middle of base line, calculate height with pyhtagoros (60 degree triangle)
        float cameraHeight = Mathf.Sqrt(Mathf.Pow(maxDimension, 2) - (maxDimension / 4f));
        this.transform.position = new Vector3(world.Width / 2f, cameraHeight, world.Height);

        // set camera to a lower height, calculate z offset with pyhtagoros (approximate)
        // float cameraHeight = maxDimension  / 2;
        // transform.position = new Vector3( world.Width / 2f, cameraHeight, world.Height + Mathf.Sqrt( Mathf.Pow(1.6f * maxDimension, 2) - Mathf.Pow(cameraHeight, 2) ) - maxDimension);
        this.transform.LookAt(new Vector3(world.Width / 2f, 0, world.Height / 2f));

        // init light
        GameObject theLight = GameObject.Find("Light");
        theLight.transform.position = this.transform.position;
        theLight.transform.LookAt(new Vector3(world.Width / 2f, 0, world.Height / 2f));
        theLight.light.range = this.transform.position.y;

        //// Mathf.Sqrt( Mathf.Pow(world.Height,2) - world.Height/4f );
        this.CreateActor(game, game.Avatar);

        this.world = game.WorldData;
        this.selfId = game.Avatar.Id + game.Avatar.Type;
        Operations.RadarSubscribe(game.Peer, game.WorldData.Name);
    }

    #endregion

    #endregion

    /// <summary>
    /// The create actor.
    /// </summary>
    /// <param name="game">
    /// The game.
    /// </param>
    /// <param name="actor">
    /// The actor.
    /// </param>
    private void CreateActor(Game game, Item actor)
    {
        GameObject actorCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ItemBehaviour actorBehaviour = (ItemBehaviour)actorCube.AddComponent(typeof(ItemBehaviour));
        actorBehaviour.Initialize(game, actor, this);
    }

    /// <summary>
    /// The decrease view distance.
    /// </summary>
    private void DecreaseViewDistance()
    {
        InterestArea cam;
        this.Game.TryGetCamera(0, out cam);
        float[] viewDistance = (float[])cam.ViewDistanceEnter.Clone();
        viewDistance[0] = Math.Max(0, viewDistance[0] - (this.Game.WorldData.TileDimensions[0] / 2));
        viewDistance[1] = Math.Max(0, viewDistance[1] - (this.Game.WorldData.TileDimensions[1] / 2));
        cam.SetViewDistance(viewDistance);
    }

    /// <summary>
    /// The increase view distance.
    /// </summary>
    private void IncreaseViewDistance()
    {
        InterestArea cam;
        this.Game.TryGetCamera(0, out cam);
        float[] viewDistance = (float[])cam.ViewDistanceEnter.Clone();
        viewDistance[0] = Math.Min(this.Game.WorldData.Width, viewDistance[0] + (this.Game.WorldData.TileDimensions[0] / 2));
        viewDistance[1] = Math.Min(this.Game.WorldData.Height, viewDistance[1] + (this.Game.WorldData.TileDimensions[1] / 2));
        cam.SetViewDistance(viewDistance);
    }

    /// <summary>
    /// The move.
    /// </summary>
    private void Move()
    {
        if (Time.time > this.nextMoveTime)
        {
            if (this.lastMovePosition != null)
            {
                this.Game.Avatar.MoveAbsolute(this.lastMovePosition, this.Game.Avatar.Rotation);
                this.lastMovePosition = null;
            }

            // up to 20 times per second
            this.nextMoveTime = Time.time + 0.05f;
        }
    }

    /// <summary>
    /// The move absolute.
    /// </summary>
    /// <param name="newPosition">
    /// The new position.
    /// </param>
    private void MoveAbsolute(float[] newPosition)
    {
        this.OnRadarUpdate(this.game.Avatar.Id, this.game.Avatar.Type, newPosition);
        this.lastMovePosition = newPosition;
    }

    /// <summary>
    /// The move actor to mouse position.
    /// </summary>
    private void MoveActorToMousePosition()
    {
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, UnityEngine.Camera.main.farClipPlane, 1)) 
        {
            ////byte[] colorBytes = System.BitConverter.GetBytes(engine.Actor.Color);
            ////hit.transform.renderer.material.color = new Color((float)colorBytes[2]/byte.MaxValue, (float)colorBytes[1]/byte.MaxValue, (float)colorBytes[0]/byte.MaxValue);
            float[] newPosition = new float[2];
            ////newPosition[0] = this.Game.WorldData.Width - hit.point.x;
            newPosition[0] = hit.point.x;
            newPosition[1] = hit.point.z;
            this.MoveAbsolute(newPosition);
        }
    }

    /// <summary>
    /// The move relative.
    /// </summary>
    /// <param name="offset">
    /// The offset.
    /// </param>
    private void MoveRelative(float[] offset)
    {
        float[] newPosition = (float[])this.Game.Avatar.Position.Clone();
        Settings settings = (Settings)this.game.Settings;
        newPosition[0] += offset[0] * settings.AutoMoveVelocity;
        newPosition[1] += offset[1] * settings.AutoMoveVelocity;
        this.MoveAbsolute(newPosition);
    }

    /// <summary>
    /// The read keyboard input.
    /// </summary>
    private void ReadKeyboardInput()
    {
        if (this.changeText)
        {
            if (Input.GetKey(KeyCode.Return))
            {
                this.changeText = false;
                return;
            }

            if (Input.GetKey(KeyCode.Backspace))
            {
                if (this.lastKeyPress + 0.1f < Time.time)
                {
                    if (this.Game.Avatar.Text.Length > 0)
                    {
                        this.Game.Avatar.SetText(this.Game.Avatar.Text.Remove(this.Game.Avatar.Text.Length - 1));
                        this.lastKeyPress = Time.time;
                    }
                }

                return;
            }

            this.Game.Avatar.SetText(this.Game.Avatar.Text + Input.inputString);
            return;
        }

        if (Input.GetKey(KeyCode.F1))
        {
            this.changeText = true;
            return;
        }

        if (Input.GetKey(KeyCode.Keypad5) || Input.GetKey(KeyCode.C))
        {
            if (this.lastKeyPress + 0.3f < Time.time)
            {
                float[] newPosition = new float[2];
                newPosition[0] = this.Game.WorldData.BottomRightCorner[0] / 2;
                newPosition[1] = this.Game.WorldData.BottomRightCorner[1] / 2;
                this.Game.Avatar.MoveAbsolute(newPosition, this.Game.Avatar.Rotation);
                this.lastKeyPress = Time.time;
            }
        }

        Settings settings = (Settings)this.Game.Settings;

        if (Input.GetKey(KeyCode.M))
        {
            if (this.lastKeyPress + 0.3f < Time.time)
            {
                settings.AutoMove = !settings.AutoMove;
                this.lastKeyPress = Time.time;
            }
        }

        if (Input.GetKey(KeyCode.Keypad8) || Input.GetKey(KeyCode.W))
        {
            this.MoveRelative(Game.MoveUp);
        }

        if (Input.GetKey(KeyCode.Keypad4) || Input.GetKey(KeyCode.A))
        {
            this.MoveRelative(Game.MoveLeft);
        }

        if (Input.GetKey(KeyCode.Keypad2) || Input.GetKey(KeyCode.S))
        {
            this.MoveRelative(Game.MoveDown);
        }

        if (Input.GetKey(KeyCode.Keypad6) || Input.GetKey(KeyCode.D))
        {
            this.MoveRelative(Game.MoveRight);
        }

        if (Input.GetKey(KeyCode.Keypad7))
        {
            this.MoveRelative(Game.MoveUpLeft);
        }

        if (Input.GetKey(KeyCode.Keypad9))
        {
            this.MoveRelative(Game.MoveUpRight);
        }

        if (Input.GetKey(KeyCode.Keypad1))
        {
            this.MoveRelative(Game.MoveDownLeft);
        }

        if (Input.GetKey(KeyCode.Keypad3))
        {
            this.MoveRelative(Game.MoveDownRight);
        }

        if (Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Period))
        {
            if (this.lastKeyPress + 0.05f < Time.time)
            {
                this.IncreaseViewDistance();
                this.lastKeyPress = Time.time;
            }
        }

        if (Input.GetKey(KeyCode.KeypadMinus) || Input.GetKey(KeyCode.Comma))
        {
            if (this.lastKeyPress + 0.05f < Time.time)
            {
                this.DecreaseViewDistance();
                this.lastKeyPress = Time.time;
            }
        }

        if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Slash))
        {
            if (this.lastKeyPress + 0.05f < Time.time)
            {
                InterestArea cam;
                this.Game.TryGetCamera(0, out cam);
                cam.ResetViewDistance();
                this.lastKeyPress = Time.time;
            }
        }
    }
}