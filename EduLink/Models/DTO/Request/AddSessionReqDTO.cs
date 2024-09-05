namespace EduLink.Models.DTO.Request
{
    public class AddSessionReqDTO
    {
        public int EventID { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string Details { get; set; }
        public int SessionCapacity { get; set; }
    }
}
