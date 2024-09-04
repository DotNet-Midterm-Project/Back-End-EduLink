namespace EduLink.Models.DTO.Response
{
    public class EventResDTO
    {
        public int EventID { get; set; }
        public string Title { get; set; }
        public EventLocation Location { get; set; }
        public string EventDescription { get; set; }
        public string Details { get; set; }
        public EventType EventType { get; set; }
        public EventStatus EventStatus { get; set; }
        public int Capacity { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public string EventAdress { get; set; }
  
    }
}
