using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class LessonWithProgressDTO
    {
        public int LessonId { get; set; }
        public string Titre { get; set; }
        public string? URL { get; set; }
        public string? Description { get; set; }
        public TimeSpan Duration { get; set; }
        public string CourseName { get; set; }

        public int LastSecond { get; set; }
        public bool IsCompleted { get; set; }
    }
}
