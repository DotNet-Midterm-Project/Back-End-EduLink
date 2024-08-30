namespace EduLink.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }


        public ICollection<Reservation> Reservations { get; set; }


    }
}
