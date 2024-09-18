﻿using EduLink.Data;
using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace EduLink.Repositories.Services
{
    public class TaskService : ITask
    {
        private readonly EduLinkDbContext context;
        private readonly IEmailService email;

        public TaskService(EduLinkDbContext _context, IEmailService Email)
            {
                context = _context;
            email = Email;
        }



        public async Task<MessageResDTO> CreateTaskAsync(CreateTaskReqDto createTaskReqDto, int studentId)
        {
            if (createTaskReqDto == null)
            {
                return new MessageResDTO
                {
                    Message = "Request DTO is null"
                };
            }

            // Check if the group exists
            var existGroup = await context.Groups.FindAsync(createTaskReqDto.GroupId);
            if (existGroup == null)
            {
                return new MessageResDTO
                {
                    Message = "The Group Not Found"
                };
            }

            var studentRoleInGroup = await context.GroupMembers
                .Where(gm => gm.GroupId == createTaskReqDto.GroupId && gm.StudentID == studentId)
                .Select(gm => gm.Role)
                .FirstOrDefaultAsync();

            if (studentRoleInGroup == null)
            {
                return new MessageResDTO
                {
                    Message = "The Student is not a member of this Group"
                };
            }

            if (studentRoleInGroup != MemberRole.Leader)
            {
                return new MessageResDTO
                {
                    Message = "Only the Group Leader can create tasks"
                };
            }

            // Check if the assigned student is part of the group
            var existStudentInGroup = await context.GroupMembers
                .AnyAsync(gm => gm.GroupId == createTaskReqDto.GroupId && gm.StudentID == createTaskReqDto.StudentId);

            if (!existStudentInGroup)
            {
                return new MessageResDTO
                {
                    Message = "The Assigned Student is not found in this Group"
                };
            }
            var studentEmaile = await context.Students
             .Where(e => e.StudentID == createTaskReqDto.StudentId)
             .Include(e => e.User) 
             .Select(e => e.User.Email) 
             .FirstOrDefaultAsync();
            var emailSubject = $"New Task Assigned: {createTaskReqDto.TaskName}";
            var emailDescriptionHtml = $@"
            <p>Dear Student,</p>
            <p>We would like to inform you that a new task has been assigned to you:</p>
            <p><strong>Task Name:</strong> {createTaskReqDto.TaskName}</p>
            <p><strong>Due Date:</strong> {createTaskReqDto.DueDate}</p>
            <p><strong>Description:</strong> {createTaskReqDto.Description}</p>
            <p>Please ensure you complete this task by the due date.</p>
            <p>Best regards,</p>
            <p>EduLink Team</p>";

            // Convert HTML to plain text
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(emailDescriptionHtml);
            var emailDescriptionPlain = htmlDoc.DocumentNode.InnerText;



            await email.SendEmailAsync(studentEmaile, emailSubject, emailDescriptionHtml);

            // Create the task
            var task = new ProjectTask
            {
                TaskName = createTaskReqDto.TaskName,
                AssignedTo = createTaskReqDto.StudentId,
                Description = createTaskReqDto.Description,
                DueDate = createTaskReqDto.DueDate,
                GroupId = createTaskReqDto.GroupId,
                Status = Models.TaskStatus.Pending
            };

            context.ProjectTasks.Add(task);
            await context.SaveChangesAsync();

            return new MessageResDTO
            {
                Message = "Task created successfully"
            };
        }
        public async Task<List<TaskResDto>> AllTasksForStudent(int groupId,int studentId)
        {
 
            var existGroup = await context.Groups.FindAsync(groupId);
            if (existGroup == null)
            {
                return null; 
            }

            var existStudentinGroup = await context.GroupMembers
                .AnyAsync(gm => gm.GroupId == groupId && gm.StudentID == studentId);

            if (!existStudentinGroup)
            {
                return null; 
            }
            var tasks = await context.ProjectTasks
                .Where(t => t.GroupId == groupId && t.AssignedTo == studentId)
                .Select(t => new TaskResDto
                {
                    TaskName = t.TaskName,
                    ProjectTaskId = t.ProjectTaskId,
                    DueDate = t.DueDate,
                    Description = t.Description,
                    Status = t.Status.ToString(),
                    GroupId = t.GroupId
                })
                .ToListAsync();

            return tasks;
        }

        public async Task<TaskResDto> GetTaskById(int studentId, int groupId, int taskId)
        {
     
            var existGroup = await context.Groups.FindAsync(groupId);
            if (existGroup == null)
            {

                return null;
            }

            var existStudentInGroup = await context.GroupMembers
                .AnyAsync(gm => gm.GroupId == groupId && gm.StudentID == studentId);

            if (!existStudentInGroup)
            {
                return null; 
            }
            var task = await context.ProjectTasks
                .Where(t => t.GroupId == groupId && t.ProjectTaskId == taskId && t.AssignedTo == studentId)
                .Select(t => new TaskResDto
                {
                    TaskName = t.TaskName,
                    ProjectTaskId = t.ProjectTaskId,
                    DueDate = t.DueDate,
                    Description = t.Description,
                    Status = t.Status.ToString(), 
                    GroupId = t.GroupId
                })
                .FirstOrDefaultAsync();
            return task;
        }

        public async Task<MessageResDTO> UpdateTask(UpdateTaskReqDto updateTaskReqDto, int studentId)
        {
            var existGroup = await context.Groups.FindAsync(updateTaskReqDto.GroupId);
            if (existGroup == null)
            {
                return new MessageResDTO
                {
                    Message = "The Group Not Found"
                };
            }

            var groupMember = await context.GroupMembers
                .Where(gm => gm.GroupId == updateTaskReqDto.GroupId && gm.StudentID == studentId)
                .Select(gm => new { gm.Role })
                .FirstOrDefaultAsync();

            if (groupMember == null)
            {
                return new MessageResDTO
                {
                    Message = "The Student Not Found in this Group"
                };
            }
            if (groupMember.Role != MemberRole.Leader)
            {
                return new MessageResDTO
                {
                    Message = "The Student does not have permission to update this task"
                };
            }
            var task = await context.ProjectTasks
                .Where(t => t.GroupId == updateTaskReqDto.GroupId && t.ProjectTaskId == updateTaskReqDto.TaskId)
                .FirstOrDefaultAsync();
            if (task == null)
            {
                return new MessageResDTO
                {
                    Message = "The Task Not Found"
                };
            }
            task.TaskName = updateTaskReqDto.TaskName;
            task.Status = updateTaskReqDto.status;
            await context.SaveChangesAsync();

            return new MessageResDTO
            {
                Message = "Task updated successfully"
            };
        }

        public async Task<MessageResDTO> DeleteTask(int studentId, int taskId)
        {
            var task = await context.ProjectTasks
                .Include(t => t.Group) // Ensure the Group is loaded to check the GroupId and Leader
                .FirstOrDefaultAsync(t => t.ProjectTaskId == taskId);

            if (task == null)
            {
                return new MessageResDTO
                {
                    Message = "The Task Not Found"
                };
            }

            // Check if the student is a member of the group
            var groupMember = await context.GroupMembers
                .Where(gm => gm.GroupId == task.GroupId && gm.StudentID == studentId)
                .Select(gm => new { gm.Role })
                .FirstOrDefaultAsync();

            if (groupMember == null)
            {
                return new MessageResDTO
                {
                    Message = "The Student Not Found in this Group"
                };
            }

            // Check if the student is a Leader
            if (groupMember.Role != MemberRole.Leader)
            {
                return new MessageResDTO
                {
                    Message = "The Student does not have permission to delete this task"
                };
            }

            // Delete the task
            context.ProjectTasks.Remove(task);

            // Save changes to the database
            await context.SaveChangesAsync();

            return new MessageResDTO
            {
                Message = "Task deleted successfully"
            };
        }
        public async Task<MessageResDTO> UpdateTaskStatus(int taskId, int studentId, int status)
        {
            if (!Enum.IsDefined(typeof(Models.TaskStatus), status))
            {
                return new MessageResDTO
                {
                    Message = "Invalid status value"
                };
            }

            var task = await context.ProjectTasks
                .Include(t => t.Group)
                .FirstOrDefaultAsync(t => t.ProjectTaskId == taskId);

            if (task == null)
            {
                return new MessageResDTO
                {
                    Message = "Task not found"
                };
            }
            var groupMember = await context.GroupMembers
                .Where(gm => gm.GroupId == task.GroupId && gm.StudentID == studentId)
                .Select(gm => new { gm.Role })
                .FirstOrDefaultAsync();

            if (groupMember == null)
            {
                return new MessageResDTO
                {
                    Message = "Student is not a member of this group"
                };
            }
            if (groupMember.Role != MemberRole.Leader)
            {
                return new MessageResDTO
                { 
                    Message = "Only group leaders can update task status"
                };
            }
            task.Status = (Models.TaskStatus)status;
            await context.SaveChangesAsync();
            return new MessageResDTO
            {
                Message = "Task status updated successfully"
            };
        }


    }
}
