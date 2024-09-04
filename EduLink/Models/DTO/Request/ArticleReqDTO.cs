using System.ComponentModel.DataAnnotations;

namespace EduLink.Models.DTO.Request
{
    public class ArticleReqDTO
    {
        [Required]
        public int VolunteerID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string AuthorName { get; set; }
    }
}
