using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjectOrangeSunshine.Vendor
{
    public class ErrorPanel : Label
    {

        private static readonly string IMAGE_SRC = @"assets/img/test.png";

        private string error = "";

        public string Error 
        { 
            get { return error; } 
            set { error = value; } 
        }

        public ErrorPanel()
        {
            Loaded += BuildControl;
        }

        public void BuildControl(object sender, RoutedEventArgs e)
        {
            Content = Error;
            BitmapImage img = new(new Uri(IMAGE_SRC, UriKind.Relative));
            ImageBrush brush = new(img);
            Background = brush;
            Width = 250;
            HorizontalContentAlignment = HorizontalAlignment.Center;
            FontWeight = FontWeights.Normal;
        }
    }
}
