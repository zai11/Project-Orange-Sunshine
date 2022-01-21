using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ProjectOrangeSunshine.Shared
{
    public class XMLHelperMessage : XMLHelper
    {
        public XMLHelperMessage(string filename = "messages") : base(filename) {}

        public Message GetMessageDetails(int id)
        {
            int messageId = -1;
            string messageToIP = "";
            string messageFromIP = "";
            string messageTitle = "";
            string messageBody = "";
            string messageTimestamp = "";

            XElement messages = XElement.Load(@"./" + Filename + ".xml");

            foreach (XElement message in messages.Elements())
            {
                bool isProduct = false;
                foreach (XElement messageInfo in message.Elements())
                {
                    if (messageInfo.Name == "id" && messageInfo.Value == id.ToString())
                        isProduct = true;

                    if (isProduct)
                    {
                        switch (messageInfo.Name.ToString())
                        {
                            case "id":
                                messageId = int.Parse(messageInfo.Value);
                                break;
                            case "toIP":
                                messageToIP = messageInfo.Value;
                                break;
                            case "fromIP":
                                messageFromIP = messageInfo.Value;
                                break;
                            case "title":
                                messageTitle = messageInfo.Value;
                                break;
                            case "body":
                                messageBody = messageInfo.Value;
                                break;
                            case "timestamp":
                                messageTimestamp = messageInfo.Value;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            Message m = new(messageId, messageToIP, messageFromIP, messageTitle, messageBody, messageTimestamp);
            return m;
        }

        public int FindMessageID(string field, object value)
        {
            int id = -1;

            XElement messages = XElement.Load(@"./" + Filename + ".xml");

            foreach (XElement message in messages.Elements())
            {
                int tempID = -1;
                foreach (XElement messageInfo in message.Elements())
                {
                    if (messageInfo.Name == "id")
                        tempID = int.Parse(messageInfo.Value);
                    if (messageInfo.Name == field && messageInfo.Value == value.ToString())
                        id = tempID;
                }
            }

            return id;
        }

        public List<Message> GetAllMessages()
        {
            List<Message> messageList = new();

            XElement messages = XElement.Load(@"./" + Filename + ".xml");

            foreach (XElement message in messages.Elements())
            {
                int id = -1;
                string toIP = "";
                string fromIP = "";
                string title = "";
                string body = "";
                string timestamp = "";

                foreach (XElement info in message.Elements())
                {
                    switch (info.Name.ToString())
                    {
                        case "id":
                            id = int.Parse(info.Value);
                            break;
                        case "toIP":
                            toIP = info.Value;
                            break;
                        case "fromIP":
                            fromIP = info.Value;
                            break;
                        case "title":
                            title = info.Value;
                            break;
                        case "body":
                            body = info.Value;
                            break;
                        case "timestamp":
                            timestamp = info.Value;
                            break;
                        default:
                            break;
                    }
                }

                Message m = new(id, toIP, fromIP, title, body, timestamp);
                messageList.Add(m);
            }

            return messageList;
        }

        public void AddMessage(Message message)
        {
            XElement messages = XElement.Load(@"./" + Filename + ".xml");

            int id = message.Id;
            string toIP = message.ToIP;
            string fromIP = message.FromIP;
            string title = message.Title;
            string body = message.Body;
            string timestamp = message.Timestamp;

            messages.Add(new XElement("message",
                new XElement("id", id),
                new XElement("toIP", toIP),
                new XElement("fromIP", fromIP),
                new XElement("title", title),
                new XElement("body", body),
                new XElement("timestamp", timestamp)));
            messages.Save(@"./" + Filename + ".xml");
        }

        public void RemoveMessage(int id)
        {
            XElement messages = XElement.Load(@"./" + Filename + ".xml");

            foreach (XElement message in messages.Elements())
            {
                foreach (XElement info in message.Elements())
                {
                    if (info.Name == "id" && int.Parse(info.Value) == id)
                        message.Remove();
                }
            }

            messages.Save(@"./" + Filename + ".xml");
        }

        public void UpdateMessage(int id, string field, object value)
        {
            XElement messages = XElement.Load(@"./" + Filename + ".xml");

            foreach (XElement message in messages.Elements())
            {
                foreach (XElement info in message.Elements())
                {
                    if (info.Name == "id" && int.Parse(info.Value) == id)
                    {
                        foreach (XElement info2 in message.Elements())
                        {
                            if (info2.Name == field)
                                info2.Value = value.ToString() ?? "";
                        }
                    }
                }
            }

            messages.Save(@"./" + Filename + ".xml");
        }
    }
}
