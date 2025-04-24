using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int ParentId {  get; set; }
        public Parent Parent { get; set; }
        public int ? ClasseId {  get; set; }
        public Classe Classe { get; set; }
       

    }
}
