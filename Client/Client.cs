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

            string request = "Test Request";
            byte[] bytes = Encoding.UTF8.GetBytes(request);
            stream.Write(bytes, 0, bytes.Length);

            try
            {
                string response = GetResponse(ref stream);
                Console.WriteLine(response);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }

        private static string GetResponse(ref NetworkStream stream)
        {
            byte[] buffer = new byte[4096];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            if (response[response.Length - 2] != '\r' || response[response.Length - 1] != '\n')
                throw new Exception("Response had an invalid terminator suffix.");

            return response;
        }
    }
}
