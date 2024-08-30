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
        public Student Students { get; set; }
        public ICollection<Article> Articles { get; set; }
        public ICollection<EductionalContent> EductionalContent { get; set; }
        public ICollection<VolunteerCourse> VolunteerCourse { get; set; }

    }
}
