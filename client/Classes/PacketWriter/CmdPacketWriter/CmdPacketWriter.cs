using System;
using System.Net.Sockets;

using NLog;

using client.Interfaces.PacketWriter.CmdPacketWriter;

namespace client.Classes.PacketWriter.CmdPacketWriter
{
    public class CmdPacketWriter : ICmdPacketWriter
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public CmdPacketWriter()
        {
        }

        public void writeCmdPacket(NetworkStream stream, byte cmdType, byte networkType)
        {
            var cmdPacket = new byte[] {1, 0};
            cmdPacket[1] = (byte)((cmdPacket[1] | cmdType) << 1);
            cmdPacket[1] = (byte)(cmdPacket[1] | networkType);
            try
            {
                stream.Write(cmdPacket, 0, cmdPacket.Length);
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}