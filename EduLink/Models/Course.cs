using Microsoft.EntityFrameworkCore;

namespace EduLink.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public ICollection<DepartmentCourses> Department_Courses { get; set; } = new List<DepartmentCourses>();
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<VolunteerCourse> volunteerCourses { get; set; }
        public ICollection<EductionalContent> EductionalContents { get; set; }
    }
}
