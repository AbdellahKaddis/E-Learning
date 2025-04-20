using Ecommerce.BLL.Services;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseService _service;

        public CourseController(CourseService service)
        {
            _service = service;
        }
        [HttpGet]
        public ActionResult<IEnumerable<CourseDTO>> GetAllCourses()
        {
            var courses = _service.GetAllCourses();
            return courses.Any() ? Ok(courses) : NotFound("No courses Found.");
        }
        [HttpGet("{id}")]
        public ActionResult<CourseDTO> GetCourseById(int id)
        {
            var course = _service.GetCourseById(id);
            return course == null ? NotFound() : Ok(course);
        }
        [HttpPost]
        public IActionResult AddCourse([FromBody] CreateCourseDTO course)
        {
            if (course is null)
                return BadRequest("Course is invalid");
            if (string.IsNullOrWhiteSpace(course.CourseName))
                return BadRequest("CourseName is invalid");
            if (string.IsNullOrWhiteSpace(course.CourseDescription))
                return BadRequest("CourseDescription is invalid");
            if (string.IsNullOrWhiteSpace(course.Level))
                return BadRequest("Level is invalid");
            if (string.IsNullOrWhiteSpace(course.ImageCourse))
                return BadRequest("Image is invalid");

            var newCourse = _service.AddCourse(course);
            return Ok(new { message = "Course created successfully" });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateCourse(int id, UpdateCourseDTO course)
        {
            var courseExists = _service.GetCourseById(id);
            if (courseExists == null)
                return NotFound("Course not found.");

            var updated = _service.UpdateCourse(id, course);
            if (!updated)
                return StatusCode(500, "Update failed.");

            return Ok(new { message = "Course updated successfully" });
        }

    }
}
