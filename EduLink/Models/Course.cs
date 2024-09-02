using Microsoft.EntityFrameworkCore;

namespace EduLink.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }

        public ICollection<Department_Courses> Department_Courses { get; set; } = new List<Department_Courses>();
        public ICollection<Event> Reservations { get; set; }

        public ICollection<VolunteerCourse> volunteerCourses { get; set; }
        public ICollection<EductionalContent> EductionalContents { get; set; }
    }
}
