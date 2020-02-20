using System.Net.Sockets;
using System.Text;

using server.Interfaces.Options;
using server.Interfaces.PacketReader.OptsPacketReader;

using server.Classes.Options;

namespace server.Classes.PacketReader.OptsPacketReader
{
    public class FfmpegOptsPacketReader : IOptsPacketReader
    {
        public FfmpegOptsPacketReader() { }

        public IOptions receiveOptions(NetworkStream stream)
        {
            var buffer = new byte[256];
            var message = new StringBuilder();

            do
            {
                stream.Read(buffer, 0, buffer.Length);
                message.AppendFormat("{0}", Encoding.ASCII.GetString(buffer));
            } while (stream.DataAvailable);

            var ffmpegops = new FfmpegOptions();
            ffmpegops.add(message.ToString());

            return ffmpegops;
        }
    }
}