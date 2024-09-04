using System.ComponentModel.DataAnnotations;

namespace EduLink.Models.DTO.Request
{
    public class RegisterUserReqDTO
    {
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Skills { get; set; }
        public int DepartmentID { get; set; }
    }
}
