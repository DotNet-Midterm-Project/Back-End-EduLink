namespace EduLink.Models.DTO.Response
{
    public class RegisterStudentDtoResponse
    {
        public string StudentID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int DepartmentID { get; set; }
        public string Token { get; set; }
        public IList<string> Roles { get; set; }
    }
}
