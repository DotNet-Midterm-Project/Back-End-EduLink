namespace EduLink.Models.DTO.Response
{
    public class CommentResDto
    {
        public int CommentID { get; set; }
        public int ArticleID { get; set; }
        public string? UserId { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
