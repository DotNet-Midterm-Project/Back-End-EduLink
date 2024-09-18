namespace EduLink.Models.DTO.Request
{
    public class UpdateTaskReqDto
    {
        public int GroupId {  get; set; }
        public int TaskId {  get; set; }
        public string TaskName {  get; set; }
        public TaskStatus status {  get; set; }
    }
}
