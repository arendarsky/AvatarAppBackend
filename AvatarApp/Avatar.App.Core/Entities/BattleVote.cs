using System;
using Avatar.App.SharedKernel;

namespace Avatar.App.Core.Entities
{
    public class BattleVote: BaseEntity
    {
        public long SemifinalistId { get; set; }
        public long BattleId { get; set; }
        public long UserId { get; set; }
        public DateTime Date { get; set; }

        public Semifinalist Semifinalist { get; set; }
        public User User { get; set; }
        public Battle Battle { get; set; }
    }
}
