using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectOrangeSunshine.Vendor
{
    public class TextBoxInteger : TextBoxPlaceholder
    {
        public TextBoxInteger() : base()
        {
            TextChanged += TextChangedAction;
            GotKeyboardFocus += GotFocusAction;
            LostKeyboardFocus += LostFocusAction;
        }

        public void TextChangedAction(object sender, RoutedEventArgs e)
        {
            if (Text.Length > 0 && !Regex.IsMatch(Text[^1].ToString(), @"[0-9]") && !ShowPlaceholder)
            {
                int prevCaret = SelectionStart > 1 ? SelectionStart : 1;
                Text = Text[0..^1];
                SelectionStart = prevCaret - 1;
            }
        }
    }
}
