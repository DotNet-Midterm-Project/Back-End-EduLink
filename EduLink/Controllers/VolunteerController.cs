using Azure.Core;
using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Repositories.Interfaces;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Drawing;
using System.Security.Claims;

namespace EduLink.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerController : ControllerBase
    {
        private readonly IVolunteer _volunteer;
     

        public VolunteerController(IVolunteer volunteer )
        {
            _volunteer = volunteer;
            
        }

        // POST: /Volunteer/add-Event
        // Tested
        [HttpPost("add-event")]
        [Authorize(Roles = "Volunteer")]
        public async Task<IActionResult> AddEvent([FromForm] AddEventReqDTO request)
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
            
            var response = await _volunteer.AddEventsAsync(volunteerID, request );


            if (response.Message == "Volunteer is not associated with the course.")
            {
                return BadRequest(new { message = response.Message });
            }
            if (response.Message == "File size should not exceed 1 MB.")
                return BadRequest(response.Message);

            return Ok(new { message = response.Message });
        }

        // POST: /Volunteer/add-event-content
        // Tested
        [Authorize(Roles = "Volunteer")]
        [HttpPost("add-event-content")]
        public async Task<IActionResult> AddEventContent([FromForm] EventContetnReqDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid content data.");
            }
            //if (dto.UploadFile?.Length > 1 * 2048 * 2048)
            //{
            //    return StatusCode(StatusCodes.Status400BadRequest, "File Size should not exceed 2 MB");

            //}
            //string[] allowedFileExtensions = new string[] { ".pdf", ".jpg", ".jpeg", ".png" };
            //var CreateFile = await _file.SaveFileAsync(dto?.UploadFile, allowedFileExtensions);


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

            if (response.Message.Contains("Successfully"))
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        // GET: Volunteer/Generate-URL-meeting/1
        // Not Tested
        [HttpPost("generate-url/{eventId}")]
        [Authorize(Roles = "Volunteer")]
        public async Task<IActionResult> GenerateMeetingUrl(int eventId)
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

            if (response.Message == $"The URL was created successfully.")
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        // POST /Volunteer/add-article
        // Tested
        [HttpPost("add-article")]
        [Authorize(Roles = "Volunteer")]
        public async Task<IActionResult> AddArticle([FromForm] AddArticleReqDTO request  )
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


            var response = await _volunteer.AddArticleAsync(request, volunteerID );
            if (response.Message == "File size should not exceed 1 MB.")
                return BadRequest(response.Message);

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
        public async Task<IActionResult> UpdateEvent([FromForm] UpdateEventReqDTO request)
        {
            var Event = await _volunteer.GetEventByIdAsync(request.EventID);
            if(Event == null)
            {
                return NotFound("the Event NotFound");
            }

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
        public async Task<IActionResult> UpdateArticle([FromForm] UpdateArticleReqDTO request)
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

            if (request == null || request.ArticleID <= 0 || string.IsNullOrWhiteSpace(request.Title) || request.Title.Length > 200 ||
                string.IsNullOrWhiteSpace(request.ArticleContent) || request.ArticleContent.Length > 2000 || !Enum.IsDefined(typeof(ArticleStatus), request.Status))
            {
                return BadRequest("Invalid request data.");
            }


            var response = await _volunteer.UpdateArticleAsync(request, volunteerID );

            if (response == null)
            {
                return NotFound("Article not found or update failed.");
            }
            if (response.Message == "File size should not exceed 2 MB.")
                return BadRequest(response.Message);

            return Ok(response);
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
