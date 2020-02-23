using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using server.Classes.Network;
namespace server
{
    public class Server
    {
        static void Main(string[] args)
        {
            Task.Run(() => new NetworkConnectionInitializer().awaitConnections());
            Thread.Sleep(-1);
        }
    }
}