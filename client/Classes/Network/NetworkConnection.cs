using System.Net.Sockets;
using System.Threading.Tasks;

using NLog;

using client.Interfaces.PacketWriter.CmdPacketWriter;
using client.Classes.PacketWriter.CmdPacketWriter;
using client.Interfaces.Display;
using client.Classes.Display;
using System;

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

        private string _id = null;
        private IDisplay _display;

        public event EventHandler<string> ConnectionClosed;

        public event EventHandler<string> ConnectionSuccesful;

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
            try
            {
                _logger.Info($"Attempting connection to {_ip}");
                _client = new TcpClient(ip, _port);
                // Generate a unique client id
                _id = $"{_ip}:{port}-{_client.GetHashCode()}";
                // If the connection was obtained, notify any listeners
                ConnectionSuccesful.Invoke(this, _id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"\nThere was an error connecting to {ip} on port {port}");
                return;
            }
            _logger.Info($"Connecting to server: {ip}, on port {port}");
            var connType = (transportType.ToLower().Equals("tcp")) ? 1 : 0;
            _cmdWriter.writeCmdPacket(_client.GetStream(), 1, (byte)connType);
            _logger.Info(name);
            if (_name == null)
            {
                _display = new MpvDisplay(_client.GetStream(), _id);
                _display.WindowClosedEvent = ConnectionClosed;
            }
            _ = Task.Run(() => new NetworkClient(_client.GetStream(), name, _display));
        }

        /// <summary>
        /// Close the current connection, and exit this network connection.
        /// </summary>
        public void CloseConnection()
        {
            _logger.Info($"Disconnecting from {_ip}: {this.ToString()}");
            _cmdWriter?.writeCmdPacket(_client?.GetStream(), 0, 0);
            _display?.closeDisplay();
            _client?.Close();
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