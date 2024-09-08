using EduLink.Data;
using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
        private readonly IEmailService _emailService;

        public VolunteerService(EduLinkDbContext context, HelperService helperService, IEmailService emailService)
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
                ContentAddress = dto.ContentAddress,
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
                    ContentType = ec.ContentType.ToString(),
                    ContentDescription = ec.ContentDescription,
                    ContentAddress = ec.ContentAddress,
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
                EventStatus = e.EventStatus.ToString(),
                Location = e.Location.ToString(),
                Capacity = e.Capacity,
                EventType = e.EventType.ToString(),
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
                EventAddress = request.Location == EventLocation.Online ? "The session link will be sent later" : request.EventAddress,
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
                    emailDescriptionPlain = emailDescriptionPlain
            .Replace("\r\n", "\n") 
            .Replace("\n", Environment.NewLine) 
            .Replace("Dear Student,", "\nDear Student,\n")
            .Replace("We are excited", "\nWe are excited")
            .Replace("Start Time:", "\nStart Time:")
            .Replace("End Time:", "\nEnd Time:")
            .Replace("Location:", "\nLocation:")
            .Replace("Description:", "\nDescription:")
            .Replace("We hope you can join us!", "\nWe hope you can join us!\n")
            .Replace("Best regards,", "\nBest regards,\n")
            .Replace("EduLink Team", "\nEduLink Team");

            // Send email to all students
            if (studentEmails!=null) {
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
            }
        


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

            var service = GoogleCalendarService.GetCalendarService();

            // استخدم معرف منطقة زمنية معتمد من Google مثل "UTC" أو "America/New_York"
            var validTimeZone = "UTC";  // اختر المنطقة الزمنية المناسبة

            var newEvent = new Google.Apis.Calendar.v3.Data.Event
            {
                Summary = eventToUpdate.Title,
                Description = eventToUpdate.EventDescription,
                Start = new Google.Apis.Calendar.v3.Data.EventDateTime
                {
                    DateTime = eventToUpdate.StartTime.DateTime,
                    TimeZone = validTimeZone // استخدم معرف منطقة زمنية صحيح
                },
                End = new Google.Apis.Calendar.v3.Data.EventDateTime
                {
                    DateTime = eventToUpdate.EndTime.DateTime,
                    TimeZone = validTimeZone // استخدم معرف منطقة زمنية صحيح
                },
                ConferenceData = new Google.Apis.Calendar.v3.Data.ConferenceData
                {
                    CreateRequest = new Google.Apis.Calendar.v3.Data.CreateConferenceRequest
                    {
                        ConferenceSolutionKey = new Google.Apis.Calendar.v3.Data.ConferenceSolutionKey
                        {
                            Type = "hangoutsMeet"
                        },
                        RequestId = Guid.NewGuid().ToString()
                    }
                }
            };

            // إضافة الحدث إلى تقويم Google
            var calendarId = "primary";
            var request = service.Events.Insert(newEvent, calendarId);
            request.ConferenceDataVersion = 1;
            var createdEvent = await request.ExecuteAsync();

            // تحديث عنوان الحدث مع رابط Google Meet
            eventToUpdate.EventAddress = createdEvent.HangoutLink;
            _context.Events.Update(eventToUpdate);

            var result = await _context.SaveChangesAsync();

            return result > 0
                ? new MessageResDTO { Message = $"The URL was created successfully: {createdEvent.HangoutLink}" }
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
            if (eventToUpdate.SessionCount > 0)
            {
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
                --eventToUpdate.SessionCount;

                var result = await _context.SaveChangesAsync();

                return result > 0
                    ? new MessageResDTO { Message = "The session was added successfully." }
                    : new MessageResDTO { Message = "Failed to add the session." };
            }
            return new MessageResDTO { Message = "Failed to add the session." };
        }

       

        public async Task<MessageResDTO> AddArticleAsync(AddArticleReqDTO request, int volunteerId)
        {
            var volunteer = await _context.Volunteers.FindAsync(volunteerId);
            if (volunteer == null)
            {
                throw new Exception("Volunteer not found");
            }

            var article = new Article
            {
                Title = request.Title,
                ArticleContent = request.ArticleContent,
                PublicationDate = request.PublicationDate,
                Status = ArticleStatus.Visible,
                VolunteerID = volunteerId,
                Volunteer = volunteer,
               
            };

            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync(); 

            return new MessageResDTO { Message = $"The Article '{article.Title}' has been added successfully." };
        }

        public async Task<MessageResDTO> ModifyArticleStatusAsync(ModifyArticleStatusReqDTO request, int volunteerId)
        {
            var article = await _context.Articles.FindAsync(request.ArticleID);

            if (article == null)
            {
                throw new Exception("Article not found");
            }

            if (article.VolunteerID != volunteerId)
            {
                throw new Exception("You are not authorized to modify this article");
            }

            article.Status = request.Status;

            await _context.SaveChangesAsync(); 

            return new MessageResDTO { Message = $"The Article '{article.Title}' has been updated to '{request.Status}'" };
        }

        public async Task<ArticlesResDTO> GetArticlesForVolunteerAsync(int volunteerId)
        {
            var volunteer = await _context.Volunteers.FindAsync(volunteerId);
            if (volunteer == null)
            {
                throw new Exception("Volunteer not found");
            }

            var articles = await _context.Articles
                .Where(a => a.VolunteerID == volunteerId)
                .Select(a => new ArticleDTO
                {
                    ArticleID = a.ArticleID,
                    VolunteerID= volunteerId,
                    Title = a.Title,
                    ArticleContent = a.ArticleContent,
                    PublicationDate = a.PublicationDate,
                    Status = a.Status.ToString()
                })
                .ToListAsync();

            return new ArticlesResDTO
            {
                Articles = articles
            };
        }

        public async Task<ArticleDTO> GetArticleByIdAsync(int articleId)
        {
            var article = await _context.Articles
                .Where(a => a.ArticleID == articleId)
                .Select(a => new ArticleDTO
                {
                    Title = a.Title,
                    ArticleContent = a.ArticleContent,
                    VolunteerID = a.VolunteerID,
                    PublicationDate = a.PublicationDate,
                    Status = a.Status.ToString()
                })
                .FirstOrDefaultAsync();

            return article;
        }

        public async Task<MessageResDTO> UpdateArticleAsync(UpdateArticleReqDTO request, int volunteerId)
        {
            var article = await _context.Articles
                .FirstOrDefaultAsync(a => a.ArticleID == request.ArticleID && a.VolunteerID == volunteerId);

            if (article == null)
            {
                return new MessageResDTO { Message = "Article not found or volunteer not authorized" };
            }

            // Update article properties
            article.Title = request.Title;
            article.ArticleContent = request.ArticleContent;
            article.PublicationDate = request.PublicationDate;
            article.Status = request.Status;
        

            // Save changes to the database
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();

            return new MessageResDTO { Message = $"The Article '{article.Title}' updated successfully" };
        }

        public async Task<MessageResDTO> UpdateEventAsync(UpdateEventReqDTO request)
        {
            var eventToUpdate = await _context.Events.FindAsync(request.EventID);

            if (eventToUpdate == null)
            {
                return new MessageResDTO { Message = "Event not found" };
            }

            eventToUpdate.Title = request.Title;
            eventToUpdate.Location = request.Location;
            eventToUpdate.EventDescription = request.EventDescription;
            eventToUpdate.EventDetailes = request.EventDetails;
            eventToUpdate.Capacity = request.Capacity;
            eventToUpdate.EventType = request.EventType;
            eventToUpdate.EventAddress = request.EventAddress;
            eventToUpdate.SessionCount = request.SessionCount;

            _context.Events.Update(eventToUpdate);
            await _context.SaveChangesAsync();

            return new MessageResDTO { Message = "Update-event successfully" };
        }

        //public async Task<GetNotificationsResponse> GetNotificationsAsync(GetNotificationsRequest request)
        //{
        //    var notifications = await _context.Announcement
        //        .Where(a => a.Event.EventType == EventType.Workshop &&
        //                    _context.Bookings.Any(b => b.StudentID == request.StudentId && b.EventID == a.EventID))
        //        .Select(a => new NotificationResponse
        //        {
        //            NotificationID = a.AnouncementID,
        //            Message = a.Message,
        //            DateSend = a.AnounceDate,
        //            Capacity = a.Event.Capacity
        //        })
        //        .ToListAsync();

        //    return new GetNotificationsResponse { Notifications = notifications };
        //}
    }
}
