using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Net.Sockets;

using NLog;

using client.Interfaces.PacketWriter.OptsPacketWriter;

namespace client.Classes.PacketWriter.OptsPacketWriter
{
    public class  FfmpegOptsPacketWriter : IOptPacketWriter
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public FfmpegOptsPacketWriter() { }

        public void writeOptsPacket(NetworkStream stream, List<string> args)
        {
            // TODO: need to read list to make sure the values are true
            argSanityCheck(args);

            var cmdPacket = new byte[args.Count + 1];
            cmdPacket[0] = 0;
            //Append args as byte array
            Array.Copy(args.SelectMany(s => Encoding.ASCII.GetBytes(s)).ToArray(), 0, cmdPacket, 1, args.Count);

            try
            {
                stream.Write(cmdPacket, 0, cmdPacket.Length);
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void argSanityCheck(List<string> args)
        {
            // Check to see if at least 2 arguments exist or if the number of flags matches the number of arguments
            if(args.Count < 2 || args.Count % 2 != 0) throw new ArgumentException("There are not enough arguments to pass to ffmpeg");

            // Check if the flags are supported
            var flags = args.Where(str => str.Contains('-')).Select(str => str.Replace("-", "")).ToList();
            foreach(var flag in flags)
            {
                if(!Enum.IsDefined(typeof(Arguments), flag)) throw new ArgumentException("Argument " + flag + " is not supported");
            }
            
            // Determine whether the user has actually 
            if(!Enum.IsDefined(typeof(Arguments), flags.First(x => x.Equals("capture_device")))) throw new ArgumentException("A capture device must be set");
        }

        private enum Arguments
        {
            capture_device,
            crf,
            max_bitrate,
            const_bitrate,
            hencoding
        }
    }
}