using System;
using System.Collections.Generic;

namespace carpoolapp.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public int? TripId { get; set; }
        public int? UserId { get; set; }
        public DateTime? TimeCreated { get; set; }

        public virtual Trip Trip { get; set; }
        public virtual AppUser User { get; set; }
    }
}
