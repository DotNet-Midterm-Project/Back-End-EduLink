using Microsoft.AspNetCore.Identity;

namespace EduLink.Models
{
    public class User: IdentityUser
    {
        public Student Student { get; set; }
        public Admin Admin { get; set; }
    }
}
