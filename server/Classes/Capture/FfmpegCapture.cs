using server.Interfaces.Capture;
using server.Interfaces.Options;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using NLog;

namespace server.Classes.Capture
{
    public class FfmpegCapture : ICapture
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
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

        public void DELETEME()
        {


        }

        public void start(IOptions options)
        {
                        try
            {
                process.ErrorDataReceived += new DataReceivedEventHandler((o, e) => throw new ApplicationException(e.Data));
                var server = new TcpListener(IPAddress.Any, port);
                server.Start();
                Console.WriteLine("Started process.");
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    process.Start();
                    NetworkStream stream = client.GetStream();
                    await process.StandardOutput.BaseStream.CopyToAsync(stream);
                    stopwatch.Stop();
                    client.Close();
                    server.Stop();
                    Console.WriteLine($"Finished process in {stopwatch.ElapsedMilliseconds}.");
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("error global.");
            }
        }

        public void stop()
        {
            throw new System.NotImplementedException();
        }
    }
}