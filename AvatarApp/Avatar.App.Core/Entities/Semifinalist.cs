using System;
using System.Collections.Generic;
using Avatar.App.SharedKernel;

namespace Avatar.App.Core.Entities
{
    public class Semifinalist: BaseEntity
    {
        public long UserId { get; set; }
        public DateTime Date { get; set; }
        public string VideoName { get; set; }

        public User User { get; set; }
        public ICollection<BattleVote> Votes { get; set; }
        public ICollection<BattleSemifinalist> BattleSemifinalists { get; set; }
    }
}
