//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Ecommerce.DAL.Repositories;
//using Ecommerce.Models.Entities;

//namespace Ecommerce.BLL.Services
//{
//    public class CartService
//    {

//        private readonly CartRepository  _repo;


//        public CartService(CartRepository repo)
//        {
//            _repo = repo;
//        }

//        public List<Cart> GetAllcart() => _repo.GetAllCart();
//        public Cart GetCartId(int id) => _repo.GetCartId(id);
//        public Cart AddCart(Cart l) => _repo.AddCart(l);
//        //public bool DeleteCart(int id) => _repo.DeleteCart(id);
//        //public void DeleteAllCart() => _repo.DeleteAllcart();
//    }
//}
