using EduLink.Models;

public class ArticlesResDTO
{
    public List<ArticleDTO> Articles { get; set; }
}

public class ArticleDTO
{
    public string VolunteerName { get; set; }
    public int ArticleID { get; set; }
    public int VolunteerID { get; set; }
    public string Title { get; set; }
    public string ArticleContent { get; set; }
 
    public DateTime PublicationDate { get; set; }
    public string? ArticleFile  { get; set; }
    public string Status { get; set; }
}
