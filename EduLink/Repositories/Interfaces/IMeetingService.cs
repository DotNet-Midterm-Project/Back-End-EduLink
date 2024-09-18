using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;

namespace EduLink.Repositories.Interfaces
{
    public interface IMeetingService
    {
        Task<MessageResDTO> CreateMeetingAsync(MeetingRequestDTO request);
        Task<IEnumerable<MeetingResponseDTO>> GetAllMeetingsAsync(int groupId);
        Task<MeetingResponseDTO> GetMeetingByIdAsync(int groupId, int meetingId);
        Task<MeetingResponseDTO> UpdateMeetingAsync(int meetingId, UpdateMeetingRequest request);
        Task DeleteMeetingAsync(int groupId, int meetingId);
    }
}
