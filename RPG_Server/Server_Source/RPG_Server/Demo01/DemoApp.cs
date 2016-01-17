using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo01
{
    class DemoApp:ApplicationBase
    {
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new DemoPeer(initRequest.Protocol, initRequest.PhotonPeer);
        }

        protected override void Setup()
        {
        }

        protected override void TearDown()
        {
        }
    }
}
