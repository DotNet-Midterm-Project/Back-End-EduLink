using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Models;
using EduLink.Repositories.Interfaces;
using System.Security.Claims;
using System;
using EduLink.Data;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using HtmlAgilityPack;

namespace EduLink.Repositories.Services
{

    public class GroupService:IGroup
    {
        private readonly EduLinkDbContext _context;
        private readonly IEmailService _Email;


        public GroupService(EduLinkDbContext context, IEmailService Email)
        {
            _context = context;
           _Email = Email;
        }




        public async Task<MessageResDTO> CreateGroupAsync(CreateGroupReqDto createGroupDto,int leaderId )
        {
         

            // Create a new Group object
            var group = new Models.Group
            {
                GroupName = createGroupDto.GroupName,
                Description = createGroupDto.Description,
                LeaderID = leaderId
            };

           

            // Add the new group to the database
            _context.Groups.Add(group);
           
            var GroupMember = new GroupMember
            {
                GroupId = group.GroupId,
                StudentID = leaderId,
                Role = MemberRole.Leader,
            };
            _context.GroupMembers.Add(GroupMember);

            await _context.SaveChangesAsync();


            // Return a success message
            return new MessageResDTO
            {
                Message = $"Group '{group.GroupName}' created successfully."
            };
        }





        public async Task<List<GroupResDto>> GetAllGroupsAsync(int leaderId)
        {
            var groups=await _context.Groups
         .Where(g => g.LeaderID == leaderId) 
         .Select(g => new GroupResDto
         {
             GroupId = g.GroupId,
             GroupName = g.GroupName,
             LeaderID = g.LeaderID,
             Description = g.Description
         })
         .ToListAsync();

            if (groups == null)
            {
                return null;
            }
            return groups;
        }

        public async Task<GroupDetailsResDto> GetGroupByIdAsync(int groupId, int memberId)
        {
            var group = await _context.Groups
                .Include(g => g.Members)
                .ThenInclude(m => m.Student)
                .ThenInclude(s=>s.User)
                .FirstOrDefaultAsync(g => g.GroupId == groupId);

            if (group == null)
            {
                return null; 
            }

            var isMember = group.Members.Any(m => m.StudentID == memberId);

            if (!isMember)
            {
                return null; 
            }

            var groupDto = new GroupDetailsResDto
            {
                GroupId = group.GroupId,
                GroupName = group.GroupName,
                LeaderID = group.LeaderID,
                Description = group.Description,
                Members = group.Members.Select(m => new GroupMemberDto
                {
                    StudentID = m.StudentID,
                    Name = m.Student.User.UserName, 
                    Role = m.Role.ToString()
                }).ToList()
            };

            return groupDto;
        }





        public async Task<MessageResDTO> UpdateGroupAsync(int groupId ,UpdateGroupReqDto updateGroupDto, int leaderId)
        {
            var group = await _context.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.GroupId == groupId);

            if (group == null)
            {
                return new MessageResDTO
                {
                    Message = "Group not found."
                };
            }

            var isLeader = group.Members.Any(m => m.StudentID == leaderId && m.Role == MemberRole.Leader);

            if (!isLeader)
            {
                return new MessageResDTO
                {
                    Message = "Only the group leader can update the group details."
                };
            }

            group.GroupName = updateGroupDto.GroupName ?? group.GroupName;
            group.Description = updateGroupDto.Description ?? group.Description;

            _context.Groups.Update(group);
            await _context.SaveChangesAsync();

            return new MessageResDTO
            {
                Message = $"Group '{group.GroupName}' updated successfully."
            };
        }







        public async Task<bool> DeleteGroupAsync(int groupId, int leaderId)
        {
            var group = await _context.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.GroupId == groupId);

            if (group == null)
            {
                return false; 
            }

            var isLeader = group.Members.Any(m => m.StudentID == leaderId && m.Role == MemberRole.Leader);

            if (!isLeader)
            {
                return false; 
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            return true; 
        }





        public async Task<MessageResDTO> AddMemberToGroupAsync(int leaderId, AddGroupMemberReqDto memberDto)
        {
            
            var group = await _context.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.GroupId == memberDto.GroupId);

            if (group == null)
            {
                return new MessageResDTO
                {
                    Message = "Group not found."
                };
            }

          
            var isLeader = group.Members.Any(m => m.StudentID == leaderId && m.Role == MemberRole.Leader);

