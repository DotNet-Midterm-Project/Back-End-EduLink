using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Models;
using EduLink.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using EduLink.Data;
using HtmlAgilityPack;

namespace EduLink.Repositories.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly EduLinkDbContext _dbContext;
        private readonly IEmailService _Email;
        public MeetingService(EduLinkDbContext dbContext, IEmailService email)
        {
            _dbContext = dbContext;
            _Email = email;
        }

        public async Task<MessageResDTO> CreateMeetingAsync(MeetingRequestDTO request)
        {
            var emailDescriptionPlain = "";

            var group = await _dbContext.Groups
                .Include(g => g.Members)
                .ThenInclude(m => m.Student)
                .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(g => g.GroupId == request.GroupId);

            if (group != null)
            {
                var emails = group.Members
                    .Select(m => m.Student.User.Email)
                    .Where(email => !string.IsNullOrWhiteSpace(email))
                    .ToList();

                var service = GoogleCalendarService.GetCalendarService();

                // Define the timezone for the event
                var validTimeZone = "UTC";

                // Create a new event object to be sent to Google Calendar
                var newEvent = new Google.Apis.Calendar.v3.Data.Event
                {
                    Summary = group.GroupName, // Set the event title
                    Description = group.Description, // Set the event description
                    Start = new Google.Apis.Calendar.v3.Data.EventDateTime
                    {
                        DateTime = request.ScheduledDate, // Set the event start time
                        TimeZone = validTimeZone // Set the timezone for the start time
                    },
                    End = new Google.Apis.Calendar.v3.Data.EventDateTime
                    {
                        DateTime = request.ScheduledDate.AddMinutes(60), // Set the event end time 60 minutes after the start
                        TimeZone = validTimeZone // Set the timezone for the end time
                    },
                    ConferenceData = new Google.Apis.Calendar.v3.Data.ConferenceData
                    {
                        CreateRequest = new Google.Apis.Calendar.v3.Data.CreateConferenceRequest
                        {
                            ConferenceSolutionKey = new Google.Apis.Calendar.v3.Data.ConferenceSolutionKey
                            {
                                Type = "hangoutsMeet" // Specify that the meeting should be a Google Meet
                            },
                            RequestId = Guid.NewGuid().ToString() // Generate a unique request ID for the meeting
                        }
                    }
                };

                // Define the calendar ID where the event will be created (primary calendar)
                var calendarId = "primary";

                // Insert the new event into the Google Calendar
                var request1 = service.Events.Insert(newEvent, calendarId);
                request1.ConferenceDataVersion = 1; // Specify the conference data version to create a Meet link
                var createdEvent = await request1.ExecuteAsync(); // Execute the request asynchronously
                if (emails.Any())
                {
                    var emailSubject = $"New Meeting: {group.GroupName}";
                    var emailDescriptionHtml = $@"
                <p>Dear Student,</p>
                <p>We would like to inform you about an upcoming meeting for the group titled <strong>{group.GroupName}</strong>.</p>
                <p><strong>Meeting Date and Time:</strong> {request.ScheduledDate}</p>
                <p><strong>Meeting Link:</strong> <a href='{createdEvent.HangoutLink}'>Join the Meeting</a></p>
                <p>Please make sure to join the meeting using the provided link at the scheduled time.</p>
                <p>Best regards,</p>
                <p>EduLink Team</p>";

                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(emailDescriptionHtml);
                    emailDescriptionPlain = htmlDoc.DocumentNode.InnerText;

                    await _Email.SendMultipleEmailsAsync(emails, emailSubject, emailDescriptionHtml);
                }

                var meeting = new Meeting
                {
                    MeetingLink = createdEvent.HangoutLink,
                    ScheduledDate = request.ScheduledDate,
                    Announcement = emailDescriptionPlain,
                    GroupId = request.GroupId
                };

                _dbContext.Meetings.Add(meeting);
                await _dbContext.SaveChangesAsync();

                return new MessageResDTO
                {
                    Message = $"The meeting has been created successfully." +
                    $" The URL: ('{meeting.MeetingLink}') and send Announcement to ('{meeting.Group.GroupName}') Member."
                };
            }
            else
            {
                return new MessageResDTO
                {
                    Message = "Group not found."
                };
            }
        }

        public async Task<IEnumerable<MeetingResponseDTO>> GetAllMeetingsAsync(int groupId)
        {
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
            var meeting = await _dbContext.Meetings
                .FirstOrDefaultAsync(m => m.GroupId == groupId && m.Id == meetingId);

            if (meeting == null)
                return null;

            return new MeetingResponseDTO
            {
                MeetingLink = meeting.MeetingLink,
                ScheduledDate = meeting.ScheduledDate,
                Announcement = meeting.Announcement,
                GroupId = meeting.GroupId
            };
        }

        public async Task<MeetingResponseDTO> UpdateMeetingAsync(int meetingId, UpdateMeetingRequest request)
        {
            if (request == null)
                return null;

            var meeting = await _dbContext.Meetings
                .FirstOrDefaultAsync(m => m.Id == meetingId);

            meeting.ScheduledDate = request.ScheduledDate;

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
            var meeting = await _dbContext.Meetings
                .FirstOrDefaultAsync(m => m.GroupId == groupId && m.Id == meetingId);

            if (meeting == null)
                return;

            _dbContext.Meetings.Remove(meeting);
            await _dbContext.SaveChangesAsync();
        }
    }
}
