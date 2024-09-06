namespace EduLink.Models.DTO.Request
{
    public class ModifyArticleStatusReqDTO
    {
        public int ArticleID { get; set; }
        public ArticleStatus Status { get; set; }
    }
}
