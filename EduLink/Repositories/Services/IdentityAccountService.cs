﻿using EduLink.Data;
using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EduLink.Repositories.Services
{
    public class IdentityAccountService : IAccount
    {
        private readonly EduLinkDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtTokenService _jwtTokenService;
        //private RoleManager<IdentityRole> _roleManager;


        public IdentityAccountService(EduLinkDbContext context, UserManager<User> userManager,
            SignInManager<User> signInManager, JwtTokenService jwtTokenService, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            //_roleManager = roleManager;
        }

        public async Task<RegisterStudentDtoResponse> RegisterStudentAsync(RegisterStudentDtoRequest registerStudentDto, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return null;
            }

            var user = new User
            {
                UserName = registerStudentDto.UserName,
                Email = registerStudentDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerStudentDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    modelState.AddModelError("", error.Description);
                }
                return null;
            }

            //if (await _roleManager.RoleExistsAsync("Student"))
                await _userManager.AddToRoleAsync(user, "Student");

            var student = new Student
            {
                UserID = user.Id,
                User = user,
                DepartmentID = registerStudentDto.DepartmentID
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(user);
            var token = await _jwtTokenService.GenerateToken(user, TimeSpan.FromMinutes(60));

            return new RegisterStudentDtoResponse
            {
                StudentID = student.UserID,
                UserName = user.UserName,
                Email = user.Email,
                DepartmentID = student.DepartmentID,
                Token = token,  // Return the generated token
                Roles = roles

            };
        }

        public async Task<RegisterAdminDtoResponse> RegisterAdminAsync(RegisterAdminDtoRequest registerAdminDto, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return null;
            }

            var user = new User
            {
                UserName = registerAdminDto.UserName,
                Email = registerAdminDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerAdminDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    modelState.AddModelError("", error.Description);
                }
                return null;
            }

            await _userManager.AddToRoleAsync(user, "Admin");

            var admin = new Admin
            {
                AdminID = user.Id,
                User = user
            };

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(user);
            var token = await _jwtTokenService.GenerateToken(user, TimeSpan.FromMinutes(60));

            return new RegisterAdminDtoResponse
            {
                AdminID = admin.AdminID,
                UserName = user.UserName,
                Email = user.Email,
                Token = token,  // Return the generated token
                Roles = roles
            };
        }

        public async Task<bool> AddStudentToVolunteerRoleAsync(string StudentID)
        {
            var student = await _context.Students.FindAsync(StudentID);
            if (student == null)
            {
                return false;
            }

            var user = await _userManager.FindByIdAsync(student.UserID);
            if (user == null)
            {
                return false;
            }

            await _userManager.AddToRoleAsync(user, "Volunteer");

            var volunteer = new Volunteer
            {
                StudentID = student.UserID,
                Students = student
            };

            _context.Volunteers.Add(volunteer);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<LoginDtoResponse> LoginAsync(LoginDtoRequest loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return null;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, loginDto.Password, false, false);
            if (!result.Succeeded)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = await _jwtTokenService.GenerateToken(user, TimeSpan.FromMinutes(60));

            return new LoginDtoResponse
            {
                Token = token,
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles
            };
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }


    }
}
