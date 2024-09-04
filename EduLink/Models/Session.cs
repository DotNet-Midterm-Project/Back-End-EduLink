using System.ComponentModel.DataAnnotations;

namespace EduLink.Models
{

    public enum SessionStatus
    {
        open = 1,
        Closed = 2,
        Cancel = 3
    }
    public class Session
    {
        public int SessionID { get; set; }
        public int EventID { get; set; }
        public Event Event { get; set; } 
        [MaxLength(100)]
        public string StartDate { get; set; }
        [MaxLength(100)]
        public string EndDate { get; set; }
        [MaxLength(200)]
        public string Details { get; set; }
        public int Capacity { get; set; }
        public SessionStatus SessionStatus { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    }
}
