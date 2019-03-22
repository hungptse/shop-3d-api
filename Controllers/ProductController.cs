using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ShopAPI.Entities;
using ShopAPI.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private WebsiteShoppingContext _context = new WebsiteShoppingContext();
        private IHubContext<ProductHub> hub;
        public ProductController(IHubContext<ProductHub> hub)
        {
            this.hub = hub;
        }


        // GET: api/Product
        [HttpGet]
        public IEnumerable<Product> GetProduct()
        {
            return _context.Product.Include(p => p.Cate);
        }


        // GET: api/Product/5
        [HttpGet("{id}")]
        public IActionResult GetProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = _context.Product.Include(f => f.Feedback).ThenInclude(a => a.Acc).Include(p => p.Image).SingleOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("rate/{id}")]
        public IActionResult GetProductRate([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = _context.Product.SingleOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            List<Feedback> feedbacks = _context.Feedback.Where(f => f.ProId == id && f.isApprove == true).ToList();
            double? sum = 0;
            foreach (var feedback in feedbacks)
            {
                sum += feedback.Rate;
            }
            if (feedbacks.Count == 0)
            {
                return Ok(5);
            }
            double? rating = sum / feedbacks.Count;
            return Ok(rating);
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Dictionary<string, string> body)
        {

            var name = body.GetValueOrDefault("name");
            var model = body.GetValueOrDefault("model");
            var cate = body.GetValueOrDefault("cate");
            var description = body.GetValueOrDefault("description");
            var height = body.GetValueOrDefault("height");
            var weight = body.GetValueOrDefault("weight");
            var price = body.GetValueOrDefault("price");
            var quantity = body.GetValueOrDefault("quantity");
            var imgThumb = body.GetValueOrDefault("imgThumb");
            var p = new Product { Name = name, Model = model, CateId = int.Parse(cate), Description = description, Height = double.Parse(height), Weight = double.Parse(weight), Price = double.Parse(price), Quantity = int.Parse(quantity), Thumbnail = imgThumb };
            _context.Product.Add(p);
            _context.SaveChanges();
            await hub.Clients.All.SendAsync("Add", _context.Product.Include(pro => pro.Image).Include(pro => pro.Cate).SingleOrDefault(pro => pro.Id == p.Id));
            return Ok();
        }


        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}