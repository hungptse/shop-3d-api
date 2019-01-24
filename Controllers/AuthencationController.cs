using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Entities;
using ShopAPI.Helpers;
using Microsoft;
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
    public class AuthencationController : ControllerBase
    {
        private WebsiteShoppingContext context = new WebsiteShoppingContext();


        // [AUTHENCATE]
        [AllowAnonymous]
        [HttpPost("/login")]
        public ActionResult CheckLogin([FromBody] Dictionary<string,string> body)
        {
            string username = body.GetValueOrDefault("username");
            string password = body.GetValueOrDefault("password");

            Account account = context.Account.SingleOrDefault(element => element.Username == username); 
            if (account != null)
            {   
                if (PasswordEncrypt.Verify(password,account.Password))
                {
                    Response.Headers.TryAdd("Authorization",TokenServices.GetTokenFromUser(username));
                    return Ok(); 
                }
            } 
            return Unauthorized();
        }
    }
}