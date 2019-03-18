using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ShopAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private WebsiteShoppingContext _context = new WebsiteShoppingContext();

        [HttpGet]
        public IEnumerable<Order> GetOrders()
        {
            return _context.Order.Include(o => o.User).Include(o => o.OrderDetail).ThenInclude(d => d.Pro);
        }

        [HttpPost("checkout")]
        public ActionResult CheckoutOrder([FromBody] Dictionary<string, dynamic> body)
        {
            string username = body.GetValueOrDefault("userId");
            string note = body.GetValueOrDefault("note");
            double total = body.GetValueOrDefault("total");
            if (note.Equals(""))
            {
                note = "None";
            }
            Order order = new Order { Note = note, UserId = username, CreatedTime = DateTime.Now, Status = 1, Total = total };
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

        [HttpPut("change/{id}/status/{status}")]
        public ActionResult ChangeStatus([FromRoute] int id,[FromRoute] int status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Order order = _context.Order.SingleOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            order.Status = status;
            _context.SaveChanges();

            return NoContent();
        }
    }
}