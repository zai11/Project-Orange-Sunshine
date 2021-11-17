using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Server
    {

        private static TcpListener socketListener;
        private static int port = 46495;

        static void Main(string[] args)
        {
            try
            {
                socketListener = new TcpListener(IPAddress.Any, port);
                socketListener.Start();
                Console.WriteLine("Web Server Running... Press ^C to stop...");
                // Start the thread which calls the method 'StartListen'
                Thread th = new Thread(new ThreadStart(StartListen));
                th.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("An exception has occurred while listening: " + e.ToString());
            }
        }

        private static void SendToClient(string data, ref Socket socket)
        {
            Console.WriteLine("Sending Data: " + data);
            SendToClient(Encoding.ASCII.GetBytes(data), ref socket);
        }

        private static void SendToClient(byte[] data, ref Socket socket)
        {
            int numBytes = 0;
            try
            {
                if (socket.Connected)
                {
                    if ((numBytes = socket.Send(data, data.Length, 0)) == -1)
                        Console.WriteLine("Socket Error: cannot send packet");
                    else
                        Console.WriteLine("No. of bytes sent {0}", numBytes);
                }
                else
                {
                    Console.WriteLine("Connection Dropped...");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An exception has occurred: " + e.ToString());
            }
        }

        private static void StartListen()
        {

            while (true)
            {
                // Accept a new connection
                Socket socket = socketListener.AcceptSocket();
                if (socket.Connected)
                {
                    Console.WriteLine("\nClient Connected!!\n==================\nClient IP {0}\n", socket.RemoteEndPoint);
                    // Make a byte array and receive data from the client
                    byte[] receive = new byte[1024];
                    _ = socket.Receive(receive, receive.Length, 0);
                    // Convert byte to string
                    string buffer = Encoding.ASCII.GetString(receive);

                    HandleRequest(buffer, ref socket);

                    
                }
            }
        }

        private static void HandleRequest(string request, ref Socket socket)
        {

            Console.WriteLine("Request: \n" + request);
            string response = "Test response\r\n";
            SendToClient(response, ref socket);
            //mySocket.Close();
        }

        /*private static void Initialise()
        {
            Console.WriteLine("Starting server on port: " + port + "...");
            socket = new TcpListener(IPAddress.Any, port);
            socket.Start();
            socket.BeginAcceptTcpClient(new AsyncCallback(ClientConnected), null);
            Console.WriteLine("Done!");
        }

        private static void ClientConnected(IAsyncResult result)
        {
            TcpClient client = socket.EndAcceptTcpClient(result);
            client.NoDelay = false;
            socket.BeginAcceptTcpClient(new AsyncCallback(ClientConnected), null);
            Console.WriteLine("Incoming connection from " + client.Client.RemoteEndPoint.ToString());
        }*/
    }
}
