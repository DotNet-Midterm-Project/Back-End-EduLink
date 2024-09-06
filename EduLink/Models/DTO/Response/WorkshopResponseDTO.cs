namespace EduLink.Models.DTO.Response
{
    public class WorkshopResponseDTO
    {
        public int VolunteerID { get; set; }
        public string VolunteerName { get; set; }
        public string WorkshopName { get; set; }
        public string WorkshopDescription { get; set; }
        public DateTime WorkshopDateTime { get; set; }
        public string SessionLink { get; set; }
        public int Capacity { get; set; }
    }

    public class ArticleResponseDTO
    {
        public int ArticleID { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string ArticleContent { get; set; }
        public ArticleStatus Status { get; set; }
    }
}
