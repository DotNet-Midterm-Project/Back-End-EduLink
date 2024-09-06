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
        public async Task<IActionResult> GEtStudentIdAsync([FromQuery] string? Search = null)
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

                var result = await student.GetCoursesByStudentDepartmentAsync(studentID , Search);
                if (result == null || result.Count == 0)
                {
                    return NotFound("No courses found for the specified Department.");
                }

                return Ok(result);
            }

            return Unauthorized("Invalid token.");

        }
        [HttpPost("BookingWorkshop")]
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
                if (result == "Booked")
                {
                    return BadRequest("You are already booked for this WorkShop.");
                }

                return Ok(result);
            }
            return Unauthorized("Invalid token.");
        }
        [HttpPost("BookSession")]
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
                if(result == "Booked")
                {
                   return BadRequest("You are already booked for this session.");                   
                }
                if(result == "Session is fully booked.")
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            return Unauthorized("Invalid token.");
        }
    


        [HttpGet("GetCourseVolunteers/{CourseID}")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<List<DepartmentCoursesResDTO>>> GetCourseVolunteers([FromRoute] int CourseID  )
        {
            var Course = await student.GetCourseVolunteersAsync(CourseID );
            if (Course.Count == 0)
            {
                return NotFound("The Course Not Found");
            }
            return Ok(Course);
        }
  
        [HttpGet("GetBookingStudent")]
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
                    return NotFound("No Booking Found");
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
        [HttpDelete("Delete_Booking/{BookingID}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> DeleteBooking([FromRoute] int BookingID)
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

        [HttpPost("RegisterVolunteer")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> RegisterVolunteer([FromBody] VolunteerRegisterReqDTO registerDTO)
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

                
                var result = await student.RegisterVolunteerAsync(studentID, registerDTO);

                
                if (result.Message == "Student not found")
                {
                    return NotFound("Student not found.");
                }
                else if (result.Message == "You are already registered as a volunteer.")
                {
                    return BadRequest("You are already registered as a volunteer.");
                }
                else if (result.Message == "Your application is being considered to become a volunteer.")
                {
                    return Ok("Your application is being considered to become a volunteer.");
                }

           
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while registering as a volunteer.");
            }

            return Unauthorized("Invalid token.");
        }

        [HttpGet("announcements")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetAnnouncements()
        {
            var announcements = await student.GetAnnouncementsAsync();

            if (announcements == null || announcements.Count == 0)
            {
                return NotFound("No announcements found.");
            }

            return Ok(announcements);
        }


    }
}



