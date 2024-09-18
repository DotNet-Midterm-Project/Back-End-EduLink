namespace EduLink.Models.DTO.Request
{
    public class CreateTaskReqDto
    {
        public int GroupId { get; set; }
        public string TaskName { get; set; }
        public int StudentId {  get; set; }
        public DateTime DueDate { get; set; }
        public string Description { get; set; }
        
    }
}
