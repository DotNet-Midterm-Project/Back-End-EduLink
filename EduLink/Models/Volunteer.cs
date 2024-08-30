namespace EduLink.Models
{
    public class Volunteer
    {
        public int VolunteerID { get; set; }
        public string SkillDescription { get; set; }
        public int Rating { get; set; }
        public bool Availability { get; set; }
        public bool IsVolunteer { get; set; }
        public string StudentID { get; set; }   
    }
}
