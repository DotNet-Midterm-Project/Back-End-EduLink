namespace EduLink.Models.DTO.Response
{
    public class BookingForStudentResDTO
    {
        public int BookingId { get; set; }
        public string EventTitle { get; set; }
        public string CourseName { get; set; }
        public string EventLocation { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public string SessionStatus {  get; set; }
        public string EventAddress { get; set; }

    }
}
