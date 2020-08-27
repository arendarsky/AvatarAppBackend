using Avatar.App.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avatar.App.Core.Entities
{
    public class Battle : BaseEntity
    {
        public DateTime? CreationDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ICollection<BattleVote> BattleVotes { get; set; }

        public ICollection<BattleSemifinalist> BattleSemifinalists { get; set; }
    }
}
