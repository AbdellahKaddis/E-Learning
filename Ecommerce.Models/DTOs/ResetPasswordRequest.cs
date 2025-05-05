using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class ResetPasswordRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string ResetToken { get; set; }

        [Required, DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
