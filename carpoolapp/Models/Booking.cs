using System;
using System.Collections.Generic;

namespace carpoolapp.Models
{
    public partial class Booking
    {
        public int TripId { get; set; }
        public int UserId { get; set; }
        public string BookStatus { get; set; }
        public DateTime? BookingJoinDate { get; set; }
        public int BookingId { get; set; }

        public virtual Trip Trip { get; set; }
        public virtual AppUser User { get; set; }
    }
}
