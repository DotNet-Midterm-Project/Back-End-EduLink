using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace EduLink.Repositories.Interfaces
{
    public interface IAccount
    {
        Task<string> RegisterStudentAsync(RegisterUserReqDTO registerStudentDto, ModelStateDictionary modelState);
        //Maybe Move this to Admin Routes (This will be used once)
        Task<string> RegisterAdminAsync(RegisterUserReqDTO registerAdminDto, ModelStateDictionary modelState);
        
        //Move this to Student routes
        Task<bool> AddStudentToVolunteerRoleAsync(string studentID);
        Task<LoginResDTO> LoginAsync(LoginReqDTO loginDto);

        Task<LoginResDTO> RefreshToken(TokenResDTO tokenResDto);

        //Search about Logout
        Task LogoutAsync(ClaimsPrincipal userPrincipal);
    }
}
