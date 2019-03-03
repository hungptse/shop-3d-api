using System.Collections.Generic;

namespace ShopAPI.Entities
{
    public class ProductInCart
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Thumbnail { get; set; }
    }


    public class CartObj
    {
        public string accId { get; set; }
        public List<ProductInCart> Cart { get; set; }


        public CartObj(string id)
        {
            accId = id;
            Cart = new List<ProductInCart>();
        }
    }
}
