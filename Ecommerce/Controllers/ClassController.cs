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
        public IActionResult AddClass([FromBody] CreateClassDTO clase)
        {
            if (clase is null)
                return BadRequest("CLass is invalid");
            if (string.IsNullOrWhiteSpace(clase.Name))
                return BadRequest("ClassName is invalid");

            var newClass = _service.AddClass(clase);
            return Ok(new { message = "Class created successfully" });
        }
        [HttpGet]
        public ActionResult<IEnumerable<ClassDTO>> GetAllClass()
        {
            var clases = _service.GetAllclass();
            return clases.Any() ? Ok(clases) : NotFound("No Class Found.");
        }
        [HttpGet("{id}")]
        public ActionResult<ClassDTO> GetClassById(int id)
        {
            var clase = _service.GetClassById(id);
            return clase == null ? NotFound() : Ok(clase);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteClass(int id)
        {
            var success = _service.DeleteClass(id);
            return success ? Ok() : NotFound();
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateClass(int id, UpdateClassDTO cl)
        {
            var claseExists = _service.GetClassById(id);
            if (claseExists == null)
                return NotFound("Class not found.");

            var updated = _service.UpdateClass(id, cl);
            if (!updated)
                return StatusCode(500, "Update failed.");

            return Ok(new { message = "Class updated successfully" });
        }
    }
}
