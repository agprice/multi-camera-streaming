using System;
using System.Diagnostics;

namespace server
{
    class Program
    {
        static void Main(string[] args)
        {
            try{
                var process = new Process
                {
                    StartInfo = 
                    {
                        FileName = "ffmpeg",
                        Arguments = "-f v4l2 -i /dev/video0 -framerate 20 -frames:v 20 -s 1366x768 -vcodec h264 -f mpegts pipe:1",
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true
                    }
                };
                process.Start();
                var value = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                Console.WriteLine(value.Length);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
