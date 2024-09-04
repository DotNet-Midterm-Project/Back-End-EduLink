namespace EduLink.Models
{
    public class DepartmentCourses
    {
        public int CourseID { get; set; }
        public int DepartmentID { get; set; }
        public Department Department { get; set; }
        public Course Course { get; set; }

    }
}
