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

        public int? ClasseId { get; set; }

    }

}
