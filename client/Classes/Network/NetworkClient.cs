using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using client.Classes.Display;
using client.Interfaces.Display;

using NLog;
namespace client.Classes.Network
{
    /// <summary>
    /// This client handles opening and writing either to a file, or the MPV stream.
    /// </summary>
    public class NetworkClient
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private NetworkStream _stream;
        private IDisplay _display;

        /// <summary>
        /// Initializes the network Client.
        /// </summary>
        /// <param name="stream">The TCP/UDP stream to be read from</param>
        /// <param name="file">The name of the file to write to. By default no file will be written to</param>
        /// <param name="display">The display to display to if there is no file to be written to.</param>
        public NetworkClient(NetworkStream stream, string file = null, IDisplay display = null)
        {
            _display = display;
            _stream = stream;
            _ = readStream(file);
        }

        /// <summary>
        /// Begin reading the stream, and either displaying it or writing it to a file.
        /// </summary>
        /// <param name="file">Optional filename.</param>
        /// <returns></returns>
        private async Task readStream(string file = null)
        {
            if (file != null)
            {
                try
                {
                    var bw = new BinaryWriter(new FileStream(file, FileMode.Create));
                    _logger.Info("Writing stream to file");

                    await _stream.CopyToAsync(bw.BaseStream);
                    bw?.Close();
                    _logger.Info($"Done writing to file {file}");
                }
                catch (IOException ex)
                {
                    _logger.Error(ex.Message);
                }
            }
            else
            {
                _ = _display?.startDisplay();
            }
        }
    }
}