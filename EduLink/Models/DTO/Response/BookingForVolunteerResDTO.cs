namespace EduLink.Models.DTO.Response
{
    public class BookingForVolunteerResDTO
    {
        public string BookingType { get; set; }
        public int BookingID { get; set; }
        public int EventID { get; set; }
        public int? SessionID { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string BookingStatus { get; set; }
        public string EventTitle { get; set; }
        public DateTimeOffset startDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
