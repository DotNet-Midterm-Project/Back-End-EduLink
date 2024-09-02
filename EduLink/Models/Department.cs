using System.ComponentModel.DataAnnotations;

namespace EduLink.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }
        [MaxLength(200)]
        public string DepartmentName { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }
       
        public ICollection<DepartmentCourses> Department_Courses { get; set; }= new List<DepartmentCourses>();

    }
}
