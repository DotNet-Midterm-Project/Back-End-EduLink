﻿using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EduLink.Repositories.Interfaces
{
    public interface IAccount
    {
        Task<RegisterStudentResDTO> RegisterStudentAsync(RegisterStudentReqDTO registerStudentDto, ModelStateDictionary modelState);
        //Maybe Move this to Admin Routes (This will be used once)
        Task<RegisterAdminResDTO> RegisterAdminAsync(RegisterAdminReqDTO registerAdminDto, ModelStateDictionary modelState);
        
        //Move this to Student routes
        Task<bool> AddStudentToVolunteerRoleAsync(string studentID);
        Task<LoginResDTO> LoginAsync(LoginReqDTO loginDto);

        //Search about Logout
        Task LogoutAsync();
    }
}
