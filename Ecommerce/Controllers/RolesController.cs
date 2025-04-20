using Ecommerce.BLL.Services;
using Ecommerce.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _service;

        public RolesController(RoleService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetAllRoles()
        {
            var roles = await _service.GetAllRolesAsync();
            return roles.Any() ? Ok(roles) : NotFound("No Roles Found.");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoleDTO>> GetRoleById(int id)
        {
            var role = await _service.GetRoleByIdAsync(id);
            return role == null ? NotFound() : Ok(role);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoleDTO>> AddRole(CreateRoleDTO newRole)
        {
            var created = await _service.AddRoleAsync(newRole);
            return CreatedAtAction(nameof(GetRoleById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RoleDTO>> UpdateRole(int id, UpdateRoleDTO updatedRole)
        {
            if (id != updatedRole.Id) return BadRequest("ID mismatch");

            var updated = await _service.UpdateRoleAsync(updatedRole);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var success = await _service.DeleteRoleAsync(id);
            return success ? Ok("deleted successfully") : NotFound();
        }
    }
}
