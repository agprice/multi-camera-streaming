using System.Net.Sockets;
using NLog;
using server.Interfaces.Capture;
using System.Net;
using System.Threading;

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
            Logger.Info($"Recieved client {client.Client.RemoteEndPoint}");
            requestStream();
        }

        /// <summary>
        /// Write the given data to the client
        /// </summary>
        /// <param name="buffer">The data to write to the client</param>
        public void writeData(byte[] buffer)
        {
            Client.GetStream().Write(buffer);
        }

        /// <summary>
        /// Requests a copy of the stream from the ICapture device.
        /// </summary>
        private void requestStream()
        {
            Logger.Info($"Requesting stream for client: {Client.Client.RemoteEndPoint}");
            CaptureProcess.requestStream(null, this);
        }

        private void requestStreamStop() {
            Logger.Info($"Requesting stream stop for client {Client.Client.RemoteEndPoint}");
            CaptureProcess.stopStreaming(this);
        }

        public EndPoint getRemoteEndpoint()
        {
            return Client.Client.RemoteEndPoint;
        }
    }
}