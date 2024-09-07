namespace EduLink.Models.DTO.Response
{
    public class WorkshopResponseDTO
    {
        public int VolunteerID { get; set; }
        public string VolunteerName { get; set; }
        public string WorkshopName { get; set; }
        public string WorkshopDescription { get; set; }
        public DateTime WorkshopDateTime { get; set; }
        public string SessionLink { get; set; }
        public int Capacity { get; set; }
    }

    public class WorkshopsResDTO
    {
        public List<WorkshopResponseDTO> Workshops { get; set; }
    }
}
