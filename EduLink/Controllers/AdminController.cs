using Azure;
using EduLink.Models;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduLink.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdmin _Admin;

        public AdminController(IAdmin admin)
        {
            _Admin = admin;
        }


        [HttpGet("getAllCourse")]

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return (IEnumerable<Course>)await _Admin.GetAllCoursesAsync();
        }


        [HttpGet("getAllVounteer")]

        public async Task<IEnumerable<Volunteer>> GetAllVolunteersAsync()
        {
            return (IEnumerable<Volunteer>)await _Admin.GetAllVolunteersAsync();
        }


        [HttpPost("addCourse")]
        public async Task<IActionResult> AddCourseAsync(Course course)
        {
            var response = await _Admin.AddCourseAsync(course);
            return Ok(response);
        }


       // [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourseAsync(int id)
        {
            var user = await _Admin.DeleteCourseAsync(id);
            if (user == null)
            {
                return NotFound();
    }

            var response = await _Admin.DeleteCourseAsync(id);
            return Ok(response);
        }



    }
}

