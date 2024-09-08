using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using System.Drawing.Printing;

namespace EduLink.Repositories.Interfaces
{
    public interface IAdmin
    {
        Task<string> AddCourse(AddCourseReqDto addCourseReqDto);
        Task<string> AddCourseToDepartment(int DepartmentID, int CourseID);
        Task<string> AddDepartmentAsync(AddDepartmentReqDto departmentReqDto);
        Task<string> AddStudentToVolunteer(int VolunteerId);

        Task<List<CourseResDTO>> GetAllCoursesAsync(string? filterName, int pageNumber, int pageSize);
        Task<List<VolunteerResDTO>> GetAllVolunteersAsync(string? filterName,int pageNumber,int pageSize, bool? sortByRating, bool? GetBeComeVolunteerRequest);
        Task<List<GetFeedbackVolunteerResDto>> GetFeedbacksVolunteer(int VolunteerId);

        Task<string> UpdateCourse(int CourseID, UpdateCourseReqDto updateCourseReqDto);

        Task<string> DeleteVolunteer(int VolunteerId);
        Task<string> DeleteArticleAsync(int ArticleId);
        Task<string> DeleteCourse(int CourseId);
        Task<List<DepartmentResDto>> GetAllDepartmentsAsync(string? SearchName , int PageNumber , int PageSize );



    }
}
