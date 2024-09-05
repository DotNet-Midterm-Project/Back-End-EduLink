namespace EduLink.Models
{
    public class VolunteerCourse
    {
        public int VolunteerCourseID { get; set; }
        public int VolunteerID { get; set; }
        public int CourseID { get; set; }
        public Volunteer Volunteer { get; set; }  
        public Course Course { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
