using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;

namespace EduLink.Repositories.Interfaces
{
    public interface IVolunteer
    {

        Task<List<VolunteerCourseResDTO>> GetVolunteerCoursesAsync(int volunteerID);
        Task<MessageResDTO> AddEventContentAsync(EventContetnReqDTO dto);
        Task<IEnumerable<EventContentResDTO>> GetEventContentsAsync(int eventID);
        Task<List<EventResDTO>> GetEventsAsync(int volunteerID, int courseID);
        Task<MessageResDTO> AddEventsAsync(int volunteerID,AddEventReqDTO request);
        Task<MessageResDTO> CancelEventAsync(int eventId, int volunteerID);
        Task<MessageResDTO> GenerateMeetingUrlAsync(int eventId);
        Task<MessageResDTO> GenerateSessionUrlAsync(int eventId);
        Task<MessageResDTO> AddSessionAsync(AddSessionReqDTO request);

        Task<MessageResDTO> AddArticleAsync(AddArticleReqDTO request, int volunteerId);
        Task<MessageResDTO> ModifyArticleStatusAsync(ModifyArticleStatusReqDTO request, int volunteerId);
        Task<ArticlesResDTO> GetArticlesForVolunteerAsync(int volunteerId);
        Task<ArticleDTO> GetArticleByIdAsync(int articleId);
        //Task<GetNotificationsResponse> GetNotificationsAsync(GetNotificationsRequest request);
        Task<EventResDTO> GetEventByIdAsync(int eventId);
        Task<MessageResDTO> UpdateArticleAsync(UpdateArticleReqDTO request, int volunteerId);
        Task<MessageResDTO> UpdateEventAsync(UpdateEventReqDTO request);
        Task<List<BookingForVolunteerResDTO>> GetVolunteerBookingsAsync(int volunteerID);

    }
}
