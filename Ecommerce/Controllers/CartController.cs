//using Ecommerce.BLL.Services;
//using Ecommerce.DAL.Db;
//using Ecommerce.DAL.Repositories;
//using Ecommerce.Models.Entities;
//using Microsoft.AspNetCore.Mvc;

//namespace Ecommerce.Api.Controllers
//{
//    public class CartController : Controller
//    {
//        private readonly CartService _service;

//        private readonly IConfiguration _configuration;
//        public CartController(AppDbContext context, IConfiguration configuration)
//        {
//            var repo = new CartRepository(context);
//            _service = new CartService(repo);

//            _configuration = configuration;
//        }

//        [HttpGet()]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public ActionResult<IEnumerable<Cart>> GetAllCarts()
//        {
//            var Carts = _service.GetAllcart();
//            return Carts.Any() ? Ok(Carts) : NotFound("No Cart was found.");
//        }


//        [HttpGet("{id}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public ActionResult<Cart> GetCartId(int id)
//        {
//            var Cart = _service.GetCartId(id);
//            return Cart == null ? NotFound() : Ok(Cart);
//        }

//        [HttpPost("AddCart")]
//        [ProducesResponseType(StatusCodes.Status201Created)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public ActionResult<Cart> AddCart(Cart Cart)
//        {
//            var CartAdded = _service.AddCart(Cart);
//            return Ok(CartAdded);
//        }

        //[HttpDelete("DeleteCart")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public ActionResult<Cart> DeleteCart(int id)
        //{
        //    var CartDeleted = _service.DeleteCart(id);
        //    if (CartDeleted)
        //    {
        //        return Ok("Cart deleted successfully.");
        //    }
        //    else
        //    {
        //        return NotFound("Cart not found.");
        //    }
        //}
        //[HttpDelete("DeleteAll")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public ActionResult<Cart> DeleteAllCart()
        //{
        //    var CartDeleted = _service.DeleteAllCart();
        //    if (CartDeleted)
        //    {
        //        return Ok("Cart deleted successfully.");
        //    }
        //    else
        //    {
        //        return NotFound("Cart not found.");
        //    }
        //}
//    }
//}
