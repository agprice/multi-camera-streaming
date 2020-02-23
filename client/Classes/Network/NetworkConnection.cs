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

        /// <summary>
        /// Set of connection
        /// </summary>
        /// <param name="args">Arguments from the client</param>
        public NetworkConnection(string[] args)
        {
            _client = new TcpClient(args[0], _port);
            Console.WriteLine("Connection established to server");
            var connType = (args[1].ToLower().Equals("tcp")) ? 1 : 0;
            _cmdWriter.writeCmdPacket(_client.GetStream(), 1, (byte) connType);

            _ = Task.Run(() => new NetworkClient(_client.GetStream(), args[args.Length - 1]));
            Console.WriteLine("Press any key to stop stream");
            var key = Console.ReadKey();
            _cmdWriter.writeCmdPacket(_client.GetStream(), 0, 0);
        }
    }
}