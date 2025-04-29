using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class CreateParentDTO
    {
        public string Address { get; set; }
        public string Cin { get; set; }
        public string Telephone { get; set; }
        public int UserId { get; set; } 
    }

}
