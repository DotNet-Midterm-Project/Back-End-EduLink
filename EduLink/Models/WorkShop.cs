namespace EduLink.Models
{
    public class WorkShop
    {
        public int WorkShopID { get; set; }

        public int VolunteerID { get; set; }
        public Volunteer Volunteer { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string SessionLink { get; set; }
        public int Capasity { get; set; }

        public ICollection<WorkshopsRegistration> WorkshopsRegistrations { get; set; }

    }
}
