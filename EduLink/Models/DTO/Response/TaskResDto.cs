namespace EduLink.Models.DTO.Response
{
    public class TaskResDto
    {
        public string TaskName { get; set; }
        public int ProjectTaskId { get; set; }
        public DateTime DueDate { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public int GroupId { get; set; }


    }
}
