using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class CreateStudentDTO
    {
        public DateOnly DateOfBirth { get; set; }
        public int UserId { get; set; }
        public int LevelId { get; set; }
        public int ParentId { get; set; }
        public int ClasseId { get; set; }
    }

}
