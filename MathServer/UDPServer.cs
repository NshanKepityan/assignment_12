using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace MathServer
{
    class UDPServer
    {
        private UdpClient listener;
        private IPEndPoint RemoteIpEndPoint;
        private const int listenPort = 11000;
        private string op;
        private double op1;
        private double op2;
        private double result;
        private bool flag;
        private MathService mathServices = new MathService();
        public double ReadOperation()
        {
            switch (this.op)
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
                default: throw new ArgumentOutOfRangeException();
            }
            return this.result;
        }

        public void Run()
        {
            Console.WriteLine("Udp is running");

            //Creates a UdpClient for reading incoming data.
            this.listener = new UdpClient(11000);

            //the IP Address and port number of the sender. 
            this.RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            this.flag = true;
            while (flag)
            {
                
                try
                {
                    Byte[] buffer = this.listener.Receive(ref this.RemoteIpEndPoint);
                    string resivedMsg = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    string pattern = @"([+-/*]):([0-9]*):([0-9]*)";

                    if (resivedMsg.Trim() == "Quit")
                    {
                        flag = false;
                        break;
                    }
                    Regex regex = new Regex(pattern);
                    Match match = regex.Match(resivedMsg);

                    if (match.Success && match.Groups[1].Value != null && match.Groups[2].Value != null && match.Groups[2].Value != null)
                    {
                        this.op = match.Groups[1].Value;
                        this.op1 = Convert.ToDouble(match.Groups[2].Value);
                        this.op2 = Convert.ToDouble(match.Groups[3].Value);
                        ReadOperation();
                    }
                    else throw new NotSupportedException();
                    string returnData = Encoding.ASCII.GetString(buffer);

                    Console.WriteLine("This is the message you received {0} and the answer is {1}",returnData.ToString(),this.result);
                    Byte[] sendBytes = Encoding.ASCII.GetBytes($"The result is {this.result}");
                    this.listener.Send(sendBytes, sendBytes.Length, this.RemoteIpEndPoint);
                }
                catch (NotSupportedException e)
                {
                    Console.WriteLine("The expression was inccorect" + e.StackTrace);
                    Byte[] sendBytes = Encoding.ASCII.GetBytes("The expression was inccorect");
                    this.listener.Send(sendBytes, sendBytes.Length, this.RemoteIpEndPoint);
                }
            }
        }
    }
}