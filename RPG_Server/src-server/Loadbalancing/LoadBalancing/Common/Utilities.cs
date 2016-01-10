// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Utilities.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the Utilities type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.Common
{
    using System.Collections;
    using System.Globalization;

    using Photon.LoadBalancing.Operations;

    /// <summary>
    /// A collection of methods useful in one or another context.
    /// </summary>
    public static class Utilities
    {
        private static readonly string amf3IsVisblePropertyKey = ((byte)GameParameter.IsVisible).ToString(CultureInfo.InvariantCulture);

        private static readonly string amf3IsOpenPropertyKey = ((byte)GameParameter.IsOpen).ToString(CultureInfo.InvariantCulture);

        private static readonly string amf3MaxPlayerPropertyKey = ((byte)GameParameter.MaxPlayer).ToString(CultureInfo.InvariantCulture);

        private static readonly string amf3PropertiesPropertyKey = ((byte)GameParameter.Properties).ToString(CultureInfo.InvariantCulture);

        private static readonly string amf3PlayerNamePropertyKey = ((byte)ActorParameter.PlayerName).ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Converts well known properties sent by AS3/Flash clients - from string to byte-keys.
        /// </summary>
        /// <remarks>
        /// Check if peer is a flash (amf3) client because flash clients does not support byte keys in a hastable. 
        /// If a flash client likes to match a game with a specific 'MaxPlayer' value 'MaxPlayer' will be sent
        /// with the string key "255" and the max player value as int.
        /// </remarks>
        /// <param name="gameProps">A game properties hashtable.</param>
        /// <param name="actorProps">A actor properties hashtable.</param>
        public static void ConvertAs3WellKnownPropertyKeys(Hashtable gameProps, Hashtable actorProps)
        {
            // convert game properties
            if (gameProps != null && gameProps.Count > 0)
            {
                // well known property "is visible"
                if (gameProps.ContainsKey(amf3IsVisblePropertyKey))
                {
                    gameProps[(byte)GameParameter.IsVisible] = gameProps[amf3IsVisblePropertyKey];
                    gameProps.Remove(amf3IsVisblePropertyKey);
                }

                // well known property "is open"
                if (gameProps.ContainsKey(amf3IsOpenPropertyKey))
                {
                    gameProps[(byte)GameParameter.IsOpen] = gameProps[amf3IsOpenPropertyKey];
                    gameProps.Remove(amf3IsOpenPropertyKey);
                }

                // well known property "max players"
                if (gameProps.ContainsKey(amf3MaxPlayerPropertyKey))
                {
                    gameProps[(byte)GameParameter.MaxPlayer] = gameProps[amf3MaxPlayerPropertyKey];
                    gameProps.Remove(amf3MaxPlayerPropertyKey);
                }

                // well known property "props listed in lobby"
                if (gameProps.ContainsKey(amf3PropertiesPropertyKey))
                {
                    gameProps[(byte)GameParameter.Properties] = gameProps[amf3PropertiesPropertyKey];
                    gameProps.Remove(amf3PropertiesPropertyKey);
                }
            }

            // convert actor properties (if any)
            if (actorProps != null && actorProps.Count > 0)
            {
                // well known property "is visible"
                if (actorProps.ContainsKey(amf3PlayerNamePropertyKey))
                {
                    actorProps[(byte)ActorParameter.PlayerName] = actorProps[amf3PlayerNamePropertyKey];
                    actorProps.Remove(amf3PlayerNamePropertyKey);
                }
            }
        }

        /// <summary>
        /// Converts well known properties sent by AS3/Flash clients - from string to byte-keys.
        /// </summary>
        /// <param name="gamePropertyKeys">The game properties list.</param>
        /// <param name="actorPropertyKeys">The actor properties list.</param>
        public static void ConvertAs3WellKnownPropertyKeys(IList gamePropertyKeys, IList actorPropertyKeys)
        {
            // convert game properties
            if (gamePropertyKeys != null && gamePropertyKeys.Count > 0)
            {
                // well known property "is visible"
                if (gamePropertyKeys.Contains(amf3IsVisblePropertyKey))
                {
                    gamePropertyKeys.Remove(amf3IsVisblePropertyKey);
                    gamePropertyKeys.Add((byte)GameParameter.IsVisible);
                }

                // well known property "is open"
                if (gamePropertyKeys.Contains(amf3IsOpenPropertyKey))
                {
                    gamePropertyKeys.Remove(amf3IsOpenPropertyKey);
                    gamePropertyKeys.Add((byte)GameParameter.IsOpen);
                }

                // well known property "max players"
                if (gamePropertyKeys.Contains(amf3MaxPlayerPropertyKey))
                {
                    gamePropertyKeys.Remove(amf3MaxPlayerPropertyKey);
                    gamePropertyKeys.Add((byte)GameParameter.MaxPlayer);
                }

                // well known property "props listed in lobby"
                if (gamePropertyKeys.Contains(amf3PropertiesPropertyKey))
                {
                    gamePropertyKeys.Remove(amf3PropertiesPropertyKey);
                    gamePropertyKeys.Add((byte)GameParameter.Properties);
                }
            }

            // convert actor properties (if any)
            if (actorPropertyKeys != null && actorPropertyKeys.Count > 0)
            {
                // well known property "is visible"
                if (actorPropertyKeys.Contains(amf3PlayerNamePropertyKey))
                {
                    actorPropertyKeys.Remove(amf3PlayerNamePropertyKey);
                    actorPropertyKeys.Add((byte)ActorParameter.PlayerName);
                }
            }
        }
    }
}
