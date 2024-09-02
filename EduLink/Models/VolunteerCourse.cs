namespace EduLink.Models
{
    public class VolunteerCourse
    {
        public int VolunteerCourseID { get; set; }
        public int VolunteerID { get; set; }
        public int CourseID { get; set; }
        public Volunteer Volunteers { get; set; }
        public Course Courses { get; set; }

        // Add this collection to represent the one-to-many relationship with Events
        public ICollection<Event> Events { get; set; }

    }
}
