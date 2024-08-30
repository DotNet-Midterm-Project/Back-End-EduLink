using Microsoft.EntityFrameworkCore;

namespace EduLink.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public ICollection<Department_Courses> department_Courses { get; set; } = new List<Department_Courses>();


    }
}
