using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CnslGreetingsSrvr0
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ipAdrs = IPAddress.Parse("127.0.0.1");
            Socket srvr = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            srvr.Bind(new IPEndPoint(ipAdrs, 23124));
            srvr.Listen(3);
            Console.WriteLine("the server launch");
            while (true)
            {
                Socket clnt = srvr.Accept();
                Task.Run(() => MessagesHandling(clnt));
            }
        }

        private static void MessagesHandling(Socket clnt)
        {
            byte[] btsArr = new byte[30];
            string msg;
            while (true)
            {
                Array.Clear(btsArr, 0, btsArr.Length);
                clnt.Receive(btsArr);
                msg = Encoding.Default.GetString(btsArr);
                msg = msg.Trim('\0');
                Console.WriteLine($"The server:\nAt {DateTime.Now} from {clnt.RemoteEndPoint} received a string: " + msg);
                switch (msg)
                {
                    case "Date and time":
                        clnt.Send(Encoding.Default.GetBytes($"{DateTime.Now}"));
                        break;
                    case "Date":
                        clnt.Send(Encoding.Default.GetBytes($"{DateTime.Now.ToShortDateString()}"));
                        break;
                    case "Time":
                        clnt.Send(Encoding.Default.GetBytes($"{DateTime.Now.ToShortTimeString()}"));
                        break;
                    default:
                        clnt.Send(Encoding.Default.GetBytes("Hello, client!"));
                        break;
                }
            }
            clnt.Shutdown(SocketShutdown.Both);//
            clnt.Close();
        }
    }
}
