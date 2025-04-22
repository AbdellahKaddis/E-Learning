using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class CreateEnrollementDTO
    {
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int ClasseId { get; set; }
    }
}
