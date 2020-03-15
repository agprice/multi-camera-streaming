using System;
using System.Diagnostics;
using NLog;
using System.IO;
using System.Threading.Tasks;
using client.Interfaces.Display;
using client.Classes.Constants;

namespace client.Classes.Display
{
    /// <summary>
    /// This is the MPV display class, which starts an MPV 
    /// process that can be used to display the datastream.
    /// </summary>
    public class MpvDisplay : IDisplay
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public Process _process;
        private Stream _inputStream;
        private string _id = null;

        private EventHandler<string> _windowClosedEvent;
        public EventHandler<string> WindowClosedEvent { get => _windowClosedEvent; set => _windowClosedEvent = value; }

        /// <summary>
        /// Initialize a new ffmpeg capture device, and setup the callback for when the process exits.
        /// </summary>
        public MpvDisplay(Stream inputStream, string id)
        {
            _id = id;
            _inputStream = inputStream;
            _process = new Process
            {
                StartInfo =
                    {
                        FileName = ConfigRuntimeConstants.MPV,
                        Arguments = "--no-config --no-terminal --profile low-latency -",
                        UseShellExecute = false,
                        RedirectStandardOutput = false,
                        RedirectStandardInput = true,
                        RedirectStandardError = false,
                        CreateNoWindow = true
                    },
                EnableRaisingEvents = true
            };
        }

        private void DisconnectOnWindowClosed(object sender, EventArgs e)
        {
            _logger.Info($"mpv process has exited for server {_id}");
            _windowClosedEvent?.Invoke(this, _id);
        }
        /// <summary>
        /// Starts the MPV process, and copies the stream data over to be displayed.
        /// </summary>
        /// <returns></returns>
        public async Task startDisplay()
        {
            _logger.Info($"Opening MPV with network stream for server {_id}");
            _process.Exited += DisconnectOnWindowClosed;
            _process.Start();
            await _inputStream.CopyToAsync(_process.StandardInput.BaseStream);
            _process.Kill();
        }

        /// <summary>
        /// Closes the MPV process, shutting down the streaming display.
        /// </summary>
        public void closeDisplay()
        {
            _logger.Info($"Closing MPV for server {_id}");
            _process.Exited -= DisconnectOnWindowClosed;
            _process.Kill();
        }
    }
}