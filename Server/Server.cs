using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Server
    {

        private static TcpListener socket;
        private static int port = 46495;
        public void Initialise()
        {
            Console.WriteLine("Starting server on port: " + port + "...");
            socket = new TcpListener(IPAddress.Any, port);
            socket.Start();
            socket.BeginAcceptTcpClient(new AsyncCallback(ClientConnected), null);
            Console.WriteLine("Done!");
        }

        private static void ClientConnected(IAsyncResult _result)
        {
            TcpClient _client = socket.EndAcceptTcpClient(_result);
            _client.NoDelay = false;
            socket.BeginAcceptTcpClient(new AsyncCallback(ClientConnected), null);
            Console.WriteLine("Incoming connection from " + _client.Client.RemoteEndPoint.ToString());
        }
    }
}
