using EduLink.Data;
using EduLink.Models;
using Microsoft.EntityFrameworkCore;

namespace EduLink.Repositories.Services
{
    public class HelperService
    {
        private readonly EduLinkDbContext _context;

        public HelperService(EduLinkDbContext context)
        {
            _context = context;
        }

        public  async Task UpdateEventStatusesAsync()
        {
            var currentDateTime = DateTimeOffset.UtcNow;

    
            var events = await _context.Events.ToListAsync();

            foreach (var currentEvent in events)
            {
                if (currentEvent.EndTime < currentDateTime && currentEvent.EventStatus == EventStatus.Scheduled)
                {
           
                    currentEvent.EventStatus = EventStatus.Completed;
                }
                else if (currentEvent.StartTime <= currentDateTime && currentEvent.EndTime > currentDateTime && currentEvent.EventStatus == EventStatus.Scheduled)
                {
   
                    currentEvent.EventStatus = EventStatus.Ongoing;
                }

                if (currentEvent.Capacity <= _context.Bookings.Count(b => b.EventID == currentEvent.EventID))
                {
                    currentEvent.EventStatus = EventStatus.Closed;
                }
            }

    
            await _context.SaveChangesAsync();
        }
    }
}
