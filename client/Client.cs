using System;
using System.Linq;
using System.Threading.Tasks;
using client.Classes.Network;
using System.Collections.Generic;

namespace client
{
    class Client
    {
        static void Main(string[] args)
        {
            string ip = "127.0.0.1";
            string name = "test-tcp";
            string cmd = "";
            IDictionary<string, NetworkConnection> serverDictionary = new Dictionary<string, NetworkConnection>();
            while (cmd != "q")
            {
                Console.Write("Please input a command: ");
                cmd = Console.ReadLine();
                string[] commandArgs = cmd.Split(" ");
                switch (commandArgs.FirstOrDefault())
                {
                    case "list":
                        var dictionary = serverDictionary.Select(kvp => kvp.Key + ": " + kvp.Value.ToString());
                        Console.WriteLine(string.Join(Environment.NewLine, dictionary));
                        break;
                    case "connect":
                        if (commandArgs.Length >= 2)
                        {
                            Console.WriteLine($"Connecting to client: {commandArgs[1]}");
                            ip = commandArgs[1];
                            serverDictionary[ip] = new NetworkConnection();
                            // Temporarily use random names for files to differentiate the streams.
                            Random rnd = new Random();
                            Task.Run(() => serverDictionary[ip].ConnectTo(ip, "tcp", name + rnd.Next(1, 1000) + ".mp4"));
                        }
                        else
                        {
                            Console.WriteLine("Please specify an IP.");
                        }
                        break;
                    case "disconnect":
                        ip = commandArgs[1];
                        serverDictionary[ip].CloseConnection();
                        break;
                    default:
                        Console.WriteLine("Command not found. Use 'help' to view list of valid commands.");
                        break;
                }
            }
        }
    }
}
