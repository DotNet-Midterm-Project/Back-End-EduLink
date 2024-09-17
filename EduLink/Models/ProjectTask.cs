namespace EduLink.Models
{
    public class ProjectTask
    {
        public int ProjectTaskId { get; set; }
        public int GroupId { get; set; }
        public string TaskName { get; set; }
        public int AssignedTo { get; set; }
        public TaskStatus Status { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }

        // Navigation properties
        public virtual Group Group { get; set; }
        public virtual GroupMember AssignedMember { get; set; }
    }
    public enum TaskStatus
    {
        Pending,
        Confirmed,
        Completed,
        Canceled
    }
}
