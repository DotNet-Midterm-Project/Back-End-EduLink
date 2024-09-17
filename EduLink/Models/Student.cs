namespace EduLink.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }
        public Volunteer Volunteer { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Group> LedGroups { get; set; }
        public virtual ICollection<GroupMember> GroupMemberships { get; set; } 
    }
}
