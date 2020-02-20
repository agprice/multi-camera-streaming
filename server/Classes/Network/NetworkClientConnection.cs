using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using NLog;

namespace server.Classes.Network
{
    public class NetworkClientConnection
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        TcpClient Client;
        public NetworkClientConnection(TcpClient client)
        {
            Client = client;
            Logger.Info($"Recieved client {client}");
            sendStream();
        }

        private void sendStream() {

        }
    }
}