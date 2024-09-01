using System.ComponentModel.DataAnnotations;

namespace EduLink.Models.DTO.Request
{
    public class RegisterAdminDtoRequest
    {
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
