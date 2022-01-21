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
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {

        private bool submitted = false;
        private bool hasError = false;
        private string errorMessage = "";

        public bool Submitted
        {
            get { return submitted; }
            set { submitted = value; }
        }

        public bool HasError
        {
            get { return hasError; }
            set { hasError = value; }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        public AddProduct()
        {
            InitializeComponent();
        }

        public void AddProductButtonClick(object sender, RoutedEventArgs e)
        {
            Submitted = true;
            if (NameTextBox.ShowPlaceholder)
            {
                HasError = true;
                ErrorMessage = "ERROR: Name field must be set.";
            }
            else if (DescriptionTextBox.ShowPlaceholder)
            {
                HasError = true;
                ErrorMessage = "ERROR: Description field must be set.";
            }
            else if (PriceTextBox.ShowPlaceholder)
            {
                HasError = true;
                ErrorMessage = "ERROR: Price field must be set.";
            }

            if (!HasError)
            {
                XMLHelperProduct productXML = new();
                List<Product> products = productXML.GetAllProducts();
                int id = products[^1].Id + 1;
                string name = NameTextBox.Text;
                string description = DescriptionTextBox.Text;
                float price = float.Parse(PriceTextBox.Text[1..]);

                Product product = new(id, name, description, price);

                productXML.AddProduct(product);
            }

            Close();
        }

        public void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
