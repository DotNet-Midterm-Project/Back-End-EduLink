using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
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
        public async Task<IActionResult> AddEducationalContent([FromHeader] string Authorization, [FromBody] EducationalContentDtoReq dto)
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
    }
}
