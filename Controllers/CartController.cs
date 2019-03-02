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

        [HttpGet]
        public ActionResult<CartObj> GetAllCart()
        {
            return Ok(carts);
        }


        [HttpGet("{userId}")]
        public ActionResult<CartObj> GetCartByUser(string userId)
        {
            var cartOfUser = carts.SingleOrDefault(cart => cart.accId == userId);
            if (cartOfUser != null)
            {
                return Ok(cartOfUser);
            }
            //else
            //{
            //    CartObj cartNewUser = new CartObj(userId);
            //    carts.Add(cartNewUser);
            //    return Ok(cartNewUser);
            //}
            return BadRequest();
        }

        [HttpPost("{userId}")]
        public ActionResult<CartObj> addToCart([FromRoute] string userId, [FromBody] Dictionary<string, string> body)
        {
            var cartOfUser = carts.SingleOrDefault(cart => cart.accId == userId);
            var idPro = int.Parse(body.GetValueOrDefault("id"));
            var quantity = int.Parse(body.GetValueOrDefault("quantity"));
            var price = float.Parse(body.GetValueOrDefault("price"));
            var name = body.GetValueOrDefault("name");
            if (cartOfUser == null)
            {
                CartObj cartNewUser = new CartObj(userId);
                cartNewUser.Cart.Add(new ProductInCart { ID = idPro, Name = name, Price = price, Quantity = quantity });
                carts.Add(cartNewUser);
                return Ok(cartNewUser);
            }

            if (cartOfUser.Cart.SingleOrDefault(p => p.ID == idPro) == null)
            {
                ProductInCart productInCart = new ProductInCart { ID = idPro, Name = name, Price = price, Quantity = quantity };
                cartOfUser.Cart.Add(productInCart);
            }
            else
            {
                ProductInCart productInCart = cartOfUser.Cart.SingleOrDefault(p => p.ID == idPro);
                productInCart.Quantity += quantity;
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

        [HttpPut("changeUser")]
        public ActionResult<CartObj> updateUserID([FromBody] Dictionary<string, string> body)
        {
            var oldUser = body.GetValueOrDefault("old_id");
            var newUser = body.GetValueOrDefault("new_id");

            var cartOfOld = carts.SingleOrDefault(cart => cart.accId == oldUser);
            var cartOfNew = carts.SingleOrDefault(cart => cart.accId == newUser);

            if (cartOfOld != null && cartOfNew != null)
            {
                return NoContent();
            }
            else if (cartOfNew != null)
            {
                return Ok(cartOfNew);
            }
            else if (cartOfOld != null)
            {
                cartOfNew.Cart = cartOfOld.Cart;
                carts.Remove(cartOfOld);
                return Ok(cartOfNew);
            }
            return BadRequest();
        }
    }
}