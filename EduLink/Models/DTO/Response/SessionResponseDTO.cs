namespace EduLink.Models.DTO.Response
{
    public class SessionResponseDTO
    {
        public string CourseName { get; set; }
        public string EventTitle { get; set; }
        public string Location { get; set; }
        public string EventDescription { get; set; }
        public string EventDetails { get; set; }
        public string EventStatus { get; set; }
        public int Capacity { get; set; }
        public string EventType { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public string EventAddress { get; set; }
    }
}
