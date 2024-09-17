using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Models;
using EduLink.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;

namespace EduLink.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdmin admin;
       

        public AdminController(IAdmin admin )
        {
            this.admin = admin;
            
        }

        [HttpPost("add-course")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCourse([FromBody] AddCourseReqDto addCourseReqDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (addCourseReqDto == null)
            {
                return BadRequest("Course data is null.");
            }

            var result = await admin.AddCourse(addCourseReqDto);

            if (result == null)
            {
                return BadRequest("Invalid course details provided. Please provide both Course Name and Course Description.");
            }

            return Ok(result);
        }

        [HttpPost("add-course-to-department/{DepartmentID}/{CourseID}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCourseToDepartment([FromRoute] int DepartmentID, [FromRoute] int CourseID)
        {
            if (DepartmentID <= 0 || CourseID <= 0)
            {
                return BadRequest("Invalid department or course ID.");
            }

            var result = await admin.AddCourseToDepartment(DepartmentID, CourseID);
            if(result.Contains("already associated"))
            {
                return BadRequest(result);
            }
            if (result == null)
            {
                return NotFound("Department or course not found.");
            }

            return Ok(result);
        }

        [HttpPost("add-new-department")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDepartment([FromBody] AddDepartmentReqDto departmentReqDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(departmentReqDto.DepartmentName.IsNullOrEmpty() || departmentReqDto.Address.IsNullOrEmpty())
            {
                return BadRequest(new MessageResDTO { Message = "Invalid Department name or Department Address. Please check the input data. " });
            }
            var result = await admin.AddDepartmentAsync(departmentReqDto);

            if (result == null)
            {
                return BadRequest("Unable to add the department. Please check the input data.");
            }

            return Ok(result);
        }

        [HttpPost("student-to-become-volunteer/{VolunteerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddStudentToVolunteer([FromRoute] int VolunteerId)
        {
            if (VolunteerId <= 0)
            {
                return BadRequest("Invalid volunteer ID.");
            }

            var result = await admin.AddStudentToVolunteer(VolunteerId);

            if (result == null)
            {
                return NotFound("Volunteer not found.");
            }

            return Ok(result);
        }

        [HttpGet("get-all-courses")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCourses([FromQuery] string? SearchName = null , int PageNumber = 1, int PageSize = 100)
        {
            var result = await admin.GetAllCoursesAsync(SearchName ,  PageNumber ,PageSize );

            if (result == null || !result.Any())
            {
                return NotFound("No courses found.");
            }

            return Ok(result);
        }
        [HttpGet("get-all-departments")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDepartments([FromQuery] string? SearchName = null, int PageNumber = 1, int PageSize = 100)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await admin.GetAllDepartmentsAsync( SearchName , PageNumber , PageSize );
            if(result == null || !result.Any())
            {
                return NotFound("No Department Found");
            }
            return Ok(result);
        }
        [HttpGet("get-all-volunteers")]
        [Authorize(Roles = "Admin")]
        
        public async Task<IActionResult> GetAllVolunteers([FromQuery] string? filterName, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100, bool? sortByRating = false, bool? GetBeComeVolunteerRequest = false)
        {
            // Validate pageNumber and pageSize
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }

            var result = await admin.GetAllVolunteersAsync(filterName, pageNumber, pageSize , sortByRating, GetBeComeVolunteerRequest);

            if (result == null || !result.Any())
            {
                return NotFound("No volunteers found.");
            }

            return Ok(result);
        }

        [HttpGet("get-feedbacks-for-volunteer/{VolunteerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetFeedbacksVolunteer([FromRoute] int VolunteerId)
        {
            if (VolunteerId <= 0)
            {
                return BadRequest("Invalid volunteer ID.");
            }

            var result = await admin.GetFeedbacksVolunteer(VolunteerId);

            if (result == null || !result.Any())
            {
                return NotFound("No feedback found for this volunteer.");
            }

            return Ok(result);
        }

        [HttpPut("update-course/{CourseID}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCourse([FromRoute] int CourseID, [FromBody] UpdateCourseReqDto updateCourseReqDto)
        {
            if (CourseID <= 0)
            {
                return BadRequest("Invalid course ID.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await admin.UpdateCourse(CourseID, updateCourseReqDto);

            if (result == null)
            {
                return NotFound("Course not found.");
            }

            return Ok(result);
        }

        [HttpDelete("delete-article/{ArticleId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteArticle([FromRoute] int ArticleId)
        {
            if (ArticleId <= 0)
            {
                return BadRequest("Invalid article ID.");
            }

            var result = await admin.DeleteArticleAsync(ArticleId);

            if (result.Contains("not found."))
            {
                return NotFound(result);
            }


            return Ok(result);
        }

        [HttpDelete("delete-course/{CourseId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCourse([FromRoute] int CourseId)
        {
            if (CourseId <= 0)
            {
                return BadRequest("Invalid course ID.");
            }

            var result = await admin.DeleteCourse(CourseId);

            if (result == null)
            {
                return NotFound("Course not found.");
            }

            return Ok(result);
        }

        [HttpDelete("delete-volunteer/{VolunteerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVolunteer([FromRoute] int VolunteerId)
        {
            if (VolunteerId <= 0)
            {
                return BadRequest("Invalid volunteer ID.");
            }

            var result = await admin.DeleteVolunteer(VolunteerId);

            if (result == null)
            {
                return NotFound("Volunteer not found.");
            }

            return Ok(result);
        }
        
    }
}
