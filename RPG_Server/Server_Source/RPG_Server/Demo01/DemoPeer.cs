using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo01
{
    public class DemoPeer:PeerBase
    {
        public DemoPeer(IRpcProtocol protocol, IPhotonPeer unmanagedPeer) :base(protocol , unmanagedPeer)
        {

        }
        protected override void OnDisconnect(PhotonHostRuntimeInterfaces.DisconnectReason reasonCode,
            string reasonDetail)
        {
        }

        protected override void OnOperationRequest(OperationRequest operationRequest,
            SendParameters sendParameters)
        {
            Dictionary<byte , object> ps = new Dictionary<byte,object>();
            ps.Add(1 , "ServerDemo");
            OperationResponse response = new OperationResponse(1, ps);
            SendParameters sp = new SendParameters();
            SendOperationResponse(response, sp);
        }
    }
}
