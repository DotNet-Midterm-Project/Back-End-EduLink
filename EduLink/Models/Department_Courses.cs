namespace EduLink.Models
{
    public class Department_Courses
    {
        public int CourseID { get; set; }
        public int DepartmentID { get; set; }

        public Department department { get; set; }
        public Course course { get; set; }

    }
}
