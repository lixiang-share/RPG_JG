
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClientDemo
{

    class Listener : IPhotonPeerListener
    {

        public void DebugReturn(DebugLevel level, string message)
        {
            
        }

        public void OnEvent(EventData eventData)
        {
            
        }

        public void OnOperationResponse(OperationResponse operationResponse)
        {
            
        }

        public void OnStatusChanged(StatusCode statusCode)
        {
            
        }
    }
    class Program 
    {
        PhotonPeer peer = new PhotonPeer(new Listener(), ConnectionProtocol.Tcp);
        static void Main(string[] args)
        {
        }
    }
}
