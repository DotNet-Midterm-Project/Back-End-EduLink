using System.ComponentModel.DataAnnotations;

namespace EduLink.Models.DTO.Request
{
    public class ForgotPasswordReqDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

}
