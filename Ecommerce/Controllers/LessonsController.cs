using Ecommerce.BLL.Services;
using Ecommerce.DAL.Db;
using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonsController : Controller
    {

        private readonly LessonService _service;

        private readonly IConfiguration _configuration;
        public LessonsController(AppDbContext context, IConfiguration configuration)
        {
            var repo = new LessonRepository(context);
            _service = new LessonService(repo);

            _configuration = configuration;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Lesson>> GetAllLessons()
        {
            var Lessons = _service.GetAllLessons();
            return Lessons.Any() ? Ok(Lessons) : NotFound("No Lesson was found.");
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Lesson> GetLessonId(int id)
        {
            var Lesson = _service.GetLessonId(id);
            return Lesson == null ? NotFound() : Ok(Lesson);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Lesson> UpdateLesson(int id, Lesson l)
        {
            var lesson = _service.UpdateLesson(id, l);
            return lesson == null ? NotFound() : Ok(lesson);
        }

        [HttpPost("AddLesson")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Lesson> AddLesson(Lesson lesson)
        {
            var lessonAdded = _service.AddLesson(lesson);
            return Ok(lessonAdded);
        }

        [HttpDelete("DeleteLesson")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Lesson> DeleteLesson(int id)
        {
            var lessonDeleted = _service.DeleteLesson(id);
            if (lessonDeleted)
            {
                return Ok("Lesson deleted successfully.");
            }
            else
            {
                return NotFound("Lesson not found.");
            }
        }
    }
}
