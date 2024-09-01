using EduLink.Models.DTO.Request;
using EduLink.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduLink.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccount _accountService;

        public AccountController(IAccount accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register-student")]
        public async Task<IActionResult> RegisterStudent(RegisterStudentDtoRequest registerStudentDto)
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
        public async Task<IActionResult> RegisterAdmin(RegisterAdminDtoRequest registerAdminDto)
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
        public async Task<IActionResult> Login(LoginDtoRequest loginDto)
        {
            var authResponse = await _accountService.LoginAsync(loginDto);
            if (authResponse == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok(authResponse);
        }

        [HttpPost("logout")]
        [Authorize(Roles = "Volunteer, Student")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return Ok("User has been logged out.");
        }
    }
}
