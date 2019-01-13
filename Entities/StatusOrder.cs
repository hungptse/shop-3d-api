using System;
using System.Collections.Generic;

namespace ShopAPI.Entities
{
    public partial class StatusOrder
    {
        public StatusOrder()
        {
            Order = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Status { get; set; }

        public ICollection<Order> Order { get; set; }
    }
}
