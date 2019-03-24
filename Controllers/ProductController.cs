using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class ProductController : ControllerBase
    {

        private WebsiteShoppingContext _context = new WebsiteShoppingContext();


        // GET: api/Product
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<Product> GetProduct()
        {
            return _context.Product.Include(p => p.Cate);
        }


        // GET: api/Product/5
        [HttpGet("{id}")]
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Dictionary<string, string> body)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productExist = ProductExists(id);
            if (productExist == null)
            {
                return BadRequest();
            }
            var name = body.GetValueOrDefault("name");
            var model = body.GetValueOrDefault("model");
            var cate = body.GetValueOrDefault("cate");
            var description = body.GetValueOrDefault("description");
            var height = body.GetValueOrDefault("height");
            var weight = body.GetValueOrDefault("weight");
            var price = body.GetValueOrDefault("price");
            var quantity = body.GetValueOrDefault("quantity");
            var imgThumb = body.GetValueOrDefault("imgThumb");
            productExist.Name = name;
            productExist.Model = model;
            productExist.CateId = int.Parse(cate);
            productExist.Description = description;
            productExist.Height = double.Parse(height);
            productExist.Weight = double.Parse(weight);
            productExist.Price = double.Parse(price);
            productExist.Quantity = int.Parse(quantity);
            productExist.Thumbnail = imgThumb;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Dictionary<string, string> body)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
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
            await _context.SaveChangesAsync();
            return Ok();
        }


        private Product ProductExists(int id)
        {
            return _context.Product.Where(p => p.Id == id).FirstOrDefault();
        }
    }
}