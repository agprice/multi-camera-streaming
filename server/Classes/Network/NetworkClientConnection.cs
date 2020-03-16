using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;

using NLog;

using server.Interfaces.Capture;
using server.Interfaces.PacketReader.CmdPacketReader;
using server.Interfaces.PacketReader.OptsPacketReader;
using server.Classes.PacketReader.CmdPacketReader;
using server.Classes.PacketReader.OptsPacketReader;
using server.Interfaces.Options;

namespace server.Classes.Network
{
    /// <summary>
    /// Processes and sends data to the individual network client currently connected.
    /// </summary>
    public class NetworkClientConnection
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private TcpClient Client;
        private ICapture CaptureProcess;
        private readonly ICmdPacketReader cmdReader = new CmdPacketReader();
        private readonly IOptsPacketReader optReader = new FfmpegOptsPacketReader();

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
            IOptions opts = null;

            var cmdBuffer = new List<byte>();
            var packetType = Client.GetStream().ReadByte();
            Logger.Info("Reading options packet");
            if(packetType == 0) opts = await optReader.receiveOptions(Client.GetStream());
            packetType = Client.GetStream().ReadByte();
            if(packetType == 1) cmdBuffer = await cmdReader.readCmdPacket(Client.GetStream());
            if(cmdBuffer[1] == 1)
            {
                Logger.Info("Sending stream");
                await sendStream(opts);
            }
            cmdBuffer = await cmdReader.readCmdPacket(Client.GetStream());
            if(cmdBuffer[1] == 0) await stopStreams();
        }

        private async Task sendStream(IOptions opts)
        {
            Logger.Info($"Requesting stream for client: {Client.Client.RemoteEndPoint}");
            CaptureProcess.requestStream(opts, this);
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