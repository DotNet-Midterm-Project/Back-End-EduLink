namespace EduLink.Models.DTO.Request
{
    public class AddGroupMemberReqDto
    {
        public int StudentID { get; set; }
        public int GroupId { get; set; }
        public MemberRole Role { get; set; }
    }
}
