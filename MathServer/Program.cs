using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathServer
{
    class Program
    {
        static void Main(string[] args)
        {
           if(args[0] == "TCP")
            {
                var server = new TCPMathServer();
                server.Run();

            }
           else
            {
                var server = new UDPServer();
                server.Run();
            }
        }
    }
}
