using EduLink.Data;
using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail.Model;
using System.Security.Claims;
using System.Text;

namespace EduLink.Repositories.Services
{
    public class IdentityAccountService : IAccount
    {
        private readonly EduLinkDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtTokenService _jwtTokenService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public IdentityAccountService(EduLinkDbContext context, UserManager<User> userManager,
            SignInManager<User> signInManager, JwtTokenService jwtTokenService, RoleManager<IdentityRole> roleManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        public async Task<RegisterStudentResDTO> RegisterStudentAsync(RegisterUserReqDTO registerStudentDto, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return null;
            }

            var user = new User
            {
                UserName = registerStudentDto.UserName,
                Email = registerStudentDto.Email,
                IsAdmin = false, // Explicitly setting IsAdmin to false for students
                IsActived = true,
                IsLocked = false,
                Skills = registerStudentDto.Skills,
                Gender = registerStudentDto.Gender,
                PhoneNumber = registerStudentDto.PhoneNumber,
                DepartmentID = registerStudentDto.DepartmentID,

                
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

            if (await _roleManager.RoleExistsAsync("Student"))
            {
                await _userManager.AddToRoleAsync(user, "Student");
            }

            //Here sending verifing email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            //Change this when the server is live.
            var baseUrl = "http://localhost:5085/api/Account";
            var confirmationLink = $"{baseUrl}/confirm-email?email={user.Email}&code={code}";
            
            var subject = "Confirm your email";
            //with html
            var emailDescription = $@"
                <p>Hello,</p>
                <p>Please confirm your email address by clicking the following link:</p>
                <p><a href='{confirmationLink}'>Confirm Email</a></p>
                <p>Thank you!</p>";

            await _emailService.SendEmailAsync(user.Email, subject, emailDescription);


            var student = new Student
            {
                UserID = user.Id,
                User = user,
                // Other student properties...
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(user);
            //var token = await _jwtTokenService.GenerateToken(user, TimeSpan.FromMinutes(60));

            return new RegisterStudentResDTO
            {
                StudentID = student.StudentID,
                UserName = user.UserName,
                Email = user.Email,
                DepartmentID = user.DepartmentID,
                Token = token,
                Roles = roles
            };
        }

        public async Task<RegisterAdminResDTO> RegisterAdminAsync(RegisterUserReqDTO registerAdminDto, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return null;
            }

            var user = new User
            {
                UserName = registerAdminDto.UserName,
                Email = registerAdminDto.Email,
                IsAdmin = true, // Explicitly setting IsAdmin to false for students
                IsActived = true,
                IsLocked = false,
                Skills = registerAdminDto.Skills,
                Gender = registerAdminDto.Gender,
                PhoneNumber = registerAdminDto.PhoneNumber,
                DepartmentID = registerAdminDto.DepartmentID,
                EmailConfirmed = true,

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
           


            if (await _roleManager.RoleExistsAsync("Admin"))
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = await _jwtTokenService.GenerateToken(user, TimeSpan.FromMinutes(360));

            var register= new RegisterAdminResDTO
            {
                AdminID = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Token = token,
                Roles = roles
            };


            return register;
        }

        public async Task<LoginResDTO> LoginAsync(LoginReqDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return null;
            }
            //if (!user.EmailConfirmed)
            //    return null;

            //I am not sure about this.
            //var result = await _signInManager.PasswordSignInAsync(user.UserName, loginDto.Password, false, false);
            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
            if (!result.Succeeded)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);
            string token = null;

            if (user.IsAdmin)
            {
                // Admin-specific token generation logic
                token = await _jwtTokenService.GenerateTokenWithAdminClaims(user, TimeSpan.FromMinutes(120));
            }
            else if (roles.Contains("Volunteer"))
            {
                // Volunteer-specific logic
                var volunteer = await _context.Volunteers.Include(v => v.Student).FirstOrDefaultAsync(v => v.Student.UserID == user.Id);
                if (volunteer != null)
                {
                    token = await _jwtTokenService.GenerateTokenWithRoleData(user, "Volunteer", volunteer, TimeSpan.FromMinutes(360));
                }
            }
            else if (roles.Contains("Student"))
            {
                // Student-specific logic
                var student = await _context.Students.FirstOrDefaultAsync(s => s.UserID == user.Id);
                if (student != null)
                {
                    token = await _jwtTokenService.GenerateTokenWithRoleData(user, "Student", student, TimeSpan.FromMinutes(360));
                }
            }
            else
            {
                // Default token logic
                token = await _jwtTokenService.GenerateToken(user, TimeSpan.FromMinutes(60));
            }

            return new LoginResDTO
            {
                Token = token,
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles
            };
        }

        public async Task<bool> AddStudentToVolunteerRoleAsync(string studentID)
        {
            // Find the user by ID
            var user = await _userManager.FindByIdAsync(studentID);
            if (user == null)
            {
                return false; // User not found, return false immediately
            }

            // Ensure the role exists before adding the user to the role
            if (await _roleManager.RoleExistsAsync("Volunteer"))
            {
                // Add the user to the 'Volunteer' role
                var result = await _userManager.AddToRoleAsync(user, "Volunteer");
                //await _context.SaveChangesAsync();

                // Return true if adding the role succeeded
                return result.Succeeded;
            }

            // Return false if the role doesn't exist
            return false;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

    }
}

