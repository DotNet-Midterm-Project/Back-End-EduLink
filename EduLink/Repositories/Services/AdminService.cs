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


            eduLinkDbContext.Courses.Add(course);


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
                return "Department not found.";
            }


            var departmentCourse = new DepartmentCourses
            {
                CourseID = CourseID,
                DepartmentID = DepartmentID,
            };


            eduLinkDbContext.DepartmentCourses.Add(departmentCourse);
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
            eduLinkDbContext.Departments.Add(NewDepartment);

            await eduLinkDbContext.SaveChangesAsync();

            return "Added Successfully";

        }
        public async Task<string> AddStudentToVolunteer(int StudentId)
        {
            var student = await eduLinkDbContext.Students.FindAsync(StudentId);
            if (student == null)
            {
                return "student not Found";
            }
            var addStudentToVolunteer = new Volunteer
            {
                StudentID = StudentId,
                SkillDescription = student.User.Skills
            };
            var User = student.User;

            await userManager.AddToRoleAsync(User, "Volunteer");

            eduLinkDbContext.Volunteers.Add(addStudentToVolunteer);
            await eduLinkDbContext.SaveChangesAsync();

            return "Volunteer Approved Successfully";
        }

        public async Task<string> DeleteArticle(int ArticleId)
        {
            var Article = await eduLinkDbContext.Articles.FindAsync(ArticleId);
            if (Article == null)
            {
                return "Article Not Found ";
            }
            eduLinkDbContext.Articles.Remove(Article);
            await eduLinkDbContext.SaveChangesAsync();
            return "The rticle Removed Successfully";
        }


        public async Task<string> DeleteCourse(int CourseId)
        {
            var Course = await eduLinkDbContext.Courses.FindAsync(CourseId);
            if (Course == null)
            {
                return "Course not Found";
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
                return "Volunteer Not Found";
            }
            var userId = await eduLinkDbContext
                .Students
                .Where(s => s.StudentID == StudentId)
                .Select(u => u.UserID).FirstOrDefaultAsync();
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return "Volunteer user not found in Identity.";
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

        public async Task<List<Course>> GetAllCourses()
        {
            var Courses = await eduLinkDbContext.Courses.ToListAsync();

            return Courses;
        }

        public async Task<List<Volunteer>> GetAllVolunteers()
        {
            var Volunteers = await eduLinkDbContext.Volunteers.ToListAsync();
            return Volunteers;
        }



        public async Task<string> UpdateCourse(int CourseID, UpdateCourseReqDto updateCourseReqDto)
        {

            var course = await eduLinkDbContext.Courses.FindAsync(CourseID);


            if (course == null)
            {
                return "Course not found.";
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
