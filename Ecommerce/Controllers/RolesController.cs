using Ecommerce.BLL.Services;
using Ecommerce.DAL.Db;
using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _service;
        private readonly IConfiguration _configuration;
        public RolesController(AppDbContext context, IConfiguration configuration)
        {
            var repo = new RoleRepository(context);
            _service = new RoleService(repo);
            _configuration = configuration;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<RoleDTO>> GetAllRoles()
        {
            var roles = _service.GetAllRoles();
            return roles.Any() ? Ok(roles) : NotFound("No Roles Found.");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<RoleDTO> GetRoleById(int id)
        {
            var role = _service.GetRoleById(id);
            return role == null ? NotFound() : Ok(role);
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<RoleDTO> AddRole(RoleDTO newRole)
        {

            var created = _service.AddRole(newRole);
            return CreatedAtAction(nameof(GetRoleById), new { id = created.Id }, created);
        }


        [HttpPut("{id}")]
        public ActionResult<RoleDTO> UpdateRole(int id, RoleDTO updatedRole)
        {
            if (id != updatedRole.Id) return BadRequest("ID mismatch");

            var updated = _service.UpdateRole(updatedRole);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteRole(int id)
        {
            var success = _service.DeleteRole(id);
            return success ? Ok("deleted successfully") : NotFound();
        }
    }
}
