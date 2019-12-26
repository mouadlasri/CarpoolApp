using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carpoolapp.Models
{
    public class UserProfileViewModel
    {
        public AppUser AppUser { get; set; }
        public Credentials credentials { get; set; }
        public UserAnalytics analytics { get; set; }

    }
}
