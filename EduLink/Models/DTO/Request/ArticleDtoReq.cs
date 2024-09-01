namespace EduLink.Models.DTO.Request
{
    public class ArticleDTO
    {
        public int VolunteerID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public string AuthorName { get; set; }
    }
}
