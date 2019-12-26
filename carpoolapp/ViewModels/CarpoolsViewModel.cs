using carpoolapp.Models;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carpoolapp.ViewModels
{
    public class CarpoolsViewModel
    {
        public List<Trip> TripsList { get; set; }
        public List<Booking> BookingsList { get; set; }
        public List<AppUser> AppUserList { get; set; }
        public List<Comment> CommentsList { get; set; }
        public List<CommentsTrips> CommentsTripsList { get; set; }
        public List<BookingsDetailsTrips> BookingsDetailsTripsList { get; set; }
        public AppUser AppUser { get; set; }


    }
}
