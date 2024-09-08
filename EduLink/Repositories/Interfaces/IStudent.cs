using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;

namespace EduLink.Repositories.Interfaces
{
    public interface IStudent
    {


        Task<List<DepartmentCoursesResDTO>> GetCoursesByStudentDepartmentAsync(int StudentId, string? filterName = null);
        Task<List<VolunteerResDTO>> GetCourseVolunteersAsync(int CourseId );

        Task<List<BookingForStudentResDTO>> GetBookingForStudentAsync(int StudentId);
        Task<MessageResDTO> DeleteBookingAsynct(int StudentId, int BookingId);

        Task<MessageResDTO> AddFeedbackAsync(FeedbackReqDTO bookingDtoRequest);
        Task<string> BookWorkshopAsync(int studentId, int workshopID);
        Task<MessageResDTO> BookSessionAsync(int studentId, int sessionId);
        Task<MessageResDTO> RegisterVolunteerAsync(int studentId,VolunteerRegisterReqDTO registerDTO);
        Task<List<AnnouncementResDTO>> GetAnnouncementsAsync();

    }
}
