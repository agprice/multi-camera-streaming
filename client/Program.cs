using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

namespace client
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 9001;
            String ip = "127.0.0.1";
            if (args.Length == 0)
            {
                Task.Run(() => test(ip, port));
                Thread.Sleep(-1);
            }
            if (args.Length < 2)
            {
                Task.Run(() => test(args[0], port));
                Thread.Sleep(-1);
            }
            if (Int32.TryParse(args[1], out port))
            {
                Task.Run(() => test(args[0], port));
                Thread.Sleep(-1);
            }
            else
            {
                Console.WriteLine("Bad argument for remote port.");
            }
        }
        async static Task test(string ip, int port)
        {
            TcpClient client = new TcpClient(ip, port);
            BinaryWriter bw;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                bw = new BinaryWriter(new FileStream("test-tcp.mp4", FileMode.Create));
                await client.GetStream().CopyToAsync(bw.BaseStream);
                stopwatch.Stop();
                bw?.Close();
                Console.WriteLine($"Finished process in {stopwatch.ElapsedMilliseconds}.");
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("error IO.");
            }
        }
    }
}
