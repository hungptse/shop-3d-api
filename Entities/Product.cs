using System;
using System.Collections.Generic;

namespace ShopAPI.Entities
{
    public partial class Product
    {
        public Product()
        {
            Feedback = new HashSet<Feedback>();
            Image = new HashSet<Image>();
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public string Description { get; set; }
        public int? CateId { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public int? Qrid { get; set; }

        public Category Cate { get; set; }
        public Qr Qr { get; set; }
        public ICollection<Feedback> Feedback { get; set; }
        public ICollection<Image> Image { get; set; }
        public ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
