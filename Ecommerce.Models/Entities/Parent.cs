using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Entities
{
    public class Parent
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Cin { get; set; }
        public string Telephone { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Student> students { get; set; }
    }
}
