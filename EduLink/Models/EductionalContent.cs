namespace EduLink.Models
{
    public class EductionalContent
    {
        public int ContentID { get; set; }
        public int CourseID { get; set; }
        public int VolunteerID { get; set; }
        public string ContentType { get; set; }
        public string ContentDescription { get; set; }
        public Volunteer Volunteers { get; set; }
        public Course Courses { get; set; }
    }
}
