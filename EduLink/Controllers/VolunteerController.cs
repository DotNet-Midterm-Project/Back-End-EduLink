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

      
        [HttpGet("get-volunteering-courses")]
        public async Task<IActionResult> GetVolunteeringCourses([FromHeader] string Authorization, [FromQuery] int volunteerID)
        {
            // Validate the authorization token
            if (Authorization != "tech_token")
            {
                return Unauthorized();
            }

            var result = await _volunteer.GetVolunteerCoursesAsync(volunteerID);

            if (result == null || result.Count == 0)
            {
                return NotFound("No courses found for the specified volunteer.");
            }

            return Ok(result);
        }

    
        [HttpPost("add-educational-content")]
        public async Task<IActionResult> AddEducationalContent([FromHeader] string Authorization, [FromBody] EducationalContentDTO dto)
        {
            // Validate the authorization token
            if (Authorization != "tech_token")
            {
                return Unauthorized();
            }

            if (dto == null)
            {
                return BadRequest("Invalid content data.");
            }

            var response = await _volunteer.AddEducationalContentAsync(dto);

            return Ok(response);
        }

    
        [HttpGet("get-educational-content-course")]
        public async Task<IActionResult> GetEducationalContentForEachCourse([FromHeader] string Authorization, [FromQuery] int volunteerID, [FromQuery] int courseID)
        {
            // Validate the authorization token
            if (Authorization != "tech_token")
            {
                return Unauthorized();
            }

            var response = await _volunteer.GetEducationalContentForEachCourseAsync(volunteerID, courseID);

            if (response == null || response.EducationalContents == null || response.EducationalContents.Count == 0)
            {
                return NotFound("No educational content found for the specified course.");
            }

            return Ok(response);
        }

        [HttpPost("add-article")]
        public async Task<IActionResult> AddArticle([FromHeader] string Authorization, [FromBody] ArticleDTO dto)
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
        public async Task<IActionResult> DeleteArticle([FromHeader] string Authorization, [FromBody] DeleteArticleRequest request)
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
        public async Task<IActionResult> DeleteReservation(//[FromHeader(Name = "Authorization")] string authToken, 
            [FromBody] DeleteReservationDTO deleteReservationRequest)
        {
            //if (authToken != "tech_token")
            //{
            //    return Unauthorized("Invalid token.");
            //}

            var response = await _volunteer.DeleteReservationAsync(deleteReservationRequest);
            return Ok(response);
        }

        [HttpPut("update-reservation")]
        //[Authorize]
        public async Task<IActionResult> UpdateReservation(//[FromHeader(Name = "Authorization")] string authToken,
            [FromBody] UpdateReservationReqDTO updateReservationRequest)
        {
            //if (authToken != "tech_token")
            //{
            //    return Unauthorized("Invalid token.");
            //}

            var response = await _volunteer.UpdateReservationAsync(updateReservationRequest);
            return Ok(response);
        }

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

        [HttpGet("get-all-reservation")]
        //[Authorize]
        public async Task<IActionResult> GetAllReservations(//[FromHeader(Name = "Authorization")] string authToken,
            [FromQuery] ReservationReqDTO reservationRequest)
        {
            //if(authToken != "tech_token")
            //{
            //    return Unauthorized("Invalid token.");
            //}

            var resResrvation = await _volunteer.GetAllReservationAsync(reservationRequest);
            return Ok(resResrvation);
        }
    }
}
