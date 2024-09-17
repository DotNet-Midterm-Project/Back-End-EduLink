namespace EduLink.Models.DTO.Request
{
    public class GroupMemberUpdateReqDto
    {
        public int GroupId { get; set; }
        public int MemberId { get; set; }
        public MemberRole Role { get; set; }
    }
}
