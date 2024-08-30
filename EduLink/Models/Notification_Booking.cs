namespace EduLink.Models
{
    public class Notification_Booking
    {
        public int NotificationID { get; set; }
        public int BookingID {  get; set; }
        public string StudentID { get; set; }
        public DateTime DateSent { get; set; }
        public string Message { get; set; }
        public Student Student { get; set; }
        public Booking booking { get; set; }
    }
}
