using System.Collections.Generic;

namespace Avatar.App.Infrastructure.Models.Casting
{
    internal class VideoDb: BaseEntity
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public bool? IsApproved { get; set; }
        public bool IsActive { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }

        public UserDb User { get; set; }
        public ICollection<WatchedVideoDb> WatchedBy { get; set; }
        public ICollection<LikedVideoDb> LikedBy { get; set; }
    }
}
