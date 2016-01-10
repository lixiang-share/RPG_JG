// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpawnItem.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This operation is allowed AFTER having entered an <see cref="MmoWorld" /> with operation <see cref="EnterWorld" />.
//   It is used to create a new <see cref="Item" />.
//   See <see cref="MmoActor.OperationSpawnItem">MmoActor.OperationSpawnItem</see> for more information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.MmoDemo.Server.Operations
{
    using System.Collections;

    using Photon.MmoDemo.Common;
    using Photon.SocketServer;
    using Photon.SocketServer.Mmo;
    using Photon.SocketServer.Rpc;

    /// <summary>
    /// This operation is allowed AFTER having entered an <see cref="MmoWorld"/> with operation <see cref="EnterWorld"/>.
    /// It is used to create a new <see cref="Item"/>. 
    /// See <see cref="MmoActor.OperationSpawnItem">MmoActor.OperationSpawnItem</see> for more information.
    /// </summary>
    public class SpawnItem : Operation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpawnItem"/> class.
        /// </summary>
        /// <param name="protocol">
        /// The protocol.
        /// </param>
        /// <param name="request">
        /// The request. 
        /// </param>
        public SpawnItem(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request)
        {
        }

        /// <summary>
        /// Gets or sets an <see cref="InterestArea"/> Id for immediate subscription.
        /// This request parameter is optional.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.InterestAreaId, IsOptional = true)]
        public byte? InterestAreaId { get; set; }

        /// <summary>
        /// Gets or sets thew new <see cref="Item"/> id.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemId)]
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets thew new <see cref="Item"/> type.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.ItemType)]
        public byte ItemType { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Item"/>'s initial position.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.Position)]
        public float[] Position { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Item"/>'s intiai properties.
        /// This request parameter is optional.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.Properties, IsOptional = true)]
        public Hashtable Properties { get; set; }

        /// <summary>
        /// Gets or sets the new rotation.
        /// This request parameter is optional.
        /// </summary>
        [DataMember(Code = (byte)ParameterCode.Rotation, IsOptional = true)]
        public float[] Rotation { get; set; }

        /// <summary>
        /// Gets the operation response.
        /// </summary>
        /// <param name="errorCode">
        /// The error code.
        /// </param>
        /// <param name="debugMessage">
        /// The debug message.
        /// </param>
        /// <returns>
        /// A new operation response.
        /// </returns>
        public OperationResponse GetOperationResponse(short errorCode, string debugMessage)
        {
            var responseObject = new SpawnItemResponse { ItemId = this.ItemId, ItemType = this.ItemType, InterestAreaId = this.InterestAreaId };
            return new OperationResponse(this.OperationRequest.OperationCode, responseObject) { ReturnCode = errorCode, DebugMessage = debugMessage };
        }

        /// <summary>
        /// Gets the operation response.
        /// </summary>
        /// <param name="returnValue">
        /// The return value.
        /// </param>
        /// <returns>
        /// A new operation response.
        /// </returns>
        public OperationResponse GetOperationResponse(MethodReturnValue returnValue)
        {
            return this.GetOperationResponse(returnValue.Error, returnValue.Debug);
        }
    }
}