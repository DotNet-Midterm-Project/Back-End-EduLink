using EduLink.Data;
using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
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
                    EventID = ec.EventID,
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
            await _helperService.UpdateEventStatusesAsync();

            if (volunteerCourse == null)
            {
                return new List<EventResDTO>();
            }

            var events = await _context.Events
                .Where(e => e.VolunteerCoursID == volunteerCourse.VolunteerCourseID)
                .Include(e => e.VolunteerCourse)
                .ThenInclude(vc => vc.Volunteer)
                .ThenInclude(v => v.Student)
                .ThenInclude(s => s.User)
                .Include(e => e.VolunteerCourse.Course)
                .ToListAsync();

            var eventResponse = events.Select(e => new EventResDTO
            {
                EventID = e.EventID,
                Capacity = e.Capacity,
                Title = e.Title,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                EventDescription = e.EventDescription,
                EventStatus = e.EventStatus.ToString(),
                Location = e.Location.ToString(),
                EventType = e.EventType.ToString(),
                EventAddress = e.EventAddress,
                Details = e.EventDetailes,
                VolunteerName = e.VolunteerCourse?.Volunteer?.Student?.User?.UserName ?? "Volunteer Name is null",
                CourseName = e.VolunteerCourse?.Course?.CourseName ?? "Course Name is null",
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
                EventAddress = request.Location == EventLocation.Online ? "The Event link will be sent later" : request.EventAddress,
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
            if (studentEmails!=null && request.EventType==EventType.Workshop) {
                 
                 await _emailService.SendMultipleEmailsAsync(studentEmails, emailSubject, emailDescriptionHtml);
                

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
            // Retrieve the event from the database using the provided eventId
            var eventToUpdate = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == eventId);


            // If the event is not found, return a message indicating this
            if (eventToUpdate == null)
            {
                return new MessageResDTO { Message = "Event not found." };
            }

            // Check if the event location is online; if not, return a message
            if (eventToUpdate.Location != EventLocation.Online)
            {
                return new MessageResDTO { Message = "Meeting URL can only be created for online events." };
            }

            // Get the Google Calendar service to interact with Google Calendar API
            var service = GoogleCalendarService.GetCalendarService();

            // Define the timezone for the event
            var validTimeZone = "UTC";

            // Create a new event object to be sent to Google Calendar
            var newEvent = new Google.Apis.Calendar.v3.Data.Event
            {
                Summary = eventToUpdate.Title, // Set the event title
                Description = eventToUpdate.EventDescription, // Set the event description
                Start = new Google.Apis.Calendar.v3.Data.EventDateTime
                {
                    DateTime = eventToUpdate.StartTime.DateTime, // Set the event start time
                    TimeZone = validTimeZone // Set the timezone for the start time
                },
                End = new Google.Apis.Calendar.v3.Data.EventDateTime
                {
                    DateTime = eventToUpdate.EndTime.DateTime, // Set the event end time
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
            var request = service.Events.Insert(newEvent, calendarId);
            request.ConferenceDataVersion = 1; // Specify the conference data version to create a Meet link
            var createdEvent = await request.ExecuteAsync(); // Execute the request asynchronously

            // Update the event in the database with the newly created Google Meet link
            eventToUpdate.EventAddress = createdEvent.HangoutLink;
            _context.Events.Update(eventToUpdate);

            // Save the changes to the database
            var result = await _context.SaveChangesAsync();

            // Return a success message with the meeting URL if successful, or a failure message if not
            return result > 0
                ? new MessageResDTO { Message = $"The URL was created successfully: {createdEvent.HangoutLink}" }
                : new MessageResDTO { Message = "Failed to create the URL." };
        }

        public async Task<MessageResDTO> GenerateSessionUrlAsync(int Sessionid)
        {
            // Retrieve the event from the database using the provided eventId
            var eventToUpdate = await _context.Sessions.Include(s=> s.Event)
                .FirstOrDefaultAsync(e => e.SessionID == Sessionid);


            // If the event is not found, return a message indicating this
            if (eventToUpdate == null)
            {
                return new MessageResDTO { Message = "Event not found." };
            }

            // Check if the event location is online; if not, return a message
            if (eventToUpdate.Event.Location != EventLocation.Online)
            {
                Console.WriteLine(eventToUpdate.Event.Location);
                Console.WriteLine(eventToUpdate);
                return new MessageResDTO { Message = "Meeting URL can only be created for online events." };
            }

            // Get the Google Calendar service to interact with Google Calendar API
            var service = GoogleCalendarService.GetCalendarService();

            // Define the timezone for the event
            var validTimeZone = "UTC";

            // Create a new event object to be sent to Google Calendar
            var newEvent = new Google.Apis.Calendar.v3.Data.Event
            {
                Summary = eventToUpdate.Event.Title, // Set the event title
                Description = eventToUpdate.Details, // Set the event description
                Start = new Google.Apis.Calendar.v3.Data.EventDateTime
                {
                    DateTime = eventToUpdate.StartDate.DateTime, // Set the event start time
                    TimeZone = validTimeZone // Set the timezone for the start time
                },
                End = new Google.Apis.Calendar.v3.Data.EventDateTime
                {
                    DateTime = eventToUpdate.EndDate.DateTime, // Set the event end time
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
            var request = service.Events.Insert(newEvent, calendarId);
            request.ConferenceDataVersion = 1; // Specify the conference data version to create a Meet link
            var createdEvent = await request.ExecuteAsync(); // Execute the request asynchronously

            // Update the event in the database with the newly created Google Meet link
            eventToUpdate.SessionAddress = createdEvent.HangoutLink;
            _context.Sessions.Update(eventToUpdate);

            // Save the changes to the database
            var result = await _context.SaveChangesAsync();

            // Return a success message with the meeting URL if successful, or a failure message if not
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
                    SessionAddress = "The session link will be sent later"
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
                    VolunteerName = a.Volunteer.Student.User.UserName,
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
                    VolunteerID = a.VolunteerID,
                    ArticleID = a.ArticleID,
                    VolunteerName = a.Volunteer.Student.User.UserName,
                    Title = a.Title,
                    ArticleContent = a.ArticleContent,
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
            eventToUpdate.EventDescription = request.EventDescription;
            eventToUpdate.EventDetailes = request.EventDetails;
            eventToUpdate.EventAddress = request.EventAddress;
            eventToUpdate.Location = request.Location;
            eventToUpdate.Capacity = request.Capacity;
            eventToUpdate.EventType = request.EventType;
            eventToUpdate.SessionCount = request.SessionCount;

            _context.Events.Update(eventToUpdate);
            await _context.SaveChangesAsync();

            return new MessageResDTO { Message = "Update Event successfully" };
        }

        public async Task<List<BookingForVolunteerResDTO>> GetVolunteerBookingsAsync(int volunteerID)
        {
           
            var volunteerEvents = await _context.Events
                .Where(e => e.VolunteerCourse.VolunteerID == volunteerID)
                .Include(e => e.Bookings)
                    .ThenInclude(b => b.Student)
                    .ThenInclude(s => s.User)
                .Include(e => e.Sessions) 
                    .ThenInclude(s => s.Bookings)
                    .ThenInclude(b => b.Student)
                    .ThenInclude(s => s.User)
                .ToListAsync();


            var eventBookings = volunteerEvents
                .SelectMany(e => e.Bookings)
                .Where(s => s.SessionID == null)
                .Select(b => new BookingForVolunteerResDTO
                {
                    BookingID = b.BookingID,
                    EventID = b.EventID,
                    SessionID = null,
                    StudentID = b.StudentID,
                    StudentName = b.Student.User.UserName,
                    BookingStatus = b.BookingStatus.ToString(),
                    startDate = b.Event.StartTime,
                    EndDate = b.Event.EndTime,
                    EventTitle = b.Event.Title,
                    BookingType = "Event"
                })
                .ToList();

            
            var sessionBookings = volunteerEvents
                .SelectMany(e => e.Sessions)
                .SelectMany(s => s.Bookings)
                .Select(b => new BookingForVolunteerResDTO
                {
                    BookingID = b.BookingID,
                    EventID = b.EventID,
                    SessionID = b.SessionID,
                    StudentID = b.StudentID,
                    StudentName = b.Student.User.UserName,
                    BookingStatus = b.BookingStatus.ToString(),
                    startDate=b.Session.StartDate,
                    EndDate=b.Session.EndDate,
                    EventTitle = b.Event.Title,
                    BookingType = "Session"
                })
                .ToList();

            
            var allBookings = eventBookings.Concat(sessionBookings).ToList();

            return allBookings;
        }
      
    }
}
