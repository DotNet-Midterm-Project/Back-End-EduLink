namespace EduLink.Models
{
    public class Booking
    {
        public int  BookingID { get; set; }
        public string StudentID { get; set; }
        public Student Student { get; set; }
        public int ReservationID { get; set; }
        public Reservation Reservation { get; set; }

        public string SessionStatus {  get; set; }
        public string SessionLink { get; set; }

    }
}
