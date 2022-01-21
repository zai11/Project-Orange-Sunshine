using ProjectOrangeSunshine.Shared;
using System;
using System.Text;
using System.Net.Sockets;

namespace ProjectOrangeSunshine.Patron
{
    class Client
    {
        private static TcpClient socket = new();
        private static Connection? connection;

        private static readonly UnicodeEncoding byteConverter = new();

        public static Connection CreateConnection(string ip, int port = 46485)
        {
            socket = new();
            try
            {
                Logger.LogClientMessage("IP: '" + ip + "', PORT: '" + port + "'");
                socket.Connect(ip, port);
            }
            catch (Exception e)
            {
                Logger.LogClientError(100, e.ToString());
                Logger.LogClientError(300, "Error connecting to the server.");
                return new(false, null, 300, "Error connecting to the server.");
            }

            NetworkStream stream = socket.GetStream();
            stream.ReadTimeout = 2000;

            connection = new(true, stream);

            return connection;
        }

        public static void SendRequest(string request)
        {
            if (connection == null || connection.Stream == null)
                throw new Exception("The network stream is null. This shouldn't be the case.");
            byte[] bytes = byteConverter.GetBytes(request + "\r\n");
            connection.Stream.Write(bytes, 0, bytes.Length);
        }

        public static string GetResponse()
        {
            byte[] lengthBuffer = new byte[6 * sizeof(char)];
            int bytesRead = connection != null && connection.Stream != null ? connection.Stream.Read(lengthBuffer, 0, lengthBuffer.Length) : -1;
            string lengthStr = byteConverter.GetString(lengthBuffer, 0, bytesRead);
            int responseLength = int.Parse(lengthStr);

            byte[] buffer = new byte[responseLength];
            bytesRead = connection != null && connection.Stream != null ? connection.Stream.Read(buffer, 0, buffer.Length) : -1;
            string response = byteConverter.GetString(buffer, 0, bytesRead);

            if (response[^2] != '\r' || response[^1] != '\n')
                throw new Exception("Response had an invalid terminator suffix.");

            return response;
        }

        public static void CloseConnection()
        {
            if (connection != null && connection.Stream != null)
            {
                connection.Stream.Close();
            }
            else
            {
                Logger.LogClientWarning("There is no connection open currently.");
            }
        }
    }
}
