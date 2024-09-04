using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;

namespace EduLink.Repositories.Interfaces
{
    public interface IStudent
    {


        Task<List<DepartmentCoursesResDTO>> GetCoursesByStudentDepartmentAsync(string StudentId);
        Task<List<VolunteerResDTO>> GetCourseVolunteerAsync(int CourseId);

        Task<MessageResDTO> RegisterVolunteerAsync(VolunteerRegisterReqDTO registerDTO);
        Task<List<EventContentResDTO>> GeteducationalcontentAsync(int VolunteerId, int courseId);
        Task<List<ReservationResDTO>> GetReservationForVolunteerAsync(int VolunteerId, int courseId);
        Task<String> AddBookingAsync(int ReservationId , string studentId);
        Task<List<BookingForStudentResDTO>> GetBookingAsync(string StudentId, int ReservationId);
        Task<string> AddFeedbackAsync(FeedbackReqDTO bookingDtoRequest);
        Task<string> WorkshopsRegistrationAsync(string StudentId, int WorkshopID);
        Task<List<NotificationBookingResDTO>> GetNotificationsByStudentAsync(string studentId);
    }
}
