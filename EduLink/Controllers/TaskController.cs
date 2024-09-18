using EduLink.Models.DTO.Request;
using EduLink.Repositories.Interfaces;
using EduLink.Repositories.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduLink.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITask task;

        public TaskController(ITask task)
        {
            this.task = task;
        }
        [HttpPost("AssignTask")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> CreateTask(CreateTaskReqDto taskReqDto)
        {
 
            var studentClaimsId = User.Claims.FirstOrDefault(c => c.Type == "StudentId");
            if (studentClaimsId == null)
            {
                return Unauthorized(new { message = "Student ID not found in token." });
            }

            
            if (!int.TryParse(studentClaimsId.Value, out int studentId))
            {
                return BadRequest(new { message = "Invalid student ID." });
            }

            
            var result = await task.CreateTaskAsync(taskReqDto, studentId);

            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the task." });
            }

            if (result.Message == "The Group Not Found")
            {
                return NotFound(new { message = result.Message });
            }

            if (result.Message == "The Student Not Found in this Group")
            {
                return BadRequest(new { message = result.Message });
            }

            if (result.Message == "Only the Group Leader can create tasks")
            {
                return BadRequest(new { message = result.Message });
            }

            if (result.Message == "Task created successfully")
            {
                return Ok(new { message = result.Message });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Unexpected error occurred." });
        }
        [HttpGet("AllTasksForStudent/{groupId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetAllTasksForStudent(int groupId)
        {
            
            var studentClaimsId = User.Claims.FirstOrDefault(c => c.Type == "StudentId");
            if (studentClaimsId == null)
            {
                return Unauthorized(new { message = "Student ID not found in token." });
            }

            // Parse the student ID from the claims
            if (!int.TryParse(studentClaimsId.Value, out int studentId))
            {
                return BadRequest(new { message = "Invalid student ID." });
            }

          
            var tasks = await task.AllTasksForStudent(groupId, studentId);

            if (tasks == null || !tasks.Any())
            {
                return NotFound(new { message = "No tasks found for the student in the specified group." });
            }

            return Ok(tasks);
        }
        [HttpGet("{groupId}/Task/{taskId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetTaskById(int groupId, int taskId)
        {
            // Extract the StudentId from the claims (from the token)
            var studentClaimsId = User.Claims.FirstOrDefault(c => c.Type == "StudentId");
            if (studentClaimsId == null)
            {
                return Unauthorized(new { message = "Student ID not found in token." });
            }

            // Parse the student ID from the claims
            if (!int.TryParse(studentClaimsId.Value, out int studentId))
            {
                return BadRequest(new { message = "Invalid student ID." });
            }

            // Call the service to get the task by ID
            var gettask = await task.GetTaskById(studentId, groupId, taskId);

            if (task == null)
            {
                return NotFound(new { message = "Task not found or student does not belong to the group." });
            }

            return Ok(task);
        }
        [HttpPut("UpdateTask")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> UpdateTask(UpdateTaskReqDto updateTaskReqDto)
        {
            var StudentClaimsId = User.Claims.FirstOrDefault(c => c.Type == "StudentId");
            if (StudentClaimsId == null)
            {
                return Unauthorized(new { message = "Student ID not found in token." });
            }

            if (!int.TryParse(StudentClaimsId.Value, out int studentId))
            {
                return BadRequest(new { message = "Invalid student ID." });
            }

            var updateResult = await task.UpdateTask(updateTaskReqDto, studentId);

            if (updateResult.Message == "Task updated successfully")
            {
                return Ok(updateResult);
            }

            return BadRequest(updateResult);  // Return the error message
        }

      
        [HttpPut("UpdateTaskStatus/{taskId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> UpdateTaskStatus(int taskId, int status)
        {
            var studentClaimsId = User.Claims.FirstOrDefault(c => c.Type == "StudentId");
            if (studentClaimsId == null)
            {
                return Unauthorized(new { message = "Student ID not found in token." });
            }

            if (!int.TryParse(studentClaimsId.Value, out int studentId))
            {
                return BadRequest(new { message = "Invalid student ID." });
            }

            var result = await task.UpdateTaskStatus(taskId, studentId, status);

            if (result.Message == "Task status updated successfully")
            {
                return Ok(result);
            }

            return BadRequest(result); // Return an error message in case of failure
        }
        [HttpDelete("DeleteTask/{taskId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var studentClaimsId = User.Claims.FirstOrDefault(c => c.Type == "StudentId");
            if (studentClaimsId == null)
            {
                return Unauthorized(new { message = "Student ID not found in token." });
            }

            if (!int.TryParse(studentClaimsId.Value, out int studentId))
            {
                return BadRequest(new { message = "Invalid student ID." });
            }

            var result = await task.DeleteTask(studentId, taskId);

            if (result.Message == "Task deleted successfully")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }

}



