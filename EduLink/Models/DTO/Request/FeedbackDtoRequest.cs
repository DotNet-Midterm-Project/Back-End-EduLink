﻿namespace EduLink.Models.DTO.Request
{
    public class FeedbackDtoRequest
    {
        public int BookingId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

    }
}
