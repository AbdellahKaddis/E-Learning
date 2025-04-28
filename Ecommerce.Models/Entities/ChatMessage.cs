using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        public int InstructorId { get; set; }
        public User? User { get; set; }
        public string SenderType { get; set; }
        public string MessageText { get; set; }
        public DateTime SentAt { get; set; }
    }


}
