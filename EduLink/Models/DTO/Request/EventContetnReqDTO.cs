using System.ComponentModel.DataAnnotations;

namespace EduLink.Models.DTO.Request
{
    public class EventContetnReqDTO
    {
        [Required] 
        public int EventID { get; set; }

        [Required] 
       
        public string ContentName { get; set; }

        [Required] 
        public Content ContentType { get; set; }

      
        public string ContentDescription { get; set; }
    }

}
