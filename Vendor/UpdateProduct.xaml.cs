using ProjectOrangeSunshine.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectOrangeSunshine.Vendor
{
    /// <summary>
    /// Interaction logic for UpdateProduct.xaml
    /// </summary>
    public partial class UpdateProduct : Window
    {

        private int productID = -1;
        private string productName = "";
        private string productDescription = "";
        private float productPrice = -1;

        public int ProductID
        {
            get { return productID; }
            set { productID = value; }
        }

        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        public string ProductDescription
        {
            get { return productDescription; }
            set { productDescription = value; }
        }

        public float ProductPrice
        {
            get { return productPrice; }
            set { productPrice = value; }
        }

        public UpdateProduct()
        {
            InitializeComponent();
            Loaded += delegate
            {
                NameTextBox.Text = ProductName;
                DescriptionTextBox.Text = ProductDescription;
                PriceTextBox.Text = "$" + ProductPrice.ToString();
            };
        }

        public void UpdateProductButtonClick(object sender, RoutedEventArgs e)
        {
            XMLHelperProduct xmlProduct = new();
            xmlProduct.RemoveProduct(ProductID);

            string name = NameTextBox.Text;
            string description = DescriptionTextBox.Text;
            float price = float.Parse(PriceTextBox.Text[1..]);

            xmlProduct.AddProduct(new(ProductID, name, description, price));
            Close();
        }

        public void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
