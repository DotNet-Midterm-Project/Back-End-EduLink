using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;

namespace EduLink.Repositories.Interfaces
{
    public interface IAdmin
    {
        Task<string> AddCourse(AddCourseReqDto addCourseReqDto);
        Task<string> AddCourseToDepartment(int DepartmentID, int CourseID);
        Task<string> AddDepartment(AddDepartmentReqDto departmentReqDto);
        Task<string> AddStudentToVolunteer(int StudentId);

        Task<List<Course>> GetAllCourses();
        Task<List<Volunteer>> GetAllVolunteers();
        Task<List<GetFeedbackVolunteerResDto>> GetFeedbacksVolunteer(int VolunteerId);

        Task<string> UpdateCourse(int CourseID, UpdateCourseReqDto updateCourseReqDto);

        Task<string> DeleteVolunteer(int VolunteerId);
        Task<string> DeleteArticle(int ArticleId);
        Task<string> DeleteCourse(int CourseId);



    }
}
