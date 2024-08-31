namespace EduLink.Models
{
    public class VolunteerCourse
    {
        public int VolunteerID { get; set; }
        public int CourseID { get; set; }
        public Volunteer Volunteers { get; set; }  
        public Course Courses { get; set; }  
    }
}
