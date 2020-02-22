using System;
using System.Net.Sockets;

using NLog;

using server.Interfaces.PacketReader.CmdPacketReader;

namespace server.Classes.PacketReader.CmdPacketReader
{
    public class CmdPacketReader : ICmdPacketReader
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public byte[] readCmdPacket(NetworkStream stream)
        {
            var buffer = new byte[2];
            try
            {
                stream.Read(buffer, 0, 1);
                Console.WriteLine(buffer[0]);
                buffer[1] = (byte) (buffer[0] >> 1);
                buffer[0] = (byte) ((buffer[0] ^ buffer[1]) >> 1);
                logger.Info($"Cmd packet read {buffer[0]}, {buffer[1]}");
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }

            return buffer;
        }
    }
}