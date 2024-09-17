using Google.Apis.Calendar.v3.Data;
using System.ComponentModel.DataAnnotations;

namespace EduLink.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        [MaxLength(200)]
        public string GroupName { get; set; }
        public int LeaderID { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }

        // Navigation property
        public Student Leader { get; set; }
        public virtual ICollection<GroupMember> Members { get; set; }
        public virtual ICollection<ProjectTask> Tasks { get; set; }
        public virtual ICollection<Meeting> Meetings { get; set; }
    }
}
