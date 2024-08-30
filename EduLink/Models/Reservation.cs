namespace EduLink.Models
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public int VolunteerID { get; set; }
        public int CourseID { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public DateOnly Date { get; set; }
        public bool Status { get; set; }
    }
}
