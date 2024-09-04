using System.ComponentModel.DataAnnotations;

namespace EduLink.Models
{
    public class Announcement
    {
        public int AnouncementID { get; set; }
        public int EventID { get; set; }
        public int SessionID { get; set; }
        [MaxLength(500)]
        public string Title { get; set; }
        [MaxLength(2000)]
        public string Message { get; set; }
        public DateTime AnounceDate { get; set; }   

        public Event Event { get; set; }
    }
}
