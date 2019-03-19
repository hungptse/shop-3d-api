using Microsoft.AspNetCore.Mvc;
using ShopAPI.Entities;
using ShopAPI.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopAPI.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private WebsiteShoppingContext _context = new WebsiteShoppingContext();

        [HttpPost("login")]
        public ActionResult Token([FromBody] Dictionary<string, string> body)
        {
            string username = body.GetValueOrDefault("username");
            string password = body.GetValueOrDefault("password");

            Account account = _context.Account.SingleOrDefault(element => element.Username == username);
            if (account != null)
            {
                if (PasswordEncrypt.Verify(password, account.Password))
                {
                    Response.Headers.TryAdd("Authorization", TokenServices.GetTokenFromUser(username, _context.Role.SingleOrDefault(el => el.Id == account.RoleId).Name));
                    Response.Headers.Add("Access-Control-Expose-Headers", "Authorization");
                    return Ok();

                }
            }
            return BadRequest();
        }

        [HttpPut("change_password")]
        public async Task<IActionResult> UpdatePassword([FromBody] Dictionary<string, string> body)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string username = body.GetValueOrDefault("username");
            string password = body.GetValueOrDefault("password");

            Account account = _context.Account.SingleOrDefault(element => element.Username == username);
            if (account != null)
            {
                if (PasswordEncrypt.Verify(password, account.Password))
                {
                    account.Password = PasswordEncrypt.Encrypt(body.GetValueOrDefault("newPassword"));
                    await _context.SaveChangesAsync();
                    return Ok();

                }
            }
            
            return BadRequest();
        }


    }
}