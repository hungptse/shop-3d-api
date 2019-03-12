using ShopAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopAPI.Hubs
{
    public interface IProductClient
    {
        Task Add(Product p);
    }
}
