using server.Interfaces.Capture;
using server.Interfaces.Options;
using System;
using System.Diagnostics;
using NLog;
using System.IO;
using server.Classes.Network;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.Classes.Capture
{
    public class FfmpegCapture : ICapture
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private Stream captureStream;
        public Stream CaptureStream { get => captureStream; set => captureStream = value; }
        private IOptions lastOptions = null;
        private bool processRunning = false;
        private List<NetworkClientConnection> clientList = new List<NetworkClientConnection>();
        private Process process = new Process
        {
            StartInfo =
                    {
                        FileName = "ffmpeg",
                        Arguments = "-hide_banner -loglevel error -vaapi_device /dev/dri/renderD128 -video_size 1920x1080 -framerate 60 -vsync 2 -f x11grab -i :0.0+0,0 -vf format=nv12,hwupload -vcodec h264_vaapi -crf 23 -tune zerolatency -preset ultrafast -f mpegts pipe:1",
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
        public void requestStream(IOptions options, NetworkClientConnection client)
        {
            clientList.Add(client);
            Logger.Info("Client requesting streaming start");
            if (!processRunning)
            {
                Logger.Info("Starting the ffmpeg process");
                processRunning = true;
                process.ErrorDataReceived += new DataReceivedEventHandler((o, e) => throw new ApplicationException(e.Data));
                process.Start();
                Task.Run(() => sendDataToClients());
                CaptureStream = process.StandardOutput.BaseStream;
            }
            else
            {
                Logger.Info("Ffmpeg requested start but was already streaming.");
            }
        }

        public async Task sendDataToClients()
        {
            Logger.Info("Sending data to clients");
            while (processRunning)
            {
                byte[] buffer = new byte[2048];
                await CaptureStream.ReadAsync(buffer, 0, buffer.Length);
                foreach (var client in clientList)
                {
                    client.writeData(buffer);
                }
            }
        }

        public void stopStreaming(NetworkClientConnection client)
        {
            clientList.Remove(client);
            Logger.Info($"Client has stopped streaming. Currently {clientList.Count} clients are streaming.");
            if (clientList.Count == 0)
            {
                Logger.Info("Stopping ffmpeg process");
                CaptureStream = null;
                process.Kill();
            }
        }
    }
}