namespace EduLink.Models.DTO.Request
{
    public class UpdateArticleReqDTO
    {
        public int ArticleID { get; set; }
        public string Title { get; set; }
        public string ArticleContent { get; set; }
        public DateTime PublicationDate { get; set; }
        public ArticleStatus Status { get; set; }

    }
}
