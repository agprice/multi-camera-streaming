// System packages
using System;
using System.Text;
using System.Diagnostics;
using System.Net.Sockets;

// Custom packages
using server.Interfaces.Capture.Frame;
using server.Interfaces.Options;

namespace server.Classes.Capture.Frame
{
    public class FfmpegFrameCapture : IFrameCapture
    {
        private NetworkStream stream;
        public FfmpegFrameCapture(NetworkStream _stream)
        {
            stream = _stream;
        }

        public void CaptureFrames(IOptions options)
        {
            try
            {
                var opts = options.getOptions();
                var proc = new Process
                {
                    StartInfo =
                    {
                        FileName = "ffmpeg",
                        Arguments = opts,
                        UseShellExecute = false,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                    }
                };
                proc.ErrorDataReceived += new DataReceivedEventHandler((o, e) => Console.WriteLine(e.Data));

                // Begin process to capture and then write stream
                proc.Start();
                var data = proc.StandardOutput.ReadLine();
                proc.WaitForExit();
                WriteToStream(Encoding.ASCII.GetBytes(data));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        // This will get removed if we wrap the network calls
        private void WriteToStream(byte[] data)
        {
            Console.WriteLine(data);
        }
    }
}