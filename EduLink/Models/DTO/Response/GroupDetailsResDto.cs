namespace EduLink.Models.DTO.Response
{
    public class GroupDetailsResDto
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int LeaderID { get; set; }
        public string Description { get; set; }
        public List<GroupMemberDto> Members { get; set; }
    }

    public class GroupMemberDto
    {
        public int StudentID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
