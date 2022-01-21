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
    public class TextBoxPrice : TextBoxPlaceholder
    {
        public TextBoxPrice() : base()
        {
            TextChanged += TextChangedAction;
            GotKeyboardFocus += GotFocusAction;
            LostKeyboardFocus += LostFocusAction;
        }

        public void TextChangedAction(object sender, RoutedEventArgs e)
        {
            if (Text.Length <= 0)
            {
                Text = "$";
                SelectionStart = 2;
            }

            int pointCount = 0;
            int dollarCount = 0;

            foreach (char character in Text)
            {
                if (character.Equals('.'))
                    pointCount++;
                if (character.Equals('$'))
                    dollarCount++;
            }

            if (pointCount >= 2 && Text[^1].Equals('.'))
            {
                int prevCaret = SelectionStart;
                Text = Text[0..^1];
                SelectionStart = prevCaret - 1;
            }

            if (dollarCount >= 2 && Text[^1].Equals('$'))
            {
                int prevCaret = SelectionStart;
                Text = Text[0..^1];
                SelectionStart = prevCaret - 1;
            }

            if (!Regex.IsMatch(Text[^1].ToString(), @"[0-9]|\.|\$") && !ShowPlaceholder)
            {
                int prevCaret = SelectionStart > 1 ? SelectionStart : 1;
                Text = Text[0..^1];
                SelectionStart = prevCaret - 1;
            }

            if (Text[0] != '$' && !ShowPlaceholder)
            {
                char num = Text[0];
                string temp = Text[1..] + num;
                Text = temp;
                SelectionStart = Text.Length;
            }
        }

        public override void GotFocusAction(object sender, RoutedEventArgs e)
        {
            if (Text.Equals(Placeholder))
            {
                ShowPlaceholder = false;
                Text = "$";
                Foreground = Brushes.Black;
                SelectionStart = Text.Length;
            }
        }

        public override void LostFocusAction(object sender, RoutedEventArgs e)
        {
            if (Text.Equals("$"))
            {
                ShowPlaceholder = true;
                Text = Placeholder;
                Foreground = Brushes.Gray;
            }
        }
    }
}
