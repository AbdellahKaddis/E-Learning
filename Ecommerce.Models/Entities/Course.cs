using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public string Duration { get; set; }

        public int? LevelId { get; set; }
        public Level? Level { get; set; }

        public string ImageCourse { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<Lesson>? Lessons { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;

        public List<Schedule> Schedules { get; set; }

    }
}
