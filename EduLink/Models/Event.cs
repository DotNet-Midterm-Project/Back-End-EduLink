using System.ComponentModel.DataAnnotations;

namespace EduLink.Models
{
    public enum EventStatus
    {
        Scheduled,
        Ongoing,
        Completed,
        Closed,
        Cancelled
    }

    public enum EventType
    {
        Workshop,       
        PrivateSession
    }
    public enum EventLocation
    {
        Online = 1,
        OnSite = 2,
        Hybrid = 3
    }


    public class Event
    {
        public int EventID { get; set; }
        public int VolunteerCoursID { get; set; }
        public VolunteerCourse VolunteerCourse { get; set; }
        [MaxLength(500)]
        public string Title { get; set; }
        public EventLocation Location { get; set; }
     // public int EventContetntID { get; set; }
        public  ICollection<EventContent> EventContents { get; set; }
        public string? EventBannerImage {  get; set; }

        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        [MaxLength(1000)]
        public string EventDescription { get; set; }
        [MaxLength(2000)]
        public string EventDetailes { get; set; }
        public EventStatus EventStatus { get; set; }
        public int Capacity { get; set; }  // Correct spelling from 'Capasity' to 'Capacity'
        public EventType EventType { get; set; }
        public string? EventAddress { get; set; }
        public int? SessionCount { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        // Add this collection to represent the one-to-many relationship with Announcements
        public ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
        
    }
}
