using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class ScheduleDTO
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Week { get; set; }
        public int Day { get; set; }
        public string ClasseName { get; set; }
        public string LocationName { get; set; }
        public string CourseName { get; set; }
        public string FormateurName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

}

