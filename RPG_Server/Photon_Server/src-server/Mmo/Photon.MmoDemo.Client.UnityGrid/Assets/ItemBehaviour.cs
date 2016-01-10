// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemBehaviour.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The item behaviour.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using Photon.MmoDemo.Client;

using UnityEngine;

using Object = UnityEngine.Object;

/// <summary>
/// The item behaviour.
/// </summary>
public class ItemBehaviour : MonoBehaviour
{
    /// <summary>
    /// The actor height.
    /// </summary>
    private float actorHeight;

    /// <summary>
    /// The actor text.
    /// </summary>
    private GameObject actorText;

    /// <summary>
    /// The actor view.
    /// </summary>
    private GameObject actorView;

    /// <summary>
    /// The game.
    /// </summary>
    private Game game;

    /// <summary>
    /// The item.
    /// </summary>
    private Item item;

    /// <summary>
    /// The main camera.
    /// </summary>
    private GameObject mainCamera;

    /// <summary>
    /// The radar.
    /// </summary>
    private Radar radar;

    private float lastMoveUpdateTime;

    private Vector3 lastMoveUpdate;

    /// <summary>
    /// The Destroy.
    /// </summary>
    public void Destroy()
    {
        Object.Destroy(this.actorText);
        Object.Destroy(this.actorView);
        Object.Destroy(this.gameObject);
        Object.Destroy(this);

        this.actorText = null;
        this.actorView = null;
    }

    /// <summary>
    /// The initialize.
    /// </summary>
    /// <param name="mmoGame">
    /// The game.
    /// </param>
    /// <param name="mmoItem">
    /// The item.
    /// </param>
    /// <param name="worldRadar">
    /// the radar
    /// </param>
    public void Initialize(Game mmoGame, Item mmoItem, Radar worldRadar)
    {
        const int Layer = 1;
        this.radar = worldRadar;
        this.game = mmoGame;
        this.item = mmoItem;

        this.name = "Item" + this.item.Id;
        
        if (this.game.WorldData.Height > this.game.WorldData.Width)
        {
            this.actorHeight = this.game.WorldData.Height / 5f;
        }
        else
        {
            this.actorHeight = this.game.WorldData.Width / 5f;
        }

        this.transform.localScale = new Vector3(this.game.WorldData.Width / 200f, this.actorHeight, this.game.WorldData.Height / 200f);
        this.transform.position = new Vector3(0f, this.actorHeight / 2f, 0f);
        this.gameObject.layer = Layer;
      
        this.actorView = GameObject.CreatePrimitive(PrimitiveType.Cube);
        this.actorView.name = "ActorView" + this.item.Id;
        this.actorView.layer = Layer;

        this.actorText = (GameObject)Instantiate(Resources.Load("ActorName"));
        this.actorText.name = "ActorText" + this.item.Id;
        this.actorText.transform.localScale = new Vector3(this.actorHeight / 8f, this.actorHeight / 8f, this.actorHeight / 8f);
        this.actorText.transform.renderer.material.color = new Color(0, 0, 0);
        this.actorText.layer = Layer;
        this.mainCamera = GameObject.Find("Main Camera");

        this.transform.position = new Vector3(this.item.Position[0], this.transform.position.y, this.item.Position[1]);
        //Debug.Log(this.transform.position);
        this.actorText.transform.position = new Vector3(this.item.Position[0], this.actorText.transform.position.y, this.item.Position[1]);
        this.actorView.transform.position = new Vector3(this.item.Position[0], this.actorView.transform.position.y, this.item.Position[1]);

        this.ShowActor(false);
    }

    /// <summary>
    /// The start.
    /// </summary>
    public void Start()
    {
    }

