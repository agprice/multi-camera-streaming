using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Net.Sockets;

namespace client
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => test());
            Thread.Sleep(-1);
        }
        async static Task test()
        {
            Int32 port = 9001;
            TcpClient client = new TcpClient("127.0.0.1", port);
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
