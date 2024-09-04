namespace EduLink.Models.DTO.Response
{
    public class RegisterAdminResDTO
    {
        public string AdminID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public IList<string> Roles { get; set; }
    }
}
