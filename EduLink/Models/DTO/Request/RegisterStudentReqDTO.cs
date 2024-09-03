using System.ComponentModel.DataAnnotations;

namespace EduLink.Models.DTO.Request
{
    public class RegisterStudentReqDTO
    {
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public int DepartmentID { get; set; }
    }
}
