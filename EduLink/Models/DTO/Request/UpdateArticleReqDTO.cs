using Microsoft.AspNetCore.Http;

namespace EduLink.Models.DTO.Request
{
    public class UpdateArticleReqDTO
    {
        public int ArticleID { get; set; }
        public string Title { get; set; }
        public string ArticleContent { get; set; }
        public DateTime PublicationDate { get; set; }
        public IFormFile? formFile { get; set; }
        public ArticleStatus Status { get; set; }

    }
}
