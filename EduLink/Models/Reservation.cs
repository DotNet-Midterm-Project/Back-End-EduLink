namespace EduLink.Models
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public int VolunteerID { get; set; }
        public Volunteer Volunteer { get; set; }
        public int CourseID { get; set; }
        public Course Course { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime Date { get; set; }
        public bool IsAvailable { get; set; } = true;
        public ICollection<Booking> Bookings { get; set; }
    }
}
