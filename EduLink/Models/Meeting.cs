namespace EduLink.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string MeetingLink { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Announcement { get; set; }

        // Navigation properties
        public virtual Group Group { get; set; }
    }
}
