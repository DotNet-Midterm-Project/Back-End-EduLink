namespace EduLink.Models.DTO.Response
{
    public class BookingForStudentResDTO
    {
        public string EventTitle { get; set; }
        public string CourseName { get; set; }
        public string EventLocation { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        //public DateOnly StartDate { get; set; }
        //public DateOnly EndDate { get; set; }
        public DateOnly Date { get; set; }
        public string SessionStatus {  get; set; }
        public string EventAddress { get; set; }

    }
}
