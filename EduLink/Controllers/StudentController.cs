using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using EduLink.Repositories.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace EduLink.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudent student;

        public StudentController(IStudent student)
        {
            this.student = student;
        }
        [HttpGet("getAllCourse")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GEtStudentIdAsync()
        {
            if (User.Identity is ClaimsIdentity identity)
            {
                var StudentClaims = identity.FindFirst("StudentID");
                if (StudentClaims == null)
                {
                    return Unauthorized("Student ID not found in token.");
                }
                if (!int.TryParse(StudentClaims.Value, out var studentID))
                {
                    return BadRequest("Invalid Volunteer ID in token.");
                }
                var result = await student.GetCoursesByStudentDepartmentAsync(studentID);
                if (result == null || result.Count == 0)
                {
                    return NotFound("No courses found for the specified Department.");
                }

                return Ok(result);
            }

            return Unauthorized("Invalid token.");

        }
        [HttpGet("Post")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> BookingWorkshop( int workshopID)
        {
            if (User.Identity is ClaimsIdentity identity)
            {
                var StudentClaims = identity.FindFirst("StudentID");
                if (StudentClaims == null)
                {
                    return Unauthorized("Student ID not found in token.");
                }
                if (!int.TryParse(StudentClaims.Value, out var studentID))
                {
                    return BadRequest("Invalid Volunteer ID in token.");
                }
                var result = await student.BookingWorkshop(studentID , workshopID);
                if (result == null)
                {
                    return NotFound("No courses found for the specified Department.");
                }

                return Ok(result);
            }
            return Unauthorized("Invalid token.");
        }
        [HttpGet("getAllCourse")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> BookSession(int sessionId)
        {
            if (User.Identity is ClaimsIdentity identity)
            {
                var StudentClaims = identity.FindFirst("StudentID");
                if (StudentClaims == null)
                {
                    return Unauthorized("Student ID not found in token.");
                }
                if (!int.TryParse(StudentClaims.Value, out var studentID))
                {
                    return BadRequest("Invalid Student ID in token.");
                }
                var result = await student.BookSession(studentID , sessionId);
                if (result == null )
                {
                    return NotFound("No Session found .");
                }

                return Ok(result);
            }
            return Unauthorized("Invalid token.");
        }
    


        [HttpGet("GetCourseVolunteers/{CourseID}")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<List<DepartmentCoursesResDTO>>> GetCourseVolunteerAsync([FromBody] int CourseID)
        {
            var Course = await student.GetCourseVolunteerAsync(CourseID);
            if (Course == null)
            {
                return NotFound("The Course Not Found");
            }
            return Ok(Course);
        }
  
        [HttpGet("/Getbooking")]
        [Authorize(Roles = "Student")]

        public async Task<IActionResult> GetBookingStudent()
        {
            if (User.Identity is ClaimsIdentity identity)
            {
                var StudentClaims = identity.FindFirst("StudentID");
                if (StudentClaims == null)
                {
                    return Unauthorized("Student ID not found in token.");
                }
                if (!int.TryParse(StudentClaims.Value, out var studentID))
                {
                    return BadRequest("Invalid Volunteer ID in token.");
                }
                var result = await student.GetBookingForStudentAsync(studentID);
                if (result == null || result.Count == 0)
                {
                    return NotFound("No courses found for the specified Department.");
                }

                return Ok(result);
            }
          return Unauthorized("Invalid token.");
        }

          [HttpPost("addFeedBack")]
         [Authorize(Roles = "Student")]
        public async Task<ActionResult<string>> AddFeedBack( FeedbackReqDTO feedback) {
        
            var Feedback = await student.AddFeedbackAsync(feedback);
            if (Feedback == null)
                return NotFound("Booking not found.");
            
            return Ok(Feedback);
                
        }
        [HttpDelete]
       // [Authorize(Roles = "Student")]
        public async Task<IActionResult> DeleteBooking([FromBody] int BookingID)
        {
            if (User.Identity is ClaimsIdentity identity)
            {
                var studentClaim = identity.FindFirst("StudentID");
                if (studentClaim == null)
                {
                    return Unauthorized("Student ID not found in token.");
                }

                if (!int.TryParse(studentClaim.Value, out var studentID))
                {
                    return BadRequest("Invalid Student ID in token.");
                }

                var result = await student.DeleteBookingAsynct(studentID, BookingID);
                if (result == null)
                {
                    return NotFound("Booking not found or could not be deleted.");
                }

                return Ok("Booking deleted successfully.");
            }

            return Unauthorized("Invalid token.");
        }
   

        }
}



