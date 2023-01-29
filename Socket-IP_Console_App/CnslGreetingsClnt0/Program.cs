using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CnslGreetingsClnt0
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo answer = new ConsoleKeyInfo('\0', 0, false, false, false);
            byte[] btsArr = new byte[30];
            IPAddress ipAdrs = IPAddress.Parse("127.0.0.1");
            Socket clnt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            clnt.Connect(new IPEndPoint(ipAdrs, 23124));
            Console.WriteLine("the client launch");
            while (true)
            {
                Array.Clear(btsArr, 0, btsArr.Length);
                Console.WriteLine("please, select from the following:" +
                    "\n1.\tsend a greeting\n2.\trequest a time and date\n3.\trequest a date\n4.\trequest a time\n0.\texit");
                try
                {
                    answer = Console.ReadKey();
                    if (answer.KeyChar < '0' || answer.KeyChar > '4')
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                catch (ArgumentOutOfRangeException excptn)
                {
                    MessageBox.Show(excptn.Message, "Caramba!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                switch (answer.KeyChar)
                {
                    case '1':
                        clnt.Send(Encoding.Default.GetBytes("Hello, server!"));
                        break;
                    case '2':
                        clnt.Send(Encoding.Default.GetBytes("Date and time"));
                        break;
                    case '3':
                        clnt.Send(Encoding.Default.GetBytes("Date"));
                        break;
                    case '4':
                        clnt.Send(Encoding.Default.GetBytes("Time"));
                        break;
                    default:
                        clnt.Close();
                        return;
                }
                clnt.Receive(btsArr);
                Console.WriteLine($"\n\nThe client:\nAt {DateTime.Now} from {clnt.RemoteEndPoint} received a string: " + Encoding.Default.GetString(btsArr));
            }
        }
    }
}