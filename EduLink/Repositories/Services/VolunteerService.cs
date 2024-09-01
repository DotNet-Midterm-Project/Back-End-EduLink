using EduLink.Data;
using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace EduLink.Repositories.Services
{
    public class VolunteerService : IVolunteer
    {
        private readonly EduLinkDbContext _context;

        public VolunteerService(EduLinkDbContext context)
        {
            _context = context;
        }

        public async Task<List<VolunteerCourseDTO>> GetVolunteerCoursesAsync(int volunteerID)
        {
            var courses = await _context.VolunteerCourses
                .Where(vc => vc.VolunteerID == volunteerID)
                .Select(vc => new VolunteerCourseDTO
                {
                    CourseID = vc.CourseID,
                    CourseName = vc.Course.CourseName
                }).ToListAsync();

            return courses;
        }
        public async Task<MessageResponseDTO> AddEducationalContentAsync(EducationalContentDTO dto)
        {

            var newContent = new EductionalContent
            {
                CourseID = dto.CourseID,
                VolunteerID = dto.VolunteerID,
                ContentType = dto.ContentType,
                ContentDescription = dto.ContentDescription
            };

            _context.EductionalContents.Add(newContent);
            await _context.SaveChangesAsync();


            return new MessageResponseDTO
            {
                Message = $"The content added successfully: {dto.ContentType}"
            };

        }

        public async Task<GetEducationalContentResponseDTO> GetEducationalContentForEachCourseAsync(int volunteerID, int courseID)
        {
            var contents = await _context.EductionalContents
                .Where(ec => ec.VolunteerID == volunteerID && ec.CourseID == courseID)
                .Select(ec => new EducationalContentDtoResponse
                {
                    ContentType = ec.ContentType,
                    ContentDescription = ec.ContentDescription
                }).ToListAsync();

            return new GetEducationalContentResponseDTO
            {
                EducationalContents = contents
            };
        }

        public async Task<MessageResponseDTO> AddArticleAsync(ArticleDTO dto)
        {
            // Create a new Article object from the DTO
            var newArticle = new Article
            {
                VolunteerID = dto.VolunteerID,
                Title = dto.Title,
                Description = dto.Description,
                PublicationDate = dto.PublicationDate,
                AuthorName = dto.AuthorName
            };

            // Add the new Article to the context
            _context.Articles.Add(newArticle);
            await _context.SaveChangesAsync();

            // Return a response message
            return new MessageResponseDTO
            {
                Message = $"The article '{dto.Title}' added successfully."
            };
        }
        public async Task<MessageResponseDTO> DeleteArticleAsync(int volunteerId, int articleId)
        {
            // Find the article by ID
            var article = await _context.Articles
                .FirstOrDefaultAsync(a => a.ArticleID == articleId && a.VolunteerID == volunteerId);

            if (article == null)
            {
                return new MessageResponseDTO
                {
                    Message = "Article not found or you do not have permission to delete this article."
                };
            }

            // Remove the article from the context
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            // Return a response message
            return new MessageResponseDTO
            {
                Message = $"The article '{article.Title}' deleted successfully."
            };
        }

        public async Task<List<ReservationDtoResponse>> GetAllReservationAsync(ReservationReqDTO reservation)
        {
            var reservations = await _context.Reservations
                .Where(r => r.VolunteerID == reservation.VolunteerID && r.CourseID == reservation.CourseID)
                .ToListAsync();

            var ReservationResponse = reservations
                .Select(r => new ReservationDtoResponse
                {
                    CourseID = r.CourseID,
                    VolunteerID = r.VolunteerID,
                    Date = r.Date,
                    EndTime = r.EndTime,
                    IsAvailable = r.IsAvailable,
                    StartTime = r.StartTime,
                }).ToList();

            return ReservationResponse;
        }

        public async Task<MessageResponseDTO> DeleteReservationAsync(DeleteReservationDTO deleteReservationRequest)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.VolunteerID == deleteReservationRequest.VolunteerID
                                        && r.CourseID == deleteReservationRequest.CourseID
                                        && r.ReservationID == deleteReservationRequest.ReservationID);

            if (reservation == null)
            {
                return new MessageResponseDTO { Message = "Reservation not found" };
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return new MessageResponseDTO { Message = "Delete reservation successfully" };
        }

        public async Task<MessageResponseDTO> UpdateReservationAsync(UpdateReservationReqDTO updateReservationRequest)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.VolunteerID == updateReservationRequest.VolunteerID
                                        && r.CourseID == updateReservationRequest.CourseID
                                        && r.ReservationID == updateReservationRequest.ReservationID);

            if (reservation == null)
            {
                return new MessageResponseDTO { Message = "Reservation not found" };
            }

            reservation.StartTime = updateReservationRequest.StartTime;
            reservation.EndTime = updateReservationRequest.EndTime;
            reservation.Date = updateReservationRequest.Date;

            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();

            return new MessageResponseDTO { Message = "Update reservation successfully" };
        }

        public async Task<MessageResponseDTO> AddWorkshopAsync(AddWorkshopReqDTO addWorkshopRequest)
        {
            var volunteerExists = await _context.Volunteers.AnyAsync(v => v.VolunteerID == addWorkshopRequest.VolunteerID);
            if (!volunteerExists)
            {
                return new MessageResponseDTO
                {
                    Message = "Invalid VolunteerID. The volunteer does not exist."
                };
            }

            var workshop = new WorkShop
            {
                VolunteerID = addWorkshopRequest.VolunteerID,
                Title = addWorkshopRequest.Title,
                Description = addWorkshopRequest.Description,
                Date = addWorkshopRequest.Date,
                SessionLink = addWorkshopRequest.SessionLink,
                Capasity = addWorkshopRequest.Capasity
            };

            _context.WorkShops.Add(workshop);
            await _context.SaveChangesAsync();
                 
            return new MessageResponseDTO
            {
                Message = "The workshop added successfully"
            };
        }
    }
}
