using System.ComponentModel.DataAnnotations;

namespace EduLink.Models
{

    public enum AvailabilityStatus
    {
        Available,
        Unavailable,
        Busy
    }
    public class Volunteer
    {
        public int VolunteerID { get; set; }
        public int StudentID { get; set; }
        public Student Student { get; set; }

        [MaxLength(200)]

        public string SkillDescription { get; set; }
        public float Rating { get; set; }
        public int RatingAcount { get; set; }
        public AvailabilityStatus Availability { get; set; } 
        public bool Aprrove { get; set; }

       
        public ICollection<Article> Articles { get; set; }
    
        public ICollection<VolunteerCourse> VolunteerCourse { get; set; }
        public ICollection<Event> Reservations { get; set; }
        
    }
}
