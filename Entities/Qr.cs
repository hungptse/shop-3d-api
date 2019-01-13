using System;
using System.Collections.Generic;

namespace ShopAPI.Entities
{
    public partial class Qr
    {
        public Qr()
        {
            Product = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string LinkQr { get; set; }
        public string LinkAr { get; set; }

        public ICollection<Product> Product { get; set; }
    }
}
