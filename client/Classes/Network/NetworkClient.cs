using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

using NLog;
namespace client.Classes.Network
{
    public class NetworkClient
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private NetworkStream _stream;

        public NetworkClient(NetworkStream stream, string file = null)
        {
            _stream = stream;
            _ = readStream(file);
        }
        public async Task readStream(string file = null)
        {
            if(file != null){
                try
                {
                    var bw = new BinaryWriter(new FileStream(file, FileMode.Create));
                    Console.WriteLine("Writing stream to file.");

                    await _stream.CopyToAsync(bw.BaseStream);
                    bw?.Close();
                    Console.WriteLine($"Done writing to {file}");
                }
                catch(IOException ex)
                {
                    logger.Error(ex.Message);
                }
            }
            else
            {
                // TODO: Create direct streaming using mpv
            }
        } 
    }
}