using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;

namespace EduLink.Repositories.Interfaces
{
    public interface IVolunteer
    {

        Task<List<VolunteerCourseResDTO>> GetVolunteerCoursesAsync(int volunteerID);
        Task<MessageResDTO> AddEventContentAsync(EventContetnReqDTO dto);
        Task<IEnumerable<EventContentResDTO>> GetEventContentsAsync( int eventID);
        Task<List<EventResDTO>> GetEventsAsync(int volunteerID, int courseID);
        Task<MessageResDTO> AddEventsAsync(int volunteerID,AddEventReqDTO request);
        Task<MessageResDTO> CancelEventAsync(int eventId, int volunteerID);
       Task<MessageResDTO> GenerateMeetingUrlAsync(int eventId);
        Task<MessageResDTO> AddSessionAsync(AddSessionReqDTO request);

        //Task<MessageResDTO> AddArticleAsync(ArticleReqDTO dto);
        //Task<MessageResDTO> DeleteArticleAsync(int volunteerId, int articleId);
        //Task<ArticleResDTO> GetArticleByIdAsync(int volunteerId, int articleId);
    

        //
     

    
        //Task<MessageResDTO> AddWorkshopAsync(AddWorkshopReqDTO addWorkshopRequest);
        //Task<MessageResDTO> DeleteWorkshopAsync(DeleteWorkshopReqDTO deleteWorkshopRequest);
       // Task<List<WorkshopResDTO>> GetAllWorkshopsAsync(GetAllWorkshopsReqDTO getAllWorkshopsRequest);
        //Task<List<GetWorkshopNotificationRespDTO>> GetWorkshopNotificationsAsync();
    }
}
