using Ecommerce.BLL.Services;
using Ecommerce.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly StudentService _studentService;
        private readonly UserService _userService;

        private readonly IConfiguration _configuration;

        public StudentsController(StudentService studentService, UserService userService, IConfiguration configuration)
        {
            _studentService = studentService;
            _userService = userService;
            _configuration = configuration;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return students.Any() ? Ok(students) : NotFound("No Students Found.");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentDTO>> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            return student == null ? NotFound() : Ok(student);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StudentDTO>> AddStudent(CreateUserWithStudentDTO dto)
        {
            try
            {
                var createdStudent = await _studentService.CreateStudentWithUserAsync(dto);
                return CreatedAtAction(nameof(GetStudentById), new { id = createdStudent.Id }, createdStudent);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StudentDTO>> UpdateStudent(int id, UpdateStudentWithUserDTO dto)
        {
            var updated = await _studentService.UpdateStudentWithUserAsync(id, dto);
            return updated == null ? NotFound("Student or User not found.") : Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var success = await _studentService.DeleteStudentAsync(id);
            return success ? Ok("Deleted successfully") : NotFound("No Student found.");
        }
    }
}
