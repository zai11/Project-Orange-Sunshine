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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectOrangeSunshine.Vendor
{
    /// <summary>
    /// Interaction logic for ChatButton.xaml
    /// </summary>
    public partial class ChatButton : UserControl
    {

        private string displayName = "";

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TabItem));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        public string DisplayName
        {
            get { return displayName; }
            set 
            { 
                displayName = value;
                DisplayNameLabel.Content = value;
            }
        }

        public ChatButton()
        {
            InitializeComponent();
            PreviewMouseLeftButtonUp += (sender, args) => OnClick();
        }

        void RaiseClickEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(ChatButton.ClickEvent);
            RaiseEvent(newEventArgs);
        }

        void OnClick()
        {
            RaiseClickEvent();
        }
    }
}
