using System;
using System.Diagnostics;
using System.IO;

namespace server
{
    class Server
    {
        static void Main(string[] args)
        {
            test();
        }
        static void test()
        {
            try
            {
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = "ffmpeg",
                        Arguments = "-hide_banner -loglevel error -s 1920x1080 -framerate 30 -f x11grab -i :0.0+0,0 -frames:v 512 -vcodec h264 -preset ultrafast -f mpegts pipe:1",
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
                BinaryReader binaryReader;
                try
                {
                    binaryReader = new BinaryReader(process.StandardOutput.BaseStream);
                    bw = new BinaryWriter(new FileStream("test.mp4", FileMode.Create));
                    process.StandardOutput.BaseStream.CopyTo(bw.BaseStream);
                    bw.Flush();
                    bw?.Close();
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
