using System;
using System.Collections.Generic;
using System.Text;

namespace Avatar.App.Core.Entities
{
    public class BattleVote
    {
        public Semifinalist SemifinalistId { get; set; }

        public Semifinalist Semifinalist { get; set; }

        public long UserId { get; set; }

        public User User { get; set; }

        public DateTime? Date { get; set; }

        public long BattleId { get; set; }

        public Battle Battle { get; set; }
    }
}
