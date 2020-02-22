using System.Net.Sockets;

using client.Interfaces.PacketWriter;

namespace client.Interfaces.PacketWriter.CmdPacketWriter
{
    public interface ICmdPacketWriter : IPacketWriter
    {
        void writeCmdPacket(NetworkStream stream, byte cmdType, byte networkType);
    }
}