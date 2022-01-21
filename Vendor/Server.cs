using ProjectOrangeSunshine.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ProjectOrangeSunshine.Vendor
{
    class Server
    {

        private static readonly int port = 46485;
        private static readonly TcpListener socketListener = new(IPAddress.Any, port);

        private static readonly List<Product> productList = new();
        private static readonly UnicodeEncoding byteConverter = new();

        public static void Initialise()
        {
            if (!File.Exists(@"./server.config"))
                FirstSetup();

            LoadProducts();

            try
            {
                socketListener.Start();
                Logger.LogServerMessage("Internal Server Running...");
                // Start the thread which calls the method 'StartListen'
                Thread th = new(new ThreadStart(StartListen));
                th.Start();
            }
            catch (Exception e)
            {
                Logger.LogServerError(100, "An exception has occurred while listening: " + e.ToString());
            }
        }

        private static void SendToClient(string data, ref Socket socket)
        {
            Logger.LogServerMessage("Sending Data: " + data);
            SendToClient(byteConverter.GetBytes(data), ref socket);
        }

        private static void SendToClient(byte[] data, ref Socket socket)
        {
            try
            {
                if (socket.Connected)
                {
                    int numBytes;
                    if ((numBytes = socket.Send(data, data.Length, 0)) == -1)
                        Logger.LogServerError(200, "Socket Error: cannot send packet");
                    else
                        Logger.LogServerMessage("No. of bytes sent: " + numBytes);
                }
                else
                {
                    Logger.LogClientWarning("Connection Dropped...");
                }
            }
            catch (Exception e)
            {
                Logger.LogServerError(100, "An exception has occurred: " + e.ToString());
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
                    Logger.LogServerMessage("Client Connected: " + socket.RemoteEndPoint);
                    // Make a byte array and receive data from the client
                    byte[] receive = new byte[1024];
                    _ = socket.Receive(receive, receive.Length, 0);
                    int byteCount = 0;
                    foreach(byte b in receive)
                    {
                        if (b == '\r')
                            break;
                        byteCount++;
                    }

                    byte[] receiveTrimmed = receive[0..byteCount];

                    // Convert byte to string
                    string buffer = byteConverter.GetString(receiveTrimmed);

                    HandleRequest(buffer, ref socket);
                }
            }
        }

        private static void HandleRequest(string request, ref Socket socket)
        {
            Logger.LogServerMessage("Request: '" + request + "'");
            string[] requests = request.Split(';');
            foreach(string r in requests)
            {

                Logger.LogServerMessage("Request " + r);

                string[] parts = r.Trim().Split(':');

                if (parts[0].Trim() == "PUBLIC_KEY")
                {
                    Logger.LogServerMessage("Importing public key");
                    RSAHelper.ImportPublicKey(parts[1].Trim());
                }

                if (parts[0].Trim() == "GET" && parts[1].Trim() == "PUBLIC_KEY")
                    SendPublicKey(ref socket);

                if (parts[0].Trim() == "GET" && parts[1].Trim() == "PRODUCT_LIST")
                    SendProductList(ref socket);
            }
        }

        private static void SendPublicKey(ref Socket socket)
        {
            string publicKey = RSAHelper.GetPublicKey();
            SendToClient(publicKey + "\r\n", ref socket);
        }

        private static void SendProductList(ref Socket socket)
        {
            string encryptedProducts = EncryptProductList(productList);

            string strToSend = encryptedProducts + "\r\n";

            int len = byteConverter.GetBytes(strToSend).Length;
            string lenStr = len.ToString("D6");
            strToSend = lenStr + strToSend;

            SendToClient(strToSend, ref socket);
        }

        private static string EncryptProductList(List<Product> products)
        {
            string key = AESHelper.GenerateKey();
            string p = "";
            foreach (Product product in products)
            {
                p += product.Id + ", " + product.Name + ", " + product.Description + ", " + product.Price + " \r ";
            }
            p += "}";
            byte[] encryptedProducts = AESHelper.Encrypt(p, key);
            string encryptedProductsString = Utilities.ArrayToString(encryptedProducts);
            string str = Utilities.ArrayToString(RSAHelper.Encrypt(key)) + " : " + encryptedProductsString + "\r\n";
            return str;
        }

        private static void FirstSetup()
        {
            // Set up config file
            using (StreamWriter writer = new(@"./server.config"))
            {
                writer.WriteLine("useTOR:false");
                writer.WriteLine("useRSAEncryption:true");
            }

            // Set up RSA files
            RSAHelper.ExportPublicKey("public_key.rsa");
            RSAHelper.ExportPrivateKey("private_key.rsa");

            // Set up products database
            XMLHelperProduct xmlProduct = new();
            xmlProduct.Create();

            // Set up messages database
            XMLHelperMessage xmlMessage = new();
            xmlMessage.Create();
        }
        public static void LoadProducts()
        {
            XMLHelperProduct xmlProduct = new();
            List<Product> products = xmlProduct.GetAllProducts();
            products.ForEach((product) =>
            {
                productList.Add(product);
            });
        }
    }
}
