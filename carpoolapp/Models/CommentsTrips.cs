using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace carpoolapp.Models
{
    public class CommentsTrips
    {
        [Key]
        public int CommentId { get; set; }
        public int TripId { get; set; }
        public string Username { get; set; }
        public string CommentText { get; set; }
        public byte[] ProfilePicture { get; set; }
        public DateTime? Time_Created { get; set; }
    }
}
