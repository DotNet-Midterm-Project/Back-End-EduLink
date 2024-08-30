namespace EduLink.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public ICollection<Student> students { get; set; } = new List<Student>();
        public ICollection<Department_Courses> department_Courses { get; set; }= new List<Department_Courses>();

    }
}
