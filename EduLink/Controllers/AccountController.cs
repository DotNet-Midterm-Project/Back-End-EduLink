using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.Text;

namespace EduLink.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly IAccount _accountService;
        private readonly UserManager<User> _userManager;

        public AccountController(IAccount accountService, UserManager<User> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;

        }

        [HttpPost("register-student")]
        public async Task<IActionResult> RegisterStudent(RegisterUserReqDTO registerStudentDto)
        {
            var student = await _accountService.RegisterStudentAsync(registerStudentDto, ModelState);
            if (student == null)
            {
                return BadRequest(ModelState);
            }

            return Ok(student);
        }

        [HttpPost("register-admin")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAdmin(RegisterUserReqDTO registerAdminDto)
        {
            var admin = await _accountService.RegisterAdminAsync(registerAdminDto, ModelState);
            if (admin == null)
            {
                return BadRequest(ModelState);
            }

            return Ok(admin);
        }

        //Move to admin or we need to wait for approve from admin
        [HttpPost("become-volunteer")]
        //[Authorize(Roles = "Student")]
        public async Task<IActionResult> BecomeVolunteer(string volunteerID)
        {
            var result = await _accountService.AddStudentToVolunteerRoleAsync(volunteerID);
            if (!result)
            {
                return NotFound("Student not found or user is not a student.");
            }

            return Ok("Student has become a volunteer.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginReqDTO loginDto)
        {
            var authResponse = await _accountService.LoginAsync(loginDto);
            if (authResponse == null)
            {
                return Unauthorized("Invalid Email or Password.");
            }

            return Ok(authResponse);
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<LoginResDTO>> Refresh(TokenResDTO tokenDto)
        {
            try
            {
                var result = await _accountService.RefreshToken(tokenDto);
                return Ok(result);
            }
            catch (SecurityTokenException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordReqDTO forgotPasswordDto)
        {
            var result = await _accountService.ForgotPasswordAsync(forgotPasswordDto);
            if (!result)
            {
                return BadRequest("Failed to send reset email.");
            }

            return Ok("Password reset link sent.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordReqDTO resetPasswordDto)
        {
            var result = await _accountService.ResetPasswordAsync(resetPasswordDto);
            if (!result)
            {
                return BadRequest("Error resetting password.");
            }

            return Ok("Password reset successful.");
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            var res = new ResetPasswordResDTO
            {
                Email = email,
                Token = token
            };

            return Ok(new
            {
                res,
            });
        }

        [Authorize(Roles = "Student, Volunteer")]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync(User);
            return Ok(new { message = "Logged out successfully" });
        }

        

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string email, string code)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code))
            {
                return BadRequest("Data are empty or null");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("User not found.");

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ConfirmEmailAsync(user, token);

            // Log detailed result information
            if (result.Succeeded)
                return Ok("Email confirmed successfully. You can now log in.");

            return BadRequest("Error confirming email.");
        }
    }
}
