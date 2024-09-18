using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;

namespace EduLink.Repositories.Interfaces
{
    public interface ITask
    {
        Task<MessageResDTO> CreateTaskAsync(CreateTaskReqDto createTaskReqDto, int studentId);
        Task<List<TaskResDto>> GetAllTasksForStudent(int groupId, int studentId);
        Task<TaskResDto> GetTaskById(int StudentId , int groupId, int taskId);
        Task<MessageResDTO> UpdateTask(UpdateTaskReqDto updateTaskReqDto, int studentId);
        Task<MessageResDTO> DeleteTask(int studentId, int taskId);
        Task<MessageResDTO> UpdateTaskStatus(int TaskId, int studentId, int status);
    }
}
