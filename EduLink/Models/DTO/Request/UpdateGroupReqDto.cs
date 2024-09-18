using System.ComponentModel.DataAnnotations;

namespace EduLink.Models.DTO.Request
{
    public class UpdateGroupReqDto
    {
        [MaxLength(200)]
        public string GroupName { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
    }
}
