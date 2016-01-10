// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Operations.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Tests.Disconnected
{
    using System.Collections;
    using System.Collections.Generic;

    using Photon.MmoDemo.Common;
    using Photon.SocketServer;
    
    /// <summary>
    /// The operations.
    /// </summary>
    public static class Operations
    {
        /// <summary>
        /// The attach camera.
        /// </summary>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        /// <returns>
        /// the OperationRequest
        /// </returns>
        public static OperationRequest AttachCamera(string itemId, byte? itemType)
        {
            var request = new OperationRequest { OperationCode = (byte)OperationCode.AttachInterestArea, Parameters = new Dictionary<byte, object>() };
            if (!string.IsNullOrEmpty(itemId))
            {
                request.Parameters.Add((byte)ParameterCode.ItemId, itemId);
            }

            if (itemType.HasValue)
            {
                request.Parameters.Add((byte)ParameterCode.ItemType, itemType.Value);
            }

            return request;
        }

        /// <summary>
        /// The create world.
        /// </summary>
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
        /// <returns>
        /// the OperationRequest
        /// </returns>
        public static OperationRequest CreateWorld(string worldName, float[] topLeftCorner, float[] bottomRightCorner, float[] tileDimensions)
        {
            var request = new OperationRequest { OperationCode = (byte)OperationCode.CreateWorld, Parameters = new Dictionary<byte, object>() };
            request.Parameters.Add((byte)ParameterCode.WorldName, worldName);
            request.Parameters.Add((byte)ParameterCode.TopLeftCorner, topLeftCorner);
            request.Parameters.Add((byte)ParameterCode.BottomRightCorner, bottomRightCorner);
            request.Parameters.Add((byte)ParameterCode.TileDimensions, tileDimensions);
            return request;
        }

        /// <summary>
        /// The destroy item.
        /// </summary>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        /// <returns>
        /// the OperationRequest
        /// </returns>
        public static OperationRequest DestroyItem(string itemId, byte itemType)
        {
            var request = new OperationRequest { OperationCode = (byte)OperationCode.DestroyItem, Parameters = new Dictionary<byte, object>() };
            request.Parameters.Add((byte)ParameterCode.ItemId, itemId);
            request.Parameters.Add((byte)ParameterCode.ItemType, itemType);
            return request;
        }

        /// <summary>
        /// The detach camera.
        /// </summary>
        /// <returns>
        /// the OperationRequest
        /// </returns>
        public static OperationRequest DetachCamera()
        {
            var request = new OperationRequest { OperationCode = (byte)OperationCode.DetachInterestArea, Parameters = new Dictionary<byte, object>() };
            return request;
        }

        /// <summary>
        /// The enter world.
        /// </summary>
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
        /// <returns>
        /// the OperationRequest
        /// </returns>
        public static OperationRequest EnterWorld(
            string worldName, string username, Hashtable properties, float[] position, float[] viewDistanceEnter, float[] viewDistanceExit)
        {
            var request = new OperationRequest { OperationCode = (byte)OperationCode.EnterWorld, Parameters = new Dictionary<byte, object>() };
            request.Parameters.Add((byte)ParameterCode.WorldName, worldName);
            request.Parameters.Add((byte)ParameterCode.Username, username);
            request.Parameters.Add((byte)ParameterCode.Position, position);
            request.Parameters.Add((byte)ParameterCode.ViewDistanceEnter, viewDistanceEnter);
            request.Parameters.Add((byte)ParameterCode.ViewDistanceExit, viewDistanceExit);
            if (properties != null)
            {
                request.Parameters.Add((byte)ParameterCode.Properties, properties);
            }

            return request;
        }

        /// <summary>
        /// The exit world.
        /// </summary>
        /// <returns>
        /// the OperationRequest
        /// </returns>
        public static OperationRequest ExitWorld()
        {
            var request = new OperationRequest { OperationCode = (byte)OperationCode.ExitWorld, Parameters = new Dictionary<byte, object>() };
            return request;
        }

        /// <summary>
        /// The move operation.
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
        /// <returns>
        /// the OperationRequest
        /// </returns>
        public static OperationRequest Move(string itemId, byte? itemType, float[] position)
        {
            var request = new OperationRequest { OperationCode = (byte)OperationCode.Move, Parameters = new Dictionary<byte, object>() };
            request.Parameters.Add((byte)ParameterCode.Position, position);
            if (itemId != null)
            {
                request.Parameters.Add((byte)ParameterCode.ItemId, itemId);
            }

            if (itemType.HasValue)
            {
                request.Parameters.Add((byte)ParameterCode.ItemType, itemType.Value);
            }

            return request;
        }

        /// <summary>
        /// The raise generic event.
        /// </summary>
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
        /// <returns>
        /// the OperationRequest
        /// </returns>
        public static OperationRequest RaiseGenericEvent(
            string itemId, byte? itemType, byte customEventCode, object eventData, Reliability eventReliability, EventReceiver eventReceiver)
        {
            var request = new OperationRequest { OperationCode = (byte)OperationCode.RaiseGenericEvent, Parameters = new Dictionary<byte, object>() };
            request.Parameters.Add((byte)ParameterCode.CustomEventCode, customEventCode);
            if (eventData != null)
            {
                request.Parameters.Add((byte)ParameterCode.EventData, eventData);
            }

            request.Parameters.Add((byte)ParameterCode.EventReliability, (byte)eventReliability);
            request.Parameters.Add((byte)ParameterCode.EventReceiver, (byte)eventReceiver);
            if (itemId != null)
            {
                request.Parameters.Add((byte)ParameterCode.ItemId, itemId);
            }

            if (itemType.HasValue)
            {
                request.Parameters.Add((byte)ParameterCode.ItemType, itemType.Value);
            }

            return request;
        }

        /// <summary>
        /// The set properties.
        /// </summary>
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
        /// <returns>
        /// the OperationRequest
        /// </returns>
        public static OperationRequest SetProperties(string itemId, byte? itemType, Hashtable propertiesSet, ArrayList propertiesUnset)
        {
            var request = new OperationRequest { OperationCode = (byte)OperationCode.SetProperties, Parameters = new Dictionary<byte, object>() };
            
            if (propertiesSet != null)
            {
                request.Parameters.Add((byte)ParameterCode.PropertiesSet, propertiesSet);
            }

            if (propertiesUnset != null)
            {
                request.Parameters.Add((byte)ParameterCode.PropertiesUnset, propertiesUnset);
            }

            if (itemId != null)
            {
                request.Parameters.Add((byte)ParameterCode.ItemId, itemId);
            }

            if (itemType.HasValue)
            {
                request.Parameters.Add((byte)ParameterCode.ItemType, itemType.Value);
            }

            return request;
        }

        /// <summary>
        /// The set view distance.
        /// </summary>
        /// <param name="viewDistanceEnter">
        /// The view Distance Enter.
        /// </param>
        /// <param name="viewDistanceExit">
        /// The view Distance Exit.
        /// </param>
        /// <returns>
        /// the OperationRequest
        /// </returns>
        public static OperationRequest SetViewDistance(float[] viewDistanceEnter, float[] viewDistanceExit)
        {
            var request = new OperationRequest { OperationCode = (byte)OperationCode.SetViewDistance, Parameters = new Dictionary<byte, object>() };
            request.Parameters.Add((byte)ParameterCode.ViewDistanceEnter, viewDistanceEnter);
            request.Parameters.Add((byte)ParameterCode.ViewDistanceExit, viewDistanceExit);
            return request;
        }

        /// <summary>
        /// The spawn item.
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
        /// <param name="properties">
        /// The properties.
        /// </param>
        /// <param name="subscribe">
        /// The subscribe.
        /// </param>
        /// <returns>
        /// the OperationRequest
        /// </returns>
        public static OperationRequest SpawnItem(string itemId, byte itemType, float[] position, Hashtable properties, bool subscribe)
        {
            var request = new OperationRequest { OperationCode = (byte)OperationCode.SpawnItem, Parameters = new Dictionary<byte, object>() };
            request.Parameters.Add((byte)ParameterCode.Position, position);
            request.Parameters.Add((byte)ParameterCode.ItemId, itemId);
            request.Parameters.Add((byte)ParameterCode.ItemType, itemType);
            request.Parameters.Add((byte)ParameterCode.Subscribe, subscribe);
            if (properties != null)
            {
                request.Parameters.Add((byte)ParameterCode.Properties, properties);
            }

            return request;
        }

        /// <summary>
        /// The subscribe item.
        /// </summary>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        /// <param name="propertiesRevision">
        /// The properties revision.
        /// </param>
        /// <returns>
        /// the OperationRequest
        /// </returns>
        public static OperationRequest SubscribeItem(string itemId, byte itemType, int? propertiesRevision)
        {
            var request = new OperationRequest { OperationCode = (byte)OperationCode.SubscribeItem, Parameters = new Dictionary<byte, object>() };
            request.Parameters.Add((byte)ParameterCode.ItemId, itemId);
            request.Parameters.Add((byte)ParameterCode.ItemType, itemType);
            if (propertiesRevision.HasValue)
            {
                request.Parameters.Add((byte)ParameterCode.PropertiesRevision, propertiesRevision);
            }

            return request;
        }

        /// <summary>
        /// The unsubscribe item.
        /// </summary>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        /// <returns>
        /// the OperationRequest
        /// </returns>
        public static OperationRequest UnsubscribeItem(string itemId, byte itemType)
        {
            var request = new OperationRequest { OperationCode = (byte)OperationCode.UnsubscribeItem, Parameters = new Dictionary<byte, object>() };
            request.Parameters.Add((byte)ParameterCode.ItemId, itemId);
            request.Parameters.Add((byte)ParameterCode.ItemType, itemType);

            return request;
        }
    }
}