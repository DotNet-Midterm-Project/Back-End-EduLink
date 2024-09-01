using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;

namespace EduLink.Repositories.Interfaces
{
    public interface IAdmin
    {
        Task<IEnumerable<Admin>> GetAllCoursesAsync();
        Task<IEnumerable<Admin>> GetAllVolunteersAsync();

        Task <MessageResponseDTO> AddCourseAsync(Course course);
        Task <MessageResponseDTO> DeleteCourseAsync(int id);
    }
}
