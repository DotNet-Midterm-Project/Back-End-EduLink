using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;

namespace EduLink.Repositories.Interfaces
{
    public interface ICommon
    {
        Task<List<WorkshopResponseDTO>> GetAllWorkshopsAsync();
        Task<List<ArticleResponseDTO>> GetAllArticlesAsync();
        Task<List<ArticleResponseDTO>> GetArticlesByVolunteerAsync(int volunteerId);
        Task<List<EventContentResponseDTO>> GetEventContentAsync(int eventId);
        Task<List<EventResponseDTO>> GetEventsForVolunteerAsync(GetEventsRequestDTO eventsRequestDTO);
        Task<List<SessionResponseDTO>> GetEventSessionsAsync(GetEventSessionRequestDTO EventSessionRequestDTO);
    }

}
