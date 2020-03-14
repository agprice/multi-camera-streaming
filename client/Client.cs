using System;
using System.Linq;
using System.Threading.Tasks;
using client.Classes.Network;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using client.Classes.Constants;

namespace client
{
    class Client
    {
        static void Main(string[] args)
        {
            // Use the appsettings.json to configure defaults.
            var builder = new ConfigurationBuilder().AddJsonFile(ConfigRuntimeConstants.SETTINGS_FILE, optional: false, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            string port = configuration.GetSection(ConfigRuntimeConstants.NETWORK)["port"];
            string ip = "127.0.0.1";
            string name = "test-tcp";
            string cmd = "";
            // This dictionary keeps track of all the connections
            IDictionary<string, NetworkConnection> serverDictionary = new Dictionary<string, NetworkConnection>();
            // Interactive CLI stays open until q is pressed.
            while (cmd.ToLower() != "q")
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
                            var ipArgs = commandArgs[1].Split(":");
                            ip = ipArgs[0];
                            if (ipArgs.Length == 2) {
                                port = ipArgs[1];
                            }
                            serverDictionary[ip] = new NetworkConnection();
                            if (commandArgs.Length >= 3)
                            {
                                name = commandArgs[2];
                            }
                            Task.Run(() => serverDictionary[ip].ConnectTo(ip, "tcp", name + ".mp4", Int32.Parse(port)));
                        }
                        else
                        {
                            Console.WriteLine("Please specify an IP.");
                        }
                        break;
                    case "disconnect":
                        ip = commandArgs[1];
                        serverDictionary[ip].CloseConnection();
                        serverDictionary.Remove(ip);
                        break;
                    case "q":
                        foreach(KeyValuePair<string, NetworkConnection> server in serverDictionary) {
                            server.Value.CloseConnection();
                        }
                        break;
                    default:
                        Console.WriteLine("Command not found. Use 'help' to view list of valid commands.");
                        break;
                }
            }
        }
    }
}
