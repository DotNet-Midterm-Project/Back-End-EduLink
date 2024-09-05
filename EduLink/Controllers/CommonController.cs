using EduLink.Models.DTO.Request;
using EduLink.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduLink.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICommon _commonService;

        public CommonController(ICommon commonService)
        {
            _commonService = commonService;
        }

        [HttpGet("workshops")]
        public async Task<IActionResult> GetAllWorkshops()
        {
            var workshops = await _commonService.GetAllWorkshopsAsync();
            return Ok(workshops);
        }

        [HttpGet("articles")]
        public async Task<IActionResult> GetAllArticles()
        {
            var articles = await _commonService.GetAllArticlesAsync();
            return Ok(articles);
        }

        [HttpGet("articles/volunteer/{volunteerId}")]
        public async Task<IActionResult> GetArticlesByVolunteer(int volunteerId)
        {
            var articles = await _commonService.GetArticlesByVolunteerAsync(volunteerId);
            return Ok(articles);
        }

        [HttpGet("eventcontent/{eventId}")]
        public async Task<IActionResult> GetEventContent(int eventId)
        {
            var eventContent = await _commonService.GetEventContentAsync(eventId);
            return Ok(eventContent);
        }

        [HttpPost("volunteer/events")]
        public async Task<IActionResult> GetEventsForVolunteer([FromBody] GetEventsRequestDTO request)
        {
            var events = await _commonService.GetEventsForVolunteerAsync(request);
            return Ok(events);
        }

        [HttpPost("event/sessions")]
        public async Task<IActionResult> GetEventSessions([FromBody] GetEventSessionRequestDTO request)
        {
            var sessions = await _commonService.GetEventSessionsAsync(request);
            return Ok(sessions);
        }
    }

}
