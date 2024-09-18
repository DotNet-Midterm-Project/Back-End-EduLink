namespace EduLink.Models.DTO.Response
{
    public class MeetingResponseDTO
    {
        public string MeetingLink { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Announcement { get; set; }
        public int GroupId { get; set; }
    }

}
