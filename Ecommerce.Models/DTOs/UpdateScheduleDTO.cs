using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class UpdateScheduleDTO
    {
        public int? Year { get; set; }
        public int? Week { get; set; }
        public int? Day { get; set; }
        public int? ClasseId { get; set; }
        public int? LocationId { get; set; }
        public int? CourseId { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
