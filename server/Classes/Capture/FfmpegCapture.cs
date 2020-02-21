using server.Interfaces.Capture;
using server.Interfaces.Options;
using System;
using System.Diagnostics;
using NLog;
using System.IO;

namespace server.Classes.Capture
{
    public class FfmpegCapture : ICapture
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private Stream captureStream;
        public Stream CaptureStream { get => captureStream; set => captureStream = value; }
        private IOptions lastOptions = null;
        private uint numberStreaming = 0;
        private bool processRunning = false;
        private Process process = new Process
        {
            StartInfo =
                    {
                        FileName = "ffmpeg",
                        Arguments = "-hide_banner -loglevel error -s 1920x1080 -framerate 30 -f x11grab -i :0.0+0,0 -frames:v 600 -vcodec h264 -crf 23 -tune zerolatency -preset ultrafast -f mpegts pipe:1",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardInput = false,
                        RedirectStandardError = false,
                        CreateNoWindow = true
                    },
            EnableRaisingEvents = true
        };

        public FfmpegCapture()
        {
            process.Exited += ((object sender, System.EventArgs e) =>
            {
                processRunning = false;
                Logger.Info("ffmpeg process has exited.");
                // Logger.Info($"Ffmpeg process exited with code: {process.ExitCode}. It ran for {Math.Round((process.ExitTime - process.StartTime).TotalMilliseconds)}");
            });
        }

        public void start(IOptions options)
        {
            numberStreaming++;
            Logger.Info("Client requesting streaming start");
            if (!processRunning)
            {
                Logger.Info("Starting the ffmpeg process");
                processRunning = true;
                process.ErrorDataReceived += new DataReceivedEventHandler((o, e) => throw new ApplicationException(e.Data));
                process.Start();
                CaptureStream = process.StandardOutput.BaseStream;
            }
            else
            {
                Logger.Info("Ffmpeg requested start but was already streaming.");
            }
        }

        public void stop()
        {
            numberStreaming--;
            Logger.Info($"Client has stopped streaming. Currently {numberStreaming} clients are streaming.");
            if (numberStreaming == 0)
            {
                Logger.Info("Stopping ffmpeg process");
                CaptureStream = null;
                process.Kill();
            }
        }
    }
}