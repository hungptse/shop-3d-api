using System;
using System.Collections.Generic;

namespace ShopAPI.Entities
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public int? Rate { get; set; }
        public string Comment { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? ProId { get; set; }

        public Product Pro { get; set; }
    }
}
