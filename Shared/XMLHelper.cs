using System;
using System.Xml.Linq;

namespace ProjectOrangeSunshine.Shared
{
    public abstract class XMLHelper
    {

        private string filename;

        public string Filename
        {
            get { return filename; }

            set { filename = value; }
        }

        public XMLHelper(string filename)
        {
            this.filename = filename;
        }

        public void Create()
        {
            XDocument doc = new (
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(filename));

            doc.Save(@"./" + filename + ".xml");
        }
    }
}
