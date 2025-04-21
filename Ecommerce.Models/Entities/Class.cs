using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Entities
{
    public class Classe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Student>? students { get; set; }
    }
}
