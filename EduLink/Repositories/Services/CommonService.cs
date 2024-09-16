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
                .Where(e => e.EventType == EventType.Workshop && e.EventStatus != EventStatus.Cancelled)
                .Select(e => new WorkshopResponseDTO
                {
                    VolunteerID = e.VolunteerCourse.Volunteer.VolunteerID,
                    VolunteerName = e.VolunteerCourse.Volunteer.Student.User.UserName,
                    WorkshopName = e.Title,
                    WorkshopDescription = e.EventDescription,
                    WorkshopDateTime = e.StartTime.DateTime,
                    SessionLink = e.Location == EventLocation.Online ? e.EventAddress : "The session link will be sent later",
                    Capacity = e.Capacity
                })
                .ToListAsync();
        }

        public async Task<ArticlesResDTO> GetAllArticlesAsync()
        {
            var articles = await _context.Articles
                .Where(a => a.Status == ArticleStatus.Visible)
                .Select(a => new ArticleDTO
                {
                    ArticleID = a.ArticleID,
                    VolunteerID = a.VolunteerID,
                    VolunteerName = a.Volunteer.Student.User.UserName,
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

        public async Task<ArticlesResDTO> GetArticlesByVolunteerIdAsync(int volunteerId)
        {
            var articles = await _context.Articles
                .Where(a => a.VolunteerID == volunteerId && a.Status == ArticleStatus.Visible)
                .Select(a => new ArticleDTO
                {
                    ArticleID = a.ArticleID,
                    VolunteerID = a.VolunteerID,
                    VolunteerName = a.Volunteer.Student.User.UserName,
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

        public async Task<List<EventContentResDTO>> GetEventContentByEventIdAsync(int eventId)
        {
            var contents = await _context.EventContents
                .Where(ec => ec.EventID == eventId)
                .Select(ec => new EventContentResDTO
                {
                    ContentID = ec.ContentID,
                    ContentAddress = ec.ContentAddress,
                    ContentType = ec.ContentType.ToString(),
                    ContentDescription = ec.ContentDescription,
                    ContentName = ec.ContentName,
                    EventID = ec.EventID
                })
                .ToListAsync();

            return contents;
        }

        public async Task<List<EventResDTO>> GetEventsByVolunteerAndCourseAsync(int volunteerId, int courseId)
        {
            var events = await _context.Events
                 .Where(e => e.VolunteerCourse.VolunteerID == volunteerId && e.VolunteerCourse.CourseID == courseId)
                 .Select(e => new EventResDTO
                 {
                     EventID = e.EventID,
                     VolunteerName = e.VolunteerCourse.Volunteer.Student.User.UserName,
                     CourseName = e.VolunteerCourse.Course.CourseName,
                     Title = e.Title,
                     Location = e.Location.ToString(),
                     EventDescription = e.EventDescription,
                     Details = e.EventDetailes,
                     EventStatus = e.EventStatus.ToString(),
                     Capacity = e.Capacity,
                     EventType = e.EventType.ToString(),
                     StartTime = e.StartTime,
                     EndTime = e.EndTime,
                     EventAddress = e.EventAddress
                 })
                 .ToListAsync();

            return events;
        }

        public async Task<List<SessionResponseDTO>> GetSessionsByEventAsync(int eventId)
        {
            var sessions = await _context.Sessions
                .Where(s => s.EventID == eventId)
                .Select(s => new SessionResponseDTO
                {
                    CourseName = s.Event.VolunteerCourse.Course.CourseName,
                    EventTitle = s.Event.Title,
                    Location = s.Event.Location.ToString(),
                    EventDescription = s.Event.EventDescription,
                    EventDetails = s.Event.EventDetailes,
                    EventStatus = s.SessionStatus.ToString(),
                    Capacity = s.Capacity,
                    EventType = s.Event.EventType.ToString(),
                    StartTime = s.StartDate,
                    EndTime = s.EndDate,
                    EventAddress = s.Event.EventAddress
                })
                .ToListAsync();
            return sessions;
        }
  


        public async Task<ArticleDetailsResDTO> GetArticleByIdAsync(int id)
        {
            var article = await _context.Articles
                .Include(a => a.Volunteer)               
                .ThenInclude(v => v.Student)           
                .ThenInclude(s => s.User)                
                .Include(a => a.Comments)              
                .ThenInclude(c => c.User)               
                .Include(a => a.Likes)                   
                .FirstOrDefaultAsync(a => a.ArticleID == id);

            if (article == null)
            {
                return null;
            }

            
            var commentsDto = article.Comments.Select(c => new CommentDTO
            {
                CommentID = c.CommentID,
                Content = c.CommentText,
                CommenterName = c.User.UserName, 
                CommentDate = c.CreatedAt,
            }).ToList();

           
            var articleDetailsDto = new ArticleDetailsResDTO
            {
                ArticleID = article.ArticleID,
                VolunteerID = article.VolunteerID,
                VolunteerName = article.Volunteer.Student.User.UserName,
                Title = article.Title,
                ArticleContent = article.ArticleContent,
                PublicationDate = article.PublicationDate,
                Status = article.Status.ToString(),
                LikesCount = article.Likes.Count, 
                Comments = commentsDto 
            };

            return articleDetailsDto;
        }

    }


}
