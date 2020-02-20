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

        Process process = new Process
        {
            StartInfo =
                    {
                        FileName = "ffmpeg",
                        Arguments = "-hide_banner -loglevel error -s 1920x1080 -framerate 30 -f x11grab -i :0.0+0,0 -frames:v 3000 -vcodec h264 -crf 23 -tune zerolatency -preset ultrafast -f mpegts pipe:1",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardInput = false,
                        RedirectStandardError = false,
                        CreateNoWindow = true
                    }
        };

        public void start(IOptions options)
        {
            process.ErrorDataReceived += new DataReceivedEventHandler((o, e) => throw new ApplicationException(e.Data));
            process.Start();
            CaptureStream = process.StandardOutput.BaseStream;
        }

        public void stop()
        {
            CaptureStream = null;
            process.Kill();
        }
    }
}