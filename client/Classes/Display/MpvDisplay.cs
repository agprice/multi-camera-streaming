using System;
using System.Diagnostics;
using NLog;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using client.Interfaces.Display;
using client.Classes.Constants;

namespace client.Classes.Display
{
    public class MpvDisplay : IDisplay
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private Process _process;
        private Stream _inputStream;

        /// <summary>
        /// Initialize a new ffmpeg capture device, and setup the callback for when the process exits.
        /// </summary>
        public MpvDisplay(Stream inputStream)
        {
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

            _process.Exited += ((object sender, System.EventArgs e) =>
            {
                _logger.Info("ffmpeg process has exited.");
            });
        }

        public async Task startDisplay()
        {
            _logger.Info($"Opening MPV with network stream");
            _process.Start();
            await _inputStream.CopyToAsync(_process.StandardInput.BaseStream);
        }

        public void closeDisplay()
        {
            _logger.Info($"Closing MPV");
            _process.Kill();
        }
    }
}