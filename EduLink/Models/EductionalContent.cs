namespace EduLink.Models
{
    public class EductionalContent
    {
        public int ContentID { get; set; }
        public int CourseID { get; set; }
        public string ContentName { get; set; }
        public float FileLength { get; set; }
        public int VolunteerID { get; set; }
        public Enum ContentType { get; set; }
        public string ContentDescription { get; set; }
        public Volunteer Volunteers { get; set; }
        public Course Courses { get; set; }
    }
    public enum Content {
        Presentation,
        Document,
        Video , 
        Book,
    }

}
