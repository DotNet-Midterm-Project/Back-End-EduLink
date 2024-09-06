using EduLink.Data;
using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EduLink.Repositories.Services
{
    public class AdminService : IAdmin
    {
        private readonly EduLinkDbContext eduLinkDbContext;
        private readonly UserManager<User> userManager;

        public AdminService(EduLinkDbContext eduLinkDbContext, UserManager<User> userManager)
        {
            this.eduLinkDbContext = eduLinkDbContext;
            this.userManager = userManager;
        }

        public async Task<string> AddCourse(AddCourseReqDto addCourseReqDto)
        {

            var course = new Course
            {
                CourseName = addCourseReqDto.CourseName,
                CourseDescription = addCourseReqDto.CourseDescription,
            };


            await eduLinkDbContext.Courses.AddAsync(course);


            await eduLinkDbContext.SaveChangesAsync();


            return $"The Course was added successfully ";

        }

        public async Task<string> AddCourseToDepartment(int DepartmentID, int CourseID)
        {

            var departmentName = await eduLinkDbContext.Departments
                .Where(e => e.DepartmentID == DepartmentID)
                .Select(e => e.DepartmentName)
                .FirstOrDefaultAsync();

            if (departmentName == null)
            {
                return null;
            }


            var departmentCourse = new DepartmentCourses
            {
                CourseID = CourseID,
                DepartmentID = DepartmentID,
            };


            await eduLinkDbContext.DepartmentCourses.AddAsync(departmentCourse);
            await eduLinkDbContext.SaveChangesAsync();
            return $"The Course was added successfully to the {departmentName} department.";
        }

        public async Task<string> AddDepartment(AddDepartmentReqDto departmentReqDto)
        {
            var NewDepartment = new Department
            {
                DepartmentName = departmentReqDto.DepartmentName,
                Address = departmentReqDto.Address,

            };
            await eduLinkDbContext.Departments.AddAsync(NewDepartment);

            await eduLinkDbContext.SaveChangesAsync();

            return "department   added  Successfully";

        }

        //Aprovall Volunteer
        public async Task<string> AddStudentToVolunteer(int volunteerId)
        {
            var volunteer = await eduLinkDbContext.Volunteers
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

            return "Volunteer Approved Successfully";
        }

        public async Task<string> DeleteArticle(int ArticleId)
        {
            var Article = await eduLinkDbContext.Articles.FindAsync(ArticleId);
            if (Article == null)
            {
                return null;
            }
            eduLinkDbContext.Articles.Remove(Article);
            await eduLinkDbContext.SaveChangesAsync();
            return "The Article Removed Successfully";
        }


        public async Task<string> DeleteCourse(int CourseId)
        {
            var Course = await eduLinkDbContext.Courses.FindAsync(CourseId);
            if (Course == null)
            {
                return null;
            }
            eduLinkDbContext.Courses.Remove(Course);
            await eduLinkDbContext.SaveChangesAsync();
            return "Course deleted Successfully";
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

        public async Task<List<CourseResDTO>> GetAllCourses()
        {
            var courses = await eduLinkDbContext.Courses.ToListAsync();

            var courseDtos = courses.Select(course => new CourseResDTO
            {
                CourseID = course.CourseID,
                CourseName = course.CourseName,
                CourseDescription = course.CourseDescription,
                // Assuming you have a way to get VolunteerID from the course, otherwise set it accordingly

            }).ToList();
            if (courseDtos.Count == 0)
            {
                return null;
            }

            return courseDtos;
        }

        public async Task<List<VolunteerResDTO>> GetAllVolunteers()
        {
            var volunteers = await eduLinkDbContext.Volunteers
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
                .Where(f => f.Booking.Event.VolunteerCourse.VolunteerID == VolunteerId)
                .ToListAsync();


            var feedbackDtos = feedbacks.Select(f => new GetFeedbackVolunteerResDto
            {
                Rating = f.Rating,
                Comment = f.Comment,
                CourseName = f.Booking.Event.VolunteerCourse.Course.CourseName
            }).ToList();

            return feedbackDtos;
        }
    }
}