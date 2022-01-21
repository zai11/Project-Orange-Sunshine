using System.Collections.Generic;

namespace ProjectOrangeSunshine.Shared
{
    public class Product
    {

        public static readonly Product NULL = new(-1, "", "", -1);

        private int id;
        private string name;
        private string description;
        private float price;
        private readonly List<Review> reviews;

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }

        public float Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
            }
        }

        public Product(int id, string name, string description, float price)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.price = price;
            reviews = new();
        }

        public void AddReview(Review review)
        {
            reviews.Add(review);
        }

        public override string ToString()
        {
            return "{ id: " + Id + ", name: " + Name + ", description: " + Description + ", price: " + Price + " }";
        }
    }
}
