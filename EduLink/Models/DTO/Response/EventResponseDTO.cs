namespace EduLink.Models.DTO.Response
{
    public class EventResponseDTO
    {
        public string VolunteerName { get; set; }
        public string CourseName { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string EventDescription { get; set; }
        public string EventDetails { get; set; }
        public EventStatus EventStatus { get; set; }
        public int Capacity { get; set; }
        public EventType EventType { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public string EventAddress { get; set; }
    }
}
