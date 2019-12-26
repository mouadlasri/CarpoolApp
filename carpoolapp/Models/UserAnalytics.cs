using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace carpoolapp.Models
{
    public class UserAnalytics
    {
        [Key]
        public int UserAnalyticsId { get; set; }
        public int? NbrOfTripsCreated { get; set; }
        public int? NbrOfTripsJoined { get; set; }
        public int? NbrOfTripsAvailable { get; set; }
    }
}
