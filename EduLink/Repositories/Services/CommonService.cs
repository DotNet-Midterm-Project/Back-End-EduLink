using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Models;
using EduLink.Repositories.Interfaces;
using EduLink.Data;
using Microsoft.EntityFrameworkCore;

namespace EduLink.Repositories.Services
{
    public class CommonService : ICommon
    {
        private readonly EduLinkDbContext _context;

        public CommonService(EduLinkDbContext context)
        {
            _context = context;
        }

        public async Task<List<WorkshopResponseDTO>> GetAllWorkshopsAsync()
        {
            return await _context.Events
                .Where(e => e.EventType == (int)EventType.Workshop)
                .Select(e => new WorkshopResponseDTO
                {
                    VolunteerID = e.VolunteerCourse.Volunteer.VolunteerID,
                    VolunteerName = e.VolunteerCourse.Volunteer.Student.User.UserName,
                    WorkshopName = e.Title,
                    WorkshopDescription = e.EventDescription,
                    WorkshopDateTime = e.StartTime.DateTime,
                    SessionLink = "https://your-link-here.com",//get link here.
                    Capacity = e.Capacity
                })
                .ToListAsync();
        }

        public async Task<List<ArticleResponseDTO>> GetAllArticlesAsync()
        {
            return await _context.Articles
                .Where(a => a.Status == ArticleStatus.Visible)
                .Select(a => new ArticleResponseDTO
                {
                    ArticleID = a.ArticleID,
                    Title = a.Title,
                    PublicationDate = a.PublicationDate,
                    ArticleContent = a.ArticleContent,//error in spilling
                    Status = a.Status
                })
                .ToListAsync();
        }

        public async Task<List<ArticleResponseDTO>> GetArticlesByVolunteerAsync(int volunteerId)
        {
            return await _context.Articles
                .Where(a => a.VolunteerID == volunteerId && a.Status == ArticleStatus.Visible)
                .Select(a => new ArticleResponseDTO
                {
                    ArticleID = a.ArticleID,
                    Title = a.Title,
                    PublicationDate = a.PublicationDate,
                    ArticleContent = a.ArticleContent,
                    Status = a.Status
                })
                .ToListAsync();
        }

        public async Task<List<EventContentResponseDTO>> GetEventContentAsync(int eventId)
        {
            return await _context.EventContents
                .Where(ec => ec.EventID == eventId)
                .Select(ec => new EventContentResponseDTO
                {
                    ContentID = ec.ContentID,
                    ContentType = ec.ContentType.ToString(),//converting enum to string
                    ContentDescription = ec.ContentDescription,
                    ContentName = ec.ContentName,
                    EventID = ec.EventID
                })
                .ToListAsync();
        }

        public async Task<List<EventResponseDTO>> GetEventsForVolunteerAsync(GetEventsRequestDTO request)
        {
            return await _context.Events
                .Where(e => e.VolunteerCourse.Volunteer.VolunteerID == request.VolunteerID &&
                            e.VolunteerCourse.VolunteerCourseID == request.CourseID)
                .Select(e => new EventResponseDTO
                {
                    VolunteerName = e.VolunteerCourse.Volunteer.Student.User.UserName,
                    CourseName = e.VolunteerCourse.Course.CourseName,
                    Title = e.Title,
                    Location = e.Location.ToString(),//converting enum to string
                    EventDescription = e.EventDescription,
                    EventDetails = e.EventDetailes,
                    EventStatus = e.EventStatus,
                    Capacity = e.Capacity,
                    EventType = (EventType)e.EventType,//converting int to Enum
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    EventAddress = e.EventAddress
                })
                .ToListAsync();
        }

        public async Task<List<SessionResponseDTO>> GetEventSessionsAsync(GetEventSessionRequestDTO request)
        {
            return await _context.Sessions
                .Where(s => s.EventID == request.EventID)
                .Select(s => new SessionResponseDTO
                {
                    CourseName = s.Event.VolunteerCourse.Course.CourseName,
                    EventTitle = s.Event.Title,
                    Location = s.Event.Location.ToString(),
                    EventDescription = s.Event.EventDescription,
                    EventDetails = s.Event.EventDetailes,
                    EventStatus = s.Event.EventStatus,
                    Capacity = s.Capacity,
                    EventType = (EventType)s.Event.EventType,//converting int to Enum type --> EventType
                    StartTime = s.Event.StartTime,
                    EndTime = s.Event.EndTime,
                    EventAddress = s.Event.EventAddress
                })
                .ToListAsync();
        }
    }

}
