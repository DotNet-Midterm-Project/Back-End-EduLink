using System.Data;

namespace EduLink.Models.DTO.Request
{
    public class MeetingRequestDTO
    {
        public int GroupId { get; set; }
        public DateTime ScheduledDate { get; set; }
        //public string Announcement { get; set; }
        //public string MeetingLink { get; set; }
    }
}
