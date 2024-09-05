namespace EduLink.Models.DTO.Response
{
    public class VolunteerResDTO
    {
        public int VolunteerID { get; set; }
        public string VolunteerName {  get; set; }
        public float Rating { get; set; }
        public string SkillDescription { get; set; }
        public string Email { get; set; }
        public int AcountRating { get; set; }
        public string Availability { get; set; }

        public string? PhoneNumber {  get; set; }


    }
}
