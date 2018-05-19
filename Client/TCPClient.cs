using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;

namespace Client
{
    public class TCPClient
    {
        private TcpClient client;

        public void Run()
        {
            this.client = new TcpClient();
            Console.WriteLine("Connecting.....");
            this.client.Connect("127.0.0.1", 65410);

            Console.WriteLine("Connected");
            while (true)
            {
                try
                {

                    Console.WriteLine("Enter the expression in 'operator:first value:second value' form ");
                    byte[] sendMsg = Encoding.ASCII.GetBytes(Console.ReadLine());

                    Stream stm = this.client.GetStream();
                    stm.Write(sendMsg, 0, sendMsg.Length);

                    byte[] resivedMsg = new byte[100];
                    int k = stm.Read(resivedMsg, 0, 100);

                    Console.WriteLine(Encoding.UTF8.GetString(resivedMsg, 0, resivedMsg.Length));
                }

                catch (Exception e)
                {
                    Console.WriteLine("Error..... " + e.StackTrace);
                }
            }
            this.client.Close();
        }

    }
}