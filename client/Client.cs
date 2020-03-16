using System;
using System.Linq;
using System.Threading.Tasks;
using client.Classes.Network;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using client.Classes.Constants;

namespace client
{
    /// <summary>
    /// This is the main class for the client application. It spawns all connections to servers, and manages them via a CLI.
    /// </summary>
    class Client
    {
        static string helpDialog =
        @"Welcome to the Multi-Camera Streaming Basic CLI! The following commands are availabe:
        'help': Display this dialog
        'connect [ip][:port] [outfilename]': This command connects to a server. The port and output file are optional.
            If no output file is specified, then the client will attemp to open the stream with an MPV client.
            examples:
                connect 192.168.1.123:9001 mySaveFile.mp4
                connect 192.168.1.123
        'disconnect [id]': This disconnects the specified server. Get the ID with the list command. Any partial 
            ID will disconnect any IDs that match. E.G. 'disconnect 127' will disconnect any idea that matches 127*
        'list': List all the connected clients and their IDs.
        'q': Quit the client, closing any active connections.

        If previewing using MPV windows, the session with the server will be closed when the MPV window is closed.
        ";

        /// <summary>
        /// This main contains a lazily written CLI which allows for interfacing with the client.
        /// </summary>
        /// <param name="args">CLI args, not used</param>
        static void Main(string[] args)
        {
            // Use the appsettings.json to configure defaults.
            var builder = new ConfigurationBuilder().AddJsonFile(ConfigRuntimeConstants.SETTINGS_FILE, optional: false, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            string port = configuration.GetSection(ConfigRuntimeConstants.NETWORK)["port"];
            string ip = "127.0.0.1";
            string name = null;
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
                            Console.WriteLine($"Attempting connection to server: {commandArgs[1]}");
                            var ipArgs = commandArgs[1].Split(":");
                            ip = ipArgs[0];
                            if (ipArgs.Length == 2)
                            {
                                port = ipArgs[1];
                            }
                            NetworkConnection newServer = new NetworkConnection();
                            // Setup callbacks on succesful connections, or connection closed
                            newServer.ConnectionSuccesful += ((object sender, string id) =>
                            {
                                Console.WriteLine($"\nConnected to {id}");
                                serverDictionary[id] = sender as NetworkConnection;
                                Console.Write("Please input a command: ");
                            });
                            newServer.DisplayClosed += ((object sender, string id) =>
                            {
                                Console.WriteLine($"\nDisconnecting from {id}");
                                serverDictionary[id].CloseConnection();
                                serverDictionary.Remove(id);
                            });
                            if (commandArgs.Length >= 3)
                            {
                                name = commandArgs[2];
                            }
                            else
                            {
                                name = null;
                            }
                            // Start attempted connection
                            Task.Run(() => newServer.ConnectTo(ip, "tcp", name, Int32.Parse(port)));
                        }
                        else
                        {
                            Console.WriteLine("Please specify an IP.");
                        }
                        break;
                    case "disconnect":
                        if (commandArgs.Length < 2)
                        {
                            Console.WriteLine("Please specify an ID. View connections with the 'list' command.");
                            continue;
                        }
                        ip = commandArgs[1];
                        var serversToDisconnect = serverDictionary.Where(kvp => kvp.Key.Contains(ip));
                        if (serversToDisconnect != null)
                        {
                            foreach (var serverKVP in serversToDisconnect)
                            {
                                Console.WriteLine($"\nDisconnecting from {serverKVP.Key}");
                                serverDictionary[serverKVP.Key].CloseConnection();
                                serverDictionary.Remove(serverKVP.Key);
                            }
                        }
                        break;
                    case "help":
                        Console.WriteLine(helpDialog);
                        break;
                    case "q":
                        foreach (KeyValuePair<string, NetworkConnection> server in serverDictionary)
                        {
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
