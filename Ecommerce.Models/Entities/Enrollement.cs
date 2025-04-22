using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Entities
{
    public class Enrollement
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }

        public int CategoryId { get; set; }

        public  Category Category { get; set; }

        public int ClasseId { get; set; }

        public Classe Classe { get; set; }

        public DateTime EnrollementDate { get; set; }
    }
}