    /// <summary>
    /// The update.
    /// Update is called once per frame
    /// </summary>
    public void Update()
    {
        if (this.item == null || this.item.IsVisible == false)
        {
            this.ShowActor(false);
            return;
        }

        this.radar.OnRadarUpdate(this.item.Id, this.item.Type, this.item.Position);

        byte[] colorBytes = BitConverter.GetBytes(this.item.Color);
        this.SetActorColor(new Color((float)colorBytes[2] / byte.MaxValue, (float)colorBytes[1] / byte.MaxValue, (float)colorBytes[0] / byte.MaxValue));


        ////Vector3 newPosition = new Vector3(this.game.WorldData.Width - this.item.Position[0], this.transform.position.y, this.item.Position[1]);
        Vector3 newPos = new Vector3(this.item.Position[0], this.transform.position.y, this.item.Position[1]);
        if (newPos != this.lastMoveUpdate)
        {
            this.lastMoveUpdate = newPos;
            this.lastMoveUpdateTime = Time.time;
        }

        // move smoothly
        float lerpT = (Time.time - lastMoveUpdateTime) / 0.05f;
        bool moveAbsolute = this.ShowActor(true);

        if (moveAbsolute)
        {
            //// Debug.Log("move absolute: " + newPos);
        
            this.transform.position = newPos;
        }
        else if (newPos != this.transform.position)
        {
            //// Debug.Log("move lerp: " + newPos);

            this.transform.position = Vector3.Lerp(transform.position, newPos, lerpT);
        }
        
        // view distance
        if (this.item.ViewDistanceEnter[0] > 0 && this.item.ViewDistanceEnter[1] > 0)
        {
            // get higher when the view distance gets smaller
            float maxHeight = this.actorHeight / 4f;
            float minHeight = maxHeight * 0.4f;
            float diffHeight = maxHeight - minHeight;

            float viewDistance = Math.Min(this.game.WorldData.Width, this.item.ViewDistanceEnter[0]);
            float percentViewDistance = viewDistance / this.game.WorldData.Width;
            float theHeight = maxHeight - (diffHeight * percentViewDistance);

            Vector3 newActorViewPosition = new Vector3(newPos.x, theHeight / 2f, newPos.z);
            if (moveAbsolute || !this.actorView.transform.renderer.enabled)
            {
                this.actorView.transform.renderer.enabled = true;
                this.actorView.transform.position = newActorViewPosition;
            }
            else
            {
                this.actorView.transform.position = Vector3.Lerp(this.actorView.transform.position, newActorViewPosition, lerpT);
            }

            // scale changes slowly
            this.actorView.transform.localScale = Vector3.Lerp(
                this.actorView.transform.localScale,
                new Vector3(this.item.ViewDistanceEnter[0] * 2f, theHeight, this.item.ViewDistanceEnter[1] * 2f),
                Time.time);
        }
        else
        {
            // view property missing
            this.actorView.transform.renderer.enabled = false;
        }

        TextMesh textMesh = (TextMesh)this.actorText.GetComponent(typeof(TextMesh));

        ////textMesh.text = string.Format("{0} ({1},{2})", this.item.Text, this.item.Position[0], this.item.Position[1]);
        textMesh.text = this.item.Text;
        Vector3 newActorTextPosition = new Vector3(newPos.x, this.transform.position.y * 2, newPos.z);
        if (moveAbsolute)
        {
            this.actorText.transform.position = newActorTextPosition;
        }
        else
        {
            this.actorText.transform.position = Vector3.Lerp(this.actorText.transform.position, newActorTextPosition, lerpT);
        }
      

        // look into oposite direction of camera
        Vector3 lookAt = new Vector3(
            this.actorText.transform.position.x,
            (2 * this.actorText.transform.position.y) - this.mainCamera.transform.position.y,
            (2 * this.actorText.transform.position.z) - this.mainCamera.transform.position.z - 1);
        this.actorText.transform.LookAt(lookAt);
    }

    /// <summary>
    /// The set actor color.
    /// </summary>
    /// <param name="actorColor">
    /// The actor color.
    /// </param>
    private void SetActorColor(Color actorColor)
    {
        this.transform.renderer.material.color = actorColor;
        this.actorView.transform.renderer.material.color = actorColor;
    }

    /// <summary>
    /// The show actor.
    /// </summary>
    /// <param name="show">
    /// The show.
    /// </param>
    /// <returns>
    /// true if state is switched.
    /// </returns>
    private bool ShowActor(bool show)
    {
        if (this.transform.renderer.enabled != show)
        {
            this.transform.renderer.enabled = show;
            this.actorView.transform.renderer.enabled = show;
            this.actorText.transform.renderer.enabled = show;
            return true;
        }

        return false;
    }
}
