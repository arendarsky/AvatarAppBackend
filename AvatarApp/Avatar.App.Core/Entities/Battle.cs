using System;
using System.Collections.Generic;
using Avatar.App.SharedKernel;

namespace Avatar.App.Core.Entities
{
    public class Battle : BaseEntity
    {
        public DateTime CreationDate { get; set; }
        public DateTime EndDate { get; set; }
        public int WinnersNumber { get; set; }

        public ICollection<BattleVote> Votes { get; set; }
        public ICollection<BattleSemifinalist> BattleSemifinalists { get; set; }
    }
}
