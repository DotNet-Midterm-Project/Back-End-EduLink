using EduLink.Data;
using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace EduLink.Repositories.Services
{
    public class IdentityAccountService : IAccount
    {
        private readonly EduLinkDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtTokenService _jwtTokenService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityAccountService(EduLinkDbContext context, UserManager<User> userManager,
            SignInManager<User> signInManager, JwtTokenService jwtTokenService, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _roleManager = roleManager;
        }

        public async Task<RegisterStudentResDTO> RegisterStudentAsync(RegisterStudentReqDTO registerStudentDto, ModelStateDictionary modelState)
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

            if (await _roleManager.RoleExistsAsync("Student"))
                await _userManager.AddToRoleAsync(user, "Student");

            // Ensure we use the correct property assignments based on your model
            var student = new Student
            {
                UserID = user.Id,  // Use UserID as your model defines it as a string
                User = user,
                //DepartmentID = registerStudentDto.DepartmentID // If you have a DepartmentID property
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(user);
            var token = await _jwtTokenService.GenerateToken(user, TimeSpan.FromMinutes(60));

            return new RegisterStudentResDTO
            {
                StudentID = student.StudentID,  // This should now correctly map to the DB-generated ID
                UserName = user.UserName,
                Email = user.Email,
                DepartmentID = user.DepartmentID, // Assuming DepartmentID exists in RegisterStudentResDTO
                Token = token,
                Roles = roles
            };
        }

        public async Task<RegisterAdminResDTO> RegisterAdminAsync(RegisterAdminReqDTO registerAdminDto, ModelStateDictionary modelState)
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

            // Ensure the role exists before adding the user to the role
            if (await _roleManager.RoleExistsAsync("Admin"))
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = await _jwtTokenService.GenerateToken(user, TimeSpan.FromMinutes(60));

            return new RegisterAdminResDTO
            {
                AdminID = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Token = token,
                Roles = roles
            };
        }

        public async Task<bool> AddStudentToVolunteerRoleAsync(string studentID)
        {
            var user = await _userManager.FindByIdAsync(studentID);
            if (user == null)
            {
                return false;
            }

            // Ensure the role exists before adding the user to the role
            if (await _roleManager.RoleExistsAsync("Volunteer"))
            {
                await _userManager.AddToRoleAsync(user, "Volunteer");
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<LoginResDTO> LoginAsync(LoginReqDTO loginDto)
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

            // Generate token based on the role
            string token = null;
            if (roles.Contains("Volunteer"))
            {
                // Fetch Volunteer data
                var volunteer = await _context.Volunteers
                    .Include(v => v.Student)
                    .FirstOrDefaultAsync(v => v.Student.UserID == user.Id);

                if (volunteer != null)
                {
                    token = await _jwtTokenService.GenerateTokenWithRoleData(user, "Volunteer", volunteer, TimeSpan.FromMinutes(60));
                }
            }
            else if (roles.Contains("Student"))
            {
                // Fetch Student data
                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.UserID == user.Id);

                if (student != null)
                {
                    token = await _jwtTokenService.GenerateTokenWithRoleData(user, "Student", student, TimeSpan.FromMinutes(60));
                }
            }
            else
            {
                // Fallback to generating a general token without role-specific data
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

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }


    }
}
