namespace EduLink.Models
{
    public class Booking
    {
        public int  BookingID { get; set; }
        public int StudentID { get; set; }
        public Student Student { get; set; }

        public int EventID { get; set; }
        public Event Event { get; set; }

        public string SessionStatus {  get; set; }
        public string SessionLink { get; set; }
        public Feedback Feedbacks { get; set; }
    

    }
}
