using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Ecommerce.Models.DTOs
{
    public class LessonDto
    {
        public int LessonId { get; set; }
        public string titre { get; set; }
        public string? URL { get; set; }

        public string? Description { get; set; }

        public TimeSpan Duration { get; set; }
        public string CourseName { get; set; }
    }
}


