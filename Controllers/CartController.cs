using Microsoft.AspNetCore.Mvc;
using ShopAPI.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ShopAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private static List<CartObj> carts = new List<CartObj>();

        [HttpGet("{userId}")]
        public ActionResult<CartObj> GetCartByUser(string userId)
        {
            var cartOfUser = carts.SingleOrDefault(cart => cart.accId == userId);
            if (cartOfUser != null)
            {
                return Ok(cartOfUser);
            }
            CartObj cartNewUser = new CartObj(userId);
            carts.Add(cartNewUser);
            return Ok(cartNewUser);
        }

        [HttpPost("{userId}")]
        public ActionResult<CartObj> addToCart([FromRoute] string userId, [FromBody] Dictionary<string, string> body)
        {
            var cartOfUser = carts.SingleOrDefault(cart => cart.accId == userId);
            if (cartOfUser == null)
            {
                CartObj cartNewUser = new CartObj(userId);
                carts.Add(cartNewUser);
                return Ok(cartNewUser);
            }
            var idPro = int.Parse(body.GetValueOrDefault("id"));
            var quantity = int.Parse(body.GetValueOrDefault("quantity"));
            var price = float.Parse(body.GetValueOrDefault("price"));
            var name = body.GetValueOrDefault("name");
            if (cartOfUser.Cart.SingleOrDefault(p => p.ID == idPro) == null)
            {
                ProductInCart productInCart = new ProductInCart { ID = idPro, Name = name, Price = price, Quantity = quantity };
                cartOfUser.Cart.Add(productInCart);
            }
            else
            {
                ProductInCart productInCart = cartOfUser.Cart.SingleOrDefault(p => p.ID == idPro);
                productInCart.Quantity++;
            }
            return Ok(cartOfUser);

            //carts.Add(new CartObj(userId));
        }

        [HttpPut("{userId}")]
        public ActionResult<CartObj> removeProduct([FromRoute] string userId, [FromBody] Dictionary<string, string> body)
        {
            var cartOfUser = carts.SingleOrDefault(cart => cart.accId == userId);
            if (cartOfUser != null)
            {
                var idPro = int.Parse(body.GetValueOrDefault("id"));
                if (cartOfUser.Cart.SingleOrDefault(p => p.ID == idPro) != null)
                {
                    ProductInCart productInCart = cartOfUser.Cart.Single(p => p.ID == idPro);
                    cartOfUser.Cart.Remove(productInCart);
                    return Ok(cartOfUser);
                }
            }
            return BadRequest();
        }
    }
}