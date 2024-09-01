namespace EduLink.Models.DTO.Response
{
    public class GetWorkshopNotificationRespDTO
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public DateTime DateSend { get; set; }
        public int Capasity { get; set; }
    }
}
