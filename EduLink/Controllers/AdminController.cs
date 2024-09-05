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
        public async Task<string> AddCourse([FromBody] AddCourseReqDto addCourseReqDto)
        {
            var AddCourse = await admin.AddCourse(addCourseReqDto);
            return AddCourse;
        }
        [HttpPost("AddCourseToDepartment/{DepartmentID}/{CourseID}")]
        [Authorize(Roles = "Admin")]
        public async Task<string> AddCourseToDepartment([FromRoute] int DepartmentID, [FromRoute] int CourseID)
        {
            var AddCourseToDepartment = await admin.AddCourseToDepartment(DepartmentID, CourseID);
            return AddCourseToDepartment;
        }


        [HttpPost("AddNewDepartment")]
        [Authorize(Roles = "Admin")]
        public async Task<string> AddDepartment([FromBody] AddDepartmentReqDto departmentReqDto)
        {
            var NewDepartment = await admin.AddDepartment(departmentReqDto);
            return NewDepartment;
        }



        [HttpPost("StudentBeComeVolunteer/{StudentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<string> AddStudentToVolunteer([FromRoute] int StudentId)
        {
            var AddStudentToVolunteer = await admin.AddStudentToVolunteer(StudentId);
            return AddStudentToVolunteer;

        }
        [HttpDelete("DeleteArticle/{ArticleId}")]
        [Authorize(Roles = "Admin")]
        public async Task<string> DeleteArticle(int ArticleId)
        {
            var DeleteArticle = await admin.DeleteArticle(ArticleId);
            return DeleteArticle;
        }
        [HttpDelete("DeleteCourse/{CourseId}")]
        [Authorize(Roles = "Admin")]
        public async Task<string> DeleteCourse(int CourseId)
        {
            var DeleteCourse = await admin.DeleteCourse(CourseId);
            return DeleteCourse;
        }
        [HttpDelete("DeleteVolunteer/{VolunteerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<string> DeleteVolunteer(int VolunteerId)
        {
            var DeleteVolunteer = await admin.DeleteVolunteer(VolunteerId);
            return DeleteVolunteer;
        }
        [HttpGet("GetAllCourse")]
        [Authorize(Roles = "Admin")]
        public async Task<List<Course>> GetAllCourses()
        {
            var GetCourses = await admin.GetAllCourses();
            return GetCourses;
        }
        [HttpGet("GetAllVolunteer")]
        [Authorize(Roles = "Admin")]
        public async Task<List<Volunteer>> GetAllVolunteers()
        {
            var GetAllVolunteers = await admin.GetAllVolunteers();
            return GetAllVolunteers;
        }
        [HttpPut("UpdateCourse/{CourseID}")]
        [Authorize(Roles = "Admin")]
        public async Task<string> UpdateCourse([FromRoute] int CourseID, [FromBody] UpdateCourseReqDto updateCourseReqDto)
        {
            var UpdateCourse = await admin.UpdateCourse(CourseID,
                updateCourseReqDto);
            return UpdateCourse;
        }
        [HttpPut("GetFeedbackFromVolunteer/{VolunteerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<List<GetFeedbackVolunteerResDto>> GetFeedbacskVolunteer([FromRoute] int VolunteerId)
        {
            var GetFeedbackVolunteer = await admin.GetFeedbacksVolunteer(VolunteerId);
            return GetFeedbackVolunteer;
        }

    }

}
