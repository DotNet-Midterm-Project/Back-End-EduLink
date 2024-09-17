namespace EduLink.Models
{
    public class GroupMember
    {
        public int GroupMemberId { get; set; }
        public int GroupId { get; set; }
        public int StudentID { get; set; }
        public MemberRole Role { get; set; } // Change to enum type

        // Navigation properties
        public virtual Group Group { get; set; }
        public virtual Student Student { get; set; }
        public virtual ICollection<ProjectTask> AssignedTasks { get; set; }  // Tasks assigned to this member
    }

    public enum MemberRole
    {
        Member,
        Leader,
        Moderator,
        Contributor
    }
}
