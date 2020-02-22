using System;
using System.Net.Sockets;
using System.Threading.Tasks;

using NLog;

using client.Interfaces.PacketWriter.CmdPacketWriter;
using client.Classes.PacketWriter.CmdPacketWriter;

namespace client.Classes.Network
{
    public class NetworkConnection
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ICmdPacketWriter _cmdWriter = new CmdPacketWriter();
        private static int _port = 9001;
        private TcpClient _client;

        public NetworkConnection(string[] args)
        {
            _client = new TcpClient(args[0], _port);
            Console.WriteLine("Connection Established to server");
            var connType = (args[1].ToLower().Equals("tcp")) ? 1 : 0;
            _cmdWriter.writeCmdPacket(_client.GetStream(), 1, (byte) connType);

            _ = Task.Run(() => new NetworkClient(_client.GetStream(), args[args.Length - 1]));
        }
    }
}