namespace EduLink.Models.DTO.Response
{
    public class ArticleDetailsResDTO
    {
        public int ArticleID { get; set; }
        public int VolunteerID { get; set; }
        public string VolunteerName { get; set; }
        public string Title { get; set; }
        public string ArticleContent { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Status { get; set; }
        public int LikesCount { get; set; } 
        public List<CommentDTO> Comments { get; set; } 
    }
    public class CommentDTO
    {
        public int CommentID { get; set; }
        public string Content { get; set; }
        public string CommenterName { get; set; } // اسم الشخص الذي علّق
        public DateTime CommentDate { get; set; }
    }
}
