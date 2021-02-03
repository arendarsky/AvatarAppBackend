using System;
using System.Collections.Generic;

namespace Avatar.App.Infrastructure.Models.Semifinal
{
    internal class BattleDb : BaseEntity
    {
        public DateTime CreationDate { get; set; }
        public DateTime EndDate { get; set; }
        public int WinnersNumber { get; set; }
        public bool Closed { get; set; }

        public ICollection<BattleVoteDb> Votes { get; set; }
        public ICollection<BattleSemifinalistDb> BattleSemifinalists { get; set; }
    }
}
