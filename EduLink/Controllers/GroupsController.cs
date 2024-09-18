using EduLink.Models;
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
    public class GroupsController : ControllerBase
    {

        private readonly IGroup _groupService;

        public GroupsController(IGroup groupService)
        {
            _groupService = groupService;
        }




        [Authorize(Roles = "Student")]
        [HttpPost("Generate-Group")]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupReqDto createGroupDto)

        {
            var volunteerIDClaim = User.Claims.FirstOrDefault(c => c.Type == "StudentID");
            if (volunteerIDClaim == null)
            {
                return Unauthorized(new { message = "Student ID not found in token." });
            }

            if (!int.TryParse(volunteerIDClaim.Value, out int leaderId))
            {
                return BadRequest(new { message = "Invalid Student ID." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Call the service to create the group
            var result = await _groupService.CreateGroupAsync(createGroupDto, leaderId);

            return Ok(result); // Return success message
        }





        [Authorize(Roles = "Student")]
        [HttpPost("Add-Member")]
        public async Task<IActionResult> AddMemberToGroup([FromBody] AddGroupMemberReqDto memberDto)
        {
            var leaderIdClaim = User.Claims.FirstOrDefault(c => c.Type == "StudentID");
            if (leaderIdClaim == null)
            {
                return Unauthorized(new { message = "Student ID not found in token." });
            }

            if (!int.TryParse(leaderIdClaim.Value, out int leaderId))
            {
                return BadRequest(new { message = "Invalid Student ID." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _groupService.AddMemberToGroupAsync(leaderId, memberDto);

            if (result.Message == "Group not found.")
            {
                return NotFound(new { message = result.Message });
            }

            if (result.Message == "Only the group leader can add members.")
            {
                return BadRequest(new { message = result.Message });
            }

            if (result.Message == "Member already exists in the group.")
            {
                return Conflict(new { message = result.Message });
            }

            return Ok(result); // Return success message
        }





        [Authorize(Roles = "Student")]
        [HttpGet("Get-All-Groups")]
        public async Task<IActionResult> GetAllGroups()
        {

            var volunteerIDClaim = User.Claims.FirstOrDefault(c => c.Type == "StudentID");
            if (volunteerIDClaim == null)
            {
                return Unauthorized(new { message = "Student ID not found in token." });
            }

            if (!int.TryParse(volunteerIDClaim.Value, out int leaderId))
            {
                return BadRequest(new { message = "Invalid Student ID." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var groups = await _groupService.GetAllGroupsAsync(leaderId);
            if (groups == null) {
                return BadRequest("Groups Not Found");
            }
            return Ok(groups);
        }





        [Authorize(Roles = "Student")]
        [HttpGet("Get-Group/{groupId}")]
        public async Task<IActionResult> GetGroupById(int groupId)
        {
            var memberIdClaim = User.Claims.FirstOrDefault(c => c.Type == "StudentID");
            if (memberIdClaim == null)
            {
                return Unauthorized(new { message = "Student ID not found in token." });
            }

            if (!int.TryParse(memberIdClaim.Value, out int memberId))
            {
                return BadRequest(new { message = "Invalid Student ID." });
            }

            var groupDto = await _groupService.GetGroupByIdAsync(groupId, memberId);

            if (groupDto == null)
            {
                return NotFound(new { message = "Group not found or member not part of the group." });
            }

            return Ok(groupDto);
        }
        [Authorize(Roles = "Student")]
        [HttpGet("group-members/{groupId}")]
        public async Task<IActionResult> GetMembersOfGroup(int groupId)
        {
            var memberIdClaim = User.Claims.FirstOrDefault(c => c.Type == "StudentID");
            if (memberIdClaim == null)
            {
                return Unauthorized(new { message = "Student ID not found in token." });
            }

            if (!int.TryParse(memberIdClaim.Value, out int memberId))
            {
                return BadRequest(new { message = "Invalid Student ID." });
            }

            var members = await _groupService.GetMembersOfGroupAsync(groupId, memberId);

            if (!members.Any())
            {
                return BadRequest(new { message = "You are not a member of this group or group not found." });
            }

            return Ok(members);
        }




        [Authorize(Roles = "Student")]
        [HttpPut("Update-Group/{groupId}")]
        public async Task<IActionResult> UpdateGroup(int groupId, [FromBody] UpdateGroupReqDto updateGroupDto)
        {
            var leaderIdClaim = User.Claims.FirstOrDefault(c => c.Type == "StudentID");
            if (leaderIdClaim == null)
            {
                return Unauthorized(new { message = "Student ID not found in token." });
            }

            if (!int.TryParse(leaderIdClaim.Value, out int leaderId))
            {
                return BadRequest(new { message = "Invalid Student ID." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _groupService.UpdateGroupAsync(groupId, updateGroupDto, leaderId);

            if (result.Message.Contains("not found") || result.Message.Contains("Only the group leader"))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize(Roles = "Student")]
        [HttpPut("update-members-role")]
        public async Task<IActionResult> UpdateMemberRole([FromBody] GroupMemberUpdateReqDto memberUpdate)
        {
            var leaderIdClaim = User.Claims.FirstOrDefault(c => c.Type == "StudentID");
            if (leaderIdClaim == null)
            {
                return Unauthorized(new { message = "Student ID not found in token." });
            }

            if (!int.TryParse(leaderIdClaim.Value, out int leaderId))
            {
                return BadRequest(new { message = "Invalid Student ID." });
            }

            var result = await _groupService.UpdateMemberRoleAsync(memberUpdate, leaderId);

            if (result.Message == "Only the group leader can update member roles.")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }



        [Authorize(Roles = "Student")]
        [HttpDelete("Delete-Group/{groupId}")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            var leaderIdClaim = User.Claims.FirstOrDefault(c => c.Type == "StudentID");
            if (leaderIdClaim == null)
            {
                return Unauthorized(new { message = "Student ID not found in token." });
            }

            if (!int.TryParse(leaderIdClaim.Value, out int leaderId))
            {
                return BadRequest(new { message = "Invalid Student ID." });
            }

            var isDeleted = await _groupService.DeleteGroupAsync(groupId, leaderId);

            if (!isDeleted)
            {
                return NotFound(new { message = "Group not found or user is not the leader of the group." });
            }

            return Ok(new { message = "Group deleted successfully." });
        }




     

        [Authorize(Roles = "Student")]
        [HttpDelete("delete-member/{groupId}/{memberId}")]
        public async Task<IActionResult> RemoveMemberFromGroup(int groupId, int memberId)
        {
            var leaderIdClaim = User.Claims.FirstOrDefault(c => c.Type == "StudentID");
            if (leaderIdClaim == null)
            {
                return Unauthorized(new { message = "Student ID not found in token." });
            }

            if (!int.TryParse(leaderIdClaim.Value, out int leaderId))
            {
                return BadRequest(new { message = "Invalid Student ID." });
            }

         
            var result = await _groupService.RemoveMemberFromGroupAsync(groupId, memberId, leaderId);

            if (result.Message == "Only the group leader can remove members.")
            {
                return BadRequest(result);
            }

            return Ok(result); 
        }

       
    }
}
