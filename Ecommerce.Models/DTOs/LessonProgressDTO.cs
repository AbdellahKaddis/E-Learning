using Ecommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class LessonProgressDTO
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int LessonId { get; set; }
        public string LessonName { get; set; }
        public int LastSecond { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
