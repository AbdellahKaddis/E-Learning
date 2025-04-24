using Ecommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public string Duration { get; set; }
        public string Level { get; set; }
        public string ImageCourse {  get; set; }
        public string Formateur { get; set; }
        public string Category {  get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }


    }
}
