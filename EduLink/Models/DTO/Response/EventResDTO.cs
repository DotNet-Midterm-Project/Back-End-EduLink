namespace EduLink.Models.DTO.Response
{
    public class EventResDTO
    {
        public int EventID { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string EventDescription { get; set; }
        public string Details { get; set; }
        public string EventType { get; set; }
        public string EventStatus { get; set; }
        public int Capacity { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public string EventAdress { get; set; }
  
    }
}
