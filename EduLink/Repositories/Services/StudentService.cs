//using EduLink.Data;
//using EduLink.Models;
//using EduLink.Models.DTO.Request;
//using EduLink.Models.DTO.Response;
//using EduLink.Repositories.Interfaces;
//using Microsoft.DotNet.Scaffolding.Shared.Messaging;
//using Microsoft.EntityFrameworkCore;

//namespace EduLink.Repositories.Services
//{
//    public class StudentService : IStudent
//    {
//        private readonly EduLinkDbContext eduLinkDbContext;

//        public StudentService(EduLinkDbContext eduLinkDbContext)
//        {
//            this.eduLinkDbContext = eduLinkDbContext;
//        }



//        public async Task<List<DepartmentCoursesDtoResponse>> GetCoursesByStudentDepartmentAsync(string studentId)
//        {
//            var student = await eduLinkDbContext.Students
//                  .Include(s => s.Department)
//                  .ThenInclude(d => d.Department_Courses)
//                  .ThenInclude(dc => dc.Course)
//                  .FirstOrDefaultAsync(s => s.StudentID == studentId);
//            if (student == null)
//            {
//                return null;
//            }
//            var courses = student.Department.Department_Courses
//             .Select(dc => new DepartmentCoursesDtoResponse
//             {
//                 Course_Name = dc.Course.CourseName
//             })
//             .ToList();
//            return courses;
    
//        }

//        public async Task<List<VolunteerDtoResponse>> GetCourseVolunteerAsync(int courseId)
//        {
//            var course = await eduLinkDbContext.Courses
//                   .Include(c => c.volunteerCourses)
//                   .ThenInclude(vc => vc.Volunteers)
//                   .FirstOrDefaultAsync(c => c.CourseID == courseId);

//            if (course == null)
//            {
//                return null;
//            }


//            var volunteers = course.volunteerCourses
//                            .Select(vc => new VolunteerDtoResponse
//                            {
//                                VolunteerID = vc.Volunteers.VolunteerID,
//                                SkillDescription = vc.Volunteers.SkillDescription,
//                                Rating = vc.Volunteers.Rating,
//                                Email = vc.Volunteers.Student.Student.Email,
//                                PhoneNumber = vc.Volunteers.Student.Student.PhoneNumber
//                            })
//                            .ToList();
//            if (volunteers == null)
//                return null;

//            return volunteers;

//        }

//        public async Task<List<EducationalContentDtoResponse>> GeteducationalcontentAsync(int VolunteerId, int courseId)
//        {
//            var educatoinalContent = await eduLinkDbContext.EductionalContents
//                .Where(e => e.CourseID == courseId && e.VolunteerID == VolunteerId).ToListAsync();

//            var EducationalResponse = educatoinalContent
//                    .Select(e => new EducationalContentDtoResponse
//                    {
//                        CourseID = e.CourseID,
//                        VolunteerID = e.VolunteerID,
//                        ContentDescription = e.ContentDescription,
//                        ContentType = e.ContentType,
//                    })
//            .ToList();
//            if (EducationalResponse == null)
//                return null;

//            return EducationalResponse;

//        }

//        public async Task<List<ReservationDtoResponse>> GetReservationForVolunteerAsync(int VolunteerId, int courseId)
//        {
//            var Reservation = await eduLinkDbContext.Events
//                .Where(e => e.VolunteerID == VolunteerId && e.CourseID == courseId)
//                .ToListAsync();
//            if (Reservation == null)
//            {
//                return null;
//            }

//            var ReservationResponse = Reservation
//                .Select(e => new ReservationDtoResponse
//                {
//                    CourseID = e.CourseID,
//                    VolunteerID = e.VolunteerID,
//                    Date = e.Date,
//                    EndTime = e.EndTime,
//                    IsAvailable = e.IsAvailable,
//                    StartTime = e.StartTime,
//                }).ToList();

//            return ReservationResponse;
//        }
//        public async Task<string> AddBookingAsync(  int reservationId , string studentId)
//        {

//            //var reservation = await eduLinkDbContext.Events
//            //.Include(r => r.Bookings) 
//            //.FirstOrDefaultAsync(r => r.ReservationID == reservationId);
//            var reservation = await eduLinkDbContext.Events.FindAsync(reservationId);

//            if (reservation == null)
//            {
//                return "Reservation not found.";
//            }

            
//            if (!reservation.IsAvailable)
//            {
//                return "Reservation is no longer available.";
//            }

//            // Create a new booking
//            var booking = new Booking
//            {
//                StudentID = studentId,
//                EventID = reservationId,
//                SessionStatus = "Book",
//                SessionLink = "gf"
//            };

            
//            reservation.Bookings.Add(booking);

            
//            reservation.IsAvailable = false;

            
//            await eduLinkDbContext.SaveChangesAsync();

//            return "Booking successfully added.";
//        }
//        public async Task<List<BookingForStudentDtoResponse>> GetBookingAsync(string StudentId, int ReservationId)
//        {
//            var studentBookings = await eduLinkDbContext.Bookings
//          .Include(b => b.Event)
//          .ThenInclude(r => r.Student)
//          .Where(e => e.ReservationID == ReservationId && e.StudentID == StudentId)
//          .ToListAsync();

