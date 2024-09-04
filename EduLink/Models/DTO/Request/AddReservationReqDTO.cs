namespace EduLink.Models.DTO.Request
{
    public class AddReservationReqDTO
    {
        public int VolunteerID { get; set; }
        public int CourseID { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime Date { get; set; }
    }
}
