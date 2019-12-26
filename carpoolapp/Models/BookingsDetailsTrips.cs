using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace carpoolapp.Models
{
    public class BookingsDetailsTrips
    {

        
        public int TripId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? BookingJoinDate { get; set; }

        [Key]
        public int BookingId { get; set; }
    }
}
