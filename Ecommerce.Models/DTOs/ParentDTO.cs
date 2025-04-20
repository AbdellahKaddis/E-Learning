using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class ParentDTO
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int UserId { get; set; }
        public UserDTO User { get; set; }
    }
}
