using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOrangeSunshine.Shared
{
    public class Review
    {
        private string title;
        private string body;
        private int rating;
        private DateTime date;

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
            }
        }

        public string Body
        {
            get
            {
                return body;
            }

            set
            {
                body = value;
            }
        }

        public int Rating
        {
            get
            {
                return rating;
            }

            set
            {
                rating = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return date;
            }

            set
            {
                date = value;
            }
        }

        public Review(string title, string body, int rating, DateTime date)
        {
            this.title = title;
            this.body = body;
            this.rating = rating;
            this.date = date;
        }
    }
}
