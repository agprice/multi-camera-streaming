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

        public void awaitConnections()
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
                    TcpClient client = server.AcceptTcpClient();
                    Logger.Info($"Connected to {client}");
                    _ = Task.Run(() => new NetworkClientConnection(client));
                    NetworkStream stream = client.GetStream();
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