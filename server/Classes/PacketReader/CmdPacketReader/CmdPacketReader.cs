using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;

using NLog;

using server.Interfaces.PacketReader.CmdPacketReader;

namespace server.Classes.PacketReader.CmdPacketReader
{
    public class CmdPacketReader : ICmdPacketReader
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public async Task<List<byte>> readCmdPacket(NetworkStream stream)
        {
            var buffer = new byte[2];
            try
            {
                await stream.ReadAsync(buffer, 0, 1);
                buffer[1] = (byte) (buffer[0] >> 1);
                buffer[0] = (byte) ((buffer[0] ^ buffer[1]) >> 1);
                logger.Info($"Cmd packet read {buffer[0]}, {buffer[1]}");
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }

            return buffer.ToList();
        }
    }
}