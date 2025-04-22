using Ecommerce.BLL.Services;
using Ecommerce.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParentsController : ControllerBase
    {
        private readonly ParentService _parentService;
        private readonly UserService _userService;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;

        public ParentsController(ParentService service, UserService userService, EmailService emailService, IConfiguration configuration)
        {
            _parentService = service;
            _userService = userService;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ParentDTO>>> GetAllParents()
        {
            var parents = await _parentService.GetAllParentsAsync();
            return parents.Any() ? Ok(parents) : NotFound("No Parents Found.");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ParentDTO>> GetParentById(int id)
        {
            var parent = await _parentService.GetParentByIdAsync(id);
            return parent == null ? NotFound() : Ok(parent);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ParentDTO>> AddParent(CreateUserWithParentDTO dto)
        {
            try
            {
                var createdParent = await _parentService.CreateParentWithUserAsync(dto);
                return CreatedAtAction(nameof(GetParentById), new { id = createdParent.Id }, createdParent);
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
        public async Task<ActionResult<ParentDTO>> UpdateParent(int id, UpdateParentWithUserDTO dto)
        {

            var updated = await _parentService.UpdateParentWithUserAsync(id, dto);
            return updated == null ? NotFound("Parent or User not found.") : Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteParent(int id)
        {
            var success = await _parentService.DeleteParentAsync(id);
            return success ? Ok("Deleted successfully") : NotFound("No Parent found.");
        }
    }

}
