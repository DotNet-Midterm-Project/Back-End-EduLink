namespace EduLink.Models
{
    public class Student
    {
        public int UserId { get; set; }
        public ICollection<WorkshopsRegistration> WorkshopsRegistrations { get; set; }
        public ICollection<Booking> Bookings { get; set; }



    }
}
