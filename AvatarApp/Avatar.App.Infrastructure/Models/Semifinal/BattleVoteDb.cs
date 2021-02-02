using System;
using Avatar.App.Core.Entities;
using Avatar.App.SharedKernel;

namespace Avatar.App.Infrastructure.Models.Semifinal
{
    internal class BattleVoteDb: BaseEntity
    {
        public long SemifinalistId { get; set; }
        public long BattleId { get; set; }
        public long? UserId { get; set; }
        public DateTime Date { get; set; }

        public SemifinalistDb Semifinalist { get; set; }
        public User User { get; set; }
        public BattleDb Battle { get; set; }
    }
}
