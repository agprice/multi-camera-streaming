using System;
using System.Linq;
using System.Threading.Tasks;
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
            string cmd = "";
            while (cmd != "q")
            {
                Console.Write("Please input a command: ");
                cmd = Console.ReadLine();
                string[] commandArgs = cmd.Split(" ");
                switch (commandArgs.FirstOrDefault())
                {
                    case "connect":
                        if (commandArgs.Length >= 2)
                        {
                            Console.WriteLine($"Connecting to client: {commandArgs[1]}");
                            ip = commandArgs[1];
                            Task.Run(() => new NetworkConnection(new string[] { ip, "tcp", name }));
                        }
                        else
                        {
                            Console.WriteLine("Please specify an IP.");
                        }
                        break;
                }
            }
            // var client = new NetworkConnection(new string[]{ip, "tcp", name});
        }
        // static void testBytesPacket(int port, string ip)
        // {
        //     TcpClient client = new TcpClient(ip, port);
        //     ICmdPacketWriter cmdPacket = new CmdPacketWriter(); 
        //     try{
        //         cmdPacket.writeCmdPacket(client.GetStream(), 1, 1);
        //         Console.WriteLine("Buffer written");
        //     }
        //     catch(Exception ex)
        //     {
        //         Console.WriteLine(ex.Message);
        //     }
        //     client.Close();
        // }
        // async static Task test(string name, string ip, int port)
        // {
        //     TcpClient client = new TcpClient(ip, port);
        //     BinaryWriter bw;

        //     try
        //     {
        //         Stopwatch stopwatch = new Stopwatch();
        //         stopwatch.Start();
        //         bw = new BinaryWriter(new FileStream(name, FileMode.Create));
        //         Console.WriteLine("Writing stream to file.");
        //         await client.GetStream().CopyToAsync(bw.BaseStream);
        //         stopwatch.Stop();
        //         bw?.Close();
        //         Console.WriteLine($"Finished process in {stopwatch.ElapsedMilliseconds}.");
        //     }
        //     catch (IOException ex)
        //     {
        //         Console.WriteLine(ex.Message);
        //         Console.WriteLine("error IO.");
        //     }
        // }
    }
}
