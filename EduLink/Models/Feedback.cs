namespace EduLink.Models
{
    public class Feedback
    {
        public int FeedbackID { get; set; }
        public int BookingID { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
