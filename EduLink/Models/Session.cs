using System.ComponentModel.DataAnnotations;

namespace EduLink.Models
{

    public enum SessionStatus
    {
        Scheduled=1,
        Open = 2,
        Closed = 3,
        Cancel = 4,
        Ongoing=5,
        Completed=6,
        Cancelled=7,
    }
    public class Session
    {
        public int SessionID { get; set; }
        public int EventID { get; set; }
        public Event Event { get; set; } 
        [MaxLength(100)]
        public DateTimeOffset StartDate { get; set; }
        [MaxLength(100)]
        public DateTimeOffset EndDate { get; set; }
        [MaxLength(200)]
        public string Details { get; set; }
        public int Capacity { get; set; }
        public SessionStatus SessionStatus { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    }
}
