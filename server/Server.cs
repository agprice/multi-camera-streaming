using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using server.Classes.Network;
using server.Classes.PacketReader.CmdPacketReader;
using server.Interfaces.PacketReader.CmdPacketReader;
using System.Net;

namespace server
{
    public class Server
    {
        static void Main(string[] args)
        {
            Task.Run(() => new NetworkConnectionInitializer().awaitConnections());
            Thread.Sleep(-1);
        }
        static void TestReceivePackets()
        {
            Int32 port = 9001;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            ICmdPacketReader reader = new CmdPacketReader();
            var server = new TcpListener(IPAddress.Any, port);
            server.Start();

            try{
                var client = server.AcceptTcpClient();
                var tempBuffer = new byte[1];
                client.GetStream().Read(tempBuffer, 0, 1);
                var buffer = reader.readCmdPacket(client.GetStream());
            }
            catch(IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
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
                        Arguments = "-hide_banner -loglevel error -s 1920x1080 -framerate 30 -f x11grab -i :0.0+0,0 -frames:v 3000 -vcodec h264 -crf 23 -tune zerolatency -preset ultrafast -f mpegts pipe:1",
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

        async static Task test2()
        {
            try
            {
                var process = new Process
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
                process.ErrorDataReceived += new DataReceivedEventHandler((o, e) => throw new ApplicationException(e.Data));
                Int32 port = 9001;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
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
    }
}