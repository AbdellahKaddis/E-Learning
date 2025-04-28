using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class UpdateCourseDTO
    {
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
<<<<<<< HEAD
        public TimeSpan Duration { get; set; }
        public int LevelId { get; set; }
=======
        public string Duration { get; set; }
        public string Level { get; set; }
>>>>>>> aaec8d4816efbb6e32d967fa847ef4a5999e8e7c
        public string ImageCourse { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public DateTime Updated { get; set; }

    }
}
