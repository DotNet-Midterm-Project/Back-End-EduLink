using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Models;
using EduLink.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using EduLink.Data;
using System.ComponentModel.DataAnnotations;

namespace EduLink.Repositories.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly EduLinkDbContext _dbContext;

        public MeetingService(EduLinkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MeetingResponseDTO> CreateMeetingAsync(MeetingRequestDTO request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");

            // Validate MeetingRequestDTO properties
            if (string.IsNullOrWhiteSpace(request.MeetingLink))
                throw new ValidationException("Meeting link is required.");

            if (request.ScheduledDate == default)
                throw new ValidationException("Scheduled date is required.");

            // Add more validations as needed

            var meeting = new Meeting
            {
                MeetingLink = request.MeetingLink,
                ScheduledDate = request.ScheduledDate,
                Announcement = request.Announcement,
                GroupId = request.GroupId
            };

            _dbContext.Meetings.Add(meeting);
            await _dbContext.SaveChangesAsync();

            return new MeetingResponseDTO
            {
                MeetingLink = meeting.MeetingLink,
                ScheduledDate = meeting.ScheduledDate,
                Announcement = meeting.Announcement,
                GroupId = meeting.GroupId
            };
        }

        public async Task<IEnumerable<MeetingResponseDTO>> GetAllMeetingsAsync(int groupId)
        {
            if (groupId <= 0)
                throw new ArgumentException("Invalid group ID.", nameof(groupId));

            var meetings = await _dbContext.Meetings
                .Where(m => m.GroupId == groupId)
                .ToListAsync();

            return meetings.Select(m => new MeetingResponseDTO
            {
                MeetingLink = m.MeetingLink,
                ScheduledDate = m.ScheduledDate,
                Announcement = m.Announcement,
                GroupId = m.GroupId
            });
        }

        public async Task<MeetingResponseDTO> GetMeetingByIdAsync(int groupId, int meetingId)
        {
            if (groupId <= 0 || meetingId <= 0)
                throw new ArgumentException("Invalid group ID or meeting ID.");

            var meeting = await _dbContext.Meetings
                .FirstOrDefaultAsync(m => m.GroupId == groupId && m.Id == meetingId);

            if (meeting == null)
                throw new KeyNotFoundException("Meeting not found.");

            return new MeetingResponseDTO
            {
                MeetingLink = meeting.MeetingLink,
                ScheduledDate = meeting.ScheduledDate,
                Announcement = meeting.Announcement,
                GroupId = meeting.GroupId
            };
        }

        public async Task<MeetingResponseDTO> UpdateMeetingAsync(int groupId, int meetingId, MeetingRequestDTO request)
        {
            if (groupId <= 0 || meetingId <= 0)
                throw new ArgumentException("Invalid group ID or meeting ID.");

            if (request == null)
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");

            // Validate MeetingRequestDTO properties
            if (string.IsNullOrWhiteSpace(request.MeetingLink))
                throw new ValidationException("Meeting link is required.");

            if (request.ScheduledDate == default)
                throw new ValidationException("Scheduled date is required.");

            var meeting = await _dbContext.Meetings
                .FirstOrDefaultAsync(m => m.GroupId == groupId && m.Id == meetingId);

            if (meeting == null)
                throw new KeyNotFoundException("Meeting not found.");

            meeting.MeetingLink = request.MeetingLink;
            meeting.ScheduledDate = request.ScheduledDate;
            meeting.Announcement = request.Announcement;

            await _dbContext.SaveChangesAsync();

            return new MeetingResponseDTO
            {
                MeetingLink = meeting.MeetingLink,
                ScheduledDate = meeting.ScheduledDate,
                Announcement = meeting.Announcement,
                GroupId = meeting.GroupId
            };
        }

        public async Task DeleteMeetingAsync(int groupId, int meetingId)
        {
            if (groupId <= 0 || meetingId <= 0)
                throw new ArgumentException("Invalid group ID or meeting ID.");

            var meeting = await _dbContext.Meetings
                .FirstOrDefaultAsync(m => m.GroupId == groupId && m.Id == meetingId);

            if (meeting == null)
                throw new KeyNotFoundException("Meeting not found.");

            _dbContext.Meetings.Remove(meeting);
            await _dbContext.SaveChangesAsync();
        }
    }
}
