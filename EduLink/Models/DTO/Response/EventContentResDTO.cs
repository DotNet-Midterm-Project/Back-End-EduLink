namespace EduLink.Models.DTO.Response
{
    public class EventContentResDTO
    {
        public int ContentID { get; set; }
        public string ContentName { get; set; }
        public Content ContentType { get; set; }
        public string ContentDescription { get; set; }
        public string ContentAddress { get; set; }
        public int EventID { get; set; }
    }
}
