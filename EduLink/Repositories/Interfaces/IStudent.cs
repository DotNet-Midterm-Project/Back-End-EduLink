using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using Microsoft.AspNetCore.Mvc;

namespace EduLink.Repositories.Interfaces
{
    public interface IStudent
    {


        Task<List<DepartmentCoursesResDTO>> GetCoursesByStudentDepartmentAsync(int StudentId, string? filterName = null);
        Task<List<VolunteerResDTO>> GetVolunteersForCourseAsync(int CourseId );

        Task<List<BookingForStudentResDTO>> GetBookingsForStudentAsync(int StudentId);
        Task<MessageResDTO> DeleteBookingAsynct(int StudentId, int BookingId);

        Task<MessageResDTO> AddFeedbackAsync(FeedbackReqDTO bookingDtoRequest);
        Task<string> BookWorkshopAsync(int studentId, int workshopID);
        Task<MessageResDTO> BookSessionAsync(int studentId, int sessionId);
        Task<MessageResDTO> RegisterVolunteerAsync(int studentId,VolunteerRegisterReqDTO registerDTO);
        Task<List<AnnouncementResDTO>> GetAnnouncementsAsync();

        Task<bool> IsStudentBookedForEventAsync(int studentId, int eventId);
        Task<(MemoryStream FileStream, string ContentType, string FileName)> DownloadEventFileAsync(int eventId, int studentId);

    }
}
