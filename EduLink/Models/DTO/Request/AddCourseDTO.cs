namespace EduLink.Models.DTO.Request
{
    public class AddCourseDTO
    {
        public string CourseName { get; set; }
        public Department_Courses Department_Courses { get; set; } 
        public Reservation Reservations { get; set; }
        public VolunteerCourse volunteerCourses { get; set; }
        public EductionalContent  EductionalContents { get; set; }

    }
}
