namespace EduLink.Models.DTO.Request
{
    public class UpdateUserReqDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}
