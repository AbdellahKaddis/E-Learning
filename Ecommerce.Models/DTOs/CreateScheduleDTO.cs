using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class CreateScheduleDTO
    {
        public int Year { get; set; }
        [Range(1, 52, ErrorMessage = "Week must be between 1 and 52.")]
        public int Week { get; set; }
        [Range(0, 6, ErrorMessage = "Day must be between 0 and 6.")]
        public int Day { get; set; }
        public int ClasseId { get; set; }
        public int LocationId { get; set; }
        public int CourseId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
