using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EduLink.Models
{
    public class User: IdentityUser
    {
       public int DepartmentID {  get; set; }
        public Department Department { get; set; }
        [MaxLength(200)]
        public string Gender { get; set; }
        public bool IsAdmin {  get; set; }
        public bool IsLocked {  get; set; }

        [MaxLength(200)]
        
        public string Skills { get; set; }
        [MaxLength(200)]
        
        public string? TempCode { get; set; }
        public bool IsActived { get; set; }
        public DateTimeOffset TempCodeExpire {  get; set; }
        public Student Student { get; set; }
   

    }
}
