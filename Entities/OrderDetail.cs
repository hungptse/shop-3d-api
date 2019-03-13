using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ShopAPI.Entities
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? ProId { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }

        public Order Order { get; set; }
        public Product Pro { get; set; }
    }
}
