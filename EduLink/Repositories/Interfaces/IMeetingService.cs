using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;

namespace EduLink.Repositories.Interfaces
{
    public interface IMeetingService
    {
        Task<MeetingResponseDTO> CreateMeetingAsync(MeetingRequestDTO request);
        Task<IEnumerable<MeetingResponseDTO>> GetAllMeetingsAsync(int groupId);
        Task<MeetingResponseDTO> GetMeetingByIdAsync(int groupId, int meetingId);
        Task<MeetingResponseDTO> UpdateMeetingAsync(int groupId, int meetingId, MeetingRequestDTO request);
        Task DeleteMeetingAsync(int groupId, int meetingId);
    }

}
