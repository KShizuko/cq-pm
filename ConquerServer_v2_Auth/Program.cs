using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using ConquerServer_v2.Database;
using System.IO;

namespace ConquerServer_v2
{
    public partial class Program
    {
        static NetworkServerSocket AuthServer;
        static void Main(string[] args)
        {
            // TO-DO:
            // Add shit to make the console look cooler.
            Console.Title = "Conquer Server - Auth";

            BruteForce.BruteforceProtection.Init(7);

            AuthServer = new NetworkServerSocket();
            AuthServer.ClientBufferSize = 1000;
            AuthServer.OnConnect = new NetworkClientConnection(Auth_ClientConnect);
            AuthServer.OnReceive = new NetworkClientReceive(Auth_ClientReceive);

            AuthServer.Prepare(9959, 100);
            AuthServer.BeginAccept();

        }
    }
}
