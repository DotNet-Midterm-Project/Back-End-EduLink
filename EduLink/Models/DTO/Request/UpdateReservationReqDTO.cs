namespace EduLink.Models.DTO.Request
{
    public class UpdateReservationReqDTO
    {
        public int VolunteerID { get; set; }
        public int CourseID { get; set; }
        public int ReservationID { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime Date { get; set; }
    }
}
