using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        // GET: /Common/getAll-Workshop
        // Tested
        [HttpGet("get-all-Workshops")]
        public async Task<IActionResult> GetAllWorkshops()
        {
            try
            {
                var workshops = await _commonService.GetAllWorkshopsAsync();
                var response = new WorkshopsResDTO
                {
                    Workshops = workshops
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: /Common/get-articles
        // Tested
        [HttpGet("get-all-articles")]
        public async Task<IActionResult> GetAllArticles()
        {
            var articlesResponse = await _commonService.GetAllArticlesAsync();
            if (articlesResponse.Articles == null || !articlesResponse.Articles.Any())
            {
                return NotFound(new { message = "No articles found" });
            }

            return Ok(articlesResponse);
        }

        // GET: /Common/get-articles/2
        // Tested
        [HttpGet("get-articles-by-volunteerId/{volunteerId}")]
        public async Task<IActionResult> GetArticlesByVolunteerId(int volunteerId)
        {
            var articlesResponse = await _commonService.GetArticlesByVolunteerIdAsync(volunteerId);
            if (articlesResponse.Articles == null || !articlesResponse.Articles.Any())
            {
                return NotFound(new { message = "No articles found for this volunteer" });
            }

            return Ok(articlesResponse);
        }

        // GET: /Common/get-Event-Content?eventId=1
        // Tested
        [HttpGet("get-event-content")]
        public async Task<IActionResult> GetEventContent([FromQuery] int eventId)
        {
            var eventContentResponse = await _commonService.GetEventContentByEventIdAsync(eventId);
            if (eventContentResponse == null || !eventContentResponse.Any())
            {
                return NotFound(new { message = "No content found for the specified event" });
            }

            return Ok(eventContentResponse);
        }

        // GET: /Common/get-Events?volunteerId=2&courseId=1
        // Tested
        [HttpGet("get-events-by-volunteer-and-course")]
        public async Task<IActionResult> GetEvents([FromQuery] int volunteerId, [FromQuery] int courseId)
        {
            var eventResponse = await _commonService.GetEventsByVolunteerAndCourseAsync(volunteerId, courseId);
            if (eventResponse == null || !eventResponse.Any())
            {
                return NotFound(new { message = "No events found for the specified volunteer and course." });
            }

            return Ok(eventResponse);
        }

        // GET: /Common/get-event-session?eventId=1
        // Tested
        [HttpGet("get-event-sessions")]
        public async Task<IActionResult> GetEventSessions([FromQuery] int eventId)
        {
            var sessionResponse = await _commonService.GetSessionsByEventAsync(eventId);
            if (sessionResponse == null || !sessionResponse.Any())
            {
                return NotFound(new { message = "No sessions found for the specified event." });
            }

            return Ok(sessionResponse);
        }
    }
}


