
//using Ecommerce.BLL.Services;
//using Ecommerce.DAL.Db;
//using Ecommerce.DAL.Repositories;
//using Ecommerce.Models.DTOs;
//using Ecommerce.Models.Entities;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace Ecommerce.Api.Controllers

//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class HistorysController : Controller
//    {

//        private readonly HistoryService _service;

//        private readonly IConfiguration _configuration;
//        public HistorysController(AppDbContext context, IConfiguration configuration)
//        {
//            var repo = new HistoryRepository(context);
//            _service = new HistoryService(repo);

//            _configuration = configuration;
//        }

//        [HttpGet()]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public ActionResult<IEnumerable<History>> GetAllHistorys()
//        {
//            var Historys = _service.GetAllHistorys();
//            return Historys.Any() ? Ok(Historys) : NotFound("No History was found.");
//        }


//        [HttpGet("{id}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public ActionResult<History> GetHistoryId(int id)
//        {
//            var History = _service.GetHistoryId(id);
//            return History == null ? NotFound() : Ok();
//        }


 

//        [HttpPost("AddHistory")]
//        [ProducesResponseType(StatusCodes.Status201Created)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public ActionResult<History> AddHistory([FromBody] createHistoryDto dto)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            var HistoryAdded = _service.AddHistory(dto);
//            return CreatedAtAction(nameof(AddHistory), new { id = HistoryAdded.HistoryId }, new createHistoryDto
//            {

//                titre = HistoryAdded.titre,
//                URL = HistoryAdded.URL,
//                Duration = HistoryAdded.Duration,
//                CourseId = HistoryAdded.CourseId
//            });

//        }


//        [HttpDelete("DeleteHistory")]
//        [ProducesResponseType(StatusCodes.Status201Created)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public ActionResult<History> DeleteHistory(int id)
//        {
//            var HistoryDeleted = _service.DeleteHistory(id);
//            if (HistoryDeleted)
//            {
//                return Ok("History deleted successfully.");
//            }
//            else
//            {
//                return NotFound("History not found.");
//            }
//        }
//    }
//}
