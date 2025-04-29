using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string LevelName  { get; set; }
        public string ClassName { get; set; }

        public UserDTO User { get; set; }

        public int ParentId { get; set; }
        public ParentDTO Parent { get; set; }
    }

}
