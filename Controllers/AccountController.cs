using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAPI.Entities;
using ShopAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {

        private WebsiteShoppingContext _context = new WebsiteShoppingContext();

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Account>> Get()
        {
            return _context.Account.Include(a => a.Feedback).Include(a => a.Order).ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = _context.Account.Where(a => a.Username == id).FirstOrDefault();
            //Include(a => a.Feedback).Include(a => a.Order)
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
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

        [AllowAnonymous]
        [HttpGet("check/{username}")]
        public IActionResult checkDuplicate(string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = _context.Account.Where(a => a.Username == username).FirstOrDefault();
            //Include(a => a.Feedback).Include(a => a.Order)
            if (account == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult registerAccount([FromBody] Dictionary<string, string> body)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var username = body.GetValueOrDefault("username");
            var account = _context.Account.Where(a => a.Username == username).FirstOrDefault();
            //Include(a => a.Feedback).Include(a => a.Order)
            if (account == null)
            {
                var name = body.GetValueOrDefault("name");
                var password = body.GetValueOrDefault("password");
                var email = body.GetValueOrDefault("email");
                var phone = body.GetValueOrDefault("phone");
                var address = body.GetValueOrDefault("address");
                Account newAcc = new Account { Username = username, Name = name, Password = PasswordEncrypt.Encrypt(password), CreateAt = DateTime.Now, Address = address, Phone = phone, RoleId = 2, AvatarUrl = "https://firebasestorage.googleapis.com/v0/b/image-3d.appspot.com/o/img-user%2F17004.svg?alt=media&token=3f948179-534c-4825-9e59-4e5b91483085", Email = email, BirthDate = new DateTime(2000, 1, 1), Gender = true };
                _context.Account.Add(newAcc);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }

    }
}
