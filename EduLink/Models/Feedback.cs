using System.ComponentModel.DataAnnotations;

namespace EduLink.Models
{
    public class Feedback
    {
        public int FeedbackID { get; set; }
        public int BookingID { get; set; }
        public int Rating { get; set; }
        [MaxLength(200)]
        public string? Comment { get; set; }
        public Booking Booking { get; set; }   
    }
}
