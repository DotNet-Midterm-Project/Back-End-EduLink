namespace EduLink.Models
{
    public class Student
    {
        public int DepartmentID { get; set; }
        public int StudentID { get; set; }
        public ICollection<WorkshopsRegistration> WorkshopsRegistrations { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public User User { get; set; }
        public ICollection<Notification_Booking> Notification_Bookings { get; set; } = new List<Notification_Booking>();
        public Department Department { get; set; }



    }
}
