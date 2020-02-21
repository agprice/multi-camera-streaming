using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using NLog;
using server.Interfaces.Capture;

namespace server.Classes.Network
{
    public class NetworkClientConnection
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private TcpClient Client;
        private ICapture CaptureProcess;
        public NetworkClientConnection(TcpClient client, ICapture captureProcess)
        {
            Client = client;
            CaptureProcess = captureProcess;
            Logger.Info($"Recieved client {client}");
            _ = sendStream();
        }

        private async Task sendStream()
        {
            await CaptureProcess.CaptureStream.CopyToAsync(Client.GetStream());
        }
    }
}