using EduLink.Models;

public class ArticlesResDTO
{
    public List<ArticleDTO> Articles { get; set; }
}

public class ArticleDTO
{
    public int ArticleID { get; set; }
    public string Title { get; set; }
    public string ArticleContent { get; set; }
    public int VolunteerID { get; set; }
    public DateTime PublicationDate { get; set; }
    public string Status { get; set; }
}
