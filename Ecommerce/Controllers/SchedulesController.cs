using Ecommerce.BLL.Services;
using Ecommerce.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchedulesController : ControllerBase
    {
        private readonly ScheduleService _scheduleService;

        public SchedulesController(ScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetAllSchedules()
        {
            var schedules = await _scheduleService.GetAllSchedulesAsync();
            return schedules.Any() ? Ok(schedules) : NotFound("No Schedules Found.");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ScheduleDTO>> GetScheduleById(int id)
        {
            var schedule = await _scheduleService.GetScheduleByIdAsync(id);
            return schedule == null ? NotFound() : Ok(schedule);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddSchedules(List<CreateScheduleDTO> newSchedules)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _scheduleService.AddSchedulesAsync(newSchedules);
            return StatusCode(StatusCodes.Status201Created);

        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ScheduleDTO>> UpdateSchedule(int id, UpdateScheduleDTO dto)
        {
            var updated = await _scheduleService.UpdateScheduleAsync(id, dto);
            return updated == null ? NotFound("Schedule not found.") : Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var success = await _scheduleService.DeleteScheduleAsync(id);
            return success ? Ok("Deleted successfully") : NotFound("No Schedule found.");
        }

        [HttpGet("student/{id}/this-week")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetStudentScheduleForThisWeekAsync(int id)
        {
            var schedules = await _scheduleService.GetStudentScheduleForThisWeekAsync(id);
            return schedules.Any() ? Ok(schedules) : NotFound("No Schedules Found.");
        }
        [HttpGet("instructor/{id}/this-week")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetInstructorScheduleForThisWeekAsync(int id)
        {
            var schedules = await _scheduleService.GetInstructorScheduleForThisWeekAsync(id);
            return schedules.Any() ? Ok(schedules) : NotFound("No Schedules Found.");
        }
    }
}
