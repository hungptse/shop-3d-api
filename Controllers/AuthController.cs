using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ShopAPI.Entities;
using ShopAPI.Helpers;

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
                    Response.Headers.TryAdd("Authorization", TokenServices.GetTokenFromUser(username));
                    Response.Headers.Add("Access-Control-Expose-Headers", "Authorization");
                    return Ok();
                    
                }
            }
            return BadRequest();   
        }


    }
}