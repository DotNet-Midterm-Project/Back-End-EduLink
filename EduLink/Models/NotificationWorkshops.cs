namespace EduLink.Models
{
    public class NotificationWorkshops
    {
        public int ID { get; set; }
        public int WorkshopID { get; set; }
        public DateTime DateSend { get; set; }
        public string Message { get; set; }
    }
}
