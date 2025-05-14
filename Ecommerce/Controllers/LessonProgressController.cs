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
            return lessonProgress.Any() ? Ok(lessonProgress) : NotFound("No lesson progress found for this student.");
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteLessonProgressAsync(int id)
        {
            var success = await _service.DeleteLessonProgressAsync(id);
            return success ? Ok(new { message = "Lesson_Progress deleted successfully" }) : NotFound("No course Found.");
        }

        [HttpGet("student/{studentId}/course/{CourseId}")]
        public async Task<ActionResult<IEnumerable<LessonProgressDTO>>> GetCourseProgressByStudentIdAsync(int studentId,int CourseId)
        {
            var CourseProgress = await _service.GetCourseProgressAsync(studentId,CourseId);
            return Ok(CourseProgress);
        }
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<LessonProgressDTO>>> GetCoursesProgressByStudentLevelAsync(int studentId)
        {
            var CourseProgress = await _service.GetCoursesProgressByStudentLevelAsync(studentId);
            return Ok(CourseProgress);
        }
        [HttpGet("student/{studentId}/level/{levelId}")]
        public async Task<ActionResult<IEnumerable<LessonProgressDTO>>> GetCoursesProgressByStudentAndLevelAsync(int studentId,int levelId)
        {
            var CourseProgress = await _service.GetCoursesProgressByStudentAndLevelAsync(studentId,levelId);
            return Ok(CourseProgress);
        }

    }
}
