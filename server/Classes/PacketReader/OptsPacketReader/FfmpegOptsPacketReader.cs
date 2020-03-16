using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

using NLog;

using server.Interfaces.Options;
using server.Interfaces.PacketReader.OptsPacketReader;

using server.Classes.Options;

namespace server.Classes.PacketReader.OptsPacketReader
{
    public class FfmpegOptsPacketReader : IOptsPacketReader
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public FfmpegOptsPacketReader() { }

        public async Task<IOptions> receiveOptions(NetworkStream stream)
        {
            var bufferSize = stream.ReadByte();
            var buffer = new byte[bufferSize];
            var message = new StringBuilder();

            await stream.ReadAsync(buffer, 0, buffer.Length);
            message.AppendFormat("{0}", Encoding.ASCII.GetString(buffer));

            var ffmpegops = new FfmpegOptions();
            ffmpegops.add(message.ToString());
            
            return ffmpegops;
        }
    }
}