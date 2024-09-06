using System.ComponentModel.DataAnnotations;

namespace EduLink.Models.DTO.Request
{
    public class UpdateEventReqDTO
    {
        public int EventID { get; set; }

        [MaxLength(500)]
        public string Title { get; set; }
        public EventLocation Location { get; set; }
        [MaxLength(1000)]
        public string EventDescription { get; set; }
        [MaxLength(2000)]
        public string EventDetails { get; set; }
        public int Capacity { get; set; }
        public EventType EventType { get; set; }
        public string? EventAddress { get; set; }
        public int? SessionCount { get; set; }
    }
}
