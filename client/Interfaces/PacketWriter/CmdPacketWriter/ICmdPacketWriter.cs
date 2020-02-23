using System.Net.Sockets;

using client.Interfaces.PacketWriter;

namespace client.Interfaces.PacketWriter.CmdPacketWriter
{
    public interface ICmdPacketWriter : IPacketWriter
    {
        /// <summary>
        /// Writes the command packet to send to the server 
        /// with network connection type and command type (start or stop).
        /// The packet inclues a header to determine packet type.
        /// </summary>
        /// <param name="stream">Network stream to write packets to</param>
        /// <param name="cmdType">Command type from the client (1- start, 0 - end)</param>
        /// <param name="networkType">Type of network protocol (1 - TCP, 0 - UDP)</param>
        void writeCmdPacket(NetworkStream stream, byte cmdType, byte networkType);
    }
}