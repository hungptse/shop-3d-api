using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ShopAPI.Entities;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private WebsiteShoppingContext _context = new WebsiteShoppingContext();
        [HttpPost("checkout")]
        public ActionResult CheckoutOrder([FromBody] Dictionary<string, dynamic> body)
        {
            string username = body.GetValueOrDefault("userId");
            string note = body.GetValueOrDefault("note");
            double total = body.GetValueOrDefault("total");

            Order order = new Order {Note = note, UserId = username, CreatedTime = DateTime.Now, Status = 1, Total = total };
            _context.Order.Add(order);
            _context.SaveChanges();
            JArray products = body.GetValueOrDefault("products");
            for (int i = 0; i < products.Count; i++)
            {
                JToken product = products[i];
                int id = product.Value<int>("id");
                int quantity = product.Value<int>("quantity");
                double price = product.Value<double>("price");
                OrderDetail detail = new OrderDetail { Quantity = quantity, ProId = id, OrderId = order.Id, Price = price };
                _context.OrderDetail.Add(detail);
                _context.SaveChanges();
            }
            return Ok();
        }
    }
}