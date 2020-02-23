﻿using System;
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
            string ip = "127.0.0.1";
            string name = "test-tcp.mp4";
            if (args.Length >= 1)
            {
                name = args[0];
            }
            if (args.Length <= 1)
            {
                Task.Run(() => test(name, ip, port));
                Thread.Sleep(-1);
            }
            if (args.Length < 3)
            {
                Task.Run(() => test(name, args[1], port));
                Thread.Sleep(-1);
            }
            if (Int32.TryParse(args[2], out port))
            {
                Task.Run(() => test(name, args[1], port));
                Thread.Sleep(-1);
            }
            else
            {
                Console.WriteLine("Bad argument for remote port.");
            }
        }
        async static Task test(string name, string ip, int port)
        {
            TcpClient client = new TcpClient(ip, port);
            BinaryWriter bw;

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                bw = new BinaryWriter(new FileStream(name, FileMode.Create));
                Console.WriteLine("Writing stream to file.");
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