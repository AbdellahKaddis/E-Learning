using Ecommerce.BLL.Services;
using Ecommerce.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorsController : ControllerBase
    {
        private readonly InstructorService _instructorService;
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public InstructorsController(InstructorService service, UserService userService, IConfiguration configuration)
        {
            _instructorService = service;
            _userService = userService;
            _configuration = configuration;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<InstructorDTO>>> GetAllInstructors()
        {
            var instructors = await _instructorService.GetAllInstructorsAsync();
            return instructors.Any() ? Ok(instructors) : NotFound("No Instructors Found.");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<InstructorDTO>> GetInstructorById(int id)
        {
            var instructor = await _instructorService.GetInstructorByIdAsync(id);
            return instructor == null ? NotFound() : Ok(instructor);
        }

        [HttpGet("user/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<InstructorDTO>> GetInstructorByUserId(int id)
        {
            var instructor = await _instructorService.GetInstructorByUserIdAsync(id);
            return instructor == null ? NotFound() : Ok(instructor);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<InstructorDTO>> AddInstructor(CreateUserWithInstructorDTO dto)
        {
            try
            {
                var createdInstructor = await _instructorService.CreateInstructorWithUserAsync(dto);
                return CreatedAtAction(nameof(GetInstructorById), new { id = createdInstructor.Id }, createdInstructor);
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
        public async Task<ActionResult<InstructorDTO>> UpdateInstructor(int id, UpdateInstructorWithUserDTO dto)
        {

            var updated = await _instructorService.UpdateParentWithUserAsync(id, dto);
            return updated == null ? NotFound("Instructor or User not found.") : Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteInstructor(int id)
        {
            var success = await _instructorService.DeleteInstructorAsync(id);
            return success ? Ok("Deleted successfully") : NotFound("No Instructor found.");
        }

    }

}
