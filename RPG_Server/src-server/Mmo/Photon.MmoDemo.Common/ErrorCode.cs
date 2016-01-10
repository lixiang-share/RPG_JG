// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnCode.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This enumeration contains all known error codes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Common
{
    /// <summary>
    /// This enumeration contains all known error codes.
    /// </summary>
    public enum ReturnCode
    {
        /// <summary>
        /// The ok code.
        /// </summary>
        Ok = 0, 

        /// <summary>
        /// The fatal.
        /// </summary>
        Fatal = 1, 

        /// <summary>
        /// The parameter out of range.
        /// </summary>
        ParameterOutOfRange = 51, 

        /// <summary>
        /// The operation not supported.
        /// </summary>
        OperationNotSupported, 

        /// <summary>
        /// The invalid operation parameter.
        /// </summary>
        InvalidOperationParameter, 

        /// <summary>
        /// The invalid operation.
        /// </summary>
        InvalidOperation, 

        /// <summary>
        /// The avatar access denied.
        /// </summary>
        ItemAccessDenied, 

        /// <summary>
        /// interest area not found.
        /// </summary>
        InterestAreaNotFound, 

        /// <summary>
        /// The interest area already exists.
        /// </summary>
        InterestAreaAlreadyExists, 

        /// <summary>
        /// The world already exists.
        /// </summary>
        WorldAlreadyExists = 101, 

        /// <summary>
        /// The world not found.
        /// </summary>
        WorldNotFound, 

        /// <summary>
        /// The item already exists.
        /// </summary>
        ItemAlreadyExists, 

        /// <summary>
        /// The item not found.
        /// </summary>
        ItemNotFound
    }
}