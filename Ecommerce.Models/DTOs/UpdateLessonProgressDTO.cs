using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class UpdateLessonProgressDTO
    {
        public int StudentId { get; set; }
        public int LessonId { get; set; }
        public int CourseId { get; set; }
        public bool IsCompleted { get; set; }
        public int LastSecond { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
