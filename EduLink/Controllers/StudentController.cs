using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using EduLink.Repositories.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
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
        [HttpPost("book-workshop")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> BookWorkshop(int workshopID)
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
                var result = await student.BookWorkshopAsync(studentID, workshopID);
                if (result == null)
                {
                    return NotFound(new MessageResDTO { Message = "No Booking found" });
                }
                if (result == "Booked")
                {
                    return BadRequest("You are already booked for this WorkShop.");
                }
                if(result.Contains("fully booked")){
                    return BadRequest(new MessageResDTO { Message = "Session is fully booked." });
                }

                return Ok(result);
            }
            return Unauthorized("Invalid token.");
        }
        [HttpPost("book-session")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> BookSession(int sessionId)
        {
            if (User.Identity is ClaimsIdentity identity)
            {
                var studentClaims = identity.FindFirst("StudentID");
                if (studentClaims == null)
                {
                    return Unauthorized(new MessageResDTO { Message = "Student ID not found in token." });
                }

                if (!int.TryParse(studentClaims.Value, out var studentID))
                {
                    return BadRequest(new MessageResDTO { Message = "Invalid Student ID in token." });
                }

                var result = await student.BookSessionAsync(studentID, sessionId);
                if (result == null)
                {
                    return NotFound(new MessageResDTO { Message = "No session found." });
                }

                if (result.Message == "Booked")
                {
                    return BadRequest(new MessageResDTO { Message = "You are already booked for this WorkShop." });
                }

                if (result.Message.Contains("is fully booked"))
                {
                    return BadRequest(new MessageResDTO { Message = "Session is fully booked." }); 
                }

                return Ok(result);
            }
            return Unauthorized(new MessageResDTO { Message = "Invalid token." });
        }
        [HttpPost("add-feedBack")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<string>> AddFeedBack(FeedbackReqDTO feedback)
        {
            if (feedback.BookingId <= 0 || feedback.Rating <= 0 || feedback.Comment.IsNullOrEmpty())
            {
                return BadRequest("Invalid Input. Please check the input data.");
            }

            var result = await student.AddFeedbackAsync(feedback);
            if (result.Message == "not found") {
                return NotFound("the Booking Not Found");


            }

            if (result == null)
            {
                return NotFound("Can't add FeedBack For this Session");
            }

            if (result.Message.Contains("not Completed"))
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }
        [HttpPost("register-as-volunteer")]
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
        [HttpGet("get-all-courses")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetCoursesByStudentDepartment([FromQuery] string? Search = null)
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

                var result = await student.GetCoursesByStudentDepartmentAsync(studentID, Search);
                if (result == null || result.Count == 0)
                {
                    return NotFound("No courses found for the specified Department.");
                }

                return Ok(result);
            }

            return Unauthorized("Invalid token.");

        }



        [HttpGet("get-volunteers-for-course/{CourseID}")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<List<DepartmentCoursesResDTO>>> GetVolunteersForCourse([FromRoute] int CourseID)
        {
            var Volunteers = await student.GetVolunteersForCourseAsync(CourseID);
            if (Volunteers.Count == 0)
            {
                return NotFound("Course Not Found");
            }
            return Ok(Volunteers);
        }

        [HttpGet("get-bookings")]
        [Authorize(Roles = "Student")]

        public async Task<IActionResult> GetBookingsForStudent()
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
                var result = await student.GetBookingsForStudentAsync(studentID);
                if (result == null || result.Count == 0)
                {
                    return NotFound("No Booking Found");
                }

                return Ok(result);
            }
            return Unauthorized("Invalid token.");
        }
        [HttpGet("get-all-announcements")]
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

        //Download
        [HttpGet("download/{eventId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> DownloadEventFile(int eventId)
        {
            // Assume studentId is extracted from claims or passed in some other way
            var studentId = GetStudentIdFromClaims();


            try
            {
                var (fileStream, contentType, fileName) = await student.DownloadEventFileAsync(eventId, studentId);

                // Return the file for download
                return File(fileStream, contentType, fileName);
            }
            catch (UnauthorizedAccessException ex)
            {
                // Handle unauthorized access
                return StatusCode(403, ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                // Handle file not found
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle generic errors
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        private int GetStudentIdFromClaims()
        {
            // Replace this with the actual logic to get studentId from claims
            var studentIdClaim = User.FindFirst("StudentID");
            return int.Parse(studentIdClaim?.Value ?? "0");
        }


        [HttpDelete("delete-booking/{BookingID}")]
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

                return Ok(result.Message);
            }

            return Unauthorized("Invalid token.");
        }

        

      


    }
}



