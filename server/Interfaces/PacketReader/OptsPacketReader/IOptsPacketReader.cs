using System.Net.Sockets;

using server.Interfaces.PacketReader;
using server.Interfaces.Options;

namespace server.Interfaces.PacketReader.OptsPacketReader
{
    public interface IOptsPacketReader : IPacketReader
    {
        /// <summary>
        /// Receives options sent by the client
        /// </summary>
        /// <param name="options">string of options to return</param>
        /// <returns>Object of type IOptions holding the options</returns>
        IOptions receiveOptions(NetworkStream options);
    }
}