using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class CourseProgress
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }

        public int TotalLessons { get; set; }
        public int CompletedLessons { get; set; }
        public double PercentageCompleted { get; set; }
        public bool IsCourseCompleted { get; set; }
    }
}