            if (!isLeader)
            {
                return new MessageResDTO
                {
                    Message = "Only the group leader can add members."
                };
            }

         
            if (group.Members.Any(m => m.StudentID == memberDto.StudentID))
            {
                return new MessageResDTO
                {
                    Message = "Member already exists in the group."
                };
            }

          
            var newMember = new GroupMember
            {
                GroupId = memberDto.GroupId,
                StudentID = memberDto.StudentID,
                Role = memberDto.Role
            };

            _context.GroupMembers.Add(newMember);
            await _context.SaveChangesAsync();

            var student = await _context.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.StudentID == memberDto.StudentID);
            var studentEmaile = student.User.Email;
            var emailSubject = $"New Event: {group.GroupName}";
            var emailDescriptionHtml = $@"
           <p>Dear {student.User.UserName},</p>
           <p>We are pleased to inform you that you have been successfully added to the group <strong>{group.GroupName}</strong>.</p>
           <p><strong>Your Role:</strong> {newMember.Role.ToString()}</p>
           <p>Please log in to the system to check your group activities and responsibilities.</p>
           <p>If you have any questions, feel free to reach out.</p>
           <p>Best regards,</p> 
           <p>EduLink Team</p>";
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(emailDescriptionHtml);
            var emailDescriptionPlain = htmlDoc.DocumentNode.InnerText;
            if (student.User.Email != null)
            {

                await _Email.SendEmailAsync(studentEmaile, emailSubject, emailDescriptionHtml);
            }

            return new MessageResDTO
            {
                Message = $"Member with StudentID {newMember.StudentID} added successfully to group {group.GroupName}."
            };
        }





        public async Task<IEnumerable<GroupMemberDto>> GetMembersOfGroupAsync(int groupId, int memberId)
        {
         
            var group = await _context.Groups
                .Include(g => g.Members)
                .ThenInclude(m => m.Student)
                .ThenInclude(s=>s.User)
                .FirstOrDefaultAsync(g => g.GroupId == groupId);

            if (group == null)
            {
                return Enumerable.Empty<GroupMemberDto>();
            }

            var isMember = group.Members.Any(m => m.StudentID == memberId);
            if (!isMember)
            {
                return Enumerable.Empty<GroupMemberDto>(); 
            }

            var members = group.Members.Select(m => new GroupMemberDto
            {
                StudentID = m.StudentID,
                Name = m.Student.User.UserName, 
                Role = m.Role.ToString() 
            });

            return members;
        }





        public async Task<MessageResDTO> RemoveMemberFromGroupAsync(int groupId, int memberId, int leaderId)
        {
          
            var group = await _context.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.GroupId == groupId);

            if (group == null)
            {
                return new MessageResDTO { Message = "Group not found." };
            }
            var isMember = group.Members.Any(m => m.StudentID == leaderId);
            if (!isMember)
            {
                return new MessageResDTO { Message = "Only the group leader can remove members." };
            }

     
            var memberToRemove = group.Members.FirstOrDefault(m => m.StudentID == memberId);
            if (memberToRemove == null)
            {
                return new MessageResDTO { Message = "Member not found in the group." };
            }

          
            _context.GroupMembers.Remove(memberToRemove);
            await _context.SaveChangesAsync();

            return new MessageResDTO { Message = $"Member with ID {memberId} has been removed from the group." };
        }





        public async Task<MessageResDTO> UpdateMemberRoleAsync(GroupMemberUpdateReqDto memberUpdate, int leaderId)
        {
           
            var group = await _context.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.GroupId == memberUpdate.GroupId);

            if (group == null)
            {
                return new MessageResDTO { Message = "Group not found." };
            }


            var isMember = group.Members.Any(m => m.StudentID == leaderId);
            if (!isMember)
            {
                return new MessageResDTO { Message = "Only the group leader can update member roles." };
            }

          
            var memberToUpdate = group.Members.FirstOrDefault(m => m.StudentID == memberUpdate.MemberId);
            if (memberToUpdate == null)
            {
                return new MessageResDTO { Message = "Member not found in the group." };
            }

            
            memberToUpdate.Role = memberUpdate.Role;
            _context.GroupMembers.Update(memberToUpdate);
            await _context.SaveChangesAsync();

            return new MessageResDTO { Message = $"Member role updated to {memberUpdate.Role}." };
        }
    }
}
