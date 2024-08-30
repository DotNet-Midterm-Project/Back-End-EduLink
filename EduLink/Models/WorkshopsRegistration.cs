namespace EduLink.Models
{
    public class WorkshopsRegistration
    {

        public int WorkShopID { get; set; }
        public WorkShop WorkShop { get; set; }

        public int StudentID { get; set; }
        public Student Student { get; set; }
    }
}
