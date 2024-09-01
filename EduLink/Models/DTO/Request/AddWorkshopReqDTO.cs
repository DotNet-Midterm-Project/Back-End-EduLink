namespace EduLink.Models.DTO.Request
{
    public class AddWorkshopReqDTO
    {
        public int VolunteerID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string SessionLink { get; set; }
        public int Capasity { get; set; }
    }
}
