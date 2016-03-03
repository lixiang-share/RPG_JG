// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Operations.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Tests.Connected
{
    using System.Collections;
    using System.Collections.Generic;

    using Photon.MmoDemo.Common;

    using SocketServer;

    /// <summary>
    /// The operations.
    /// </summary>
    public static class Operations
    {
        /// <summary>
        /// The attach camera.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        public static void AttachCamera(Client client, string itemId, byte? itemType)
        {
            var data = new Dictionary<byte, object>();

            if (!string.IsNullOrEmpty(itemId))
            {
                data.Add((byte)ParameterCode.ItemId, itemId);
            }

            if (itemType.HasValue)
            {
                data.Add((byte)ParameterCode.ItemType, itemType.Value);
            }

            client.SendOperation((byte)OperationCode.AttachInterestArea, data, true);
        }

        /// <summary>
        /// The create world.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="worldName">
        /// The world name.
        /// </param>
        /// <param name="topLeftCorner">
        /// The top left corner.
        /// </param>
        /// <param name="bottomRightCorner">
        /// The bottom right corner.
        /// </param>
        /// <param name="tileDimensions">
        /// The tile dimensions.
        /// </param>
        public static void CreateWorld(Client client, string worldName, float[] topLeftCorner, float[] bottomRightCorner, float[] tileDimensions)
        {
            var data = new Dictionary<byte, object>
                {
                    { (byte)ParameterCode.WorldName, worldName }, 
                    { (byte)ParameterCode.TopLeftCorner, topLeftCorner }, 
                    { (byte)ParameterCode.BottomRightCorner, bottomRightCorner }, 
                    { (byte)ParameterCode.TileDimensions, tileDimensions }
                };
            client.SendOperation((byte)OperationCode.CreateWorld, data, true);
        }

        /// <summary>
        /// The destroy item.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        public static void DestroyItem(Client client, string itemId, byte itemType)
        {
            var data = new Dictionary<byte, object> { { (byte)ParameterCode.ItemId, itemId }, { (byte)ParameterCode.ItemType, itemType } };
            client.SendOperation((byte)OperationCode.DestroyItem, data, true);
        }

        /// <summary>
        /// The detach camera.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        public static void DetachCamera(Client client)
        {
            client.SendOperation((byte)OperationCode.DetachInterestArea, new Dictionary<byte, object>(), true);
        }

        /// <summary>
        /// The enter world.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="worldName">
        /// The world name.
        /// </param>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="properties">
        /// The properties.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="viewDistanceEnter">
        /// The view Distance Enter.
        /// </param>
        /// <param name="viewDistanceExit">
        /// The view Distance Exit.
        /// </param>
        public static void EnterWorld(
            Client client, string worldName, string username, Hashtable properties, float[] position, float[] viewDistanceEnter, float[] viewDistanceExit)
        {
            var data = new Dictionary<byte, object>
                {
                    { (byte)ParameterCode.WorldName, worldName }, 
                    { (byte)ParameterCode.Username, username }, 
                    { (byte)ParameterCode.Position, position }, 
                    { (byte)ParameterCode.ViewDistanceEnter, viewDistanceEnter }, 
                    { (byte)ParameterCode.ViewDistanceExit, viewDistanceExit }
                };
            if (properties != null)
            {
                data.Add((byte)ParameterCode.Properties, properties);
            }

            client.SendOperation((byte)OperationCode.EnterWorld, data, true);
        }

        /// <summary>
        /// The exit world.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        public static void ExitWorld(Client client)
        {
            client.SendOperation((byte)OperationCode.ExitWorld, new Dictionary<byte, object>(), true);
        }

        /// <summary>
        /// The move operation.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        public static void Move(Client client, string itemId, byte? itemType, float[] position)
        {
            var data = new Dictionary<byte, object> { { (byte)ParameterCode.Position, position } };
            if (itemId != null)
            {
                data.Add((byte)ParameterCode.ItemId, itemId);
            }

            if (itemType.HasValue)
            {
                data.Add((byte)ParameterCode.ItemType, itemType.Value);
            }

            client.SendOperation((byte)OperationCode.Move, data, true);
        }

        /// <summary>
        /// The raise generic event.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        /// <param name="customEventCode">
        /// The custom event code.
        /// </param>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        /// <param name="eventReliability">
        /// The event reliability.
        /// </param>
        /// <param name="eventReceiver">
        /// The event receiver.
        /// </param>
        public static void RaiseGenericEvent(
            Client client, string itemId, byte? itemType, byte customEventCode, object eventData, Reliability eventReliability, EventReceiver eventReceiver)
        {
            var data = new Dictionary<byte, object>
                {
                    { (byte)ParameterCode.CustomEventCode, customEventCode }, 
                    { (byte)ParameterCode.EventReliability, (byte)eventReliability }, 
                    { (byte)ParameterCode.EventReceiver, (byte)eventReceiver }
                };
            if (eventData != null)
            {
                data.Add((byte)ParameterCode.EventData, eventData);
            }

            if (itemId != null)
            {
                data.Add((byte)ParameterCode.ItemId, itemId);
            }

            if (itemType.HasValue)
            {
                data.Add((byte)ParameterCode.ItemType, itemType.Value);
            }

            client.SendOperation((byte)OperationCode.RaiseGenericEvent, data, true);
        }

        /// <summary>
        /// The set properties.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        /// <param name="propertiesSet">
        /// The properties set.
        /// </param>
        /// <param name="propertiesUnset">
        /// The properties unset.
        /// </param>
        public static void SetProperties(Client client, string itemId, byte? itemType, Hashtable propertiesSet, ArrayList propertiesUnset)
        {
            var data = new Dictionary<byte, object>();
            if (propertiesSet != null)
            {
                data.Add((byte)ParameterCode.PropertiesSet, propertiesSet);
            }

            if (propertiesUnset != null)
            {
                data.Add((byte)ParameterCode.PropertiesUnset, propertiesUnset);
            }

            if (itemId != null)
            {
                data.Add((byte)ParameterCode.ItemId, itemId);
            }

            if (itemType.HasValue)
            {
                data.Add((byte)ParameterCode.ItemType, itemType.Value);
            }

            client.SendOperation((byte)OperationCode.SetProperties, data, true);
        }

        /// <summary>
        /// The set view distance.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="viewDistanceEnter">
        /// The view Distance Enter.
        /// </param>
        /// <param name="viewDistanceExit">
        /// The view Distance Exit.
        /// </param>
        public static void SetViewDistance(Client client, float[] viewDistanceEnter, float[] viewDistanceExit)
        {
            var data = new Dictionary<byte, object>
                {
                    { (byte)ParameterCode.ViewDistanceEnter, viewDistanceEnter }, { (byte)ParameterCode.ViewDistanceExit, viewDistanceExit } 
                };
            client.SendOperation((byte)OperationCode.SetViewDistance, data, true);
        }

        /// <summary>
        /// The spawn item.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="properties">
        /// The properties.
        /// </param>
        /// <param name="subscribe">
        /// The subscribe.
        /// </param>
        public static void SpawnItem(Client client, string itemId, byte itemType, float[] position, Hashtable properties, bool subscribe)
        {
            var data = new Dictionary<byte, object>
                {
                    { (byte)ParameterCode.Position, position }, 
                    { (byte)ParameterCode.ItemId, itemId }, 
                    { (byte)ParameterCode.ItemType, itemType }, 
                    { (byte)ParameterCode.Subscribe, subscribe }
                };
            if (properties != null)
            {
                data.Add((byte)ParameterCode.Properties, properties);
            }

            client.SendOperation((byte)OperationCode.SpawnItem, data, true);
        }

        /// <summary>
        /// The subscribe item.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        /// <param name="propertiesRevision">
        /// The properties revision.
        /// </param>
        public static void SubscribeItem(Client client, string itemId, byte itemType, int? propertiesRevision)
        {
            var data = new Dictionary<byte, object> { { (byte)ParameterCode.ItemId, itemId }, { (byte)ParameterCode.ItemType, itemType } };
            if (propertiesRevision.HasValue)
            {
                data.Add((byte)ParameterCode.PropertiesRevision, propertiesRevision);
            }

            client.SendOperation((byte)OperationCode.SubscribeItem, data, true);
        }

        /// <summary>
        /// The unsubscribe item.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        public static void UnsubscribeItem(Client client, string itemId, byte itemType)
        {
            var data = new Dictionary<byte, object> { { (byte)ParameterCode.ItemId, itemId }, { (byte)ParameterCode.ItemType, itemType } };

            client.SendOperation((byte)OperationCode.UnsubscribeItem, data, true);
        }
    }
}