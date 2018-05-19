using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("witch server do you want to work with? type TCP or UDP");
            string server = Console.ReadLine();
            if(server == "TCP")
            {
                Process.Start("C:/Users/Nshan/source/repos/MathServer/MathServer/bin/Debug/MathServer.exe", "TCP");
                Thread.Sleep(1000);
                var client = new TCPClient();
                client.Run();
            }
            else if (server == "UDP")
            {
                Process.Start("C:/Users/Nshan/source/repos/MathServer/MathServer/bin/Debug/MathServer.exe", "UDP");
                Thread.Sleep(1000);
                var client = new UDPClient();
                client.Run();
            }
            Console.ReadKey();
        }
    }
}
