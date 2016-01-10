// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RedirectedClientPeer.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Defines the RedirectedClientPeer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.LoadBalancing.MasterServer
{
    using System.Net;

    using Photon.LoadBalancing.Common;
    using Photon.LoadBalancing.Operations;
    using Photon.SocketServer;

    using PhotonHostRuntimeInterfaces;

    public class RedirectedClientPeer : PeerBase
    {
        #region Constructors and Destructors

        public RedirectedClientPeer(IRpcProtocol protocol, IPhotonPeer unmanagedPeer)
            : base(protocol, unmanagedPeer)
        {
        }

        #endregion

        #region Methods

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            var contract = new RedirectRepeatResponse();

            const byte masterNodeId = 1;

            // TODO: don't lookup for every operation!
            IPAddress publicIpAddress = PublicIPAddressReader.ParsePublicIpAddress(MasterServerSettings.Default.PublicIPAddress);
            switch (this.NetworkProtocol)
            {
                case NetworkProtocolType.Tcp:
                    contract.Address =
                        new IPEndPoint(
                            publicIpAddress, MasterServerSettings.Default.MasterRelayPortTcp + masterNodeId - 1).ToString
                            ();
                    break;
                case NetworkProtocolType.WebSocket:
                    contract.Address =
                        new IPEndPoint(
                            publicIpAddress, MasterServerSettings.Default.MasterRelayPortWebSocket + masterNodeId - 1).
                            ToString();
                    break;
                case NetworkProtocolType.Udp:
                    // no redirect through relay ports for UDP... how to handle? 
                    contract.Address =
                        new IPEndPoint(
                            publicIpAddress, MasterServerSettings.Default.MasterRelayPortUdp + masterNodeId - 1).ToString
                            ();
                    break;
            }


            var response = new OperationResponse(operationRequest.OperationCode, contract);
            response.ReturnCode = (short)ErrorCode.RedirectRepeat;
            response.DebugMessage = "redirect";

            this.SendOperationResponse(response, sendParameters);
        }

        #endregion
    }
}