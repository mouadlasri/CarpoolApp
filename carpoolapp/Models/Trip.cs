using System;
using System.Collections.Generic;

namespace carpoolapp.Models
{
    public partial class Trip
    {
        public Trip()
        {
            Booking = new HashSet<Booking>();
            Comment = new HashSet<Comment>();
        }

        public int TripId { get; set; }
        public int? UserId { get; set; }
        public string MeetingPlace { get; set; }
        public string Departure { get; set; }
        public DateTime DepartureDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public string Destination { get; set; }
        public decimal? Price { get; set; }
        public string SmokeFree { get; set; }
        public decimal? Rating { get; set; }
        public int? NbrOfSeatsOpen { get; set; }
        public int? NbrOfMaxSeats { get; set; }
        public DateTime? DateCreated { get; set; }

        public virtual AppUser User { get; set; }
        public virtual ICollection<Booking> Booking { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
    }
}
