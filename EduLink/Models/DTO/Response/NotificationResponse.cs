namespace EduLink.Models.DTO.Response
{
    public class NotificationResponse
    {
        public int NotificationID { get; set; }
        public string Message { get; set; }
        public DateTime DateSend { get; set; }
        public int Capacity { get; set; }
    }

    public class GetNotificationsResponse
    {
        public List<NotificationResponse> Notifications { get; set; }
    }
}
