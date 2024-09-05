using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;

namespace EduLink.Repositories.Interfaces
{
    public interface IStudent
    {


        Task<List<DepartmentCoursesResDTO>> GetCoursesByStudentDepartmentAsync(int StudentId);
        Task<List<VolunteerResDTO>> GetCourseVolunteersAsync(int CourseId);

        Task<List<BookingForStudentResDTO>> GetBookingForStudentAsync(int StudentId);
        Task<string> DeleteBookingAsynct(int StudentId, int BookingId);

        Task<string> AddFeedbackAsync(FeedbackReqDTO bookingDtoRequest);
        Task<string> BookingWorkshop(int studentId, int workshopID);
        Task<string> BookSession(int studentId, int sessionId);
        Task<MessageResDTO> RegisterVolunteerAsync(int studentId,VolunteerRegisterReqDTO registerDTO);
        Task<List<AnnouncementResDTO>> GetAnnouncementsAsync();

    }
}
