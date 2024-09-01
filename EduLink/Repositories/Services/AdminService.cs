using EduLink.Data;
using EduLink.Models;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduLink.Repositories.Services
{
    public class AdminService : IAdmin
    {

        private readonly EduLinkDbContext _context;

        public AdminService(EduLinkDbContext context)
        {
            _context = context;
        }

        public async Task<MessageResponseDTO> AddCourseAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return new MessageResponseDTO
            {
                Message = $"The course '{course.CourseName}' added successfully."
            };
        }

        public async Task<MessageResponseDTO> DeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return new MessageResponseDTO
            {
                Message = $"The course '{course.CourseName}' deleted successfully."
            };
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<IEnumerable<Volunteer>> GetAllVolunteersAsync()
        {
            return await _context.Volunteers.ToListAsync();
        }
    }
}
