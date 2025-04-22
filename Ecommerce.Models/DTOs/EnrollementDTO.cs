using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class EnrollementDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public int ClasseId { get; set; }
        public string ClasseName { get; set; } = string.Empty;

        public DateTime EnrollementDate { get; set; }
    }
}
