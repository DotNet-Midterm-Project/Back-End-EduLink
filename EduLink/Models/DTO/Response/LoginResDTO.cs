namespace EduLink.Models.DTO.Response
{
    public class LoginResDTO
    {
        public string UserId { get; set; }
        
        public string UserName { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public IList<string> Roles { get; set; }
    }
}
