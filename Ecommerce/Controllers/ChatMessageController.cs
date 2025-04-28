using Ecommerce.BLL.Services;
using Ecommerce.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessageController : ControllerBase
    {
        private readonly ChatMessageService _service;

        public ChatMessageController(ChatMessageService service)
        {
            _service = service;
        }
        [HttpGet("messages")]
        public async Task<IActionResult> GetMessages(int studentId, int instructorId)
        {
            if (studentId <= 0 || instructorId <= 0)
            {
                return BadRequest("Invalid student or instructor id.");
            }

            var messages = await _service.GetMessagesAsync(studentId, instructorId);

            return Ok(messages);
        }
        [HttpPost("messages")]
        public async Task<IActionResult> AddMessage([FromBody] CreateChatMessageDTO messageDto)
        {
            if (messageDto == null)
            {
                return BadRequest("Invalid message data.");
            }

            bool result = await _service.AddMessageAsync(messageDto);

            if (result)
            {
                return Ok("Message sent successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while sending the message.");
            }
        }


    }
}
