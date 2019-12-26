using System;
using System.Collections.Generic;

namespace carpoolapp.Models
{
    public partial class AppUser
    {
        public AppUser()
        {
            Booking = new HashSet<Booking>();
            Comment = new HashSet<Comment>();
            Trip = new HashSet<Trip>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public byte[] ProfilePicture { get; set; }

        public virtual Credentials User { get; set; }
        public virtual ICollection<Booking> Booking { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Trip> Trip { get; set; }
    }
}
