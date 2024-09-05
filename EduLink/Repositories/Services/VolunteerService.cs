using EduLink.Data;
using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace EduLink.Repositories.Services
{
    public class VolunteerService : IVolunteer
    {
        private readonly EduLinkDbContext _context;
        private readonly HelperService _helperService;
        private readonly EmailService _emailService;

        public VolunteerService(EduLinkDbContext context, HelperService helperService, EmailService emailService)
        {
            _context = context;
            _helperService = helperService;
            _emailService = emailService;
        }

        // Get volunteer courses
        public async Task<List<VolunteerCourseResDTO>> GetVolunteerCoursesAsync(int volunteerID)
        {
            var courses = await _context.VolunteerCourses
                .Where(vc => vc.VolunteerID == volunteerID)
                .Select(vc => new VolunteerCourseResDTO
                {
                    CourseID = vc.CourseID,
                    CourseName = vc.Course.CourseName,
                    CourseDescription = vc.Course.CourseDescription
                })
                .ToListAsync();

            return courses;
        }

        // Add event content to a specific event
        public async Task<MessageResDTO> AddEventContentAsync(EventContetnReqDTO dto)
        {
            var eventExists = await _context.Events.AnyAsync(e => e.EventID == dto.EventID);
            if (!eventExists)
            {
                return new MessageResDTO { Message = "Event not found." };
            }

            var newContent = new EventContent
            {
                EventID = dto.EventID,
                ContentName = dto.ContentName,
                ContentType = dto.ContentType,
                ContentDescription = dto.ContentDescription,
                ContentAdress = dto.ContentAdress,
            };

            await _context.EventContents.AddAsync(newContent);
            await _context.SaveChangesAsync();

            return new MessageResDTO { Message = "Event content added successfully." };
        }

        // Get all event contents for a specific event
        public async Task<IEnumerable<EventContentResDTO>> GetEventContentsAsync(int eventID)
        {
            await _helperService.UpdateEventStatusesAsync();

            var eventContents = await _context.Events
                .Where(e => e.EventID == eventID)
                .SelectMany(e => e.EventContents)
                .Select(ec => new EventContentResDTO
                {
                    ContentID = ec.ContentID,
                    ContentName = ec.ContentName,
                    ContentType = ec.ContentType,
                    ContentDescription = ec.ContentDescription,
                    ContentAdress = ec.ContentAdress,
                })
                .ToListAsync();

            if (eventContents == null || !eventContents.Any())
            {
                throw new InvalidOperationException("No content found for the specified event.");
            }

            return eventContents;
        }

        // Get all events for a specific course
        public async Task<List<EventResDTO>> GetEventsAsync(int volunteerID, int courseID)
        {
            var volunteerCourse = await _context.VolunteerCourses
                .FirstOrDefaultAsync(vc => vc.VolunteerID == volunteerID && vc.CourseID == courseID);

            if (volunteerCourse == null)
            {
                return new List<EventResDTO>();
            }

            var events = await _context.Events
                .Where(e => e.VolunteerCoursID == volunteerCourse.VolunteerCourseID)
                .ToListAsync();

            var eventResponse = events.Select(e => new EventResDTO
            {
                EventID = e.EventID,
                Title = e.Title,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                EventDescription = e.EventDescription,
                EventStatus = e.EventStatus,
                Location = e.Location,
                Capacity = e.Capacity,
                EventType = e.EventType,
                EventAdress = e.EventAddress,
                Details = e.EventDetailes,
            }).ToList();

            return eventResponse;
        }
        // Add Event
        public async Task<MessageResDTO> AddEventsAsync(int volunteerID, AddEventReqDTO request)
        {
            if (request == null)
            {
                return new MessageResDTO
                {
                    Message = "Invalid request data."
                };
            }

            var volunteerCourse = await _context.VolunteerCourses
                .FirstOrDefaultAsync(vc => vc.VolunteerID == volunteerID && vc.CourseID == request.CourseID);

            if (volunteerCourse == null)
            {
                return new MessageResDTO
                {
                    Message = "Volunteer is not associated with the course."
                };
            }

            var eventEntity = new Event
            {
                VolunteerCoursID = volunteerCourse.VolunteerCourseID,
                Title = request.Title,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Location = request.Location,
                EventType = request.EventType,
                EventDescription = request.EventDescription,
                EventDetailes = request.Details,
                EventAddress = request.Location == EventLocation.Online ? "The session link will be sent later" : request.EventAdress,
                Capacity = request.Capacity,
                SessionCount = request.EventType == EventType.PrivateSession ? request.SessionCounts : 0,
                EventStatus = EventStatus.Scheduled,
            };

            await _context.Events.AddAsync(eventEntity);
            await _context.SaveChangesAsync();

            // Get all student email addresses
            var studentEmails = await _context.Students
                .Include(s => s.User) // Include User table to access Email property
                .Select(s => s.User.Email) // Select the Email from the User table
                .ToListAsync();

            // Create the email content
            var emailSubject = $"New Event: {eventEntity.Title}";
            var emailDescriptionHtml = $@"
        <p>Dear Student,</p>
        <p>We are excited to announce a new event titled <strong>{eventEntity.Title}</strong>.</p>
        <p><strong>Start Time:</strong> {eventEntity.StartTime}</p>
        <p><strong>End Time:</strong> {eventEntity.EndTime}</p>
        <p><strong>Location:</strong> {eventEntity.EventAddress}</p>
        <p><strong>Description:</strong> {eventEntity.EventDescription}</p>
        <p>We hope you can join us!</p>
        <p>Best regards,</p>
        <p>EduLink Team</p>";

            // Convert HTML to plain text
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(emailDescriptionHtml);
            var emailDescriptionPlain = htmlDoc.DocumentNode.InnerText;

            // Send email to all students
            foreach (var email in studentEmails)
            {
                await _emailService.SendEmailAsync(email, emailSubject, emailDescriptionHtml);

            }

            // Create and store announcement
            var announcement = new Announcement
            {
                EventID = eventEntity.EventID,
                Title = eventEntity.Title,
                Message = emailDescriptionPlain, // Store as plain text
                AnounceDate = DateTime.UtcNow
            };

            await _context.Announcement.AddAsync(announcement);
            await _context.SaveChangesAsync();

            return new MessageResDTO
            {
                Message = $"Event '{request.Title}' added successfully and announcement sent."
            };
        }

        // Cancel an event
        public async Task<MessageResDTO> CancelEventAsync(int eventId, int volunteerID)
        {
            var eventToCancel = await _context.Events
                .Include(e => e.VolunteerCourse)
                .FirstOrDefaultAsync(e => e.EventID == eventId);

            if (eventToCancel == null)
            {
                return new MessageResDTO { Message = "Event not found." };
            }

            var isVolunteerAssociated = await _context.Events
                .AnyAsync(e => e.EventID == eventId && e.VolunteerCourse.VolunteerID == volunteerID);

            if (!isVolunteerAssociated)
            {
                return new MessageResDTO { Message = "Volunteer is not associated with this event." };
            }

            eventToCancel.EventStatus = EventStatus.Cancelled;

            _context.Events.Update(eventToCancel);
            var result = await _context.SaveChangesAsync();

            return result > 0
                ? new MessageResDTO { Message = "The event was cancelled successfully." }
                : new MessageResDTO { Message = "Failed to cancel the event." };
        }

        // Generate a meeting URL for an online event
        public async Task<MessageResDTO> GenerateMeetingUrlAsync(int eventId)
        {
            var eventToUpdate = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == eventId);

            if (eventToUpdate == null)
            {
                return new MessageResDTO { Message = "Event not found." };
            }

            if (eventToUpdate.Location != EventLocation.Online)
            {
                return new MessageResDTO { Message = "Meeting URL can only be created for online events." };
            }

            var meetingUrl = $"https://meeting.service.com/{Guid.NewGuid()}";

            eventToUpdate.EventAddress = meetingUrl;
            _context.Events.Update(eventToUpdate);

            var result = await _context.SaveChangesAsync();

            return result > 0
                ? new MessageResDTO { Message = "The URL was created successfully." }
                : new MessageResDTO { Message = "Failed to create the URL." };
        }

        // Add a session to an event
        public async Task<MessageResDTO> AddSessionAsync(AddSessionReqDTO request)
        {
            var eventToUpdate = await _context.Events
                .Include(e => e.Sessions)
                .FirstOrDefaultAsync(e => e.EventID == request.EventID);

            if (eventToUpdate == null)
            {
                return new MessageResDTO { Message = "Event not found." };
            }

            if (eventToUpdate.EventType != EventType.PrivateSession)
            {
                return new MessageResDTO { Message = "Sessions can only be added to events of type PrivateSession." };
            }

            var newSession = new Session
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Details = request.Details,
                Capacity = request.SessionCapacity,
                SessionStatus = SessionStatus.Open,
            };

            eventToUpdate.Sessions.Add(newSession);
            _context.Events.Update(eventToUpdate);

            var result = await _context.SaveChangesAsync();

            return result > 0
                ? new MessageResDTO { Message = "The session was added successfully." }
                : new MessageResDTO { Message = "Failed to add the session." };
        }

        // Commented code

        // Add an article
        /*
        public async Task<MessageResDTO> AddArticleAsync(ArticleReqDTO dto)
        {
            var newArticle = new Article
            {
                VolunteerID = dto.VolunteerID,
                Title = dto.Title,
                ArticleContent = dto.ArticleContent,
                PublicationDate = dto.PublicationDate,
                AuthorName = dto.AuthorName
            };

            _context.Articles.Add(newArticle);
            await _context.SaveChangesAsync();

            return new MessageResDTO
            {
                Message = $"The article '{dto.Title}' added successfully."
            };
        }
        */

        // Delete an article
        /*
        public async Task<MessageResDTO> DeleteArticleAsync(int volunteerId, int articleId)
        {
            var article = await _context.Articles
                .FirstOrDefaultAsync(a => a.ArticleID == articleId && a.VolunteerID == volunteerId);

            if (article == null)
            {
                return new MessageResDTO
                {
                    Message = "Article not found or not associated with the volunteer."
                };
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return new MessageResDTO
            {
                Message = "Article deleted successfully."
            };
        }
        */
    }
}
