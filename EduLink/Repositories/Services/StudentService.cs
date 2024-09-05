﻿using EduLink.Data;
using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;

namespace EduLink.Repositories.Services
{
    public class StudentService : IStudent
    {
        private readonly EduLinkDbContext eduLinkDbContext;

        public StudentService(EduLinkDbContext eduLinkDbContext)
        {
            this.eduLinkDbContext = eduLinkDbContext;
        }



        public async Task<List<DepartmentCoursesResDTO>> GetCoursesByStudentDepartmentAsync(int studentId)
        {
            // Fetch the UserID for the given student
            var userId = await eduLinkDbContext.Students
                .Where(s => s.StudentID == studentId)
                .Select(e => e.UserID)
                .FirstOrDefaultAsync();

            if (userId == null)
            {
                return new List<DepartmentCoursesResDTO>();
            }

            // Fetch the DepartmentID for the user
            var userDepartmentId = await eduLinkDbContext.Users
                .Where(e => e.Id == userId)
                .Select(e => e.DepartmentID)
                .FirstOrDefaultAsync();

            if (userDepartmentId == null)
            {
                return new List<DepartmentCoursesResDTO>();
            }

            // Fetch the courses for the department
                    var departmentCourses = await eduLinkDbContext.Departments
            .Where(d => d.DepartmentID == userDepartmentId)
            .SelectMany(d => d.Department_Courses)
            .Include(dc => dc.Course) // Eagerly load the Course
            .ToListAsync();

            if (departmentCourses == null || !departmentCourses.Any())
            {
                return new List<DepartmentCoursesResDTO>();
            }

            // Map the department courses to DTO
            var departmentDto = departmentCourses.Select(dc => new DepartmentCoursesResDTO
            {
                CourseId = dc.CourseID,
                Course_Name = dc.Course.CourseName // Ensure Course is not null
            }).ToList();

            return departmentDto;
        }


        public async Task<List<VolunteerResDTO>> GetCourseVolunteersAsync(int courseId)
        {
            // Retrieve the list of Volunteer IDs associated with the specified Course ID
            var volunteers = await eduLinkDbContext.VolunteerCourses
                .Where(c => c.CourseID == courseId)
                .Select(e => e.VolunteerID)
                .ToListAsync();


            var volunteerCourse = await eduLinkDbContext.Volunteers
                .Where(c => volunteers.Contains(c.VolunteerID))
                .ToListAsync();


            var VolunteerDto = volunteerCourse.Select(dc => new VolunteerResDTO
            {
                VolunteerID = dc.VolunteerID,
                VolunteerName = dc.Student.User.UserName,
                SkillDescription = dc.SkillDescription,
                Email = dc.Student.User.Email,
                PhoneNumber = dc.Student.User.PhoneNumber,
                Rating = dc.Rating,
                AcountRating = dc.RatingAcount,
                Availability = dc.Availability.ToString(),
            }).ToList();

            return VolunteerDto;
        }
        public async Task<List<BookingForStudentResDTO>> GetBookingForStudentAsync(int StudentId)
        {
            var StudentBooks = await eduLinkDbContext.Bookings
              .Include(b => b.Event)
               .ThenInclude(e => e.VolunteerCourse)
               .ThenInclude(vc => vc.Course)
                 .Where(b => b.StudentID == StudentId)
                 .ToListAsync();
            var BookingDto = StudentBooks.Select(b => new BookingForStudentResDTO
            {
                EventTitle = b.Event.Title,
                CourseName = b.Event.VolunteerCourse.Course.CourseName,
                EventLocation = b.Event.Location.ToString(), 
                StartTime = b.Event.StartTime,
                EndTime = b.Event.EndTime,
                SessionStatus = b.BookingStatus.ToString(),  
                EventAddress = b.Event.EventAddress
            }).ToList();
            return BookingDto;
        }

        public async Task<string> DeleteBookingAsynct(int StudentId, int BookingId)
        {
            var booking = await eduLinkDbContext.Bookings
              .Where(e => e.StudentID == StudentId && e.BookingID == BookingId)
              .FirstOrDefaultAsync();

            
            if (booking != null)
            {
                eduLinkDbContext.Bookings.Remove(booking);
                await eduLinkDbContext.SaveChangesAsync();
                return "Booking removed successfully";
            }


            return null;
        }

        public async Task<string> AddFeedbackAsync(FeedbackReqDTO bookingDtoRequest)
        {
            var booking = await eduLinkDbContext.Bookings.FindAsync(bookingDtoRequest.BookingId);

            if (booking == null)
                return null;

            if (booking.BookingStatus.ToString() != "Completed")
                return null;

            var Feedback = new Feedback
            {
                Comment = bookingDtoRequest.Comment,
                Rating = bookingDtoRequest.Rating,
                BookingID = bookingDtoRequest.BookingId
            };
            eduLinkDbContext.Feedbacks.Add(Feedback);
            await eduLinkDbContext.SaveChangesAsync();
            return $"Add Feed Back successfully on {booking.BookingID}";
        }





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


//        public async Task<MessageResponseDTO> RegisterVolunteerAsync(VolunteerRegisterReqDTO registerDTO)
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
        //                if (!volunteer.VolunteerCourse.Any(vc => vc.EventID == courseId))
        //                {
        //                    volunteer.VolunteerCourse.Add(new VolunteerCourse
        //                    {
        //                        VolunteerID = volunteer.VolunteerID,
        //                        EventID = courseId
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



        public async Task<string> BookSession(int studentId, int sessionId)
        {
            
            var session = await eduLinkDbContext.Sessions
                .Include(s => s.Event)
                .FirstOrDefaultAsync(s => s.SessionID == sessionId);

            if (session == null)
            {
                return null;
            }

           
            if (session.SessionStatus != SessionStatus.Open)
            {
                return null;
            }

            if (session.Capacity <= 0)
            {
                return "Session is fully booked.";
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

            return $"Registered successfully for session {session.StartDate}.";
        }
        public async Task<string> BookingWorkshop(int studentId, int workshopID)
        {           
            var workshop = await eduLinkDbContext.Events.FindAsync(workshopID);            
            if (workshop == null || workshop.EventType != EventType.Workshop)
            {
                return null;
            }           
            if (workshop.Capacity <= 0)
            {
                return null;
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
    }
}    

