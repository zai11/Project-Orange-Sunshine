using System;
using System.IO;
using System.Text;
using System.Net.Sockets;

namespace Client
{
    class Client
    {
        private static readonly TcpClient socket = new TcpClient();

        private const string IP = "192.168.1.9";
        private const int PORT = 46495;

        static void Main(string[] args)
        {
            try
            {
                socket.Connect(IP, PORT);
            }
            catch (Exception)
            {
                Console.WriteLine("Error connecting to the server.");
                return;
            }

            NetworkStream stream = socket.GetStream();
            stream.ReadTimeout = 2000;

            //StreamWriter writer = new StreamWriter(stream);

            string request = "Test Request";

            StreamReader reader = new StreamReader(stream, Encoding.UTF8);

            byte[] bytes = Encoding.UTF8.GetBytes(request);
            stream.Write(bytes, 0, bytes.Length);

            string response = reader.ReadToEnd();

            Console.WriteLine(response);
        }

    }
}
