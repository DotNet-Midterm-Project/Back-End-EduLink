using Microsoft.EntityFrameworkCore;

namespace EduLink.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }

        public ICollection<DepartmentCourses> DepartmentCourses { get; set; } = new List<DepartmentCourses>();
        public ICollection<Event> Events { get; set; }

        public ICollection<VolunteerCourse> volunteerCourses { get; set; }
        
    }
}
