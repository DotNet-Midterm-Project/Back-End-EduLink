using Microsoft.AspNetCore.Identity;

namespace EduLink.Models
{
    public class User: IdentityUser
    {
        public Student Student { get; set; }
        public string Gender { get; set; }
        public bool IsAdmin {  get; set; }
        public bool IsLocked {  get; set; }
        public string Skills { get; set; }

    }
}
