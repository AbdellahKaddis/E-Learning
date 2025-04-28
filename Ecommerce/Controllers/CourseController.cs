using Ecommerce.BLL.Services;
using Ecommerce.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseService _service;
        private readonly EmailService _emailService;
        private readonly UserService _userService;

        public CourseController(CourseService service, EmailService emailService, UserService userService)
        {
            _service = service;
            _emailService = emailService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDTO>>> GetAllCourses()
        {
            var courses = await _service.GetAllCoursesAsync();
            return courses.Any() ? Ok(courses) : NotFound("No courses Found.");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDTO>> GetCourseById(int id)
        {
            var course = await _service.GetCourseByIdAsync(id);
            return course == null ? NotFound() : Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] CreateCourseDTO course)
        {
            if (course is null ||
                string.IsNullOrWhiteSpace(course.CourseName) ||
                string.IsNullOrWhiteSpace(course.CourseDescription) ||
                string.IsNullOrWhiteSpace(course.ImageCourse))
            {
                return BadRequest("Invalid Course data.");
            }

            var result = await _service.AddCourseAsync(course);
            return result ? Ok(new { message = "Course created successfully" })
                          : StatusCode(500, "Failed to create course.");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseDTO course)
        {
            var courseExists = await _service.GetCourseByIdAsync(id);
            if (courseExists == null)
                return NotFound("Course not found.");

            var updated = await _service.UpdateCourseAsync(id, course);
            if (!updated)
                return StatusCode(500, "Update failed.");

            return Ok(new { message = "Course updated successfully" });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var success = await _service.DeleteCourseAsync(id);
            return success ? Ok(new { message = "Course deleted successfully" }) : NotFound("No course Found.");
        }
    }
}