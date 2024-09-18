using EduLink.Models.DTO.Request;
using EduLink.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Student")]
        [HttpPost]
        public async Task<IActionResult> CreateMeeting([FromBody] MeetingRequestDTO request)
        {
            if (request == null)
                return BadRequest("Invalid request.");

            var meeting = await _meetingService.CreateMeetingAsync(request);
            return Ok(meeting);
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public async Task<IActionResult> GetAllMeetings(int groupId)
        {
            var meetings = await _meetingService.GetAllMeetingsAsync(groupId);
            return Ok(meetings);
        }

        [HttpGet("{meetingId}")]
        public async Task<IActionResult> GetMeeting(int groupId, int meetingId)
        {
            if (groupId <= 0 || meetingId <= 0)
                return BadRequest("Invalid group ID or meeting ID.");

            var meeting = await _meetingService.GetMeetingByIdAsync(groupId, meetingId);
            if (meeting == null)
                return BadRequest("Meeting not found.");

            return Ok(meeting);
        }

        [Authorize(Roles = "Student")]
        [HttpPut("{meetingId}")]
        public async Task<IActionResult> UpdateMeeting(int meetingId, [FromBody] UpdateMeetingRequest request)
        {
            var updatedMeeting = await _meetingService.UpdateMeetingAsync(meetingId, request);
            if (updatedMeeting == null)
            {
                return BadRequest("Request cannot be null");
            }
            return Ok(updatedMeeting);
        }

        [Authorize(Roles = "Student")]
        [HttpDelete("{meetingId}")]
        public async Task<IActionResult> DeleteMeeting(int groupId, int meetingId)
        {
            if (groupId <= 0 || meetingId <= 0)
            {
                return BadRequest(new { message = "Invalid group ID or meeting ID." });
            }

            try
            {
                await _meetingService.DeleteMeetingAsync(groupId, meetingId);
                return Ok(new { message = "Meeting deleted successfully." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Meeting not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
