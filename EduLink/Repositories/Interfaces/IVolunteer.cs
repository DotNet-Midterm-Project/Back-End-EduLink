using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;

namespace EduLink.Repositories.Interfaces
{
    public interface IVolunteer
    {

        Task<List<VolunteerCourseResDTO>> GetVolunteerCoursesAsync(int volunteerID);
        Task<MessageResDTO> AddEventContentAsync(EventContetnReqDTO dto);
        Task<IEnumerable<EventContentResDTO>> GetEventContentsAsync(int volunteerID, int eventID);
        Task<MessageResDTO> AddArticleAsync(ArticleReqDTO dto);
        Task<MessageResDTO> DeleteArticleAsync(int volunteerId, int articleId);
        Task<ArticleResDTO> GetArticleByIdAsync(int volunteerId, int articleId);
        Task<MessageResDTO> AddReservationAsync(AddReservationReqDTO request);

        //
        Task<List<ReservationResDTO>> GetAllReservationAsync(ReservationReqDTO reservationRequest);
     //   Task<MessageResDTO> DeleteReservationAsync(DeleteReservationReqDTO deleteReservationRequest);
        Task<MessageResDTO> UpdateReservationAsync(UpdateReservationReqDTO updateReservationRequest);
        Task<MessageResDTO> AddWorkshopAsync(AddWorkshopReqDTO addWorkshopRequest);
        Task<MessageResDTO> DeleteWorkshopAsync(DeleteWorkshopReqDTO deleteWorkshopRequest);
       // Task<List<WorkshopResDTO>> GetAllWorkshopsAsync(GetAllWorkshopsReqDTO getAllWorkshopsRequest);
        Task<List<GetWorkshopNotificationRespDTO>> GetWorkshopNotificationsAsync();
    }
}
