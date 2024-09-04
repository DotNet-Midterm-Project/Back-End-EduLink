using System.ComponentModel.DataAnnotations;

namespace EduLink.Models.DTO.Request
{
    public class AddEventReqDTO
    {
        [Required]
        public int CourseID { get; set; }
        [Required]
        public string Title { get; set; }
            
        [Required]
        public EventLocation Location { get; set; }
        [Required]
        public string EventDescription { get; set; }

        public string Details { get; set; }
        [Required]
        public EventType EventType { get; set; }
        public int Capacity { get; set; }
        [Required]
        public DateTimeOffset StartTime { get; set; }
        [Required]
        public DateTimeOffset EndTime { get; set; }
        public string EventAdress { get; set; }
        public int SessionCounts { get; set; }
    }
}
