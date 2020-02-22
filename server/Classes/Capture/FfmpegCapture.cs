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
        public Stream CaptureStream { get => captureStream; set => captureStream = value; }
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private Stream captureStream;
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
        /// <summary>
        /// Initialize a new ffmpeg capture device, and setup the callback for when the process exits.
        /// </summary>
        public FfmpegCapture()
        {
            process.Exited += ((object sender, System.EventArgs e) =>
            {
                processRunning = false;
                Logger.Info("ffmpeg process has exited.");
            });
        }
        /// <summary>
        /// Request the client begin recieving data from the server.
        /// </summary>
        /// <param name="options">The set of options to pass to the capture process</param>
        /// <param name="client">The client who will recieve the stream data</param>
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
        /// <summary>
        /// Remove the client from the list of clients recieving the stream. If no clients are streaming, stop the streaming service.
        /// </summary>
        /// <param name="client">The client to be removed from the stream</param>
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
        public async Task sendDataToClients()
        {
            Logger.Info("Sending data to clients");
            while (processRunning || CaptureStream.Length > 0)
            {
                byte[] buffer = new byte[2048];
                await CaptureStream.ReadAsync(buffer, 0, buffer.Length);
                Parallel.ForEach(clientList, client =>
                {
                    client.writeData(buffer);
                });
            }
        }
    }
}