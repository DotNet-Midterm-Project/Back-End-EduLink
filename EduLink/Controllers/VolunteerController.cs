using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using EduLink.Repositories.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [Authorize(Roles = "volunteer")]
        [HttpGet("volunteer-courses")]
        public async Task<IActionResult> GetVolunteeringCourses( [FromQuery] int volunteerID)
        {
        

            var result = await _volunteer.GetVolunteerCoursesAsync(volunteerID);

            if (result == null || result.Count == 0)
            {
                return NotFound("No courses found for the specified volunteer.");
            }

            return Ok(result);
        }

        [Authorize(Roles = "volunteer")] 
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

        [HttpGet("get-event-contents/{volunteerID}/{eventID}")]
        public async Task<IActionResult> GetEventContentsAsync( int volunteerID,  int eventID)
        {
            try
            {
                var result = await _volunteer.GetEventContentsAsync(volunteerID, eventID);
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

        [HttpPost("add-article")]
        public async Task<IActionResult> AddArticle([FromHeader] string Authorization, [FromBody] ArticleReqDTO dto)
        {
            // Validate the authorization token
            if (Authorization != "tech_token")
            {
                return Unauthorized();
            }

            if (dto == null)
            {
                return BadRequest("Invalid article data.");
            }

            var response = await _volunteer.AddArticleAsync(dto);

            return Ok(response);
        }

        [HttpDelete("delete-article")]
        public async Task<IActionResult> DeleteArticle([FromHeader] string Authorization, [FromBody] DeleteArticleReqDTO request)
        {
            // Validate the authorization token
            if (Authorization != "tech_token")
            {
                return Unauthorized();
            }

            if (request == null || request.VolunteerID <= 0 || request.ArticleID <= 0)
            {
                return BadRequest("Invalid request data.");
            }

            var response = await _volunteer.DeleteArticleAsync(request.VolunteerID, request.ArticleID);

            return Ok(response);
        }

        [HttpGet("get-all-reservation")]
        //[Authorize]
        public async Task<IActionResult> GetAllReservations(//[FromHeader(Name = "Authorization")] string authToken,
            [FromQuery] ReservationReqDTO reservationRequest)
        {
            //if(authToken != "tech_token")
            //{
            //    return Unauthorized("Invalid token.");
            //}
            if (reservationRequest == null)
            {
                return BadRequest();
            }

            var resResrvation = await _volunteer.GetAllReservationAsync(reservationRequest);
            return Ok(resResrvation);
        }

        [HttpDelete("delete-reservation")]
        //[Authorize]
        //public async Task<IActionResult> DeleteReservation(//[FromHeader(Name = "Authorization")] string authToken, 
        //    [FromBody] DeleteReservationReqDTO deleteReservationRequest)
        //{
        //    //if (authToken != "tech_token")
        //    //{
        //    //    return Unauthorized("Invalid token.");
        //    //}

        //    var response = await _volunteer.DeleteReservationAsync(deleteReservationRequest);
        //    return Ok(response);
        //}

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

        [HttpPost("add-workshop")]
        //[Authorize]
        public async Task<IActionResult> AddWorkshop(//[FromHeader(Name = "Authorization")] string authToken, 
            [FromBody] AddWorkshopReqDTO addWorkshopRequest)
        {
            //if (authToken != "tech_token")
            //{
            //    return Unauthorized("Invalid token.");
            //}

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _volunteer.AddWorkshopAsync(addWorkshopRequest);
            return Ok(response);
        }

        [HttpDelete("delete-workshop/{id}")]
        //[Authorize]
        public async Task<IActionResult> DeleteWorkshop(//[FromHeader(Name = "Authorization")] string authToken,
                      [FromRoute] int id, [FromBody] DeleteWorkshopReqDTO deleteWorkshopRequest)
        {
            if (id != deleteWorkshopRequest.WorkshopID)
            {
                return BadRequest(new { message = "Workshop ID in the route does not match the ID in the request body." });
            }

            var result = await _volunteer.DeleteWorkshopAsync(deleteWorkshopRequest);

            if (result.Message == "Workshop not found or does not belong to this volunteer.")
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        //[Authorize]
        [HttpGet("get-all-workshop/{volunteerID}")]
        //public async Task<IActionResult> GetAllWorkshops(//[FromHeader(Name = "Authorization")] string authToken,
        //    [FromRoute] int volunteerID)
        //{
        //    var workshops = await _volunteer.GetAllWorkshopsAsync(new GetAllWorkshopsReqDTO { VolunteerID = volunteerID });

        //    if (workshops == null || !workshops.Any())
        //    {
        //        return NotFound(new { message = "No workshops found for this volunteer." });
        //    }

        //    return Ok(new { Workshop = workshops });
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

        [HttpGet("get-article/")]
        [Authorize]
        public async Task<IActionResult> GetArticle( [FromBody] GetArticleReqDTO request)
        {
            var article = await _volunteer .GetArticleByIdAsync(request.VolunteerID, request.AricaleID);

            if (article == null)
            {
                return NotFound(new { message = "Article not found" });
            }

            return Ok(new { Article = article });
        }

        [HttpPost("add-reservation")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> AddReservation([FromBody] AddReservationReqDTO request)
        {
            var response = await _volunteer.AddReservationAsync(request);

            if (response.Message == "Student is not associated with the course.")
            {
                return BadRequest(new { message = response.Message });
            }

            return Ok(new { message = response.Message });
        }

    }
}
