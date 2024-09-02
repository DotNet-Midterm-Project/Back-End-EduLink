namespace EduLink.Models
{
    public class VolunteerCourse
    {
        public int VolunteerCourseID { get; set; }
        public int VolunteerID { get; set; }
        public int CourseID { get; set; }
        public Volunteer Volunteers { get; set; }  
        public Course Courses { get; set; }  
        public ICollection<EductionalContent> EductionalContent { get; set; } = new HashSet<EductionalContent>();
        
    }
}
