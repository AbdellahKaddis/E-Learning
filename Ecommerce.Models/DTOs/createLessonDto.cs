﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.DTOs
{
    public class createLessonDto
    {

        public string titre { get; set; }
        public string? URL { get; set; }
        public TimeSpan Duration { get; set; }
        public int CourseId { get; set; }
    }
}


