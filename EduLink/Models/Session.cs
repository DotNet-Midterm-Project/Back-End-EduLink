using System.ComponentModel.DataAnnotations;

namespace EduLink.Models
{
    public class Session
    {
        public int SessionID { get; set; }
        public int EventID { get; set; }
        [MaxLength(100)]
        public string StartDate { get; set; }
        [MaxLength(100)]

        public string EndDate { get; set; }
        [MaxLength(200)]

        public string Details { get; set; }
    }
}
