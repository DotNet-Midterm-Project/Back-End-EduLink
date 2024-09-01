using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EduLink.Repositories.Interfaces
{
    public interface IAccount
    {
        Task<RegisterStudentDtoResponse> RegisterStudentAsync(RegisterStudentDtoRequest registerStudentDto, ModelStateDictionary modelState);
        //Maybe Move this to Admin Routes (This will be used once)
        Task<RegisterAdminDtoResponse> RegisterAdminAsync(RegisterAdminDtoRequest registerAdminDto, ModelStateDictionary modelState);
        
        //Move this to Volunteer routes
        Task<bool> AddStudentToVolunteerRoleAsync(string studentID);
        Task<LoginDtoResponse> LoginAsync(LoginDtoRequest loginDto);

        //Search about Logout
        Task LogoutAsync();
    }
}
