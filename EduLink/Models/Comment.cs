namespace EduLink.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public int ArticleID { get; set; }
        public string? UserID { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Article Article { get; set; }
        public User User { get; set; }
    }
}