using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class CreateCourseDTO
    {
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }

        public int? LevelId { get; set; }

        public string Duration { get; set; }

        public string ImageCourse { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
