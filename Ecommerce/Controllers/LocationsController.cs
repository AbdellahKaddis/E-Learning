using Ecommerce.BLL.Services;
using Ecommerce.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly LocationService _service;

        public LocationsController(LocationService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<LocationDTO>>> GetAllLocations()
        {
            var locations = await _service.GetAllLocationsAsync();
            return locations.Any() ? Ok(locations) : NotFound("No Locations Found.");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocationDTO>> GetLocationById(int id)
        {
            var location = await _service.GetLocationByIdAsync(id);
            return location == null ? NotFound() : Ok(location);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LocationDTO>> AddLocation(CreateLocationDTO newLocation)
        {
            var created = await _service.AddLocationAsync(newLocation);
            return CreatedAtAction(nameof(GetLocationById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LocationDTO>> UpdateLocation(int id, UpdateLocationDTO updatedLocation)
        {
            if (id != updatedLocation.Id) return BadRequest("ID mismatch");

            var updated = await _service.UpdateLocationAsync(updatedLocation);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var success = await _service.DeleteLocationAsync(id);
            return success ? Ok("deleted successfully") : NotFound();
        }
    }
}