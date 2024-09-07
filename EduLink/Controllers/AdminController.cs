using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Models;
using EduLink.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduLink.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdmin admin;

        public AdminController(IAdmin admin)
        {
            this.admin = admin;
        }
        [HttpPost("AddCourse")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCourse([FromBody] AddCourseReqDto addCourseReqDto)
        {
            var AddCourse = await admin.AddCourse(addCourseReqDto);
            if (addCourseReqDto == null)
            {
                return NotFound();
            }
            return Ok(AddCourse);
        }
        [HttpPost("AddCourseToDepartment/{DepartmentID}/{CourseID}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCourseToDepartment([FromRoute] int DepartmentID, [FromRoute] int CourseID)
        {

            var AddCourseToDepartment = await admin.AddCourseToDepartment(DepartmentID, CourseID);
            if (AddCourseToDepartment == null)
                return NotFound("DepartmentNotfound");


            return Ok(AddCourseToDepartment);
        }


        [HttpPost("AddNewDepartment")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDepartment([FromBody] AddDepartmentReqDto departmentReqDto)
        {
            var NewDepartment = await admin.AddDepartment(departmentReqDto);
            if (NewDepartment == null)
            {
                return NotFound();
            }
            return Ok(NewDepartment);
        }



        [HttpPost("StudentBeComeVolunteer/{VolunteerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddStudentToVolunteer([FromRoute] int VolunteerId)
        {
            var AddStudentToVolunteer = await admin.AddStudentToVolunteer(VolunteerId);
            if (AddStudentToVolunteer == null)
            {
                return NotFound();
            }
            return Ok(AddStudentToVolunteer);

        }
        [HttpDelete("DeleteArticle/{ArticleId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteArticle(int ArticleId)
        {
            var DeleteArticle = await admin.DeleteArticle(ArticleId);
            if (DeleteArticle == null) return NotFound();
            return Ok(DeleteArticle);
        }
        [HttpDelete("DeleteCourse/{CourseId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCourse(int CourseId)
        {
            var DeleteCourse = await admin.DeleteCourse(CourseId);
            if (DeleteCourse == null)
                return NotFound();
            return Ok(DeleteCourse);
        }
        [HttpDelete("DeleteVolunteer/{VolunteerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVolunteer(int VolunteerId)
        {
            var DeleteVolunteer = await admin.DeleteVolunteer(VolunteerId);
            if (DeleteVolunteer == null)
                return NotFound();
            return Ok(DeleteVolunteer);
        }
        [HttpGet("GetAllCourse")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCourses()
        {
            var GetCourses = await admin.GetAllCourses();
            if (GetAllCourses == null) return null;
            return Ok(GetCourses);
        }
        [HttpGet("GetAllVolunteer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllVolunteers()
        {
            var GetAllVolunteers = await admin.GetAllVolunteers();
            if (GetAllVolunteers == null) return NotFound();
            return Ok(GetAllVolunteers);
        }
        [HttpPut("UpdateCourse/{CourseID}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCourse([FromRoute] int CourseID, [FromBody] UpdateCourseReqDto updateCourseReqDto)
        {
            var UpdateCourse = await admin.UpdateCourse(CourseID,
                updateCourseReqDto);
            if (UpdateCourse == null) return NotFound();
            return Ok(UpdateCourse);
        }
        [HttpGet("GetFeedbackFromVolunteer/{VolunteerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetFeedbacskVolunteer([FromRoute] int VolunteerId)
        {
            var GetFeedbackVolunteer = await admin.GetFeedbacksVolunteer(VolunteerId);
            if (GetFeedbackVolunteer == null) return NotFound();
            return Ok(GetFeedbackVolunteer);
        }

    }

}