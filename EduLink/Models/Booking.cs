﻿namespace EduLink.Models
{
    public class Booking
    {
        public int  BookingID { get; set; }
        public int StudentID { get; set; }

        public Student Student { get; set; }
        public int EventID { get; set; }
        public Event Event { get; set; }
        public int SessionID { get; set; }
        public Session Session { get; set; }
        public BookingStatusenum BookingStatus {  get; set; }
        public Feedback Feedbacks { get; set; }



    }
public enum BookingStatusenum {

    Pending,        
    Confirmed,      
    Completed,
    Canceled

}
}
