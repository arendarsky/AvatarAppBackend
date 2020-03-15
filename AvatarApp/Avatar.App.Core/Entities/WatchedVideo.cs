using Avatar.App.SharedKernel;

namespace Avatar.App.Core.Entities
{
    public class WatchedVideo : BaseEntity
    {
        public User User { get; set; }
        public Video Video { get; set; }
        public long UserId { get; set; }
        public long VideoId { get; set; }
    }
}
