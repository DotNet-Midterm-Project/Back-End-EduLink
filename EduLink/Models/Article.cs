using System.ComponentModel.DataAnnotations;

namespace EduLink.Models
{
    public class Article
    {
        public int ArticleID { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string ArticaleContent { get; set; }
        public string AuthorName { get; set; }
        public DateTime PublicationDate { get; set; }
        public int VolunteerID { get; set; }
        public Volunteer Volunteer { get; set; }
        public ArticleStatus Status { get; set; }
    }
    public enum ArticleStatus
    {
        Visible,
        Hidden,

    }



}
