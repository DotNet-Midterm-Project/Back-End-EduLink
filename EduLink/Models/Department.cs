namespace EduLink.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string Address { get; set; }
        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<DepartmentCourses> Department_Courses { get; set; }= new List<DepartmentCourses>();

    }
}
