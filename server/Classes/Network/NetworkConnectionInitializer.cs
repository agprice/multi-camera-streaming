using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using NLog;
using server.Interfaces.Capture;
using server.Classes.Capture;

namespace server.Classes.Network
{
    /// <summary>
    /// This class opens network connections and sets up individual client tasks.
    /// </summary>
    public class NetworkConnectionInitializer
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ICapture Capture = new FfmpegCapture();
        /// <summary>
        /// Start the server and await connections.
        /// </summary>
        public void awaitConnections()
        {
            try
            {
                Int32 port = 9001;
                var server = new TcpListener(IPAddress.Any, port);
                server.Start();
                Logger.Info("Awaiting connections");
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Logger.Info($"Connected to {client}");
                    _ = Task.Run(() => new NetworkClientConnection(client, Capture));
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