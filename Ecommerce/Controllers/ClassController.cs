using Ecommerce.BLL.Services;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly ClasseService _service;

        public ClassController(ClasseService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> AddClass([FromBody] CreateClassDTO clase)
        {
            if (clase is null)
                return BadRequest("Class is invalid");
            if (string.IsNullOrWhiteSpace(clase.Name))
                return BadRequest("ClassName is invalid");

            var newClass = await _service.AddClassAsync(clase);
            return newClass ? Ok(new { message = "Class created successfully" })
                            : StatusCode(500, "Failed to create class.");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassDTO>>> GetAllClass()
        {
            var clases = await _service.GetAllClassAsync();
            return clases.Any() ? Ok(clases) : NotFound("No Class Found.");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClassDTO>> GetClassById(int id)
        {
            var clase = await _service.GetClassByIdAsync(id);
            return clase == null ? NotFound() : Ok(clase);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var success = await _service.DeleteClassAsync(id);
            return success ? Ok(new { message = "Class deleted successfully" }) : NotFound("No Class Found.");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateClass(int id, [FromBody] UpdateClassDTO cl)
        {
            var claseExists = await _service.GetClassByIdAsync(id);
            if (claseExists == null)
                return NotFound("Class not found.");

            var updated = await _service.UpdateClassAsync(id, cl);
            if (!updated)
                return StatusCode(500, "Update failed.");

            return Ok(new { message = "Class updated successfully" });
        }

    }
}
