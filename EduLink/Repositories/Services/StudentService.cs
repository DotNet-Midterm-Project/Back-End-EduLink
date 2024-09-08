using EduLink.Data;
using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;

using Microsoft.IdentityModel.Tokens;


namespace EduLink.Repositories.Services
{
    public class StudentService : IStudent
    {
        private readonly EduLinkDbContext eduLinkDbContext;

        public StudentService(EduLinkDbContext eduLinkDbContext)
        {
            this.eduLinkDbContext = eduLinkDbContext;
        }



        public async Task<List<DepartmentCoursesResDTO>> GetCoursesByStudentDepartmentAsync(int studentId, string? filterName = null)
        {

            var userId = await eduLinkDbContext.Students
                .Where(s => s.StudentID == studentId)
                .Select(e => e.UserID)
                .FirstOrDefaultAsync();

            var userDepartmentId = await eduLinkDbContext.Users
                .Where(e => e.Id == userId)
                .Select(e => e.DepartmentID)
                .FirstOrDefaultAsync();

            var departmentCourses = eduLinkDbContext.Departments
                .Where(d => d.DepartmentID == userDepartmentId)
                .SelectMany(d => d.Department_Courses)
                .Include(dc => dc.Course) // Eagerly load the Course
                .AsQueryable();

            if (!departmentCourses.Any())
            {
                return null;
            }

            if (!string.IsNullOrEmpty(filterName))
            {
                departmentCourses = departmentCourses.Where(x => x.Course.CourseName.Contains(filterName));
            }

            var departmentDto = await departmentCourses.Select(dc => new DepartmentCoursesResDTO
            {
                CourseId = dc.CourseID,
                Course_Name = dc.Course.CourseName // Ensure Course is not null
            }).ToListAsync();

            return departmentDto;
        }


        public async Task<List<VolunteerResDTO>> GetCourseVolunteersAsync(int courseId)
        {
            var volunteerCourses = await eduLinkDbContext.VolunteerCourses
                .Where(vc => vc.CourseID == courseId)
                .Select(vc => vc.VolunteerID)
                .ToListAsync();

            if (!volunteerCourses.Any())
            {
                return null;
            }


            var volunteers = eduLinkDbContext.Volunteers
                .Where(v => volunteerCourses.Contains(v.VolunteerID))
                .Include(v => v.Student)
                .ThenInclude(s => s.User).OrderByDescending(x => x.Rating.HasValue ? x.Rating.Value : 0);


            var volunteerDto = await volunteers.Select(v => new VolunteerResDTO
            {
                VolunteerID = v.VolunteerID,
                VolunteerName = v.Student.User.UserName , 
                SkillDescription = v.SkillDescription,
                Email = v.Student.User.Email , 
                PhoneNumber = v.Student.User.PhoneNumber ?? "No phone available", // Null check for PhoneNumber
                Rating = v.Rating?? 0,
                RatingCount = v.RatingAcount?? 0, 
                Availability = v.Availability.ToString(), // Convert enum to string
            }).ToListAsync(); // Use async ToList

            return volunteerDto;
        }
        public async Task<List<BookingForStudentResDTO>> GetBookingForStudentAsync(int StudentId)
        {
            var StudentBooks = await eduLinkDbContext.Bookings
                 .Where(b => b.StudentID == StudentId)            
                 .Include(b => b.Event)
                 .ThenInclude(e => e.VolunteerCourse)
                 .ThenInclude(vc => vc.Course)
                 .ToListAsync();

            var BookingDto = StudentBooks.Select(b => new BookingForStudentResDTO
            {         
                EventTitle = b.Event.Title,
                CourseName = b.Event.VolunteerCourse.Course.CourseName,
                EventLocation = b.Event.Location.ToString(),
                StartTime = b.Event.StartTime,
                EndTime = b.Event.EndTime,
                SessionStatus = b.BookingStatus.ToString(),
                EventAddress = b.Event.EventAddress,

            }).ToList();
            return BookingDto;
        }

        public async Task<MessageResDTO> DeleteBookingAsynct(int StudentId, int BookingId)
        {
            var booking = await eduLinkDbContext.Bookings
              .Where(e => e.StudentID == StudentId && e.BookingID == BookingId)
              .FirstOrDefaultAsync();

            
            if (booking != null)
            {
                eduLinkDbContext.Bookings.Remove(booking);
                await eduLinkDbContext.SaveChangesAsync();
                return new MessageResDTO { Message ="Booking removed successfully" };
            }


            return null;
        }

