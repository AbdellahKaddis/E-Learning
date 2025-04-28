using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class CreateUserWithStudentDTO
    {
        // User info
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Student info
        public DateOnly DateOfBirth { get; set; }
        public int ParentId { get; set; }
<<<<<<< HEAD
        public int ClasseId { get; set; }
        public int LevelId { get; set; }
=======
        public int? ClasseId { get; set; }
>>>>>>> aaec8d4816efbb6e32d967fa847ef4a5999e8e7c
    }

}
