using System.ComponentModel.DataAnnotations;

namespace EduLink.Models
{

    public enum SessionStatus
    {
        Scheduled,
        Open,
        Closed,
        Cancel,
        Ongoing,
        Completed,
        Cancelled,
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
        public string SessionAdress { get; set; }
        public SessionStatus SessionStatus { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    }
}
