using System;
using Avatar.App.SharedKernel;

namespace Avatar.App.Core.Entities
{
    public class LikedVideo : BaseEntity
    {
        public long UserId { get; set; }
        public long VideoId { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public Video Video { get; set; }
        public long? SemifinalistId { get; set; }
        public Semifinalist Semifinalist { get; set; }
        public long? BattleId { get; set; }
        public Battle Battle { get; set; }
    }
}
