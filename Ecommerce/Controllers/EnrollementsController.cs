using Ecommerce.BLL.Services;
using Ecommerce.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollementsController : ControllerBase
    {
        private readonly EnrollementService _enrollementService;

        public EnrollementsController(EnrollementService enrollementService)
        {
            _enrollementService = enrollementService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EnrollementDTO>>> GetAllEnrollements()
        {
            var enrollements = await _enrollementService.GetAllEnrollementsAsync();
            return enrollements.Any() ? Ok(enrollements) : NotFound("No Enrollements Found.");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EnrollementDTO>> GetEnrollementById(int id)
        {
            var enrollement = await _enrollementService.GetEnrollementByIdAsync(id);
            return enrollement == null ? NotFound() : Ok(enrollement);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EnrollementDTO>> AddEnrollement(CreateEnrollementDTO dto)
        {
            var createdEnrollement = await _enrollementService.AddEnrollementAsync(dto);
            return CreatedAtAction(nameof(GetEnrollementById), new { id = createdEnrollement?.Id }, createdEnrollement);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EnrollementDTO>> UpdateEnrollement(int id, UpdateEnrollementDTO dto)
        {
            var updated = await _enrollementService.UpdateEnrollementAsync(id, dto);
            return updated == null ? NotFound("Enrollement not found.") : Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEnrollement(int id)
        {
            var success = await _enrollementService.DeleteEnrollementAsync(id);
            return success ? Ok("Enrollement deleted successfully.") : NotFound("No Enrollement found.");
        }
    }
}
