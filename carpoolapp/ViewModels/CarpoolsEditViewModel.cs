using carpoolapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carpoolapp.ViewModels
{
    public class CarpoolsEditViewModel
    {
        public Trip editTrip { get; set; }
        public AppUser AppUser { get; set; }
    }
}
