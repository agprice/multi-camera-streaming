using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace server.Interfaces.PacketReader.CmdPacketReader
{
    public interface ICmdPacketReader : IPacketReader
    {
        /// <summary>
        /// Read command packet from the client
        /// </summary>
        /// <param name="stream">Network stream from the client</param>
        /// <returns>Returns the commands as an array of bytes</returns>
        Task<List<byte>> readCmdPacket(NetworkStream stream);
    }
}