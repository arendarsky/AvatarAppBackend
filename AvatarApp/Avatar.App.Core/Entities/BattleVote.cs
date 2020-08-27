using System;
using System.Collections.Generic;
using System.Text;
using Avatar.App.SharedKernel;

namespace Avatar.App.Core.Entities
{
    public class BattleVote: BaseEntity
    {
        public long SemifinalistId { get; set; }

        public Semifinalist Semifinalist { get; set; }

        public long UserId { get; set; }

        public User User { get; set; }

        public DateTime? Date { get; set; }

        public long BattleId { get; set; }

        public Battle Battle { get; set; }
    }
}
