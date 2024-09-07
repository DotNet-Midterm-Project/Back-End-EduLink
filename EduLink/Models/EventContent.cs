using System.ComponentModel.DataAnnotations;

namespace EduLink.Models
{
    public class EventContent
    {
        public int ContentID { get; set; }
       public int EventID { get; set; }
        public Event Event { get; set; }
        [MaxLength(500)]
        public string ContentName { get; set; }
       
        public Content ContentType { get; set; }
        [MaxLength(1000)]
        public string ContentDescription { get; set; }
        [MaxLength(2000)]
        public string ContentAddress { get; set; }



    }
    public enum Content {
        Presentation,
        Document,
        Video , 
        Book,
    }

}
