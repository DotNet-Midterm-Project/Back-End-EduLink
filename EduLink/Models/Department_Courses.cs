namespace EduLink.Models
{
    public class Department_Courses
    {
        public int CourseID { get; set; }
        public int DepartmentID { get; set; }

        public Department Department { get; set; }
        public Course Course { get; set; }

    }
}
