namespace EduLink.Models
{
    public class Booking
    {
        public int  BookingID { get; set; }
        public string StudentID { get; set; }
        public int ReservationID { get; set; }
        public string SessionStatus {  get; set; }
        public string SessionLink { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
    }
}
