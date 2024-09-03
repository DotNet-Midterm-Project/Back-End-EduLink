using System.ComponentModel.DataAnnotations;

namespace EduLink.Models.DTO.Request
{
    public class LoginReqDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
