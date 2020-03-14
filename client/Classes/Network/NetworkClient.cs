using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

using NLog;
namespace client.Classes.Network
{
    public class NetworkClient
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
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
                    _logger.Info("Writing stream to file");

                    await _stream.CopyToAsync(bw.BaseStream);
                    bw?.Close();
                    _logger.Info($"Done writing to file {file}");
                }
                catch(IOException ex)
                {
                    _logger.Error(ex.Message);
                }
            }
            else
            {
                // TODO: Create direct streaming using mpv
            }
        } 
    }
}