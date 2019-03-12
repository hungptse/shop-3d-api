
using Microsoft.AspNetCore.SignalR;
using ShopAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopAPI.Hubs
{
    public class ProductHub : Hub<IProductClient>
    {
        public ProductHub()
        {
        }

        public async Task Add(Product p) => await Clients.All.Add(p);
    }
}
