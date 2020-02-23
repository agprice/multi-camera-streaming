using System;
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
                    Logger.Info($"Connected to {client.Client.RemoteEndPoint}");
                    _ = Task.Run(() => new NetworkClientConnection(client, Capture));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Global exception was hit in the network initializer.");
            }
        }

    }
}