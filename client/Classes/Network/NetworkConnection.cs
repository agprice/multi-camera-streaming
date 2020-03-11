using System;
using System.Net.Sockets;
using System.Threading.Tasks;

using NLog;

using client.Interfaces.PacketWriter.CmdPacketWriter;
using client.Interfaces.PacketWriter.OptsPacketWriter;

using client.Classes.PacketWriter.OptsPacketWriter;
using client.Classes.PacketWriter.CmdPacketWriter;
using System.Collections.Generic;

namespace client.Classes.Network
{
    public class NetworkConnection
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ICmdPacketWriter _cmdWriter = new CmdPacketWriter();
        private readonly IOptPacketWriter _optWriter = new FfmpegOptsPacketWriter();
        private static int _port = 9001;
        private TcpClient _client;

        /// <summary>
        /// Set of connection
        /// </summary>
        /// <param name="args">Arguments from the client</param>
        public NetworkConnection(List<string> args)
        {
            _client = new TcpClient(args[0], _port);
            args.RemoveAt(0);

            Console.WriteLine("Connecting to server");

            var connType = (args[0].ToLower().Equals("tcp")) ? 1 : 0;

            Console.WriteLine("Requesting " + args[0] + " connection");
            args.RemoveAt(0);
            _cmdWriter.writeCmdPacket(_client.GetStream(), 1, (byte) connType);

            var serviceType = args[0];
            args.RemoveAt(0);
            
            string fileName = null;
            if(args[0].ToLower().Equals("stream"))
            {
                fileName = args[0];
                args.RemoveAt(0);
            }
            
            _optWriter.writeOptsPacket(_client.GetStream(), args);

            _ = Task.Run(() => new NetworkClient(_client.GetStream(), fileName));

            Console.WriteLine("Press any key to stop stream");
            var key = Console.ReadKey();
            _cmdWriter.writeCmdPacket(_client.GetStream(), 0, 0);
        }
    }
}