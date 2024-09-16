namespace EduLink.Models
{
    public class Like
    {
        public string UserID { get; set; }
        public int ArticleID { get; set; }

        public User User { get; set; }
        public Article Article { get; set; }
    }
}