using ProjectOrangeSunshine.Shared;
using System;
using System.Text;
using System.Net.Sockets;

namespace ProjectOrangeSunshine.Vendor
{
    class Client
    {
        private static readonly TcpClient socket = new();
        private static readonly UnicodeEncoding byteConverter = new();

        private const string IP = "192.168.1.9";
        private const int PORT = 46495;

        public static void Initialise()
        {
            try
            {
                socket.Connect(IP, PORT);
            }
            catch (Exception)
            {
                Logger.LogClientError(300, "Error connecting to the server.");
                return;
            }

            NetworkStream stream = socket.GetStream();
            stream.ReadTimeout = 2000;

            string request = "GET:PUBLIC_KEY\r";
            byte[] bytes = byteConverter.GetBytes(request);
            stream.Write(bytes, 0, bytes.Length);

            try
            {
                string response = GetResponse(ref stream);
                Logger.LogClientMessage(response);
            }
            catch (Exception e)
            {
                Logger.LogClientError(100, e.ToString());
            }

            Console.ReadLine();
        }

        private static string GetResponse(ref NetworkStream stream)
        {
            byte[] buffer = new byte[4096];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string response = byteConverter.GetString(buffer, 0, bytesRead);

            if (response[^2] != '\r' || response[^1] != '\n')
                throw new Exception("Response had an invalid terminator suffix.");

            return response;
        }
    }
}
