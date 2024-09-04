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

            // الحصول على جميع الأحداث التي تحتاج إلى تحديث
            var events = await _context.Events.ToListAsync();

            foreach (var currentEvent in events)
            {
                if (currentEvent.EndTime < currentDateTime && currentEvent.EventStatus == EventStatus.Scheduled)
                {
                    // إذا كان تاريخ الانتهاء قد مر وكان الحدث مجدولًا، قم بتحديث الحالة إلى "Completed"
                    currentEvent.EventStatus = EventStatus.Completed;
                }
                else if (currentEvent.StartTime <= currentDateTime && currentEvent.EndTime > currentDateTime && currentEvent.EventStatus == EventStatus.Scheduled)
                {
                    // إذا كان الوقت الحالي بين تاريخ البدء والانتهاء وكان الحدث مجدولًا، قم بتحديث الحالة إلى "Ongoing"
                    currentEvent.EventStatus = EventStatus.Ongoing;
                }

                // تحقق من قدرة الحدث في حالة كان مغلقًا
                if (currentEvent.Capacity <= _context.Bookings.Count(b => b.EventID == currentEvent.EventID))
                {
                    currentEvent.EventStatus = EventStatus.Closed;
                }
            }

            // حفظ التغييرات في قاعدة البيانات
            await _context.SaveChangesAsync();
        }
    }
}
