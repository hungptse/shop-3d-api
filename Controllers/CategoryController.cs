using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAPI.Entities;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private WebsiteShoppingContext _context = new WebsiteShoppingContext();

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<Category> GetCategory()
        {
            return _context.Category.Include(c => c.Product);
        }

        [HttpGet("name")]
        public IEnumerable<Category> GetCategoryName()
        {
            return _context.Category.Select(c => new Category { Id = c.Id, Name = c.Name });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.Category.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNameCate([FromRoute] int id,[FromBody] Dictionary<string, string> body)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Category category = _context.Category.SingleOrDefault(c => c.Id == id);
            if (category == null)
            {
                return BadRequest();
            }

            category.Name = body.GetValueOrDefault("name");
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Category
        //[HttpPost]
        //public async Task<IActionResult> PostCategory([FromBody] Category category)
        //{
           
        //}
    }
}