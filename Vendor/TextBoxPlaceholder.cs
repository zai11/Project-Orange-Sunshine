using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectOrangeSunshine.Vendor
{
    public class TextBoxPlaceholder : TextBox
    {
        private string placeholder = "";
        private bool showPlaceholder = true;
        
        public string Placeholder
        {
            get { return placeholder; }
            set { placeholder = value; }
        }

        public bool ShowPlaceholder 
        { 
            get { return showPlaceholder; } 
            set { showPlaceholder = value; }
        }

        public TextBoxPlaceholder()
        {
            Initialized += delegate
            {
                ShowPlaceholder = true;
                Text = Placeholder;
                Foreground = Brushes.Gray;
            };
            GotKeyboardFocus += GotFocusAction;
            LostKeyboardFocus += LostFocusAction;

            Loaded += delegate
            {
                if (Text != Placeholder)
                {
                    ShowPlaceholder = false;
                    Foreground = Brushes.Black;
                }
            };
        }

        virtual public void GotFocusAction(object sender, RoutedEventArgs e)
        {
            if (Text.Equals(Placeholder))
            {
                ShowPlaceholder = false;
                Text = "";
                Foreground = Brushes.Black;
            }
        }

        virtual public void LostFocusAction(object sender, RoutedEventArgs e)
        {
            if (Text.Equals(""))
            {
                ShowPlaceholder = true;
                Text = Placeholder;
                Foreground = Brushes.Gray;
            }
        }
    }
}
