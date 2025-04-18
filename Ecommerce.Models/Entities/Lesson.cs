using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Ecommerce.Models.Entities
{
    public class Lesson
    {
        public int LessonId { get; set; }
        public string titre { get; set; }
        public string? URL { get; set; }
        public TimeSpan Duration { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
