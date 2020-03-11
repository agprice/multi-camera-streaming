using System.Collections.Generic;
using System.Net.Sockets;

using client.Interfaces.PacketWriter.OptsPacketWriter;

namespace client.Classes.PacketWriter.OptsPacketWriter
{
    public class  FfmpegOptsPacketWriter : IOptPacketWriter
    {
        public FfmpegOptsPacketWriter() { }

        public void writeOptsPacket(NetworkStream stream, List<string> args)
        {
            // TODO: need to read list to make sure the values are true
        }
    }
}