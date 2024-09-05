using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using EduLink.Repositories.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Principal;
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
        [Authorize(Roles = "Volunteer")]
        [HttpGet("get-event-contents/{eventID}")]
        public async Task<IActionResult> GetEventContentsAsync(   int eventID)
        {

            //var volunteerIdClaim = HttpContext.User.FindFirst("VolunteerID");


            //if (volunteerIdClaim == null)
            //{
            //    return Unauthorized("Volunteer ID not found in token.");
            //}


            //if (!int.TryParse(volunteerIdClaim.Value, out var volunteerID))
            //{
            //    return BadRequest("Invalid Volunteer ID in token.");
            //}


            try
            {
                var result = await _volunteer.GetEventContentsAsync( eventID);
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


        [Authorize(Roles = "volunteer")]
        [HttpGet("/get-all-Events/{coursID}")]
        public async Task<IActionResult> GetAllEvents(int coursID)
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

        // Fetch all events for the volunteer in the specified course
        var Events = await _volunteer.GetEventsAsync(volunteerID, coursID);
        return Ok(Events);
    }
    return Unauthorized("Invalid token.");
}

        [HttpPost("add-Event")]
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


        [HttpPut("cancel-event/{eventId}")]
        [Authorize(Roles = "Volunteer")]
        public async Task<IActionResult> CancelEvent( int eventId)
        {
          
            var volunteerIDClaim = User.Claims.FirstOrDefault(c => c.Type == "volunteerID");
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


        [HttpPost("Generate-URL-meeting/{eventId}")]
        [Authorize(Roles = "Volunteer")]
        public async Task<IActionResult> GenerateMeetingUrl(int eventId)
        {
            // Extract VolunteerID from the token
            var volunteerIDClaim = User.Claims.FirstOrDefault(c => c.Type == "volunteerID");
            if (volunteerIDClaim == null)
            {
                return Unauthorized(new { message = "Volunteer ID not found in token." });
            }

            if (!int.TryParse(volunteerIDClaim.Value, out int volunteerID))
            {
                return BadRequest(new { message = "Invalid Volunteer ID." });
            }

            // Call the service to generate the meeting URL
            var response = await _volunteer.GenerateMeetingUrlAsync(eventId);

            if (response.Message == "The URL was created successfully.")
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost("add-session")]
        [Authorize(Roles = "Volunteer")]
        public async Task<IActionResult> AddSession([FromBody] AddSessionReqDTO request)
        {
            var response = await _volunteer.AddSessionAsync(request);

            if (response.Message.Contains("successfully"))
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        //[HttpPut("update-reservation")]
        ////[Authorize]
        //public async Task<IActionResult> UpdateReservation(//[FromHeader(Name = "Authorization")] string authToken,
        //    [FromBody] UpdateReservationReqDTO updateReservationRequest)
        //{
        //    //if (authToken != "tech_token")
        //    //{
        //    //    return Unauthorized("Invalid token.");
        //    //}

        //    var response = await _volunteer.UpdateReservationAsync(updateReservationRequest);
        //    return Ok(response);
        //}

     

        //[HttpGet("get-notifications-workshops")]
        //public async Task<IActionResult> GetWorkshopNotifications(//[FromHeader(Name = "Authorization")] string authToken,
        //                                                          )
        //{
        //    var notifications = await _volunteer.GetWorkshopNotificationsAsync();

        //    if (notifications == null || notifications.Count == 0)
        //    {
        //        return NotFound(new { message = "No notifications found for this student." });
        //    }

        //    return Ok(new { notifications });
        //}


        //[HttpPost("add-article")]
        //public async Task<IActionResult> AddArticle([FromHeader] string Authorization, [FromBody] ArticleReqDTO dto)
        //{
        //    // Validate the authorization token
        //    if (Authorization != "tech_token")
        //    {
        //        return Unauthorized();
        //    }

        //    if (dto == null)
        //    {
        //        return BadRequest("Invalid article data.");
        //    }

        //    var response = await _volunteer.AddArticleAsync(dto);

        //    return Ok(response);
        //}

        //[HttpDelete("delete-article")]
        //public async Task<IActionResult> DeleteArticle([FromHeader] string Authorization, [FromBody] DeleteArticleReqDTO request)
        //{
        //    // Validate the authorization token
        //    if (Authorization != "tech_token")
        //    {
        //        return Unauthorized();
        //    }

        //    if (request == null || request.VolunteerID <= 0 || request.ArticleID <= 0)
        //    {
        //        return BadRequest("Invalid request data.");
        //    }

        //    var response = await _volunteer.DeleteArticleAsync(request.VolunteerID, request.ArticleID);

        //    return Ok(response);
        //}

        //[HttpGet("get-article/")]
        //[Authorize]
        //public async Task<IActionResult> GetArticle( [FromBody] GetArticleReqDTO request)
        //{
        //    var article = await _volunteer .GetArticleByIdAsync(request.VolunteerID, request.AricaleID);

        //    if (article == null)
        //    {
        //        return NotFound(new { message = "Article not found" });
        //    }

        //    return Ok(new { Article = article });
        //}

     

    }
}
