using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using client.Interfaces.PacketWriter.CmdPacketWriter;
using client.Classes.PacketWriter.CmdPacketWriter;
using client.Classes.Network;

namespace client
{
    class Client
    {
        static void Main(string[] args)
        {
            int port = 9001;
            string ip = "127.0.0.1";
            string name = "test-tcp.mp4";
            var client = new NetworkConnection(new string[]{ip, "tcp", name});
        }
        static void testBytesPacket(int port, string ip)
        {
            TcpClient client = new TcpClient(ip, port);
            ICmdPacketWriter cmdPacket = new CmdPacketWriter(); 
            try{
                cmdPacket.writeCmdPacket(client.GetStream(), 1, 1);
                Console.WriteLine("Buffer written");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            client.Close();
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
