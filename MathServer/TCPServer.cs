using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MathServer
{
    public class TCPMathServer
    {
        private IPAddress ipAd;
        private TcpListener listener;
        private Socket socket;
        private string op;
        private double op1;
        private double op2;
        private double result;
        private bool flag;

        private MathService mathServices = new MathService();
        public double ReadOperation()
        {
            switch(this.op)
            {
                case "+":
                    this.result = mathServices.Add(this.op1, this.op2);
                    break;
                case "-":
                    this.result = mathServices.Sub(this.op1, this.op2);
                    break;
                case "*":
                    this.result = mathServices.Mult(this.op1, this.op2);
                    break;
                case "/":
                    this.result = mathServices.Div(this.op1, this.op2);
                    break;
                default:throw new ArgumentOutOfRangeException();
            }
            return this.result;
        }

        public void Run()
        {
            Console.WriteLine("Tcp is running");
            Console.WriteLine("The server is running");
            Console.WriteLine("Waiting for a connection.....");
            this.ipAd = IPAddress.Parse("127.0.0.1");
            // use local m/c IP address, and 
            // use the same in the client

            /* Initializes the this.listener */

            this.listener = new TcpListener(ipAd, 65410);

            /* Start Listeneting at the specified port */
            this.listener.Start();
            this.socket = this.listener.AcceptSocket();

            Console.WriteLine("Connection accepted from " + this.socket.RemoteEndPoint.Serialize());
            this.flag = true;
            while (this.flag)
            {
                try
                {
                    byte[] resivedMsg = new byte[100];
                    int k = this.socket.Receive(resivedMsg);
                    string msg = Encoding.UTF8.GetString(resivedMsg, 0, resivedMsg.Length);

                    if (msg.Trim() == "Quit")
                    {
                        this.flag = false;
                        break;
                    }
                    string pattern = @"([+-/*]):([0-9]*):([0-9]*)";

                    Regex regex = new Regex(pattern);
                    Match match = regex.Match(msg);
               
                    if (match.Success && match.Groups[1].Value != null && match.Groups[2].Value != null  && match.Groups[2].Value != null)
                    {
                        this.op = match.Groups[1].Value;
                        this.op1 = Convert.ToDouble(match.Groups[2].Value);
                        this.op2 = Convert.ToDouble(match.Groups[3].Value);
                        ReadOperation();
                    }
                    else throw new NotSupportedException();

                    Console.WriteLine("Recieved...");
                    Console.WriteLine(msg);
                    Console.WriteLine("the answer is {0}", this.result);
                    this.socket.Send(Encoding.ASCII.GetBytes($"The result is {this.result}."));
                }
                catch (NotSupportedException e)
                {
                    Console.WriteLine("The expression was inccorect" + e.StackTrace);
                    this.socket.Send(Encoding.ASCII.GetBytes($"The expression was inccorect."));
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("The second argument was inccorect" + e.StackTrace);
                    this.socket.Send(Encoding.ASCII.GetBytes("The second argument was inccorect."));
                }
            }
            this.socket.Close();
            this.listener.Stop();
        }

    }
}