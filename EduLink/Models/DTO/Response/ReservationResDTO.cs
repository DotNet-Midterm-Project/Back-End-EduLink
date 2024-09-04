namespace EduLink.Models.DTO.Response
{
    public class ReservationResDTO
    {
        public int VolunteerID { get; set; }
        public int CourseID { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime Date { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
