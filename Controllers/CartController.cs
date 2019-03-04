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
        private WebsiteShoppingContext context = new WebsiteShoppingContext();

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
            var thumbnail = body.GetValueOrDefault("thumbnail");
            if (cartOfUser == null)
            {
                CartObj cartNewUser = new CartObj(userId);
                cartNewUser.Cart.Add(new ProductInCart { ID = idPro, Name = name, Price = price, Quantity = quantity, Thumbnail = thumbnail });
                carts.Add(cartNewUser);
                return Ok(cartNewUser);
            }

            if (cartOfUser.Cart.SingleOrDefault(p => p.ID == idPro) == null)
            {
                ProductInCart productInCart = new ProductInCart { ID = idPro, Name = name, Price = price, Quantity = quantity, Thumbnail = thumbnail };
                cartOfUser.Cart.Add(productInCart);
            }
            else
            {
                ProductInCart productInCart = cartOfUser.Cart.SingleOrDefault(p => p.ID == idPro);
                var newQty = productInCart.Quantity + quantity;
                var stock = context.Product.SingleOrDefault(p => p.Id == idPro).Quantity;
                if (newQty >= stock)
                {
                    productInCart.Quantity = int.Parse(stock + "");
                }
                else
                {
                    productInCart.Quantity += quantity;
                }
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
                //cartOfNew.Cart.Concat(cartOfOld.Cart);
                foreach (var product in cartOfOld.Cart)
                {
                    var isExistedProduct = cartOfNew.Cart.SingleOrDefault(p => p.ID == product.ID);
                    if (isExistedProduct != null)
                    {
                        cartOfNew.Cart.Add(product);
                    } else
                    {
                        isExistedProduct.Quantity += product.Quantity;
                    }
                }
                carts.Remove(cartOfOld);
                return Ok(cartOfNew);
            }
            else if (cartOfNew != null && cartOfOld == null)
            {
                return Ok(cartOfNew);
            }
            else if (cartOfOld != null && cartOfNew == null)
            {
                CartObj cartNew = new CartObj(newUser);
                cartNew.Cart = cartOfOld.Cart;
                carts.Remove(cartOfOld);
                carts.Add(cartNew);
                return Ok(cartNew);
            }
            return BadRequest();
        }
    }
}