using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOrangeSunshine.Shared
{
    public class Message {

        private int id;
        private string toIP;
        private string fromIP;
        private string title;
        private string body;
        private string timestamp;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string ToIP
        {
            get { return toIP; }
            set { toIP = value; }
        }

        public string FromIP
        {
            get { return fromIP; }
            set { fromIP = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        public string Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        public Message(int id, string toIP, string fromIP, string title, string body, string timestamp)
        {
            this.id = id;
            this.toIP = toIP;
            this.fromIP = fromIP;
            this.title = title;
            this.body = body;
            this.timestamp = timestamp;
        }
    }
}
