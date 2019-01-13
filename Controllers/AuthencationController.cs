using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Entities;
namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthencationController : ControllerBase
    {
        private WebsiteShoppingContext context = new WebsiteShoppingContext();


        // [A]
        [HttpPost]
        public ActionResult checkLogin([FromBody] Dictionary<string,string> body)
        {
            Console.WriteLine(body.GetValueOrDefault("username"));
            Console.WriteLine(body.GetValueOrDefault("password"));

            return BadRequest(); 
        }
    }
}