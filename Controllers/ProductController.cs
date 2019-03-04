using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAPI.Entities;
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

        // GET: api/Product
        [HttpGet]
        public IEnumerable<Product> GetProduct()
        {
            return _context.Product;
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public IActionResult GetProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = _context.Product.Include(p => p.Image).SingleOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
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
        public IActionResult PostProduct([FromBody] Dictionary<string, string> body)
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
            return Ok();
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}