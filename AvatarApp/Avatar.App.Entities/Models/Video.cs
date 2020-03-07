using System.Collections.Generic;

namespace Avatar.App.Entities.Models
{
    public class Video
    {
        public long Id { get; set; }

        public virtual User User { get; set; }

        public string Name { get; set; }
        public string Extension { get; set; }
        public bool? IsApproved { get; set; }
        public bool IsActive { get; set; }

        public double StartTime { get; set; }
        public double EndTime { get; set; }

        public ICollection<WatchedVideo> WatchedBy { get; set; }
        public ICollection<LikedVideo> LikedBy { get; set; }
    }
}
