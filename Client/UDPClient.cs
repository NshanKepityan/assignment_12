using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class UDPClient
    {

        public void Run()
        {
            UdpClient udpClient = new UdpClient();
            udpClient.Connect("127.0.0.1", 11000);
            while (true)
            {
                try
                {
                    Console.WriteLine("Enter the expression in 'operator:first value:second value' form ");
                    Byte[] sendMsg = Encoding.ASCII.GetBytes(Console.ReadLine());

                    udpClient.Send(sendMsg, sendMsg.Length);
                    IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    Byte[] buffer = udpClient.Receive(ref RemoteIpEndPoint);
                    string returnData = Encoding.ASCII.GetString(buffer);
                    Console.WriteLine(returnData.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            udpClient.Close();
        }
    }
}