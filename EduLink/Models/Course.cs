using Microsoft.EntityFrameworkCore;

namespace EduLink.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }

        public ICollection<DepartmentCourses> Department_Courses { get; set; } = new List<DepartmentCourses>();
        public ICollection<Event> Reservations { get; set; }

        public ICollection<VolunteerCourse> VolunteerCourses { get; set; }
        public ICollection<EductionalContent> EductionalContents { get; set; }
    }
}
