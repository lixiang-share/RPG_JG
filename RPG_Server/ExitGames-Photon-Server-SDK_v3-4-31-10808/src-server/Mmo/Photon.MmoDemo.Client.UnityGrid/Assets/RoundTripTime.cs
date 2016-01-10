// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoundTripTime.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The rtt.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Photon.MmoDemo.Client;

using UnityEngine;

/// <summary>
/// The rtt.
/// </summary>
public class RoundTripTime : MonoBehaviour
{
    /// <summary>
    /// The game.
    /// </summary>
    private static Game game;

    /// <summary>
    /// Gets or sets Game.
    /// </summary>
    public static Game Game
    {
        get
        {
            return game;
        }

        set
        {
            game = value;
        }
    }

    /// <summary>
    /// The start.
    /// </summary>
    public void Start()
    {
        if (!this.guiText)
        {
            Debug.Log("UtilityRoundTripTime needs a GUIText component!");
            this.enabled = false;
        }
    }

    /// <summary>
    /// The update.
    /// Update is called once per frame
    /// </summary>
    public void Update()
    {
        if (Game != null)
        {
            this.guiText.text = string.Format("{0} RTT   {1} VAR", Game.Peer.RoundTripTime, Game.Peer.RoundTripTimeVariance);
        }
    }
}