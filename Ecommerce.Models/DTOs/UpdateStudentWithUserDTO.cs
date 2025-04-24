using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class UpdateStudentWithUserDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public int? ParentId { get; set; }
        public int? ClassId { get; set; }
    }
}