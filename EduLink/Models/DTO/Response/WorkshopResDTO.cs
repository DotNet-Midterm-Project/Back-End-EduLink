namespace EduLink.Models.DTO.Response
{
    public class WorkshopResDTO
    {
        // Get All Worshop for the one Student
        public string Title { get; set; }
        public string Description { get; set; }
        public int VolunteerID { get; set; }
        public DateTime Date { get; set; }
        public string SessionLink { get; set; }
        public int Capasity { get; set; }
    }
}
