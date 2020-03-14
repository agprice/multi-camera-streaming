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
        private string _name = null;
        private string _ip = null;

        /// <summary>
        /// Set of connection
        /// </summary>
        public NetworkConnection()
        {
        }

        /// <summary>
        /// Initializes this network connections connection to the client at the specified IP.
        /// </summary>
        /// <param name="ip">The IP of the client being connected to</param>
        /// <param name="transportType">The type of transport protocol requested, either TCP or UDP</param>
        /// <param name="port">The port of the server to connect to</param>
        /// <param name="name">If not null, the name of the file to be written to</param>
        public void ConnectTo(string ip, string transportType, string name = null, int port = 9001)
        {
            _ip = ip;
            _port = port;
            _name = name;
            _client = new TcpClient(ip, _port);
            _logger.Info($"Connecting to server: {ip}, on port {port}");
            var connType = (transportType.ToLower().Equals("tcp")) ? 1 : 0;
            _cmdWriter.writeCmdPacket(_client.GetStream(), 1, (byte)connType);

            _ = Task.Run(() => new NetworkClient(_client.GetStream(), name));

        }

        /// <summary>
        /// Close the current connection, and exit this network connection.
        /// </summary>
        public void CloseConnection()
        {
            _logger.Info($"Disconnecting from {_ip}: {this.ToString()}");
            _cmdWriter.writeCmdPacket(_client.GetStream(), 0, 0);
        }

        /// <summary>
        /// Get a string representation of this network connection.
        /// </summary>
        /// <returns>A string representation on this network connection</returns>
        public override string ToString()
        {
            if (_name != null)
            {
                return $"Port: {_port}, outputting to file {_name}";
            }
            return $"Port: {_port}, outputting to stream";
        }
    }
}