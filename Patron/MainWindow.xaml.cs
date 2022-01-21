using ProjectOrangeSunshine.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectOrangeSunshine.Patron
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly BindingList<Product> productList = new();

        private readonly List<Product> cart = new();

        public MainWindow()
        {


            Closed += (s, e) => Logger.WriteLog();
            Closed += (s, e) => Environment.Exit(0);

            InitializeComponent();
            
            Thread server = new(new ThreadStart(Server.Initialise));
            server.Start();


            ProductsGrid.ItemsSource = productList;

        }

        private void ConnectButtonClick(object sender, RoutedEventArgs e)
        {
            productList.Clear();
            Client.CloseConnection();
            string ip = ConnectionTB.Text;
            if (!ValidateIp(ip))
            {
                Logger.LogClientError("Invalid IP address '" + ip + "'");
                StatusLabel.Content = "ERROR: Invalid IP address.";
                return;
            }
            Connection connection = Client.CreateConnection(ip, 46485);
            if (!connection.Success)
            {
                Logger.LogClientError("Unable to connect to given ip.");
                StatusLabel.Content = "ERROR: Unable to connect to vendor.";
                return;
            }
            if (connection.Stream != null)
            {
                StatusLabel.Content = "Connecting to " + connection.Stream.Socket.RemoteEndPoint + "...";
            }
            Client.SendRequest("PUBLIC_KEY : " + RSAHelper.GetPublicKey() + "; GET : PRODUCT_LIST");
            try
            {
                RSAHelper.ImportPrivateKeyFromFile(@"./private_key.rsa");

                string response = Client.GetResponse();
                string productData = DecryptProductData(response);

                while (response[^3].Equals('\v'))
                {
                    Logger.LogClientMessage("Detected");
                    response = Client.GetResponse();
                    productData = productData[0..^3];
                    Logger.LogClientMessage(response);
                    productData += "\r " + DecryptProductData(response);
                }

                Logger.LogClientMessage("productData: " + productData);

                string[] products = productData.Split('\r')[0..^1];

                foreach(string product in products)
                {
                    Logger.LogClientMessage("DEBUG: " + product);
                    string[] productInfo = product.Split(',');

                    int id = int.Parse(productInfo[0].Trim());
                    string name = productInfo[1].Trim();
                    string description = productInfo[2].Trim();
                    float price = float.Parse(productInfo[3].Trim());

                    Product p = new(id, name, description, price);

                    productList.Add(p);
                }
                
                StatusLabel.Content = "Done.";
            }
            catch (Exception ex)
            {
                Logger.LogClientError(100, ex.ToString());
            }
        }

        private void AddToCartButtonClick(object sender, RoutedEventArgs e)
        {
            // TODO
            
        }

        private void ViewProfileButtonClick(object sender, RoutedEventArgs e)
        {
            // TODO
        }

        private void ViewCartButtonClick(object sender, RoutedEventArgs e)
        {
            // TODO
        }

        private void ChangeSelectedProduct(object sender, RoutedEventArgs e)
        {
            AddToCartButton.IsEnabled = true;
            ProductReviewsButton.IsEnabled = true;
        }

        private void ViewMessageButtonClick(object sender, RoutedEventArgs e)
        {
            // TODO
        }

        private void ProductReviewsButtonClick(object sender, RoutedEventArgs e)
        {
            // TODO
        }

        private bool ValidateIp(string ip)
        {
            bool valid = true;
            if (ip.Length == 0)
                valid = false;
            string[] parts = ip.Split('.');
            if (parts.Length != 4)
                valid = false;
            Array.ForEach(parts, (part) =>
            {
                try
                {
                    int.Parse(part);
                }
                catch (Exception)
                {
                    valid = false;
                }
            });
            return valid;
        }

        private string DecryptProductData(string response)
        {
            string[] parts = response.Split(':');
            string encryptedKey = parts[0].Trim();
            string key = RSAHelper.Decrypt(Utilities.StringToByteArray(encryptedKey));

            string encryptedData = parts[1].Trim();
            return AESHelper.Decrypt(Utilities.StringToByteArray(encryptedData), key);
        }
    }
}
