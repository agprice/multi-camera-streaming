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
            Logger.Info($"Sending stream to client: {Client.Client.RemoteEndPoint}");
            CaptureProcess.start(null);
            await CaptureProcess.CaptureStream.CopyToAsync(Client.GetStream());
            Logger.Info($"Finished sending stream to client: {Client.Client.RemoteEndPoint}");
            Client.Close();
        }
    }
}