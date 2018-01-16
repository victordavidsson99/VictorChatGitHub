using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VictorChatGitHub
{

    //Av Victor Davidsson 2018-01-15
    //Utifrån http://csharpskolan.se/article/broadcast-och-chatt

    class Program
    {

        private const int ListenPort = 11000;

        static void Main(string[] args)
        {

            //Skapa och starta en fred som körs samtidigt som resten av programmet
            var ListenThread = new Thread(Listener); //Fred
            ListenThread.Start();

            //Skapa en anslutning för att kunna skicka meddelandet
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.EnableBroadcast = true;
            IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, ListenPort);

            Thread.Sleep(1000);


            while(true)
            {
                Console.Write(">");
                string msg = Console.ReadLine();

                byte[] sendbuf = Encoding.UTF8.GetBytes(msg);
                socket.SendTo(sendbuf, ep);
                Thread.Sleep(200);
            }
        }

        static void Listener()
        {
            UdpClient listener = new UdpClient(ListenPort);

            try
            {
                while (true) //Självklart ska det vara en while
                {

                    IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, ListenPort);
                    byte[] bytes = listener.Receive(ref groupEP);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("[{0}] Received broadcast from {1} : {2}\n", DateTime.Now, groupEP.ToString(), Encoding.UTF8.GetString(bytes, 0, bytes.Length));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                listener.Close();
            }

        }
    }
}
