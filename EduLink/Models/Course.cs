using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EduLink.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        [MaxLength(200)]
        public string CourseName { get; set; }
        [MaxLength(1000)]
        public string CourseDescription { get; set; }


        public ICollection<DepartmentCourses> DepartmentCourses { get; set; } = new List<DepartmentCourses>();
        public ICollection<Event> Events { get; set; }

        public ICollection<VolunteerCourse> volunteerCourses { get; set; }
        
    }
}