//            var studentBookingResponse = studentBookings.Select(e => new BookingForStudentDtoResponse
//            {
//                StudentID = e.StudentID,
//                SessionStatus = e.SessionStatus,
//                VolunteerName = e.Reservation.Student.Student.Student.UserName, 
//                CourseID = e.Reservation.CourseID,
//                Date = e.Reservation.Date,
//                StartTime = e.Reservation.StartTime,
//                EndTime = e.Reservation.EndTime,
//                SessionLink = e.SessionLink,
//                ReservationID = e.ReservationID
//            }).ToList();
//            if (studentBookingResponse == null)
//                return null;


//            return studentBookingResponse;
//        }

//        public async Task<string> AddFeedbackAsync(FeedbackDtoRequest bookingDtoRequest)
//        {
//            var booking = await eduLinkDbContext.Bookings.FindAsync(bookingDtoRequest.BookingId);

//            if (booking == null)
//                return null;

//            if (booking.SessionStatus != "Ended")
//                return "Cannot add feedback before the session ends.";

//            var feedback = new Feedback
//            {
//                Comment = bookingDtoRequest.Comment,
//                Rating = bookingDtoRequest.Rating,
//                BookingID = bookingDtoRequest.BookingId  
//            };

//            eduLinkDbContext.Feedbacks.Add(feedback);
//            booking.Feedbacks = feedback;  
//            await eduLinkDbContext.SaveChangesAsync();

//            return $"Added feedback successfully on Booking : BookingID ={bookingDtoRequest.BookingId}";
//        }

//        public async Task<string> WorkshopsRegistrationAsync(string StudentId, int WorkshopID)
//        {
//            var workshop = await eduLinkDbContext.WorkShops
//            .Include(w => w.WorkshopsRegistrations)
//            .FirstOrDefaultAsync(w => w.WorkShopID == WorkshopID);

//            if (workshop == null)
//            {
//                return "Workshop not found.";
//            }

//            if (workshop.WorkshopsRegistrations.Any(wr => wr.StudentID == StudentId))
//            {
//                return "Student is already registered for this workshop.";
//            }

//            if (workshop.WorkshopsRegistrations.Count >= workshop.Capasity)
//            {
//                return "Workshop capacity reached.";
//            }

//            var workShopRegistration = new WorkshopsRegistration
//            {
//                WorkShopID = WorkshopID,
//                StudentID = StudentId,
//            };

//            eduLinkDbContext.Add(workShopRegistration);
//            await eduLinkDbContext.SaveChangesAsync();

//            return $"Register successfully on {workshop.Title}";

//        }


//        public async Task<MessageResponseDTO> RegisterVolunteerAsync(VolunteerRegisterDtoReq registerDTO)
//        {
//            var student=await eduLinkDbContext.Students.SingleOrDefaultAsync(S=>S.StudentID==registerDTO.StudentID);
//            if (student == null)
//            {
//                return    new MessageResponseDTO { Message = "Student Not found", };
                   
//            }
//            // Fetch the volunteer if they exist, otherwise create a new one
//            var volunteer = await eduLinkDbContext.Volunteers
//                              .Include(v => v.VolunteerCourse)
//                               .FirstOrDefaultAsync(v => v.StudentID == registerDTO.StudentID);


//            if (volunteer == null)
//            {
//                volunteer = new Student
//                {
//                    SkillDescription = registerDTO.SkillsDescription,
//                    StudentID = registerDTO.StudentID,
//                    Aprove = false,
//                    Availability = true,
//                    // Initialize the VolunteerCourse collection
//            VolunteerCourse = new List<VolunteerCourse>()
//                };

//                await eduLinkDbContext.Volunteers.AddAsync(volunteer);
//            }
//            else
//            {
//                return new MessageResponseDTO
//                {
//                    Message = "You are already registered as a volunteer."
//                };
//            }

//            // Add the courses to the volunteer
//            foreach (var courseId in registerDTO.CoursesID)
//            {
//                // Check if the volunteer is already associated with the course
//                if (!volunteer.VolunteerCourse.Any(vc => vc.CourseID == courseId))
//                {
//                    volunteer.VolunteerCourse.Add(new VolunteerCourse
//                    {
//                        VolunteerID = volunteer.VolunteerID,
//                        CourseID = courseId
//                    });
//                }
//            }

//            // Save the changes to the context
//            await eduLinkDbContext.SaveChangesAsync();

//            // Return a response message
//            return new MessageResponseDTO
//            {
//                Message = "Your application is being considered to become a volunteer."
//            };

//        }


//        public async Task<List<NotificationBookingDtoResponse>> GetNotificationsByStudentAsync(string studentId)
//        {
//            return await eduLinkDbContext.NotificationBookings
//                .Where(n => n.StudentID == studentId)
//                .Select(n => new NotificationBookingDtoResponse
//                {
//                    NotificationID = n.NotificationID,
//                    Message = n.Message,
//                    DateSend = n.DateSent
//                })
//                .ToListAsync();
//        }
//    }
//}
