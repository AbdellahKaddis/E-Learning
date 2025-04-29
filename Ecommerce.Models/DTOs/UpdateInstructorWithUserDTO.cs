using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class UpdateInstructorWithUserDTO
    {
        // User fields
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        public string? Password { get; set; }
        // Parent fields
        public string? Address { get; set; }
        public string? Cin { get; set; }
        public string? Telephone { get; set; }
        public string? Specialite { get; set; }
    }
}
