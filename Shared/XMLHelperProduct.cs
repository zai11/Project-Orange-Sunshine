using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ProjectOrangeSunshine.Shared
{
    public class XMLHelperProduct : XMLHelper
    {
        public XMLHelperProduct(string filename = "products") : base(filename) {}

        public Product GetProductDetails(int id)
        {
            int productId = -1;
            string productName = "";
            string productDescription = "";
            float productPrice = -1;

            XElement products = XElement.Load(@"./" + Filename + ".xml");

            foreach (XElement product in products.Elements())
            {
                bool isProduct = false;
                foreach (XElement productInfo in product.Elements())
                {
                    if (productInfo.Name == "id" && productInfo.Value == id.ToString())
                        isProduct = true;

                    if (isProduct)
                    {
                        switch (productInfo.Name.ToString())
                        {
                            case "id":
                                productId = int.Parse(productInfo.Value);
                                break;
                            case "name":
                                productName = productInfo.Value;
                                break;
                            case "description":
                                productDescription = productInfo.Value;
                                break;
                            case "price":
                                productPrice = float.Parse(productInfo.Value);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            Product p = new (productId, productName, productDescription, productPrice);
            return p;
        }

        /**
         * Finds the id of the first product that matches the provided field.
         * */
        public int FindProductID(string field, object value)
        {
            int id = -1;

            XElement products = XElement.Load(@"./" + Filename + ".xml");

            foreach (XElement product in products.Elements())
            {
                int tempID = -1;
                foreach (XElement productInfo in product.Elements())
                {
                    if (productInfo.Name == "id")
                        tempID = int.Parse(productInfo.Value);
                    if (productInfo.Name == field && productInfo.Value == value.ToString())
                        id = tempID;
                }
            }

            return id;
        }

        public List<Product> GetAllProducts()
        {
            List<Product> productsList = new();

            XElement products = XElement.Load(@"./" + Filename + ".xml");

            foreach (XElement product in products.Elements())
            {
                int id = -1;
                string name = "";
                string description = "";
                float price = -1;

                foreach (XElement info in product.Elements())
                {
                    switch (info.Name.ToString())
                    {
                        case "id":
                            id = int.Parse(info.Value);
                            break;
                        case "name":
                            name = info.Value;
                            break;
                        case "description":
                            description = info.Value;
                            break;
                        case "price":
                            price = float.Parse(info.Value);
                            break;
                        default:
                            break;
                    }
                }

                Product p = new(id, name, description, price);
                productsList.Add(p);
            }

            return productsList;
        }

        public void AddProduct(Product product)
        {
            XElement products = XElement.Load(@"./" + Filename + ".xml");

            int id = product.Id;
            string name = product.Name;
            string description = product.Description;
            float price = product.Price;

            products.Add(new XElement("product",
                new XElement("id", id),
                new XElement("name", name),
                new XElement("description", description),
                new XElement("price", price)));
            products.Save(@"./" + Filename + ".xml");
        }

        public void RemoveProduct(int id)
        {
            XElement products = XElement.Load(@"./" + Filename + ".xml");

            foreach (XElement product in products.Elements())
            {
                foreach (XElement info in product.Elements())
                {
                    if (info.Name == "id" && int.Parse(info.Value) == id)
                        product.Remove();
                }
            }

            products.Save(@"./" + Filename + ".xml");
        }

        public void UpdateProduct(int id, string field, object value)
        {
            XElement products = XElement.Load(@"./" + Filename + ".xml");

            foreach (XElement product in products.Elements())
            {
                foreach (XElement info in product.Elements())
                {
                    if (info.Name == "id" && int.Parse(info.Value) == id)
                    {
                        foreach (XElement info2 in product.Elements())
                        {
                            if (info2.Name == field)
                                info2.Value = value.ToString() ?? "";
                        }
                    }
                }
            }

            products.Save(@"./" + Filename + ".xml");
        }
    }
}
