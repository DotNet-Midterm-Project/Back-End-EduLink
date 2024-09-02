namespace EduLink.Models
{
    public class EductionalContent
    {
        public int ContentID { get; set; }
       public int VolunteerCourseID {  get; set; }
        public string ContentName { get; set; }
        public float FileLength { get; set; }
       public VolunteerCourse VolunteerCourse { get; set; }
        public Enum ContentType { get; set; }
        public string ContentDescription { get; set; }
       
      
    }
    public enum Content {
        Presentation,
        Document,
        Video , 
        Book,
    }

}
