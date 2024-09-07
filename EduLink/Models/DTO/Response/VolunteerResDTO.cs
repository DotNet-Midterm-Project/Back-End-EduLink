namespace EduLink.Models.DTO.Response
{
    public class VolunteerResDTO
    {
        public int VolunteerID { get; set; }
        public string VolunteerName {  get; set; }
        public float? Rating { get; set; } = 0;
        public string SkillDescription { get; set; }
        public string Email { get; set; }
        public int? RatingCount { get; set; } = 0;
        public string Availability { get; set; }
        public bool Approve { get; set; }
        public string? PhoneNumber {  get; set; }
    }
}
