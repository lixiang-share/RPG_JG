// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

/// <summary>
/// The settings.
/// </summary>
public class Settings : Photon.MmoDemo.Client.Settings
{
    /// <summary>
    /// The get default settings.
    /// </summary>
    /// <returns>
    /// the settings
    /// </returns>
    public static Settings GetDefaultSettings()
    {
        const string WorldName = "Unity3d-Island";
        const int BoxesVertical = 10;
        const int BoxesHorizontal = 10;
        const int EdgeLengthVertical = 20;
        const int EdgeLengthHorizontal = 20;

        const int IntervalMove = 1000;

        ////const int IntervalSend = 100;
        const int Velocity = 1;
        const bool AutoMove = false;
        const bool SendReliable = false;

        const bool UseTcp = false;
        const string ServerAddress = "localhost:5055";
        const string ApplicationName = "MmoDemo";

        int[] tileDimensions = new int[2];
        tileDimensions[0] = EdgeLengthHorizontal;
        tileDimensions[1] = EdgeLengthVertical;

        int[] gridSize = new int[2];
        gridSize[0] = BoxesHorizontal * EdgeLengthHorizontal;
        gridSize[1] = BoxesVertical * EdgeLengthVertical;

        Settings result = new Settings();

        // photon
        result.ServerAddress = ServerAddress;
        result.UseTcp = UseTcp;
        result.ApplicationName = ApplicationName;

        // grid
        result.WorldName = WorldName;
        result.TileDimensions = tileDimensions;
        result.GridSize = gridSize;

        // game engine
        result.AutoMoveInterval = IntervalMove;

        ////result.SendInterval = IntervalSend;
        result.SendReliable = SendReliable;
        result.AutoMoveVelocity = Velocity;
        result.AutoMove = AutoMove;

        return result;
    }

    /// <summary>
    /// The auto move.
    /// </summary>
    private bool autoMove;

    /// <summary>
    /// The auto move interval.
    /// </summary>
    private int autoMoveInterval;

    /// <summary>
    /// The auto move velocity.
    /// </summary>
    private int autoMoveVelocity;

    /////// <summary>
    /////// The send interval.
    /////// </summary>
    ////private int sendInterval;

    /// <summary>
    /// The use tcp.
    /// </summary>
    private bool useTcp;

    /// <summary>
    /// Gets or sets a value indicating whether AutoMove.
    /// </summary>
    public bool AutoMove
    {
        get
        {
            return this.autoMove;
        }

        set
        {
            this.autoMove = value;
        }
    }

    /// <summary>
    /// Gets or sets IntervalMove.
    /// </summary>
    public int AutoMoveInterval
    {
        get
        {
            return this.autoMoveInterval;
        }

        set
        {
            this.autoMoveInterval = value;
        }
    }

    /// <summary>
    /// Gets or sets Velocity.
    /// </summary>
    public int AutoMoveVelocity
    {
        get
        {
            return this.autoMoveVelocity;
        }

        set
        {
            this.autoMoveVelocity = value;
        }
    }

    /////// <summary>
    /////// Gets or sets IntervalSend.
    /////// </summary>
    ////public int SendInterval
    ////{
    ////    get
    ////    {
    ////        return this.sendInterval;
    ////    }

    ////    set
    ////    {
    ////        this.sendInterval = value;
    ////    }
    ////}

    /// <summary>
    /// Gets or sets a value indicating whether UseTcp.
    /// </summary>
    public bool UseTcp
    {
        get
        {
            return this.useTcp;
        }

        set
        {
            this.useTcp = value;
        }
    }
}