using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DemoClient_1
{
    class NetMgr
    {

        public void Start(string ip, int port)
        {
            TcpClient tc = new TcpClient();
            tc.BeginConnect(ip, port, new AsyncCallback(Connect) , tc.Client);
        }

        void Connect(IAsyncResult iar)
        {
            Console.WriteLine("begin connect to server...");
            Socket client = (Socket)iar.AsyncState;
            string str = "hello server";
            byte[] bs = Encoding.UTF8.GetBytes(str + "\n");
            client.BeginSend(bs, 0, bs.Length, 0, new AsyncCallback(SendCallBack), client);
            //try
            //{
            //    client.EndConnect(iar);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.ToString());
            //}
            //finally
            //{

            //}
        }

        void SendCallBack(IAsyncResult iar)
        {
            Console.WriteLine("Send Data complete and Start receive data");
            Socket client = (Socket)iar.AsyncState;
            StateObj state = new StateObj();
            state.buff = new byte[1024];
            state.client = client;
            client.BeginReceive(state.buff, 0, state.buff.Length, 0, new AsyncCallback(Receive), state);
        }

        private static void Receive(IAsyncResult iar)
        {
            try
            {
                StateObj so = (StateObj)iar.AsyncState;
                StringBuilder sb = new StringBuilder();
                // Create the state object.     
                Socket client = so.client;
                // Begin receiving the data from the remote device.     
                int bytesRead = client.EndReceive(iar);
                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.     

                    sb.Append(Encoding.UTF8.GetString(so.buff, 0, bytesRead));
                    // Get the rest of the data.     
                    Console.WriteLine(sb.ToString());
                    Console.ReadKey();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

    class StateObj
    {
        public Socket client;
        public byte[] buff;
    }
}
