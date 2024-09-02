namespace EduLink.Models
{
    public enum EventStatus
    {
        Scheduled,
        Ongoing,
        Completed,
        Cancelled
    }

    public enum EventType
    {
        Workshop,       
        PrivateSession
    }


    public class Event
    {
        public int EventID { get; set; }
        public string Title { get; set; }
        public bool Location { get; set; }
        public int VolunteerCoursID { get; set; }
        public VolunteerCourse VolunteerCourse { get; set; }
       
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime EventDate { get; set; }
        public bool EventStatus { get; set; }
        public int Capasity { get; set; }
        public int EventType { get; set; }
        public string? SessionURL { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
