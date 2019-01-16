﻿using System;
using System.Collections.Generic;

namespace ShopAPI.Entities
{
    public partial class Category
    {
        public Category()
        {
            Product = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Product> Product { get; set; }
    }
}
