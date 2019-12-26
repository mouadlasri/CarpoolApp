using System;
using System.Collections.Generic;

namespace carpoolapp.Models
{
    public partial class Credentials
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }
        public DateTime? LastLoggedin { get; set; }
        public DateTime? LastLoggedOut { get; set; }

        public virtual AppUser AppUser { get; set; }
    }
}
