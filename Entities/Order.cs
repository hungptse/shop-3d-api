using System;
using System.Collections.Generic;

namespace ShopAPI.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public DateTime? CreatedTime { get; set; }
        public double? Total { get; set; }
        public int? Status { get; set; }
        public string UserId { get; set; }

        public StatusOrder StatusNavigation { get; set; }
        public Account User { get; set; }
        public ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