        public async Task<MessageResDTO> AddFeedbackAsync(FeedbackReqDTO feedbackDtoRequest)
        {
            var booking = await eduLinkDbContext.Bookings
                .Include(b => b.Event)
                .ThenInclude(e => e.VolunteerCourse)
                .ThenInclude(vc => vc.Volunteer)
                .FirstOrDefaultAsync(b => b.BookingID == feedbackDtoRequest.BookingId);

            if (booking == null)
            {
                return null;
            }

            if (booking.BookingStatus.ToString() != "Completed")
            {
                return new MessageResDTO { Message = "The booking is not completed." };
            }

            var feedback = new Feedback
            {
                Comment = feedbackDtoRequest.Comment,
                Rating = feedbackDtoRequest.Rating,
                BookingID = feedbackDtoRequest.BookingId
            };

            var checkEvent = await eduLinkDbContext.Events.FindAsync(feedback.BookingID);
            if(checkEvent == null)
            {
                return new MessageResDTO { Message = "not found" };
            }
           
                var volunteer = booking.Event.VolunteerCourse.Volunteer;
                  

                    volunteer.Rating = ((volunteer.Rating * volunteer.RatingAcount) + feedback.Rating) / (volunteer.RatingAcount + 1);
                    volunteer.RatingAcount++;
                    eduLinkDbContext.Feedbacks.Add(feedback);
                    await eduLinkDbContext.SaveChangesAsync();
                
            return null;

            
   
        }

        public async Task<MessageResDTO> RegisterVolunteerAsync(int studentId, VolunteerRegisterReqDTO registerDTO)
        {
           
            var student = await eduLinkDbContext.Students.SingleOrDefaultAsync(s => s.StudentID == studentId);
            if (student == null)
            {
                return new MessageResDTO { Message = "Student not found" };
            }

           
            var volunteer = await eduLinkDbContext.Volunteers
                .Include(v => v.VolunteerCourse) 
                .FirstOrDefaultAsync(v => v.StudentID == studentId);

          
            if (volunteer == null)
            {
                volunteer = new Volunteer
                {
                    SkillDescription = registerDTO.SkillsDescription,
                    StudentID = studentId,
                    Approve = false,
                    Availability = AvailabilityStatus.Available,
                    VolunteerCourse = new List<VolunteerCourse>() 
                };

                await eduLinkDbContext.Volunteers.AddAsync(volunteer);
            }
            else
            {
              
                return new MessageResDTO
                {
                    Message = "You are already registered as a volunteer."
                };
            }

          
            foreach (var courseId in registerDTO.CoursesID)
            {
                
                if (!volunteer.VolunteerCourse.Any(vc => vc.CourseID == courseId))
                {
                    volunteer.VolunteerCourse.Add(new VolunteerCourse
                    {
                        VolunteerID = volunteer.VolunteerID,
                        CourseID = courseId
                    });
                }
            }

           
            await eduLinkDbContext.SaveChangesAsync();

         
            return new MessageResDTO
            {
                Message = "Your application is being considered to become a volunteer."
            };
        }




        public async Task<MessageResDTO> BookSessionAsync(int studentId, int sessionId)
        {
            var session = await eduLinkDbContext.Sessions
                .Include(s => s.Event)
                .FirstOrDefaultAsync(s => s.SessionID == sessionId);

            if (session == null)
            {
                return null;
            }

            var isExist = await eduLinkDbContext.Bookings
                .AnyAsync(e => e.StudentID == studentId && e.SessionID == sessionId);

            if (isExist)
            {
                return new MessageResDTO { Message = "Booked" };
            }

            if (session.Capacity <= 0)
            {
                return new MessageResDTO { Message = "Session is fully booked." };
            }

            var booking = new Booking
            {
                StudentID = studentId,
                EventID = session.EventID,
                SessionID = sessionId,
                BookingStatus = BookingStatusenum.Pending
            };

            eduLinkDbContext.Bookings.Add(booking);
            session.Capacity--;

            if (session.Capacity == 0)
            {
                session.SessionStatus = SessionStatus.Closed;
            }

            await eduLinkDbContext.SaveChangesAsync();

            return new MessageResDTO { Message = $"Registered successfully for session starting on {session.StartDate}." };
        }
        public async Task<string> BookWorkshopAsync(int studentId, int workshopID)
        {           
            var workshop = await eduLinkDbContext.Events.FindAsync(workshopID);            
            if (workshop == null || workshop.EventType != EventType.Workshop)
            {
                return null;
            }
            var isExist = await eduLinkDbContext.Bookings
              .AnyAsync(e => e.StudentID == studentId && e.EventID == workshopID);
            if (isExist)
            {
                return "Booked";
            }
            if (workshop.Capacity <= 0)
            {
                return ($"The workshop '{workshop.Title}' on {workshop.StartTime:MMMM dd, yyyy} is fully booked.");
            }            
            var booking = new Booking
            {
                StudentID = studentId,
                EventID = workshopID,
                BookingStatus = BookingStatusenum.Pending,
               
            };            
            eduLinkDbContext.Bookings.Add(booking);            
            workshop.Capacity--;
            if (workshop.Capacity == 0) {
               workshop.EventStatus = EventStatus.Closed;           
            }
            await eduLinkDbContext.SaveChangesAsync();            
            return $"Registered successfully for {workshop.Title}.";
        }


        public async Task<List<AnnouncementResDTO>> GetAnnouncementsAsync()
        {
            return await eduLinkDbContext.Announcement
                .OrderByDescending(a => a.AnounceDate) // Order by announcement date descending
                .Select(a => new AnnouncementResDTO
                {
                    AnouncementID = a.AnouncementID,
                    Title = a.Title,
                    Message = a.Message,
                    AnounceDate = a.AnounceDate
                })
                .ToListAsync();
        }
    }
}    

