using QuotesIdentificationLbrry0;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace CnsleTcpLstnrServer0
{
    class ServerCls
    {
        static void Main()
        {
            TcpListener srvr = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 25545));
            BinaryFormatter fullServerBnryFrmtr = new BinaryFormatter();
            int clientsMax = 2;
            int quotesPerClientMax = 3;
            int clientsCount = 0;
            srvr.Start();
            Console.WriteLine("the server launch");
            while (true)
            {
                TcpClient clnt = srvr.AcceptTcpClient();
                if (clientsCount < clientsMax)
                {
                    clientsCount++;
                    Console.WriteLine(DateTime.Now.ToShortTimeString() + "\tconnected\t" + clnt.Client.RemoteEndPoint);
                    Task.Run(() =>
                    {
                        ClientHandling(quotesPerClientMax, clnt);
                        clientsCount--;
                    });
                }
                else
                {
                    // Возможным предстаёт применение кодов сообщений на месте передачи их в строковом виде.
                    fullServerBnryFrmtr.Serialize(clnt.GetStream(), "sorry the server is full. please try again later");
                    clnt.Close();
                }
            }
        }

        private static void ClientHandling(int quotesPerClientMax, TcpClient clnt)
        {
            QuotesStrctre qts;
            BinaryFormatter bnryFrmtr = new BinaryFormatter();
            AccountIdentification pseudoPasswordsLbrry = new AccountIdentification();
            string pseudo, password, qte = String.Empty;
            int logInAttemptsCount = 0;
            int quotesCount = 0;
            bnryFrmtr.Serialize(clnt.GetStream(), "server ok");
            while (logInAttemptsCount < 3)
            {
                pseudo = (string)bnryFrmtr.Deserialize(clnt.GetStream());
                if (pseudo == "exit")
                {
                    break;
                }
                password = (string)bnryFrmtr.Deserialize(clnt.GetStream());
                if (pseudoPasswordsLbrry.PseudoPasswordArr
                                        .Any(accnt => accnt.Pseudo == pseudo && accnt.Password == password))
                {
                    bnryFrmtr.Serialize(clnt.GetStream(), "logging ok");
                    while (quotesCount < quotesPerClientMax)
                    {
                        if ((string)bnryFrmtr.Deserialize(clnt.GetStream()) == "request" && quotesCount != quotesPerClientMax - 1)
                        {
                            quotesCount++;
                            qte = qts.RandomQuote;
                            bnryFrmtr.Serialize(clnt.GetStream(), qte);
                            Console.WriteLine(qte + "\twas sent to serialization");
                        }
                        else
                        {
                            logInAttemptsCount = 3;
                            break;
                        }
                    }
                }
                else
                {
                    logInAttemptsCount++;
                    if (logInAttemptsCount < 3)
                    {
                        bnryFrmtr.Serialize(clnt.GetStream(), "wrong pseudo/password");
                    }
                }
            }
            bnryFrmtr.Serialize(clnt.GetStream(), "the limit reached");
            Console.WriteLine(DateTime.Now.ToShortTimeString() + "\tdisconnected\t" + clnt.Client.RemoteEndPoint);
            clnt.Close();
        }
    }
}
