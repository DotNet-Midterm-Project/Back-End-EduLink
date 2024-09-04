//using EduLink.Data;
//using EduLink.Models;
//using EduLink.Models.DTO.Request;
//using EduLink.Models.DTO.Response;
//using EduLink.Repositories.Interfaces;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.EntityFrameworkCore;
//namespace EduLink.Repositories.Services
//{
//    public class VolunteerService : IVolunteer
//{
//    private readonly EduLinkDbContext _context;

//    public VolunteerService(EduLinkDbContext context)
//    {
//        _context = context;
//    }

//    public async Task<List<VolunteerCourseResDTO>> GetVolunteerCoursesAsync(int volunteerID)
//    {
//        var courses = await _context.VolunteerCourses
//            .Where(vc => vc.VolunteerID == volunteerID)
//            .Select(vc => new VolunteerCourseResDTO
//            {
//                CourseID = vc.CourseID,
//                CourseName = vc.Courses.CourseName
//            }).ToListAsync();

//        return courses;
//    }
//    public async Task<MessageResDTO> AddEventContentAsync(EventContetnReqDTO dto)
//    {

          
//            var eventExists = await _context.Events.AnyAsync(e => e.EventID == dto.EventID);
//            if (!eventExists)
//            {
//                return new MessageResDTO { Message = "Event not found." };
//            }

      
//            var newContent = new EventContent
//            {
//                EventID = dto.EventID,
//                ContentName = dto.ContentName,
//                ContentType = dto.ContentType,
//                ContentDescription = dto.ContentDescription
//            };

//            await _context.AddAsync(newContent);

//            return new MessageResDTO {  Message = "Event content added successfully." };
        

//    }

//        public async Task<IEnumerable<EventContentResDTO>> GetEventContentsAsync(int volunteerID, int eventID)
//        {
     
//            var eventContent = await _context.Events
//                .Where(e => e.EventID == eventID && e.VolunteerCourse.VolunteerID == volunteerID)
//                .SelectMany(e => e.EventContents)
//                .Select(ec => new EventContentResDTO
//                {
//                    ContentID = ec.ContentID,
//                    ContentName = ec.ContentName,
//                    ContentType = ec.ContentType,
//                    ContentDescription = ec.ContentDescription
//                })
//                .ToListAsync();

//            if (eventContent == null || !eventContent.Any())
//            {
//                throw new InvalidOperationException("No content found for the specified volunteer and event.");
//            }

//            return eventContent;
//        }

//        public async Task<MessageResDTO> AddArticleAsync(ArticleReqDTO dto)
//        {
//            Create a new Article object from the DTO
//        var newArticle = new Article
//        {
//            VolunteerID = dto.VolunteerID,
//            Title = dto.Title,
//            ArticaleContent = dto.ArticaleContent,
//            PublicationDate = dto.PublicationDate,
//            AuthorName = dto.AuthorName
//        };

//            Add the new Article to the context
//        _context.Articles.Add(newArticle);
//            await _context.SaveChangesAsync();

//            Return a response message
//        return new MessageResDTO
//        {
//            Message = $"The article '{dto.Title}' added successfully."
//        };
//        }
//        public async Task<MessageResDTO> DeleteArticleAsync(int volunteerId, int articleId)
//        {
//            Find the article by ID
//           var article = await _context.Articles
//               .FirstOrDefaultAsync(a => a.ArticleID == articleId && a.VolunteerID == volunteerId);

//            if (article == null)
//            {
//                return new MessageResDTO
//                {
//                    Message = "Article not found or you do not have permission to delete this article."
//                };
//            }

//            Remove the article from the context
//            _context.Articles.Remove(article);
//            await _context.SaveChangesAsync();

//            Return a response message
//            return new MessageResDTO
//        {
//            Message = $"The article '{article.Title}' deleted successfully."
//        };
//        }

//        public async Task<List<ReservationResDTO>> GetAllReservationAsync(ReservationReqDTO reservation)
//        {
//            var reservations = await _context.Events
//                .Where(r => r.VolunteerID == reservation.VolunteerID && r.CourseID == reservation.CourseID)
//                .ToListAsync();

//            var ReservationResponse = reservations
//                .Select(r => new ReservationResDTO
//                {
//                    CourseID = r.CourseID,
//                    VolunteerID = r.VolunteerID,
//                    Date = r.Date,
//                    EndTime = r.EndTime,
//                    IsAvailable = r.IsAvailable,
//                    StartTime = r.StartTime,
//                }).ToList();

//            return ReservationResponse;
//        }

//        public async Task<MessageResDTO> DeleteReservationAsync(DeleteReservationReqDTO deleteReservationRequest)
//        {
//            var reservation = await _context.Events
//                .FirstOrDefaultAsync(r => r.VolunteerID == deleteReservationRequest.VolunteerID
//                                        && r.CourseID == deleteReservationRequest.CourseID
//                                        && r.ReservationID == deleteReservationRequest.ReservationID);

//            if (reservation == null)
//            {
//                return new MessageResDTO { Message = "Reservation not found" };
//            }

//            _context.Events.Remove(reservation);
//            await _context.SaveChangesAsync();

//            return new MessageResDTO { Message = "Delete reservation successfully" };
//        }

