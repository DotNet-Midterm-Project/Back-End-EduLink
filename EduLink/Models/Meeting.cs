using System.ComponentModel.DataAnnotations;

namespace EduLink.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        [MaxLength(500)]
        public string MeetingLink { get; set; }
        public DateTime ScheduledDate { get; set; }
        [MaxLength(2500)]
        public string Announcement { get; set; }

        // Navigation properties
        public virtual Group Group { get; set; }
    }
}
