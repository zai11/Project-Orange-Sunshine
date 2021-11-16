using System;
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
                socketListener = new TcpListener(port);
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
                Console.WriteLine("Socket Type" + socket.SocketType);
                if (socket.Connected)
                {
                    Console.WriteLine("\nClient Connected!!\n==================\nClient IP {0}\n", socket.RemoteEndPoint);
                    // Make a byte array and receive data from the client
                    byte[] bReceive = new byte[1024];
                    int i = socket.Receive(bReceive, bReceive.Length, 0);
                    // Convert byte to string
                    string buffer = Encoding.ASCII.GetString(bReceive);

                    HandleRequest(buffer, ref socket);

                    
                }
            }
        }

        private static void SendHeader(string sHttpVersion, string sMIMEHeader, int iTotBytes, string sStatusCode, ref Socket mySocket)
        {
            string sBuffer = "";
            // If Mime type is not provided set default to text/html
            if (sMIMEHeader.Length == 0)
                sMIMEHeader = "text/html";

            // Create HTML Header using: https://en.wikipedia.org/wiki/List_of_HTTP_header_fields
            sBuffer += sHttpVersion + sStatusCode + "\r\n";
            sBuffer += "Server: cx1193719-b\r\n";
            sBuffer += "Content-Type: " + sMIMEHeader + "\r\n";
            sBuffer += "Accept-Ranges: bytes\r\n";
            sBuffer += "Content-Length: " + iTotBytes + "\r\n\r\n";
            byte[] bSendData = Encoding.ASCII.GetBytes(sBuffer);
            SendToClient(bSendData, ref mySocket);
            Console.WriteLine("Total Bytes: " + iTotBytes.ToString());
        }

        private static void HandleRequest(string request, ref Socket socket)
        {

            Console.WriteLine("Request: \n" + request);
            string response = "Test response";
            SendHeader("HTTP/1.1", "", response.Length, " Connection Test", ref socket);
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
