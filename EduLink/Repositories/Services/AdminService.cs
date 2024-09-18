using EduLink.Data;
using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Drawing.Printing;

namespace EduLink.Repositories.Services
{
    public class AdminService : IAdmin
    {
        private readonly EduLinkDbContext eduLinkDbContext;
        private readonly UserManager<User> userManager;
        private readonly IFile _file;
        private readonly IEmailService  _email;
        public AdminService(EduLinkDbContext eduLinkDbContext, UserManager<User> userManager , IFile file, IEmailService email )
        {
            this.eduLinkDbContext = eduLinkDbContext;
            this.userManager = userManager;
            _file = file;
            _email = email;
        }

        public async Task<string> AddCourse(AddCourseReqDto addCourseReqDto)
        {
            // Validate input
            if (string.IsNullOrEmpty(addCourseReqDto.CourseName) || string.IsNullOrEmpty(addCourseReqDto.CourseDescription))
            {
                return null;
            }

            // Create and add course to the database
            var course = new Course
            {
                CourseName = addCourseReqDto.CourseName,
                CourseDescription = addCourseReqDto.CourseDescription,
            };

            await eduLinkDbContext.Courses.AddAsync(course);
            await eduLinkDbContext.SaveChangesAsync();

            return "The Course was added successfully";
        }

        public async Task<string> AddCourseToDepartment(int DepartmentID, int CourseID)
        {
            // Check if department exists
            var department = await eduLinkDbContext.Departments.FindAsync(DepartmentID);
            if (department == null)
            {
                return null;
            }

            // Check if course exists
            var course = await eduLinkDbContext.Courses.FindAsync(CourseID);
            if (course == null)
            {
                return null;
            }

            // Add course to department
            var departmentCourse = new DepartmentCourses
            {
                CourseID = CourseID,
                DepartmentID = DepartmentID,
            };
            var CheckCourseInTheDepartment = await eduLinkDbContext.DepartmentCourses
                .Where(e => e.CourseID == departmentCourse.CourseID && e.DepartmentID == departmentCourse.DepartmentID)
                .FirstOrDefaultAsync();
            if (CheckCourseInTheDepartment != null)
            {
                return ($"The course '{course.CourseName}' is already associated with the department '{department.DepartmentName}'.");
            }
            await eduLinkDbContext.DepartmentCourses.AddAsync(departmentCourse);
            await eduLinkDbContext.SaveChangesAsync();

            return $"The Course was added successfully to the {department.DepartmentName} department.";
        }

        public async Task<string> AddDepartmentAsync(AddDepartmentReqDto departmentReqDto)
        {
            var newDepartment = new Department
            {
                DepartmentName = departmentReqDto.DepartmentName,
                Address = departmentReqDto.Address
            };

            await eduLinkDbContext.Departments.AddAsync(newDepartment);
            await eduLinkDbContext.SaveChangesAsync();

            return "Department added successfully.";
        }

        //Aprovall Volunteer
        public async Task<string> AddStudentToVolunteer(int volunteerId)
        {
            var volunteer = await eduLinkDbContext.Volunteers
                .Include(v => v.VolunteerCourse)
        .Include(v => v.Student) 
        .ThenInclude(s => s.User) 
        
        .FirstOrDefaultAsync(v => v.VolunteerID == volunteerId);
            if (volunteer == null)
            {
                return null;
            }
            volunteer.Approve = true;
          
            var user = volunteer.Student.User;

            await userManager.AddToRoleAsync(user, "Volunteer");
            await eduLinkDbContext.SaveChangesAsync();
           
            var studentEmaile = user.Email;
            var emailSubject = $"New Event: Volunteer Approval";
             var emailDescriptionHtml = $@"
            <p>Dear {user.UserName},</p>
            <p>We are pleased to inform you that you have been approved as a volunteer.</p>
            <p>Please log in to the system to check your activities and responsibilities.</p>
            <p>If you have any questions, feel free to reach out.</p>
            <p>Best regards,</p> 
            <p>EduLink Team</p>";
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(emailDescriptionHtml);
            var emailDescriptionPlain = htmlDoc.DocumentNode.InnerText;
            if (user.Email != null)
            {

                await _email.SendEmailAsync(studentEmaile, emailSubject, emailDescriptionHtml);
            }


            return "Volunteer Approved Successfully";
        }

        public async Task<string> DeleteArticleAsync(int articleId)
        {
            var article = await eduLinkDbContext.Articles.FindAsync(articleId);
            if (article == null)
            {
                return $"Article with ID {articleId} not found.";
            }
            if (article.ArticleFile != null) {
               await _file.DeleteFileAsync(article.ArticleFile);
            
            }


            eduLinkDbContext.Articles.Remove(article);
            await eduLinkDbContext.SaveChangesAsync();

            return "The article was removed successfully.";
        }


        public async Task<string> DeleteCourse(int CourseId)
        {
            // Check if course exists
            var course = await eduLinkDbContext.Courses.FindAsync(CourseId);
            if (course == null)
            {
                return null;
            }

            // Remove course
            eduLinkDbContext.Courses.Remove(course);
            await eduLinkDbContext.SaveChangesAsync();

            return "Course deleted successfully.";
        }

