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
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using SendGrid.Helpers.Mail.Model;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
        private readonly IConfiguration _configuration;

        public IdentityAccountService(EduLinkDbContext context, UserManager<User> userManager,
            SignInManager<User> signInManager, JwtTokenService jwtTokenService,
            RoleManager<IdentityRole> roleManager, IEmailService emailService, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _roleManager = roleManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<string> RegisterStudentAsync(RegisterUserReqDTO registerStudentDto, ModelStateDictionary modelState)
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

            return "Student registration successful.";
        }

        public async Task<string> RegisterAdminAsync(RegisterUserReqDTO registerAdminDto, ModelStateDictionary modelState)
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

            return "Admin registration successful. Welcome to the administration team!";
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
            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
            if (!result.Succeeded)
            {
                return null;
            }

            return await CreateDtoUserResponseAsync(user, true);
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
        //Refresh Token here
        public async Task<LoginResDTO> RefreshToken(TokenResDTO tokenResDto)
        {
            var principle = GetPrincipalFromExpiredToken(tokenResDto.AccessToken);
            var user = await _userManager.FindByNameAsync(principle.Identity.Name);
            if (user == null || user.RefreshToken != tokenResDto.RefreshToken ||
                user.RefreshTokenExpireTime <= DateTime.Now)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }
            return await CreateDtoUserResponseAsync(user, false);
        }

        //Some methods used in refresh token
        // generate the refresh token:
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        //get principal from the expired access token: 
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = JwtTokenService.ValidateToken(_configuration);
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principle = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token");
            }
            return principle;
        }

        //for token
        private async Task<string> GenerateTokenBasedOnRole(User user)
        {
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
            return token;

        }

        private async Task<LoginResDTO> CreateDtoUserResponseAsync(User user, bool populateExp)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            if (populateExp)
                user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return new LoginResDTO
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user),
                AccessToken = await GenerateTokenBasedOnRole(user),
                RefreshToken = refreshToken
            };

        }

        public async Task LogoutAsync(ClaimsPrincipal userPrincipal)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);
            if (user != null)
            {
                user.RefreshTokenExpireTime = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);
            }
        }

    }
}

