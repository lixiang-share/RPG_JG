// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnterWorldResponse.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This operation is allowed BEFORE having entered an <see cref="MmoWorld" /> with operation <see cref="EnterWorld" /> or AFTER having exited it with operaiton <see cref="OperationCode.ExitWorld" />.
//   See <see cref="MmoPeer.OperationEnterWorld">MmoPeer.OperationEnterWorld</see> for more information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Operations
{
    using Photon.MmoDemo.Common;
    using Photon.SocketServer.Mmo;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// This operation is allowed BEFORE having entered an <see cref="MmoWorld"/> with operation <see cref="EnterWorld"/> or AFTER having exited it with operaiton <see cref="OperationCode.ExitWorld"/>.
    /// See <see cref="MmoPeer.OperationEnterWorld">MmoPeer.OperationEnterWorld</see> for more information.
    /// </summary>
    public class EnterWorldResponse
    {
        /// <summary>
        /// Gets or sets the <see cref="GridWorld"/>'s bottom right corner.
        /// This response parameter is submitted with error code <see cref="ReturnCode.Ok"/>.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.BottomRightCorner, IsOptional = true)]
        public float[] BottomRightCorner { get; set; }
               
        /// <summary>
        /// Gets or sets the <see cref="GridWorld"/>'s tile dimensions.
        /// This response parameter is submitted with error code <see cref="ReturnCode.Ok"/>.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.TileDimensions, IsOptional = true)]
        public float[] TileDimensions { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="GridWorld"/>'s top left corner.
        /// This response parameter is submitted with error code <see cref="ReturnCode.Ok"/>.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.TopLeftCorner, IsOptional = true)]
        public float[] TopLeftCorner { get; set; }

        /// <summary>
        /// Gets or sets the name of the selected <see cref="MmoWorld"/>.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.WorldName)]
        public string WorldName { get; set; }
    }
}