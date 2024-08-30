namespace EduLink.Models
{
    public class Article
    {
        public int ArticleID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public DateOnly PublicationDate { get; set; }
        public int VolunteerID { get; set; }
        public Volunteer Volunteer { get; set; }
    }
}