//        public async Task<MessageResDTO> UpdateReservationAsync(UpdateReservationReqDTO updateReservationRequest)
//        {
//            var reservation = await _context.Events
//                .FirstOrDefaultAsync(r => r.VolunteerID == updateReservationRequest.VolunteerID
//                                        && r.CourseID == updateReservationRequest.CourseID
//                                        && r.ReservationID == updateReservationRequest.ReservationID);

//            if (reservation == null)
//            {
//                return new MessageResDTO { Message = "Reservation not found" };
//            }

//            reservation.StartTime = updateReservationRequest.StartTime;
//            reservation.EndTime = updateReservationRequest.EndTime;
//            reservation.Date = updateReservationRequest.Date;

//            _context.Events.Update(reservation);
//            await _context.SaveChangesAsync();

//            return new MessageResDTO { Message = "Update reservation successfully" };
//        }

//        public async Task<MessageResDTO> AddWorkshopAsync(AddWorkshopReqDTO addWorkshopRequest)
//        {
//            var volunteerExists = await _context.Volunteers.AnyAsync(v => v.VolunteerID == addWorkshopRequest.VolunteerID);
//            if (!volunteerExists)
//            {
//                return new MessageResDTO
//                {
//                    Message = "Invalid VolunteerID. The volunteer does not exist."
//                };
//            }

//            var workshop = new WorkShop
//            {
//                VolunteerID = addWorkshopRequest.VolunteerID,
//                Title = addWorkshopRequest.Title,
//                ArticaleContent = addWorkshopRequest.ArticaleContent,
//                Date = addWorkshopRequest.Date,
//                SessionLink = addWorkshopRequest.SessionLink,
//                Capasity = addWorkshopRequest.Capasity
//            };

//            _context.WorkShops.Add(workshop);
//            await _context.SaveChangesAsync();

//            return new MessageResDTO
//            {
//                Message = "The workshop added successfully"
//            };
//        }

//        public async Task<MessageResDTO> DeleteWorkshopAsync(DeleteWorkshopReqDTO deleteWorkshopRequest)
//        {
//            var workshop = await _context.WorkShops
//                .FirstOrDefaultAsync(w => w.WorkShopID == deleteWorkshopRequest.WorkshopID && w.VolunteerID == deleteWorkshopRequest.VolunteerID);

//            if (workshop == null)
//            {
//                return new MessageResDTO
//                {
//                    Message = "Workshop not found or does not belong to this volunteer."
//                };
//            }

//            _context.WorkShops.Remove(workshop);
//            await _context.SaveChangesAsync();

//            return new MessageResDTO
//            {
//                Message = "The Workshop deleted successfully"
//            };
//        }

//        public async Task<List<WorkshopResDTO>> GetAllWorkshopsAsync(GetAllWorkshopsReqDTO getAllWorkshopsRequest)
//        {
//            Query the workshops for the given volunteer
   
//           var workshops = await _context.WorkShops
//               .Where(w => w.VolunteerID == getAllWorkshopsRequest.VolunteerID)
//               .Select(w => new WorkshopResDTO
//               {
//                   Title = w.Title,
//                   ArticaleContent = w.ArticaleContent,
//                   VolunteerID = w.VolunteerID,
//                   Date = w.Date,
//                   SessionLink = w.SessionLink,
//                   Capasity = w.Capasity
//               })
//               .ToListAsync();

//            return workshops;
//        }

//        public async Task<List<GetWorkshopNotificationRespDTO>> GetWorkshopNotificationsAsync()
//        {
//            var notifications = await _context.NotificationWorkshops
//                .Include(n => n.WorkShop) // Include the related WorkShop entity
//                .Where(n => n.ID > 0) // Filter by WorkshopID and StudentID if necessary
//                .Select(n => new GetWorkshopNotificationRespDTO
//                {
//                    ID = n.ID,
//                    Message = n.Message,
//                    DateSend = n.DateSend,
//                    Capasity = n.WorkShop.Capasity
//                })
//                .ToListAsync();

//            return notifications;
//        }

//        public async Task<ArticleResDTO> GetArticleByIdAsync(int volunteerId, int articleId)
//        {
//            var article = await _context.Articles
//                .FirstOrDefaultAsync(a => a.VolunteerID == volunteerId && a.ArticleID == articleId);

//            if (article == null)
//            {
//                return null;
//            }

//            return new ArticleResDTO
//            {
//                Title = article.Title,
//                ArticaleContent = article.ArticaleContent,
//                AuthorName = article.AuthorName,
//                VolunteerID = article.VolunteerID,
//                PublicationDate = article.PublicationDate
//            };
//        }

//        public async Task<MessageResDTO> AddReservationAsync(AddReservationReqDTO request)
//        {

//            var volunteerCourse = await _context.VolunteerCourses
//                .FirstOrDefaultAsync(vc => vc.VolunteerID == request.VolunteerID && vc.CourseID == request.CourseID);

//            if (volunteerCourse == null)
//            {
//                return new MessageResDTO
//                {
//                    Message = "Student is not associated with the course."
//                };
//            }


//            var reservation = new Event
//            {
//                VolunteerID = request.VolunteerID,
//                CourseID = request.CourseID,
//                StartTime = request.StartTime,
//                EndTime = request.EndTime,
//                EventDate = request.Date
//            };


//            await _context.Events.AddAsync(reservation);
//            await _context.SaveChangesAsync();

//            return new MessageResDTO
//            {
//                Message = "Reservation added successfully."
//            };
//        }
//    }
//}
