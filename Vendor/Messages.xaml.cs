using ProjectOrangeSunshine.Shared;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ProjectOrangeSunshine.Vendor
{
    /// <summary>
    /// Interaction logic for Messages.xaml
    /// </summary>
    public partial class Messages : Window
    {

        private Message[] messageList = Array.Empty<Message>();

        public Message[] MessageList
        {
            get { return messageList; }
            set
            {
                messageList = value;
                RefreshPage();
            }
        }

        public Messages()
        {
            InitializeComponent();
            LoadMessages();
        }

        private void LoadMessages()
        {
            XMLHelperMessage xmlMessage = new();
            List<Message> messages = xmlMessage.GetAllMessages();
            MessageList = messages.ToArray();
        }

        private void RefreshPage()
        {
            ChatStackPanel.Children.Clear();
        }
    }
}
