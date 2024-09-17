using Azure.Core;
using EduLink.Data;
using EduLink.Models;
using EduLink.Repositories.Interfaces;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;

namespace EduLink.Repositories.Services
{
    public class HelperService
    {
        private readonly EduLinkDbContext _context;
        private readonly IEmailService _Email;

        public HelperService(EduLinkDbContext context, IEmailService Email)
        {
            _context = context;
            _Email = Email;
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
        public async Task UpdateSessionStatusesAsync()
        {
            var currentDateTime = DateTimeOffset.UtcNow;

       
            var sessions = await _context.Sessions.ToListAsync();

            foreach (var session in sessions)
            {
                
                if (session.EndDate < currentDateTime && session.SessionStatus == SessionStatus.Scheduled)
                {
                    session.SessionStatus = SessionStatus.Completed;
                }
              
                else if (session.StartDate <= currentDateTime && session.EndDate > currentDateTime && session.SessionStatus == SessionStatus.Scheduled)
                {
                    session.SessionStatus = SessionStatus.Ongoing;
                }

                if (session.Capacity <= _context.Bookings.Count(b => b.SessionID == session.SessionID))
                {
                    session.SessionStatus = SessionStatus.Closed;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookingStatusesAsync()
        {
            var currentDateTime = DateTimeOffset.UtcNow;

            var bookings = await _context.Bookings
                .Include(b => b.Event)  // Include event data
                .Include(b => b.Session)// Include session data if applicable
                .Include(b=>b.Student)
                .ThenInclude(s=>s.User)
                .ToListAsync();

            foreach (var booking in bookings)
            {
            
                if (booking.BookingStatus == BookingStatusenum.Canceled)
                {
                    continue;
                }

                if (booking.Event != null)
                {
               
                    if (booking.Event.EndTime < currentDateTime && booking.BookingStatus != BookingStatusenum.Completed)
                    {
                        booking.BookingStatus = BookingStatusenum.Completed;
                    }
                    else if (booking.Event.StartTime <= currentDateTime && booking.Event.EndTime > currentDateTime && booking.BookingStatus == BookingStatusenum.Pending)
                    {
                  
                        booking.BookingStatus = BookingStatusenum.Confirmed;
                        var studentEmaile = booking.Student.User.Email;
                        var emailSubject = $"New Event: {booking.Event.Title}";
                        var emailDescriptionHtml = $@"
                            <p>Dear Student,</p>
                            <p>This is a reminder that your session for the event titled <strong>{booking.Event.Title}</strong> is about to start.</p>
                            <p><strong>Start Time:</strong> {booking.Event.StartTime}</p>
                            <p><strong>Location:</strong> {booking.Event.EventAddress}</p>
                            <p>Please make sure to attend the session at the designated location on time.</p>
                            <p>Best regards,</p>
                            <p>EduLink Team</p>";

                        // Convert HTML to plain text
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(emailDescriptionHtml);
                        var emailDescriptionPlain = htmlDoc.DocumentNode.InnerText;



                        // Send email to all students
                        if (studentEmaile != null)
                        {

                            await _Email.SendEmailAsync(studentEmaile, emailSubject, emailDescriptionHtml);


                            // Create and store announcement
                            var announcement = new Announcement
                            {
                                EventID = booking.EventID,
                                Title = booking.Event.Title,
                                Message = emailDescriptionPlain, // Store as plain text
                                AnounceDate = DateTime.UtcNow
                            };

                            await _context.Announcement.AddAsync(announcement);
                            await _context.SaveChangesAsync();
                        }

                    }
                    else if (booking.Event.EventStatus == EventStatus.Closed && booking.BookingStatus != BookingStatusenum.Canceled)
                    {
                       
                        booking.BookingStatus = BookingStatusenum.Canceled;
                    }
                }

              
                if (booking.Session != null)
                {
                 
                    if (booking.Session.EndDate < currentDateTime && booking.BookingStatus != BookingStatusenum.Completed)
                    {
                        booking.BookingStatus = BookingStatusenum.Completed;
                    }
                    else if (booking.Session.StartDate <= currentDateTime && booking.Session.EndDate > currentDateTime && booking.BookingStatus == BookingStatusenum.Pending)
                    {
                        
                        booking.BookingStatus = BookingStatusenum.Confirmed;
                        var studentEmaile = booking.Student.User.Email;
                        var emailSubject = $"New Event: {booking.Event.Title}";
                        var emailDescriptionHtml = $@"
                            <p>Dear Student,</p>
                            <p>This is a reminder that your session for the event titled <strong>{booking.Event.Title}</strong> is about to start.</p>
                            <p><strong>Start Time:</strong> {booking.Session.StartDate}</p>
                            <p><strong>Location:</strong> {booking.Session.SessionAddress}</p>
                            <p>Please make sure to attend the session at the designated location on time.</p>
                            <p>Best regards,</p>
                            <p>EduLink Team</p>";

                        // Convert HTML to plain text
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(emailDescriptionHtml);
                        var emailDescriptionPlain = htmlDoc.DocumentNode.InnerText;



                        // Send email to all students
                        if (studentEmaile != null )
                        {

                          await _Email.SendEmailAsync(studentEmaile, emailSubject, emailDescriptionHtml);


                            // Create and store announcement
                            var announcement = new Announcement
                            {
                                EventID = booking.EventID,
                                Title = booking.Event.Title,
                                Message = emailDescriptionPlain, // Store as plain text
                                AnounceDate = DateTime.UtcNow
                            };

                            await _context.Announcement.AddAsync(announcement);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else if (booking.Session.SessionStatus == SessionStatus.Closed && booking.BookingStatus != BookingStatusenum.Canceled)
                    {
                       
                        booking.BookingStatus = BookingStatusenum.Canceled;
                    }
                }
            }

          
            await _context.SaveChangesAsync();
        }
    }
}
