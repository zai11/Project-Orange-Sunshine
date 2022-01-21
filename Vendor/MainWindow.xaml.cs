using ProjectOrangeSunshine.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectOrangeSunshine.Vendor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly BindingList<Product> productList = new();
        private readonly XMLHelperProduct xmlProduct = new();

        private Product selectedProduct = Product.NULL;

        public MainWindow()
        {
            Closed += (s, e) => Logger.WriteLog();
            Closed += (s, e) => Environment.Exit(0);

            InitializeComponent();

            Thread server = new(new ThreadStart(Server.Initialise));

            server.Start();
            LoadProducts();

            ProductsGrid.ItemsSource = productList;
        }

        private void AddProductButtonClick(object sender, RoutedEventArgs e)
        {
            AddProduct addProductWindow = new();
            addProductWindow.Closed += delegate
            {
                AddProductErrorDelegate(ref addProductWindow);
            };
            addProductWindow.Show();
        }

        private void ViewMessagesButtonClick(object sender, RoutedEventArgs e)
        {
            Messages messagesWindow = new();
            messagesWindow.Show();
        }

        private void ViewProfileButtonClick(object sender, RoutedEventArgs e)
        {
            // TODO
        }

        private void ChangeSelectedProduct(object sender, RoutedEventArgs e)
        {
            RemoveProductButton.IsEnabled = true;
            UpdateProductButton.IsEnabled = true;
            //ProductReviewsButton.IsEnabled = true;
            selectedProduct = (Product) ProductsGrid.SelectedItem;
        }

        private void ProductReviewsButtonClick(object sender, RoutedEventArgs e)
        {
            // TODO
        }

        private void RemoveProductButtonClick(object sender, RoutedEventArgs e)
        {
            xmlProduct.RemoveProduct(selectedProduct.Id);
            LoadProducts();
            Server.LoadProducts();
        }

        private void UpdateProductButtonClick(object sender, RoutedEventArgs e)
        {
            // TODO
            UpdateProduct updateProductWindow = new();
            updateProductWindow.ProductID = selectedProduct.Id;
            updateProductWindow.ProductName = selectedProduct.Name;
            updateProductWindow.ProductDescription = selectedProduct.Description;
            updateProductWindow.ProductPrice = selectedProduct.Price;

            updateProductWindow.Closed += delegate
            {
                LoadProducts();
                Server.LoadProducts();
            };

            updateProductWindow.Show();
        }

        private void LoadProducts()
        {
            productList.Clear();
            List<Product> products = xmlProduct.GetAllProducts();
            products.ForEach((product) =>
            {
                productList.Add(product);
            });
        }

        private void AddProductErrorDelegate(ref AddProduct previousWindow)
        {
            if (previousWindow.HasError)
            {
                AddProductError addProductErrorWindow = new();
                addProductErrorWindow.HasError = previousWindow.HasError;
                addProductErrorWindow.ErrorMessage = previousWindow.ErrorMessage;
                addProductErrorWindow.NameTextBox.Text = previousWindow.NameTextBox.Text;
                addProductErrorWindow.DescriptionTextBox.Text = previousWindow.DescriptionTextBox.Text;
                addProductErrorWindow.PriceTextBox.Text = previousWindow.PriceTextBox.Text;
                addProductErrorWindow.Closed += delegate
                {
                    AddProductErrorDelegate(ref addProductErrorWindow);
                };
                addProductErrorWindow.Show();
            }
            else
            {
                LoadProducts();
                Server.LoadProducts();
            }
        }

        private void AddProductErrorDelegate(ref AddProductError previousWindow)
        {
            if (previousWindow.HasError && previousWindow.Submitted)
            {
                AddProductError addProductErrorWindow = new();
                addProductErrorWindow.HasError = previousWindow.HasError;
                addProductErrorWindow.ErrorMessage = previousWindow.ErrorMessage;
                addProductErrorWindow.NameTextBox.Text = previousWindow.NameTextBox.Text;
                addProductErrorWindow.DescriptionTextBox.Text = previousWindow.DescriptionTextBox.Text;
                addProductErrorWindow.PriceTextBox.Text = previousWindow.PriceTextBox.Text;
                addProductErrorWindow.Closed += delegate
                {
                    AddProductErrorDelegate(ref addProductErrorWindow);
                };
                addProductErrorWindow.Show();
            }
            else
            {
                LoadProducts();
            }
        }
    }
}
