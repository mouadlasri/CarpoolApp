using carpoolapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carpoolapp.ViewModels
{
    public class RegisterUserViewModel
    {
        public Credentials newCredentials { get; set; }
        public AppUser newAppUser { get; set; }
    }
}
