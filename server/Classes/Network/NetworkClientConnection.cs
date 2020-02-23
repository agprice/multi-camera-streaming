using System.Net.Sockets;
using System.Threading.Tasks;

using NLog;

using server.Interfaces.Capture;
using server.Interfaces.PacketReader.CmdPacketReader;
using server.Classes.PacketReader.CmdPacketReader;

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
            Logger.Info($"Sending stream to client: {Client.Client.RemoteEndPoint}");
            CaptureProcess.requestStream(null, this);
            // await CaptureProcess.CaptureStream.CopyToAsync(Client.GetStream());
            // Logger.Info($"Finished sending stream to client: {Client.Client.RemoteEndPoint}");
            // Client.Close();
        }

        private async Task stopStreams()
        {
            CaptureProcess.stopStreaming(this);
        }
    }
}