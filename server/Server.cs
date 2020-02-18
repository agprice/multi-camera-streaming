using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace server
{
    class Server
    {
        static void Main(string[] args)
        {
            Task.Run(() => test());
            Thread.Sleep(12000);
        }
        async static Task test()
        {
            try
            {
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = "ffmpeg",
                        Arguments = "-hide_banner -loglevel error -s 1920x1080 -framerate 30 -f x11grab -i :0.0+0,0 -frames:v 60 -vcodec h264 -crf 23 -preset ultrafast -f mpegts pipe:1",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardInput = false,
                        RedirectStandardError = false,
                        CreateNoWindow = true
                    }
                };
                process.ErrorDataReceived += new DataReceivedEventHandler((o, e) => throw new ApplicationException(e.Data));
                process.Start();
                Console.WriteLine("Started process.");
                BinaryWriter bw;
                Stopwatch stopwatch = Stopwatch.StartNew();
                try
                {
                    bw = new BinaryWriter(new FileStream("test.mp4", FileMode.Create));
                    await process.StandardOutput.BaseStream.CopyToAsync(bw.BaseStream);
                    bw?.Close();
                    stopwatch.Stop();
                    Console.WriteLine($"Finished process in {stopwatch.ElapsedMilliseconds}.");
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("error IO.");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                    Console.WriteLine("error global.");

            }
        }
    }
}
