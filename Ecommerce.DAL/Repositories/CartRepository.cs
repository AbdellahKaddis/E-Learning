//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Ecommerce.DAL.Db;
//using Ecommerce.Models.Entities;

//namespace Ecommerce.DAL.Repositories
//{
//    public class CartRepository
//    {
//        private readonly AppDbContext _context;

//        public CartRepository(AppDbContext context)
//        {
//            _context = context;
//        }

//        public List<Cart> GetAllCart()
//        {
//            return _context.Cart.ToList();
//        }
//        public Cart GetCartId(int id)
//        {
//            return _context.Cart.FirstOrDefault(u => u.idCart == id);
//        }
//        public Cart AddCart(Cart l)
//        {
//            var cart = new Cart
//            {
//                idCart = l.idCart,
//                UserId = l.UserId,


//                CourseId = l.CourseId
//            };
//            _context.Cart.Add(cart);
//            _context.SaveChanges();
//            return cart;
//        }
        //public bool DeleteCart(int id)
        //{
        //    var cart = _context.Cart.Find(id);
        //    if (cart != null)
        //    {
        //        _context.Lesson.Remove(cart);
        //        _context.SaveChanges();
        //        return true;
        //    }
        //    return false;
        //}
        //public void DeleteAllcart()
        //        {
        //            var cart= _context.Cart.ToList();

        //            // Check if there are any items to remove.
        //            if (cart == null || !cart.Any())
        //            {
        //            ; 
        //            }
        //            else
        //            {
        //                // Remove all cart items.
        //                _context.Cart.RemoveRange(cart);
        //                _context.SaveChanges();

        //            }
        //        }
//    }
//}
