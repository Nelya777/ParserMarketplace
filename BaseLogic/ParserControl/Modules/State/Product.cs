using ParserControl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserControl.Modules.State
{
    public enum StatusProduct
    {
        EmptyProduct = 1,
        FilledProduct = 2
    }


    public class Product : IProduct
    {
        private string _id;
        private string _photoLink;
        private string _name;
        private string _price;
        private string _link;
        private string _shopName;
        private StatusProduct _status;

        public string Id => _id;
        public string PhotoLink => _photoLink;
        public string Name => _name;
        public string Price => _price;
        public string Link => _link;
        public string ShopName => _shopName;
        public StatusProduct Status => _status;

        public Product(string photoLink = "", string name = "", string price = "", string link = "", string shopName = "")
        {
            _photoLink = photoLink;
            _name = name;
            _price = price;
            _link = link;
            _shopName = shopName;
            _id = link;
            _status = photoLink == "" && name == "" && price == "" && link == "" && shopName == "" ? StatusProduct.EmptyProduct : StatusProduct.FilledProduct;
        }
    }
}
