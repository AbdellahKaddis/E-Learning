using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class StudentWithParentDTO
    {
        public string StudentFullName { get; set; }
        public string ParentFullName { get; set; }
        public string ParentEmail { get; set; }  
    }
}
