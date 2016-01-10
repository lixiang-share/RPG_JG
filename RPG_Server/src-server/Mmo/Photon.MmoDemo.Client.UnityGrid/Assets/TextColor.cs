// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextColor.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The text color.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

/// <summary>
/// The text color.
/// </summary>
public class TextColor : MonoBehaviour
{
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
        else
        {
            this.guiText.material.color = Color.black;
        }
    }

    /// <summary>
    /// The update.
    /// </summary>
    public void Update()
    {
    }
}