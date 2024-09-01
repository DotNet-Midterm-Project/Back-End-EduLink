﻿using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        [HttpGet("getAllCourse/{StudentID}")]
        public async Task<ActionResult<List<DepartmentCoursesDtoResponse>>> GetAllCoursesByStudentDepartmentAsync([FromHeader] string Authorization, [FromRoute] string StudentID)
        {
            // Validate the authorization token
            if (Authorization != "Bearer JWT_token_here")
                return Unauthorized();
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var Course = await student.GetCoursesByStudentDepartmentAsync(StudentID);
            if (Course == null)
            {
                return NotFound();
            }
            return Ok(Course);
        }
        [HttpGet("GetCourseVolunteers/{CourseID}")]
        public async Task<ActionResult<List<DepartmentCoursesDtoResponse>>> GetAllCoursesByStudentDepartmentAsync([FromHeader] string Authorization, [FromRoute] int CourseID)
        {
            // Validate the authorization token
            if (Authorization != "Bearer JWT_token_here")
                return Unauthorized();
            var Course = await student.GetCourseVolunteerAsync(CourseID);
            if (Course == null)
            {
                return NotFound("The Courses Not Found");
            }
            return Ok(Course);
        }
        [HttpGet("GetEducationalContent/{VolunteerID}/{courseID}")]
        public async Task<ActionResult<List<EducationalContentDtoResponse>>> GetEducationalContent([FromHeader] string Authorization, [FromRoute] int VolunteerID, [FromRoute] int courseID)
        {
            // Validate the authorization token
            if (Authorization != "Bearer JWT_token_here")
                return Unauthorized();
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var EducationalContent = await student.GeteducationalcontentAsync(VolunteerID, courseID);
            if ((EducationalContent == null))
            {
                return NotFound();
            }

            return Ok(EducationalContent);
        }
        [HttpGet("GetReservations/{VolunteerID}/{courseID}")]
        public async Task<ActionResult<List<ReservationDtoResponse>>> GetReservations([FromHeader] string Authorization, [FromRoute] int VolunteerID, [FromRoute] int courseID)
        {
            if (Authorization != "Bearer JWT_token_here")
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var Reservation = await student.GetReservationForVolunteerAsync(VolunteerID, courseID);
            if (Reservation == null) {
                return NotFound();
            }
            return Ok(Reservation);

        }
        [HttpPost("AddBooking/{ReservationID}/{StudentID}")]
        public async Task<ActionResult<string>> AddBooking([FromHeader] string Authorization, [FromRoute] int ReservationID , [FromRoute] string StudentID)
        {
            if (Authorization != "Bearer JWT_token_here")
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var AddBooking = await student.AddBookingAsync(ReservationID, StudentID);
            if (AddBooking == "Reservation not found.") { 
               return NotFound(AddBooking);
            }
            if(AddBooking == "Reservation is no longer available.")
            {
                return BadRequest(AddBooking);
            }
            return Ok(AddBooking);

        }
        [HttpGet("/getbooking/{StudentID}/{ReservationID}")]
        public async Task<ActionResult<List<BookingForStudentDtoResponse>>> getbookingStudent([FromHeader] string Authorization,[FromRoute] string StudentID ,[FromRoute] int ReservationID)
        {
            if (Authorization != "Bearer JWT_token_here")
                return Unauthorized();
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var BookingStudent = await student.GetBookingAsync(StudentID, ReservationID);
            if (BookingStudent == null)
            {
                return NotFound();
            }
            return Ok(BookingStudent);

        }
        [HttpPost("addFeedBack")]
        public async Task<ActionResult<string>> AddFeedBack([FromHeader] string Authorization, FeedbackDtoRequest feedback) {
            if (Authorization != "Bearer JWT_token_here")
                return Unauthorized();
            var Feedback = await student.AddFeedbackAsync(feedback);
            if (Feedback == null)
                return NotFound("Booking not found.");
            
            return Ok(Feedback);
                
        }
       [HttpPost("WorkshopsRegistratio/{StudnetID}/{“WorkshopID}")]
       public async Task<ActionResult<String>> WorkshopsRegistratio([FromHeader] string Authorization,[FromRoute] string StudnetID , [FromRoute] int WorkshopID)
        {
            if (Authorization != "Bearer JWT_token_here")
                return Unauthorized();
            var workshopsRegistratio = await student.WorkshopsRegistrationAsync(StudnetID, WorkshopID);
            if(workshopsRegistratio ==  "Workshop not found." || workshopsRegistratio == "Workshop capacity reached.")
            {
                return BadRequest(workshopsRegistratio);
            }
            return Ok(workshopsRegistratio);
         }

    }
}