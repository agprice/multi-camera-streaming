using System.Collections.Generic;
using System.Net.Sockets;

using client.Interfaces.PacketWriter;

namespace client.Interfaces.PacketWriter.OptsPacketWriter
{
    public interface IOptPacketWriter : IPacketWriter
    {
        void writeOptsPacket(NetworkStream stream, List<string> args);
    }
}