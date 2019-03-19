using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private WebsiteShoppingContext _context = new WebsiteShoppingContext();
       
        // GET api/values
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<Account>> Get()
        {
            return _context.Account.ToArray();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = _context.Account.SingleOrDefault(a => a.Username == id);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST api/values
       

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile([FromRoute] string id, [FromBody] Dictionary<string, string> body)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Account account = _context.Account.SingleOrDefault(a => a.Username.Equals(id));
            if (account == null)
            {
                return BadRequest();
            }

            account.Name = body.GetValueOrDefault("name");
            account.Gender = bool.Parse(body.GetValueOrDefault("gender"));
            account.BirthDate = DateTime.Parse(body.GetValueOrDefault("birthdate"));
            account.Email = body.GetValueOrDefault("email");
            account.Phone = body.GetValueOrDefault("phone");
            account.Address = body.GetValueOrDefault("address");
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
