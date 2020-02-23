using System.Net.Sockets;
using System.Threading.Tasks;

using NLog;

using server.Interfaces.Capture;
using server.Interfaces.PacketReader.CmdPacketReader;
using server.Classes.PacketReader.CmdPacketReader;
using System.Net;

namespace server.Classes.Network
{
    public class NetworkClientConnection
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private TcpClient Client;
        private ICapture CaptureProcess;
        private readonly ICmdPacketReader cmdReader = new CmdPacketReader();

        public NetworkClientConnection(TcpClient client, ICapture captureProcess)
        {
            Client = client;
            CaptureProcess = captureProcess;
            Logger.Info($"Recieved client {client.Client.RemoteEndPoint}");
            _ = beginNetworkConnection();
        }

        /// <summary>
        /// Write the given data to the client
        /// </summary>
        /// <param name="buffer">The data to write to the client</param>
        public void writeData(byte[] buffer)
        {
            Client.GetStream().Write(buffer);
        }

        private async Task beginNetworkConnection()
        {
            var buffer = new byte[2];
            Client.GetStream().Read(buffer, 0, 1);
            Logger.Info("reading command packet");
            if(buffer[0] == 1) buffer = await cmdReader.readCmdPacket(Client.GetStream());
            if(buffer[1] == 1) await sendStream();
            await Client.GetStream().ReadAsync(buffer, 0, 1);
            buffer = await cmdReader.readCmdPacket(Client.GetStream());
            if(buffer[1] == 0) await stopStreams();
        }

        private async Task sendStream()
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

        private async Task stopStreams()
        {
            CaptureProcess.stopStreaming(this);
        }
    }
}