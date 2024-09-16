using EduLink.Models.DTO.Request;
using EduLink.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace EduLink.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerController : ControllerBase
    {
        private readonly IVolunteer _volunteer;

        public VolunteerController(IVolunteer volunteer)
        {
            _volunteer = volunteer;
        }

        // POST: /Volunteer/add-Event
        // Tested
        [HttpPost("add-event")]
        [Authorize(Roles = "Volunteer")]
        public async Task<IActionResult> AddEvent([FromBody] AddEventReqDTO request)
        {
            var volunteerIDClaim = User.Claims.FirstOrDefault(c => c.Type == "VolunteerID");
            if (volunteerIDClaim == null)
            {
                return Unauthorized(new { message = "Volunteer ID not found in token." });
            }

            if (!int.TryParse(volunteerIDClaim.Value, out int volunteerID))
            {
                return BadRequest(new { message = "Invalid Volunteer ID." });
            }

            var response = await _volunteer.AddEventsAsync(volunteerID, request);


            if (response.Message == "Volunteer is not associated with the course.")
            {
                return BadRequest(new { message = response.Message });
            }

            return Ok(new { message = response.Message });
        }

        // POST: /Volunteer/add-event-content
        // Tested
        [Authorize(Roles = "Volunteer")]
        [HttpPost("add-event-content")]
        public async Task<IActionResult> AddEventContent([FromBody] EventContetnReqDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid content data.");
            }

            var response = await _volunteer.AddEventContentAsync(dto);

            return Ok(response);
        }

        // POST: /Volunteer/add-session
        // Tested
        [HttpPost("add-session")]
        [Authorize(Roles = "Volunteer")]
        public async Task<IActionResult> AddSession([FromBody] AddSessionReqDTO request)
        {
            var response = await _volunteer.AddSessionAsync(request);

            if (response.Message.Contains("The session was added successfully."))
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        // GET: Volunteer/Generate-URL-meeting/1
        //  Tested
        [HttpPost("generate-event-url/{eventId}")]
        [Authorize(Roles = "Volunteer")]
        public async Task<IActionResult> GenerateEventUrl(int eventId)
        {
            var volunteerIDClaim = User.Claims.FirstOrDefault(c => c.Type == "VolunteerID");
            if (volunteerIDClaim == null)
            {
                return Unauthorized(new { message = "Volunteer ID not found in token." });
            }

            if (!int.TryParse(volunteerIDClaim.Value, out int volunteerID))
            {
                return BadRequest(new { message = "Invalid Volunteer ID." });
            }

            var response = await _volunteer.GenerateMeetingUrlAsync(eventId);

            if (response.Message == $"Failed to create the URL.")
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }

        // Post: Volunteer/Generate-URL-/1
        //  Tested
        [HttpPost("generate-session-url/{SessionId}")]
        [Authorize(Roles = "Volunteer")]
        public async Task<IActionResult> GeneratesessionUrl(int SessionId)
        {
            var volunteerIDClaim = User.Claims.FirstOrDefault(c => c.Type == "VolunteerID");
            if (volunteerIDClaim == null)
            {
                return Unauthorized(new { message = "Volunteer ID not found in token." });
            }

            if (!int.TryParse(volunteerIDClaim.Value, out int volunteerID))
            {
                return BadRequest(new { message = "Invalid Volunteer ID." });
            }

            var response = await _volunteer.GenerateSessionUrlAsync(SessionId);

            if (response.Message == $"Failed to create the URL.")
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }

        // POST /Volunteer/add-article
        // Tested
        [HttpPost("add-article")]
        [Authorize(Roles = "Volunteer")]
        public async Task<IActionResult> AddArticle([FromBody] AddArticleReqDTO request)
        {

            var volunteerIDClaim = User.Claims.FirstOrDefault(c => c.Type == "VolunteerID");
            if (volunteerIDClaim == null)
            {
                return Unauthorized(new { message = "Volunteer ID not found in token." });
            }

            if (!int.TryParse(volunteerIDClaim.Value, out int volunteerID))
            {
                return BadRequest(new { message = "Invalid Volunteer ID." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _volunteer.AddArticleAsync(request, volunteerID);

            return Ok(response);
        }



        // GET: /Volunteer/volunteer-courses
        // Tested
        [Authorize(Roles = "Volunteer")]
        [HttpGet("volunteer-courses")]
        public async Task<IActionResult> GetVolunteerCourses()
        {
            if (User.Identity is ClaimsIdentity identity)
            {
                // Extract the Volunteer ID claim from the JWT token
                var volunteerIdClaim = identity.FindFirst("VolunteerID");
                if (volunteerIdClaim == null)
                {
                    return Unauthorized("Volunteer ID not found in token.");
                }

                if (!int.TryParse(volunteerIdClaim.Value, out var volunteerID))
                {
                    return BadRequest("Invalid Volunteer ID in token.");
                }

                // Fetch volunteer courses
                var result = await _volunteer.GetVolunteerCoursesAsync(volunteerID);

                if (result == null || result.Count == 0)
                {
                    return NotFound("No courses found for the specified volunteer.");
                }

                return Ok(result);
            }
            return Unauthorized("Invalid token.");
        }

        // GET: /get-all-Events/1
        // Tested
        [HttpGet("get-events/{courseID}")]
        [Authorize(Roles = "Volunteer")]
        public async Task<IActionResult> GetAllEvents(int courseID)
        {
            if (courseID <= 0)
            {
                return BadRequest("Invalid course ID.");
            }

            if (User.Identity is ClaimsIdentity identity)
            {
                var volunteerIdClaim = identity.FindFirst("VolunteerID");
                if (volunteerIdClaim == null)
                {
                    return Unauthorized("Volunteer ID not found in token.");
                }

                if (!int.TryParse(volunteerIdClaim.Value, out var volunteerID))
                {
                    return BadRequest("Invalid Volunteer ID in token.");
                }

                var events = await _volunteer.GetEventsAsync(volunteerID, courseID);

                if (events == null || !events.Any())
                {
                    return NotFound("No events found for this course and volunteer.");
                }

                return Ok(events);
            }

            return Unauthorized("Invalid token.");
        }

        // GET: /Volunteer/get-event-contents/1
        // Tested
        [Authorize(Roles = "Volunteer")]
        [HttpGet("get-event-content/{eventID}")]
        public async Task<IActionResult> GetEventContentsAsync(int eventID)
        {
            try
            {
                var result = await _volunteer.GetEventContentsAsync(eventID);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // GET: /Volunteer/get-articles
        // Tested
        [HttpGet("get-articles")]
        [Authorize]
        public async Task<IActionResult> GetArticles()
        {
            // Assuming that volunteerId can be obtained from the token or the request
            var volunteerIDClaim = User.Claims.FirstOrDefault(c => c.Type == "VolunteerID");
            if (volunteerIDClaim == null)
            {
                return Unauthorized(new { message = "Volunteer ID not found in token." });
            }

            if (!int.TryParse(volunteerIDClaim.Value, out int volunteerID))
            {
                return BadRequest(new { message = "Invalid Volunteer ID." });
            }

            var articles = await _volunteer.GetArticlesForVolunteerAsync(volunteerID);
            if (articles == null)
            {
                return NotFound(new { message = "No articles found for the volunteer." });
            }

            return Ok(articles);
        }

        // GET: /Volunteer/get-article/2
        // Tested
        [HttpGet("get-article/{articleId}")]
        [Authorize]
        public async Task<IActionResult> GetArticleByID(int articleId)
        {
            var volunteerIDClaim = User.Claims.FirstOrDefault(c => c.Type == "VolunteerID");
            if (volunteerIDClaim == null)
            {
                return Unauthorized(new { message = "Volunteer ID not found in token." });
            }

            if (!int.TryParse(volunteerIDClaim.Value, out int volunteerID))
            {
                return BadRequest(new { message = "Invalid Volunteer ID." });
            }

            var article = await _volunteer.GetArticleByIdAsync(articleId);

            if (article == null || article.VolunteerID != volunteerID)
            {
                return NotFound(new { message = "No article found for this volunteer." });
            }

            return Ok(article);
        }

        // PUT: /Volunteer/Update-Event
        // Tested
        [HttpPut("update-event")]
        [Authorize]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventReqDTO request)
        {
            var response = await _volunteer.UpdateEventAsync(request);
            return Ok(response);
        }

        // PUT: /Volunteer/cancel-event/1
        // Tested      
        [HttpPut("cancel-event/{eventId}")]
        [Authorize(Roles = "Volunteer")]
        public async Task<IActionResult> CancelEvent(int eventId)
        {

            var volunteerIDClaim = User.Claims.FirstOrDefault(c => c.Type == "VolunteerID");
            if (volunteerIDClaim == null)
            {
                return Unauthorized(new { message = "Volunteer ID not found in token." });
            }

            if (!int.TryParse(volunteerIDClaim.Value, out int volunteerID))
            {
                return BadRequest(new { message = "Invalid Volunteer ID." });
            }

            var response = await _volunteer.CancelEventAsync(eventId, volunteerID);

            if (response.Message == "The event was cancelled successfully.")
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        // PUT: /Volunteer/update-article
        // Tested
        [HttpPut("update-article")]
        [Authorize]
        public async Task<IActionResult> UpdateArticle([FromBody] UpdateArticleReqDTO request)
        {
            var volunteerIDClaim = User.Claims.FirstOrDefault(c => c.Type == "VolunteerID");
            if (volunteerIDClaim == null)
            {
                return Unauthorized(new { message = "Volunteer ID not found in token." });
            }

            if (!int.TryParse(volunteerIDClaim.Value, out int volunteerID))
            {
                return BadRequest(new { message = "Invalid Volunteer ID." });
            }

            if (request == null)
            {
                return BadRequest("Request cannot be null.");
            }

            if (request.ArticleID <= 0)
            {
                return BadRequest("Invalid Article ID.");
            }

            if (string.IsNullOrWhiteSpace(request.Title) || request.Title.Length > 200)
            {
                return BadRequest("Title is required and should not exceed 200 characters.");
            }

            if (string.IsNullOrWhiteSpace(request.ArticleContent) || request.ArticleContent.Length > 2000)
            {
                return BadRequest("Article content is required and should not exceed 2000 characters.");
            }

            try
            {
                var response = await _volunteer.UpdateArticleAsync(request, volunteerID);

                if (response == null)
                {
                    return NotFound("Article not found or update failed.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: /Volunteer/hidden-article
        // Tested
        [HttpPut("hidden-article")]
        [Authorize]
        public async Task<IActionResult> ModifyArticleStatus([FromBody] ModifyArticleStatusReqDTO request)
        {
            var volunteerIDClaim = User.Claims.FirstOrDefault(c => c.Type == "VolunteerID");
            if (volunteerIDClaim == null)
            {
                return Unauthorized(new { message = "Volunteer ID not found in token." });
            }

            if (!int.TryParse(volunteerIDClaim.Value, out int volunteerID))
            {
                return BadRequest(new { message = "Invalid Volunteer ID." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var message = await _volunteer.ModifyArticleStatusAsync(request, volunteerID);

            return Ok(new { message });
        }
    }
}
