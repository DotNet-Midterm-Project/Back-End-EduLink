using System.ComponentModel.DataAnnotations;

namespace EduLink.Models.DTO.Response
{
    public class GroupResDto
    {
        public int GroupId { get; set; }

        [MaxLength(200)]
        public string GroupName { get; set; }

        public int LeaderID { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
    }
}
