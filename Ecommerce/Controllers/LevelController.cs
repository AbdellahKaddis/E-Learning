using Ecommerce.BLL.Services;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelController : ControllerBase
    {
        private readonly LevelService _service;

        public LevelController(LevelService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddLevel([FromBody] CreateLevelDTO level)
        {
            if (level is null)
                return BadRequest("Level is invalid");
            if (string.IsNullOrWhiteSpace(level.Name))
                return BadRequest("LevelName is invalid");

            var newLevel = await _service.AddLevelAsync(level);
            return newLevel ? Ok(new { message = "Level created successfully" })
                            : StatusCode(500, "Failed to create level.");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LevelDTO>>> GetAllLevels()
        {
            var levels = await _service.GetAllLevelsAsync();
            return levels.Any() ? Ok(levels) : NotFound("No Level Found.");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LevelDTO>> GetLevelById(int id)
        {
            var level = await _service.GetLevelByIdAsync(id);
            return level == null ? NotFound() : Ok(level);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteLevel(int id)
        {
            var success = await _service.DeleteLevelAsync(id);
            return success ? Ok(new { message = "Level deleted successfully" }) : NotFound("No Level Found.");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateLevel(int id, [FromBody] UpdateLevelDTO level)
        {
            var levelExists = await _service.GetLevelByIdAsync(id);
            if (levelExists == null)
                return NotFound("Level not found.");

            var updated = await _service.UpdateLevelAsync(id, level);
            if (!updated)
                return StatusCode(500, "Update failed.");

            return Ok(new { message = "Level updated successfully" });
        }
    }
}
