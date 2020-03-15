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
            // Checks for allowed values
            argSanityCheck(args);

            // Convert list to string then to byte array to send to the network
            var argsAsBytes = Encoding.ASCII.GetBytes(string.Join(" ", args));
            var optPacket = new byte[argsAsBytes.Length + 1];
            optPacket[0] = 0;
            //Append argument string as byte array
            Array.Copy(argsAsBytes, 0, optPacket, 1, argsAsBytes.Length);

            try
            {
                stream.Write(optPacket, 0, optPacket.Length);
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void argSanityCheck(List<string> args)
        {
            // Check to see if at least 2 arguments exist or if the number of flags matches the number of arguments
            if(args.Count % 2 != 0) throw new ArgumentException("There are not enough arguments to pass to ffmpeg");

            // Check if the flags are supported
            var flags = args.Where(str => str.Contains('-')).Select(str => str.Replace("-", "")).ToList();
            foreach(var flag in flags)
            {
                if(!Enum.IsDefined(typeof(Arguments), flag)) throw new ArgumentException("Argument " + flag + " is not supported");
            }
            
            // Determine whether the user has actually 
            if(!Enum.IsDefined(typeof(Arguments), flags.First(x => x.Equals(Arguments.format.ToString())))) throw new ArgumentException("A capture device must be set");
        }

        private enum Arguments
        {
            format,
        }
    }
}