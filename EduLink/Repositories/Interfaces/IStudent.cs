using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;

namespace EduLink.Repositories.Interfaces
{
    public interface IStudent
    {


        Task<List<DepartmentCoursesDtoResponse>> GetCoursesByStudentDepartmentAsync(string StudentId);
        Task<List<VolunteerDtoResponse>> GetCourseVolunteerAsync(int CourseId);

        Task<MessageResponseDTO> RegisterVolunteerAsync(VolunteerRegisterDtoReq registerDTO);
        Task<List<EducationalContentDtoResponse>> GeteducationalcontentAsync(int VolunteerId, int courseId);
        Task<List<ReservationDtoResponse>> GetReservationForVolunteerAsync(int VolunteerId, int courseId);
        Task<String> AddBookingAsync(int ReservationId , string studentId);
        Task<List<BookingForStudentDtoResponse>> GetBookingAsync(string StudentId, int ReservationId);
        Task<string> AddFeedbackAsync(FeedbackDtoRequest bookingDtoRequest);
        Task<string> WorkshopsRegistrationAsync(string StudentId, int WorkshopID);
        Task<List<NotificationBookingDtoResponse>> GetNotificationsByStudentAsync(string studentId);
    }
}
