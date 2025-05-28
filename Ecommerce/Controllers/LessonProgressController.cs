using Ecommerce.BLL.Services;
using Ecommerce.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonProgressController : ControllerBase
    {
        private readonly LessonProgressService _service;

        public LessonProgressController(LessonProgressService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LessonProgressDTO>>> GetAllLessonProgressAsync()
        {
            var lesPro = await _service.GetAllLessonProgressAsync();
            return lesPro.Any() ? Ok(lesPro) : NotFound("No Lesson_Progress Found.");
        }

        [HttpGet("{studentId}")]
        public async Task<ActionResult<IEnumerable<LessonProgressDTO>>> GetLessonProgressByStudentIdAsync(int studentId)
        {
            var lessonProgress = await _service.GetLessonProgressByStudentIdAsync(studentId);
            return lessonProgress.Any() ? Ok(lessonProgress) : NotFound("No lesson progress found for this student.");
        }

        [HttpGet("LessonProgressByStudentIdAndCourseId/{studentId}/{courseId}")]
        public async Task<ActionResult<IEnumerable<LessonProgressDTO>>> GetLessonProgressByStudentIdAndCourseIdAsync(int studentId, int courseId)
        {
            var lessonProgress = await _service.GetLessonProgressByStudentIdAndCourseIdAsync(studentId, courseId);
            return lessonProgress.Any() ? Ok(lessonProgress) : NotFound("No lesson progress found for this student and course.");
        }

        [HttpPost]
        public async Task<IActionResult> AddCourseAsync([FromBody] CreateLessonProgressDTO les)
        {
            if (les == null)
                return BadRequest("Lesson_Progress is invalid");

            var isNewRecord = await _service.AddOrUpdateCourseAsync(les);

            if (isNewRecord)
                return Ok(new { message = "LessonProgress created successfully" });
            else
                return Ok(new { message = "LessonProgress updated successfully" });
        }

        [HttpPut("student/{studentId}/lesson/{lessonId}")]
        public async Task<IActionResult> UpdateLessonProgressAsync(int studentId, int lessonId, [FromBody] UpdateLessonProgressDTO les)
        {
            if (les == null)
                return BadRequest("Lesson progress data is invalid");

            var updated = await _service.UpdateLessonProgressAsync(studentId, lessonId, les);
            if (!updated)
                return NotFound("Lesson progress not found for the given student and lesson.");

            return Ok(new { message = "Lesson progress updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLessonProgressAsync(int id)
        {
            var deleted = await _service.DeleteLessonProgressAsync(id);
            if (!deleted)
                return NotFound("Lesson progress not found.");

            return Ok(new { message = "Lesson progress deleted successfully" });
        }

        [HttpGet("CourseProgress/{studentId}/{courseId}")]
        public async Task<IActionResult> GetCourseProgressAsync(int studentId, int courseId)
        {
            var result = await _service.GetCourseProgressAsync(studentId, courseId);
            return Ok(result);
        }

        [HttpGet("CoursesProgressByLevel/{studentId}")]
        public async Task<IActionResult> GetCoursesProgressByStudentLevelAsync(int studentId)
        {
            var result = await _service.GetCoursesProgressByStudentLevelAsync(studentId);
            return Ok(result);
        }

        [HttpGet("CoursesProgressByStudentAndLevel/{studentId}/{levelId}")]
        public async Task<IActionResult> GetCoursesProgressByStudentAndLevelAsync(int studentId, int levelId)
        {
            var result = await _service.GetCoursesProgressByStudentAndLevelAsync(studentId, levelId);
            return Ok(result);
        }
        [HttpGet("LessonsWithProgress/{studentId}/{courseId}")]
        public async Task<ActionResult<IEnumerable<LessonWithProgressDTO>>> GetLessonsWithProgressByStudentAndCourseAsync(int studentId, int courseId)
        {
            var result = await _service.GetLessonsWithProgressByStudentAndCourseAsync(studentId, courseId);
            return result.Any() ? Ok(result) : NotFound("No lessons found for the specified course or student.");
        }

    }
}
