namespace EduLink.Models
{
    public class Anouncement
    {
        public int AnouncementID { get; set; }
        public int EventID { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime AnounceDate { get; set; }   

        public WorkShop WorkShop { get; set; }
    }
}
