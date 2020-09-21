using System.Collections.Generic;
using Avatar.App.SharedKernel;

namespace Avatar.App.Core.Entities
{
    public class Video: BaseEntity
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public bool? IsApproved { get; set; }
        public bool IsActive { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }

        public User User { get; set; }
        public ICollection<WatchedVideo> WatchedBy { get; set; }
        public ICollection<LikedVideo> LikedBy { get; set; }
    }
}
