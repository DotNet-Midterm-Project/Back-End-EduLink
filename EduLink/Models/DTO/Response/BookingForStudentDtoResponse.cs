namespace EduLink.Models.DTO.Response
{
    public class BookingForStudentDtoResponse
    {
        public string StudentID { get; set; }
        public string SessionStatus { get; set; }
        public string VolunteerName { get; set; }
        public int CourseID {  get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public string SessionLink { get; set; }

        public int ReservationID { get; set; }



    }
}
