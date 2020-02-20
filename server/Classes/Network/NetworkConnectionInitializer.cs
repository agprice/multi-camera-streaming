using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using NLog;

namespace server.Classes.Network
{
    public class NetworkConnectionInitializer
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        async Task awaitConnections()
        {
            try
            {
                Int32 port = 9001;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                var server = new TcpListener(IPAddress.Any, port);
                server.Start();
                Logger.Info("Awaiting connections");
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Logger.Info($"Connected to {client}");
                    NetworkStream stream = client.GetStream();
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("error global.");
            }
        }

    }
}