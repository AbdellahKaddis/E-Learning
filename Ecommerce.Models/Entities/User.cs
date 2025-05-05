
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int RoleId { get; set; }
        public List<Course>? Course { get; set; }

        public Role Role { get; set; }

        public Parent Parent { get; set; }

        public Student Student { get; set; }

        public Instructor Instructor { get; set; }

        //fields for reset password
        public string? OtpHash { get; set; }      // Hashed OTP
        public DateTime? OtpExpiry { get; set; }
        public bool? OtpUsed { get; set; }
        public string? ResetToken { get; set; }   // For password reset
        public DateTime? ResetTokenExpiry { get; set; }
    }
}
