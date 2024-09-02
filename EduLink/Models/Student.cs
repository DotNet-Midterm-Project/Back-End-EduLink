namespace EduLink.Models
{
    public class Student
    {
        public int  StudentID { get; set; }
        public int DepartmentID { get; set; }
        public string UserID { get; set; }
        public ICollection<WorkshopsRegistration> WorkshopsRegistrations { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public User User { get; set; }
       
        public Department Department { get; set; }
        public Volunteer Volunteers { get; set; }
        
    }
}
