using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;

namespace EduLink.Repositories.Interfaces
{
    public interface IGroup
    {

        Task<MessageResDTO> CreateGroupAsync(CreateGroupReqDto createGroupDto, int LeaderId);
        Task<List<GroupResDto>> GetAllGroupsAsync(int leaderId);
        Task<GroupDetailsResDto> GetGroupByIdAsync(int groupId , int memberId);
        Task<MessageResDTO> UpdateGroupAsync(int groupId, UpdateGroupReqDto updateGroupDto,int leaderId);
        Task<bool> DeleteGroupAsync(int groupId,int leaderId);


        Task<MessageResDTO> AddMemberToGroupAsync(int leaderid, AddGroupMemberReqDto member);
        Task<IEnumerable<GroupMemberDto>> GetMembersOfGroupAsync(int groupId,int memberId);
        Task<MessageResDTO> RemoveMemberFromGroupAsync(int groupId, int memberId ,int leaderId);
       Task<MessageResDTO> UpdateMemberRoleAsync( GroupMemberUpdateReqDto memberUpdate, int LeaderId);

    }
}
