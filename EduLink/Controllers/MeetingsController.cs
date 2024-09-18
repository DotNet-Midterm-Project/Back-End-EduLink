using EduLink.Models.DTO.Request;
using EduLink.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EduLink.Controllers
{
    //[Route("groups/{groupId}/meetings")]
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly IMeetingService _meetingService;

        public MeetingsController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeeting([FromBody] MeetingRequestDTO request)
        {
            if (request == null)
                return BadRequest("Invalid request.");

            var meeting = await _meetingService.CreateMeetingAsync(request);
            return CreatedAtAction(nameof(GetMeeting), new { groupId = meeting.GroupId }, meeting);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMeetings(int groupId)
        {
            var meetings = await _meetingService.GetAllMeetingsAsync(groupId);
            return Ok(meetings);
        }

        [HttpGet("{meetingId}")]
        public async Task<IActionResult> GetMeeting(int groupId, int meetingId)
        {
            var meeting = await _meetingService.GetMeetingByIdAsync(groupId, meetingId);
            return Ok(meeting);
        }

        [HttpPut("{meetingId}")]
        public async Task<IActionResult> UpdateMeeting(int groupId, int meetingId, [FromBody] MeetingRequestDTO request)
        {
            var updatedMeeting = await _meetingService.UpdateMeetingAsync(groupId, meetingId, request);
            return Ok(updatedMeeting);
        }

        [HttpDelete("{meetingId}")]
        public async Task<IActionResult> DeleteMeeting(int groupId, int meetingId)
        {
            await _meetingService.DeleteMeetingAsync(groupId, meetingId);
            return Ok(new { message = "Meeting deleted successfully." });
        }
    }

}
