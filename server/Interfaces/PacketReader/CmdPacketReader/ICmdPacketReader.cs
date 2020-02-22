using System.Net.Sockets;

namespace server.Interfaces.PacketReader.CmdPacketReader
{
    public interface ICmdPacketReader : IPacketReader
    {
        byte[] readCmdPacket(NetworkStream stream);
    }
}