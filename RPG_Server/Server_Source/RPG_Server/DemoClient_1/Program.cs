using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DemoClient_1
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient tc = new TcpClient("127.0.0.1", 12345);
            string str = "hello world";
            //for (int i = 0; i < 1024*46; i++)
            //{
            //    str = str + "hello world";
            //}
            byte[] bs = Encoding.UTF8.GetBytes(str+"\n");
            // tc.
            Console.WriteLine(bs.Length);
            tc.Client.Send(bs);
            byte[] buff = new byte[1024];
            int len = 0;
            while ((len = tc.Client.Receive(buff)) <= 0) { }
            string content = Encoding.UTF8.GetString(buff, 0, len);
            Console.WriteLine(content);
            Console.ReadKey();
            tc.Close();
        }
    }
}
