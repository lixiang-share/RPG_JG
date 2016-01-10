// -------------------------------------------------------------------------------------------
// <copyright file="MethodReturnValue.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The <see cref="MethodReturnValue" /> combines an error code with a debug message.
//   It is recommended to return a <see cref="MethodReturnValue" /> rather than throwing an exception in order to maintain optimal server performance.
//   If an additional object needs to be returned use <see cref="MethodReturnValue{T}" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server
{
    /// <summary>
    /// The <see cref="MethodReturnValue"/> combines an error code with a debug message. 
    /// </summary>
    public struct MethodReturnValue
    {
        /// <summary>
        /// The debug message ok.
        /// </summary>
        internal const string DebugMessageOk = "Ok";

        /// <summary>
        /// The error code ok.
        /// </summary>
        internal const short ErrorCodeOk = 0;

        /// <summary>
        /// The ok value.
        /// </summary>
        private static readonly MethodReturnValue success = new MethodReturnValue { Error = ErrorCodeOk, Debug = DebugMessageOk };

        /// <summary>
        /// Gets Ok value.
        /// </summary>
        public static MethodReturnValue Ok
        {
            get
            {
                return success;
            }
        }

        /// <summary>
        /// Gets or sets DebugMessage.
        /// </summary>
        public string Debug { get; set; }

        /// <summary>
        /// Gets or sets ReturnCode.
        /// </summary>
        public short Error { get; set; }

        /// <summary>
        /// Implicit conversion to a boolean. 
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// True if <see cref="Error"/> == 0, otherwise false.
        /// </returns>
        public static implicit operator bool(MethodReturnValue value)
        {
            return value.Error == ErrorCodeOk;
        }

        /// <summary>
        /// Implicit conversion of an int to a MethodReturnValue.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// A new MethodReturnValue with <see cref="Error"/> = <paramref name="value"/>.
        /// </returns>
        public static implicit operator MethodReturnValue(short value)
        {
            return Fail(value, value.ToString());
        }

        /// <summary>
        /// Implicit conversion to an int.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <paramref name="value"/>'s property <see cref="Error"/>.
        /// </returns>
        public static implicit operator int(MethodReturnValue value)
        {
            return value.Error;
        }

        /// <summary>
        /// Creates a new instance with the given error code and debug message.
        /// </summary>
        /// <param name="errorCode">
        /// The error code.
        /// </param>
        /// <param name="debug">
        /// The debug.
        /// </param>
        /// <returns>
        /// A new MethodReturnValue instance.
        /// </returns>
        public static MethodReturnValue Fail(short errorCode, string debug)
        {
            return new MethodReturnValue { Error = errorCode, Debug = debug };
        }
    }
}