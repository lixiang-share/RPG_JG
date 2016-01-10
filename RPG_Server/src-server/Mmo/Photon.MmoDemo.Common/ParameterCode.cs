// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterCode.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This enumeration contains the values used for event parameter, operation request parameter and operation response parameter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Common
{
    /// <summary>
    /// This enumeration contains the values used for event parameter, operation request parameter and operation response parameter.
    /// </summary>
    public enum ParameterCode : byte
    {
        /// <summary>
        /// The event code.
        /// </summary>
        EventCode = 60, 

        /// <summary>
        /// The username.
        /// </summary>
        Username = 91, 

        /// <summary>
        /// The old position.
        /// </summary>
        OldPosition = 92, 

        /// <summary>
        /// The position.
        /// </summary>
        Position = 93, 

        /// <summary>
        /// The properties.
        /// </summary>
        Properties = 94, 

        /// <summary>
        /// The item id.
        /// </summary>
        ItemId = 95, 

        /// <summary>
        /// The item type.
        /// </summary>
        ItemType = 96, 

        /// <summary>
        /// The properties revision.
        /// </summary>
        PropertiesRevision = 97, 

        /// <summary>
        /// The custom event code.
        /// </summary>
        CustomEventCode = 98, 

        /// <summary>
        /// The event data.
        /// </summary>
        EventData = 99, 

        /// <summary>
        /// The top left corner.
        /// </summary>
        TopLeftCorner = 100, 

        /// <summary>
        /// The tile dimensions.
        /// </summary>
        TileDimensions = 101, 

        /// <summary>
        /// The bottom right corner.
        /// </summary>
        BottomRightCorner = 102, 

        /// <summary>
        /// The world name.
        /// </summary>
        WorldName = 103, 

        /// <summary>
        /// The view distance.
        /// </summary>
        ViewDistanceEnter = 104, 

        /// <summary>
        /// The properties set.
        /// </summary>
        PropertiesSet = 105, 

        /// <summary>
        /// The properties unset.
        /// </summary>
        PropertiesUnset = 106, 

        /// <summary>
        /// The event reliability.
        /// </summary>
        EventReliability = 107, 

        /// <summary>
        /// The event receiver.
        /// </summary>
        EventReceiver = 108, 

        /// <summary>
        /// The subscribe.
        /// </summary>
        Subscribe = 109, 

        /// <summary>
        /// The view distance exit.
        /// </summary>
        ViewDistanceExit = 110, 

        /// <summary>
        /// The interest area id.
        /// </summary>
        InterestAreaId = 111, 

        /// <summary>
        /// The counter receive interval.
        /// </summary>
        CounterReceiveInterval = 112, 

        /// <summary>
        /// The counter name.
        /// </summary>
        CounterName = 113, 

        /// <summary>
        /// The counter time stamps.
        /// </summary>
        CounterTimeStamps = 114, 

        /// <summary>
        /// The counter values.
        /// </summary>
        CounterValues = 115,
        
        /// <summary>
        /// The current rotation.
        /// </summary>
        Rotation = 116,

        /// <summary>
        /// The previous rotation.
        /// </summary>
        OldRotation = 117
    }
}