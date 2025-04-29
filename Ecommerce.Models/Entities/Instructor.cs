using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Entities
{
    public class Instructor
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Cin { get; set; }
        public string Telephone { get; set; }
        public string Specialite { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