        public async Task<string> DeleteVolunteer(int VolunteerId)
        {
            var Volunteer = await eduLinkDbContext.Volunteers.FindAsync(VolunteerId);
            var StudentId = await eduLinkDbContext.Volunteers
                .Where(v => v.VolunteerID == VolunteerId)
                .Select(e => e.StudentID)
                .FirstOrDefaultAsync();
            if (Volunteer == null)
            {
                return null;
            }
            var userId = await eduLinkDbContext
                .Students
                .Where(s => s.StudentID == StudentId)
                .Select(u => u.UserID).FirstOrDefaultAsync();
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                if (role == "Volunteer")
                    await userManager.RemoveFromRoleAsync(user, role);
            }
            eduLinkDbContext.Volunteers.Remove(Volunteer);

            await eduLinkDbContext.SaveChangesAsync();
            return "Volunteer and associated roles were removed successfully.";
        }

        public async Task<List<CourseResDTO>> GetAllCoursesAsync(string? filterName, int pageNumber, int pageSize)
        {
            // Start with all courses as a queryable entity
            var coursesQuery = eduLinkDbContext.Courses.AsQueryable();

            // Apply filtering if a filter is provided
            if (!string.IsNullOrEmpty(filterName))
            {
                coursesQuery = coursesQuery.Where(c => c.CourseName.Contains(filterName));
            }

            // Apply pagination
            var skipNum = (pageNumber - 1) * pageSize;

            var courseDtos = await coursesQuery
                .Skip(skipNum)
                .Take(pageSize)
                .Select(course => new CourseResDTO
                {
                    CourseID = course.CourseID,
                    CourseName = course.CourseName,
                    CourseDescription = course.CourseDescription,
                })
                .ToListAsync();

            return courseDtos;
        }

        public async Task<List<VolunteerResDTO>> GetAllVolunteersAsync(string? filterName, int pageNumber, int pageSize, bool? sortByRating , bool? GetBeComeVolunteerRequest)
        {
         
            if (pageNumber < 1 || pageSize < 1)
            {
                throw new ArgumentException("Page number and page size must be greater than zero.");
            }

            // Create the base query
            var volunteersQuery = eduLinkDbContext.Volunteers.AsQueryable();

            
            if (!string.IsNullOrEmpty(filterName))
            {
                volunteersQuery = volunteersQuery.Where(v => v.Student.User.UserName.Contains(filterName));
            }

            
         
            
                if (sortByRating.Value)
                {
                    volunteersQuery = volunteersQuery.OrderByDescending(v => v.Rating);
                }
            

            
                if (GetBeComeVolunteerRequest.Value)
                {
                    volunteersQuery = volunteersQuery.Where(v => v.Approve == false);
                }
            


            var skipNum = (pageNumber - 1) * pageSize;

            
            var volunteers = await volunteersQuery
                .Skip(skipNum)
                .Take(pageSize)
                .Select(v => new VolunteerResDTO
                {
                    VolunteerID = v.VolunteerID,
                    VolunteerName = v.Student.User.UserName,
                    Rating = v.Rating,
                    SkillDescription = v.SkillDescription,
                    Email = v.Student.User.Email,
                    RatingCount = v.RatingAcount,
                    Availability = v.Availability.ToString(),
                    Approve = v.Approve
                })
                .ToListAsync();

            return volunteers;
        }

        public async Task<string> UpdateCourse(int CourseID, UpdateCourseReqDto updateCourseReqDto)
        {

            var course = await eduLinkDbContext.Courses.FindAsync(CourseID);


            if (course == null)
            {
                return null;
            }


            course.CourseName = updateCourseReqDto.CourseName;
            course.CourseDescription = updateCourseReqDto.CourseDescription;


            await eduLinkDbContext.SaveChangesAsync();

            return "Course updated successfully.";
        }
        public async Task<List<GetFeedbackVolunteerResDto>> GetFeedbacksVolunteer(int VolunteerId)
        {

            var feedbacks = await eduLinkDbContext.Feedbacks
                .Include(e=>e.Booking)
                .ThenInclude(e=>e.Event)
                .ThenInclude(e=>e.VolunteerCourse)
                .ThenInclude(e=>e.Course)
                .Where(f => f.Booking.Event.VolunteerCourse.VolunteerID == VolunteerId)
                 
                .ToListAsync();

         
            if (feedbacks == null || feedbacks.Count == 0)
            {
                return new List<GetFeedbackVolunteerResDto>();
            }

            
            var feedbackDtos = feedbacks.Select(f => new GetFeedbackVolunteerResDto
            {
                Rating = f.Rating,
                Comment = f.Comment,
                CourseName = f.Booking?.Event?.VolunteerCourse?.Course?.CourseName ?? "Unknown Course"
            }).ToList();

            return feedbackDtos;
        }

        public async Task<List<DepartmentResDto>> GetAllDepartmentsAsync(string? SearchName, int PageNumber, int PageSize)
        {
            var departmentsQuery = eduLinkDbContext.Departments.AsQueryable();

            // Check if there are any departments
            if (!departmentsQuery.Any())
            {
                return new List<DepartmentResDto>(); // Return an empty list if no departments exist
            }

            // Filter by department name if SearchName is provided
            if (!string.IsNullOrEmpty(SearchName))
            {
                departmentsQuery = departmentsQuery.Where(d => d.DepartmentName.Contains(SearchName));
            }

            // Apply pagination
            var skipNum = (PageNumber - 1) * PageSize;
            var paginatedDepartments = await departmentsQuery
                .Skip(skipNum)
                .Take(PageSize)
                .ToListAsync();

            // Project to DTO
            var departmentDto = paginatedDepartments.Select(e => new DepartmentResDto
            {
                Address = e.Address,
                DepartmentName = e.DepartmentName
            }).ToList();

            return departmentDto;
        }
    }
}