using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Entities
{
    public class LessonProgress
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        public int LessonId { get; set; }
        public Lesson? Lesson { get; set; }
        public int LastSecond { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
