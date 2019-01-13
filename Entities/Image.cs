using System;
using System.Collections.Generic;

namespace ShopAPI.Entities
{
    public partial class Image
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int? ProId { get; set; }

        public Product Pro { get; set; }
    }
}
